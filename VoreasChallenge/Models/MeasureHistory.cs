using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VoreasChallenge.Models
{
	/// <summary>
	/// 計測履歴データクラス
	/// </summary>
	public class MeasureHistory
	{
		public int ID { get; set; }						// データID

		[DataType(DataType.Date)]
		public DateTime? MeasureDay { get; set; }			// 測定日
		public string Grade { get; set; }					// 学年
		public string SportsType { get; set; }				// スポーツタイプ
	}
}
