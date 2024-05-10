using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VoreasChallenge.Models
{
	/// <summary>
	/// 個人データクラス
	/// </summary>
	public class PersonalData
	{
		public int ID { get; set; }					// データID
		public string SportsType { get; set; }		// スポーツタイプ
		public string Name { get; set; }			// 名前
		public string Grade { get; set; }			// 学年
		public string Sex { get; set; }				// 性別
	}
}
