/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2016-08-26
 * Time: 10:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace Demo
{
    /// <summary>
    /// Description of AsyncAwaitDownloader.
    /// </summary>
    public class AsyncAwaitDownloader
    {
        public AsyncAwaitDownloader()
        {
        }

        public static async Task<long> DownloadFile(string url)
        {
            Console.WriteLine("任务线程id：{0}", Thread.CurrentThread.ManagedThreadId);
            long length;
            using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/download_async_await", FileMode.OpenOrCreate))
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                if (request != null)
                {
                    using (WebResponse response = await request.GetResponseAsync())
                    {
                        using (Stream s = response.GetResponseStream())
                        {
                            //await s.CopyToAsync(fs);
                            await SaveStreamToFile(s, fs, response.ContentLength);
                        }
                    }
                }
                length = fs.Length;
            }
            return length;
        }

        static async Task<long> SaveStreamToFile(Stream src, FileStream fsDest, long totalLength)
        {
            int bufferSize = 2048;
            byte[] buffer = new byte[bufferSize];
            int readLength = 0;
            long totalReadLength = 0;
            int lastPercent = 0;
            int thisPercent = 0;
            while (true)
            {
                readLength = src.Read(buffer, 0, bufferSize);
                if (readLength > 0)
                {
                    fsDest.Write(buffer, 0, readLength);
                    totalReadLength += readLength;
                    thisPercent = (int)(1.0 * totalReadLength / totalLength * 100);
                    if (thisPercent != lastPercent)
                    {
                        Console.WriteLine("download percent:{0}", thisPercent);
                        lastPercent = thisPercent;
                    }
                }
                else
                {
                    Console.WriteLine("download completed:{0}", thisPercent);
                    break;
                }
                Thread.Sleep(2);
            }
            return totalReadLength;
        }
    }
}











