/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2016-08-25
 * Time: 14:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("this is an Asynchronous demo");
            Console.WriteLine("press any key to continue...");
            Console.ReadKey();

            Console.WriteLine("主线程id:{0}", Thread.CurrentThread.ManagedThreadId);

            //string url="http://download.microsoft.com/download/9/5/A/95A9616B-7A37-4AF6-BC36-D6EA96C8DAAE/dotNetFx40_Full_x86_x64.exe";
            string url = "http://localhost/FileServer/ChromeStandalone_52.0.2743.116_Setup.exe";
            //1、apm
            //ApmDownloadHelper.DownloadFileAsync(url);

            //2、epm
            //EpmDownloadHelper epmDownloader=new EpmDownloadHelper(url);
            //epmDownloader.DownloadFileAsync();
            //取消
            //empDownloader.Cancel();

            //3、tpm
            //SynchronizationContext syncContext=SynchronizationContext.Current;winform等ui调用时，需要这个
            //CancellationTokenSource cancelTokenSrc=new CancellationTokenSource();
            //IProgress<int>	progress=new Progress<int>(p=>{
            //                                          	//syncContext.Post(new SendOrPostCallback((result)=>Console.WriteLine("dowloaded percent:{0}",(int)result)),p);
            //                                          	Console.WriteLine("dowloaded percent:{0}",p);
            //                                         });
            //Task taskDownload=new Task(()=>
            //                           {
            //                           	TapDownloadHelper.DownloadFile(url,cancelTokenSrc.Token,progress,0);
            //                           },cancelTokenSrc.Token);

            //taskDownload.Start();

            //Thread.Sleep(10000);
            //cancelTokenSrc.Cancel();

            //4、async 、await
            DownloadTest(url);

            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }

        static async void DownloadTest(string url)
        {
            long length = await AsyncAwaitDownloader.DownloadFile(url);

        }
    }
}