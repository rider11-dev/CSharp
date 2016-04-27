/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2016-04-27
 * Time: 15:04
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace iterator
{
	/// <summary>
	/// Description of Demo.
	/// </summary>
	public class DateRange
	{
		DateTime StartDay;
		DateTime EndDay;
		
		public IEnumerable<DateTime> Range
		{
			get{
				for(DateTime day =StartDay;day<=EndDay;day=day.AddDays(1))
				{
					yield return day;
				}
			}
		}
		
		public DateRange(DateTime startDay,DateTime endDay)
		{
			StartDay=startDay;
			EndDay=endDay;
		}
		
	}
}
