/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2016-08-25
 * Time: 14:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.IO;

namespace Demo
{
	/// <summary>
	/// 基于委托的异步
	/// </summary>
	public class ApmDownloadHelper
	{
		public ApmDownloadHelper()
		{
			Console.WriteLine("use ApmDownloadHelper...");
		}
		
		public static void DownloadFileAsync(string url)
		{
			RequestState reqState=new RequestState();
			try
			{
				HttpWebRequest req=(HttpWebRequest)WebRequest.Create(url);
				
				reqState.Request=req;
				
				req.BeginGetResponse(new AsyncCallback(ResponseCallback),reqState);
				
			} 
			catch(Exception ex)
			{
				 Console.WriteLine("DownloadFileAsync Error,Message is:{0}",ex.Message);
				 reqState.CloseFile();
			}
		}
		
		static void ResponseCallback(IAsyncResult callbackResult)
		{
			RequestState reqState=(RequestState)callbackResult.AsyncState;
			HttpWebRequest req=reqState.Request;
			reqState.Response=(HttpWebResponse)req.EndGetResponse(callbackResult);
			
			Stream responseStream=reqState.Response.GetResponseStream();
			reqState.ResponseStream=responseStream;
			
			IAsyncResult asyncRead=responseStream.BeginRead(reqState.ReadBuffer,0,reqState.ReadBuffer.Length,ReadCallback,reqState);
		}
		
		static void ReadCallback(IAsyncResult asyncResult)
		{
			RequestState reqState=(RequestState)asyncResult.AsyncState;
			try
			{
				Stream responseStream=reqState.ResponseStream;
				int readLength=responseStream.EndRead(asyncResult);
				reqState.LengthRead+=readLength;
				if(readLength>0)
				{
					reqState.FileStream.Write(reqState.ReadBuffer,0,readLength);
					responseStream.BeginRead(reqState.ReadBuffer,0,reqState.ReadBuffer.Length,ReadCallback,reqState);
					Console.WriteLine("downloaded percent:{0}",100.0*reqState.LengthRead/reqState.Response.ContentLength);
				}
				else
				{
					Console.WriteLine("\nThe Length of the File is: {0}", reqState.FileStream.Length);
					reqState.Response.Close();
					reqState.CloseFile();
				}
			}
			catch(Exception ex)
			{
				 Console.WriteLine("ReadCallback Error,Message is:{0}",ex.Message);
				 reqState.CloseFile();
			}
		}
	}
}

















