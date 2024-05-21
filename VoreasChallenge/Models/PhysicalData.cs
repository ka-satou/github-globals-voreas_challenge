using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VoreasChallenge.Models
{
	/// <summary>
	/// 体格データクラス
	/// </summary>
	public class PhysicalData
	{
		[Key, Column(Order = 1)]
		public int ID { get; set; }							// データID

		[Key, Column(Order = 2), DataType(DataType.Date)]
		public DateTime? MeasureDay { get; set; }			// 測定日

		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float Height { get; set; }					// 身長
		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float ShittingHeight { get; set; }			// 座高
		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float LowerLimbLength { get; set; }			// 下肢長
		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float Weight { get; set; }					// 体重
		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float BodyFat { get; set; }					// 体脂肪
	}
}
