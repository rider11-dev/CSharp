/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2016-08-25
 * Time: 15:02
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.IO;

namespace Demo
{
	/// <summary>
	/// Description of RequestState.
	/// </summary>
	public class RequestState
	{
		const int BufferSize=1024;
		const string FileName="download";
		public RequestState()
		{
			ReadBuffer=new byte[BufferSize];
		}
		public string DownloadUrl{get;set;}
		public HttpWebRequest Request{get;set;}
		public HttpWebResponse Response{get;set;}
		public Stream ResponseStream{get;set;}
		public byte[] ReadBuffer{get;private set;}
		FileStream _fileStream=null;
		public FileStream FileStream
		{
			get
			{
				if(_fileStream==null||_fileStream.CanSeek==false)
				{
					string fullPath=AppDomain.CurrentDomain.BaseDirectory+"/"+FileName;
					_fileStream=new FileStream(fullPath,FileMode.OpenOrCreate);
				}
				return _fileStream;
			}
		}
		public long LengthRead{get;set;}
		public void CloseFile()
		{
			if(this.FileStream!=null)
			{
				this.FileStream.Close();
			}
		}
	}
}
