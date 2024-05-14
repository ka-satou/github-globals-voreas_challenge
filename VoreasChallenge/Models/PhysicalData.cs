using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VoreasChallenge.Models
{
	/// <summary>
	/// 体格データクラス
	/// </summary>
	public class PhysicalData
	{
		public int ID { get; set; }							// データID

		[DataType(DataType.Date)]
		public DateTime MeasureDay { get; set; }			// 測定日

		public float Height { get; set; }					// 身長
		public float ShittingHeight { get; set; }			// 座高
		public float LowerLimbLength { get; set; }			// 下肢長
		public float Weight { get; set; }					// 体重
		public float BodyFat { get; set; }					// 体脂肪
	}
}
