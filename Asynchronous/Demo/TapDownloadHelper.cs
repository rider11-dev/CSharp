/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2016-08-26
 * Time: 8:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Threading;
using System.Net;
using System.IO;

namespace Demo
{
	/// <summary>
	/// 基于任务的异步
	/// </summary>
	public class TapDownloadHelper
	{
		public TapDownloadHelper()
		{
		}
		
		public static void DownloadFile(string url,CancellationToken cancelToken,IProgress<int> progress,long dowloadedSize=0)
		{
			Console.WriteLine("任务线程id:{0}",Thread.CurrentThread.ManagedThreadId);
			
			HttpWebRequest request=null;
			HttpWebResponse response=null;
			Stream responseStream=null;
			int bufferSize=2048;
			byte[] buffer=new byte[bufferSize];
			FileStream fileStream=new FileStream(AppDomain.CurrentDomain.BaseDirectory+"/download_tap",FileMode.OpenOrCreate);
			fileStream.Seek(dowloadedSize,SeekOrigin.Begin);
			try
			{
				request=(HttpWebRequest)WebRequest.Create(url);
				if(dowloadedSize!=0)
				{
					request.AddRange(dowloadedSize);
				}
				
				response=(HttpWebResponse)request.GetResponse();
				responseStream=response.GetResponseStream();
				
				int readSize=0;
				int lastPercent=0;
				int thisPercent=0;
				long totalSize=response.ContentLength;//这里可能不准确，因为请求中如果指定了Range，则totalSize将不是文件总大小了
				while(true)
				{
					if(cancelToken.IsCancellationRequested==true)
					{
						Console.WriteLine("download canceled,url:{0},{1} bits dowloaded",url,dowloadedSize);
						response.Close();
						fileStream.Close();
						break;
					}
					
					readSize=responseStream.Read(buffer,0,bufferSize);
					if(readSize>0)
					{
						dowloadedSize+=readSize;
						fileStream.Write(buffer,0,readSize);
						
						thisPercent=(int)((float)dowloadedSize/(float)totalSize*100);
						if(lastPercent!=thisPercent)
						{
							progress.Report(thisPercent);
							lastPercent=thisPercent;
						}
					}
					else
					{
						Console.WriteLine("download completed,url:{0},totalSize:{1}",url,dowloadedSize);
						response.Close();
						fileStream.Close();
					}
					
					Thread.Sleep(2);
				}
			}
			catch(AggregateException ex)
			{
				// 因为调用Cancel方法会抛出OperationCanceledException异常
                // 将任何OperationCanceledException对象都视为以处理
				ex.Handle(e=>e is OperationCanceledException);
				Console.WriteLine("download task canceled");
				
				if(response!=null)
				{
					response.Close();
				}
				if(fileStream!=null)
				{
					fileStream.Close();
				}
			}
		}
	}
}





















