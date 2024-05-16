using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VoreasChallenge.Models
{
	/// <summary>
	/// 個人データクラス
	/// </summary>
	public class PersonalData
	{
		[Key]
		public int ID { get; set; }					// データID
		public int SportsType { get; set; }			// スポーツタイプ
		public string Name { get; set; }			// 名前
		public int Grade { get; set; }				// 学年
		public int Sex { get; set; }				// 性別

		[DataType(DataType.Date)]
		public DateTime BirthDay { get; set; }		// 誕生日
	}
}
