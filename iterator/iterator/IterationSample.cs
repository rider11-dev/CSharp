/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2016-04-27
 * Time: 8:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;

namespace iterator
{
	/// <summary>
	/// Description of IterationSample.
	/// </summary>
	public class IterationSample:IEnumerable
	{
	
		object[] _values;
		int _startPoint;
		public object[] Values{ get { return this._values; } }
		public int StartPoint{ get { return this._startPoint; } }
		public IterationSample(object[] values, int startPoint)
		{
			this._values = values;
			this._startPoint = startPoint;
		}
		/// <summary>
		/// 使用自定义枚举器
		/// </summary>
		/// <returns></returns>
//		public IEnumerator GetEnumerator()
//		{
//			return new IterationSampleIterator(this);
//		}
		
		/// <summary>
		/// 使用yield return
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			for(int index=0;index<_values.Length;index++)
			{
				object val=_values[(index+_startPoint)%_values.Length];
				yield return val;
				Console.WriteLine("yield return:"+val);//在调用方通过Current获取到val后，下次MoveNext时，会回来调用本行代码
			}
		}
	}
}
