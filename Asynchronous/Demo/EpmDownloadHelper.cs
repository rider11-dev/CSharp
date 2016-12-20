/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2016-08-25
 * Time: 16:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace Demo
{
	/// <summary>
	/// 基于事件的异步
	/// </summary>
	public class EpmDownloadHelper
	{
		 BackgroundWorker bgWorkerDownload;
		 RequestState reqState;
		public EpmDownloadHelper(string url)
		{
			Console.WriteLine("use ApmDownloadHelper...");
			
			reqState=new RequestState{DownloadUrl=url};
				
				bgWorkerDownload=new BackgroundWorker
				{
					WorkerReportsProgress=true,
					WorkerSupportsCancellation=true
				};
				bgWorkerDownload.DoWork+=DoWork;
				bgWorkerDownload.ProgressChanged+=(sernder,e)=>
				{
					Console.WriteLine("downloaded percent:{0}",e.ProgressPercentage);
				};
				bgWorkerDownload.RunWorkerCompleted+=(sender,e)=>
				{
					 if (e.Error != null)
		            {
					 	Console.WriteLine("completed but error:{0}",e.Error.Message);
					 	reqState.Response.Close();
					 	reqState.CloseFile();
					 	return;
		            }
					 if(e.Cancelled)
					 {
					 	Console.WriteLine("download Cancelled,url:{0},{1} bit data downloaded",reqState.DownloadUrl,reqState.LengthRead);
					 	reqState.Response.Close();
					 	reqState.CloseFile();
					 	return;
					 }
					 
					 //下载完成
					 Console.WriteLine("download completed,url:{0},total size:{1}",reqState.DownloadUrl,reqState.LengthRead);
					 reqState.Response.Close();
					 reqState.CloseFile();
				};
		}
		
		public  void DownloadFileAsync()
		{
			if(bgWorkerDownload.IsBusy!=true)
			{
				bgWorkerDownload.RunWorkerAsync();
			}
			else
			{
				Console.WriteLine("bgWorkerDownload is already running!");
			}
		}
		
	  void DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker bgWorker=sender as BackgroundWorker;
			try
			{
				HttpWebRequest req=(HttpWebRequest)WebRequest.Create(reqState.DownloadUrl);
				// If the part of the file have been downloaded, 
                // The server should start sending data from the DownloadSize to the end of the data in the HTTP entity.
				if(reqState.LengthRead!=0)
				{
					req.AddRange(reqState.LengthRead);
				}
				
				reqState.Request=req;
				reqState.Response=(HttpWebResponse)req.GetResponse();
				reqState.ResponseStream=reqState.Response.GetResponseStream();
				int readSize=0;
				int lastPercent=0;//上次进度
				while(true)
				{
					if(bgWorker.CancellationPending==true)
					{
						e.Cancel=true;
						break;
					}
					readSize=reqState.ResponseStream.Read(reqState.ReadBuffer,0,reqState.ReadBuffer.Length);
					if(readSize>0)
					{
						//写入本地文件
						reqState.FileStream.Write(reqState.ReadBuffer,0,readSize);
						
						reqState.LengthRead+=readSize;
						int thisPercent=(int)((float)reqState.LengthRead/(float)reqState.Response.ContentLength*100);
						if(lastPercent!=thisPercent)
						{
							bgWorker.ReportProgress(thisPercent);
							lastPercent=thisPercent;
						}
					}
					else
					{
						break;
					}
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine("error when DoWork:"+ex.Message);
			}
		}
	  
	  public void Cancel()
	  {
	  	if(bgWorkerDownload.IsBusy&&bgWorkerDownload.WorkerSupportsCancellation==true)
	  	{
	  		bgWorkerDownload.CancelAsync();
	  	}
	  }
	}
}











