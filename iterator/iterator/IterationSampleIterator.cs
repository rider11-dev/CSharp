/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2016-04-27
 * Time: 8:39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;

namespace iterator
{
	/// <summary>
	/// Description of IterationSampleIterator.
	/// </summary>
	public class IterationSampleIterator:IEnumerator
	{
		IterationSample _target;
		int _position;
		public IterationSampleIterator(IterationSample target)
		{
			this._target = target;
			this._position = -1;
		}
		
		#region IEnumerator implementation

		bool IEnumerator.MoveNext()
		{
			if (this._position != _target.Values.Length) {
				this._position++;
			}
			return this._position < this._target.Values.Length;
		}

		void IEnumerator.Reset()
		{
			this._position=-1;
		}

		object IEnumerator.Current {
			get {
				if (this._position == -1 || this._position == _target.Values.Length) {
					return null;
				}
				int index=this._position+this._target.StartPoint;
				index=index%this._target.Values.Length;
				return this._target.Values[index];
			}
		}

		#endregion

		
	}
}
