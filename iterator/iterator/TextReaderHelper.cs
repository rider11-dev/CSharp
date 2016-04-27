/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2016-04-27
 * Time: 15:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace iterator
{
	/// <summary>
	/// Description of TextReader.
	/// </summary>
	public class TextReaderHelper
	{
		public string FileName;
		public TextReaderHelper(string fileName)
		{
			FileName=fileName;
		}
		
		public IEnumerable<string> ReadLines()
		{
			return ReadLinesCore(delegate{
			                 	return File.OpenText(FileName);
			                 });
		}
		
		private IEnumerable<string> ReadLinesCore(Func<TextReader> provider)
		{
			using(TextReader reader=provider())
			{
				string line;
				while((line=reader.ReadLine())!=null)
				{
					yield return line;
				}
			}
		}
	}
}
