/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2016-04-27
 * Time: 8:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace iterator
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			
			// TODO: Implement Functionality Here
//			TestCSharp1_1();
//			TestIteratorProcess();
//			TestYieldBreak();
//			TestFinally();
//			TestFinally2();
//			TestDateRange();
			TestTextReaderHelper();
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		static void TestCSharp1_1()
		{
			IterationSample iteration = new IterationSample(new object[] {
				'a',
				'b',
				'c',
				'd'
			}, 0);
			//两种迭代方式
			//1
//			IEnumerator iterator = iteration.GetEnumerator();
//			while (iterator.MoveNext()) {
//				Console.WriteLine(iterator.Current.ToString());
//			}
			//2
			foreach(object iterator in iteration)
			{
				Console.WriteLine(iterator.ToString());
			}
		}
		
		#region 迭代器工作流程测试
		/// <summary>
		/// 迭代器工作流程测试
		/// ①第一次调用MoveNext前， CreateEnumerable中的代码不会被调用
		/// ②所有工作在调用MoveNext时就完成了，后去Current不会执行任何代码
		/// ③在yield return的位置，代码停止执行，下次调用MoveNext时又继续执行
		/// ④一个方法的不同地方可以编写多个yield return语句
		/// ⑤代码不会在最后yield return处结束，而是通过返回false的MoveNext调用来结束方法的执行
		/// </summary>
		static void TestIteratorProcess()
		{
			IEnumerable<int> iterable = CreateEnumerable();
			IEnumerator<int> iterator = iterable.GetEnumerator();
			Console.WriteLine("Starting to iterate");
			while (true) {
				Console.WriteLine("Calling MoveNext...");
				bool result = iterator.MoveNext();
				Console.WriteLine("...MoveNext result={0}", result);
				if (!result) {
					break;
				}
				Console.WriteLine("Fetching Current...");
				Console.WriteLine("...Current result={0}", iterator.Current);
			}
		}
		
		static readonly string padding = new string(' ', 30);
		static IEnumerable<int> CreateEnumerable()
		{
			Console.WriteLine("{0} Start of CreateEnumerable()", padding);
			for (int i = 0; i < 3; i++) {
				Console.WriteLine("{0} About to yield {1}", padding, i);
				yield return i;
				Console.WriteLine("{0} After yield", padding);
			}
			Console.WriteLine("{0} Yielding final value", padding);
			yield return -1;
			Console.WriteLine("{0} End of CreateEnumerable()", padding);
		}
		#endregion
		
		
		#region yield break
		static void TestYieldBreak()
		{
			DateTime stop = DateTime.Now.AddSeconds(2);
			foreach (int i in CountWithTimeLimit(stop)) {
				Console.WriteLine("Received {0}", i);
				Thread.Sleep(300);
			}
		}
		
		static void TestFinally()
		{
			DateTime stop = DateTime.Now.AddSeconds(2);
			foreach (int i in CountWithTimeLimit(stop)) {
				Console.WriteLine("Received {0}", i);
				if (i > 3) {
					Console.WriteLine("Returning");
					return;//此时，调用方停止执行迭代器后，迭代器的finally依然执行（因为foreach循环自己的finally代码块中会调用IEnumerator的Dispose方法，从而执行迭代器内的finally代码块）
				}
				Thread.Sleep(300);
			}
		}
		
		static void TestFinally2()
		{
			DateTime stop = DateTime.Now.AddSeconds(2);
			IEnumerable<int> iterable=CountWithTimeLimit(stop);
			IEnumerator<int> iterator=iterable.GetEnumerator();
			
			iterator.MoveNext();
			Console.WriteLine("Received {0}",iterator.Current);
			
			iterator.MoveNext();
			Console.WriteLine("Received {0}",iterator.Current);
			
			iterator.Dispose();//此时触发finally代码块
		}
		
		static IEnumerable<int> CountWithTimeLimit(DateTime limit)
		{
			try {
				for (int i = 1; i <= 100; i++) {
					if (DateTime.Now > limit) {
						Console.WriteLine("Yield break!");
						yield break;
					}
					yield return i;
				}
			} finally {
				Console.WriteLine("Stopping!");//不管yield break或i>100，该处代码都会执行
			}
		}
		#endregion
		
		//应用案例
		static void TestDateRange()
		{
			DateRange range=new DateRange(DateTime.Now,DateTime.Now.AddDays(10));
			foreach(DateTime day in range.Range){
				Console.WriteLine("day:"+day.ToString("yyy.MM.dd"));
			}
		}
		
		
		static void TestTextReaderHelper()
		{
			TextReaderHelper txtReader=new TextReaderHelper("data");
			foreach(string line in txtReader.ReadLines())
			{
				Console.WriteLine("Line:"+line);
			}
		}
		
		
		
		
		
		
	}
}