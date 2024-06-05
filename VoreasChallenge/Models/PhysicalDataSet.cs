using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VoreasChallenge.Models
{
	/// <summary>
	/// 体格データクラス(表示用)
	/// </summary>
	public class PhysicalDataSet
	{
		public int ID { get; set; }							// データID

		[DataType(DataType.Date)]
		public DateTime? MeasureDay { get; set; }			// 測定日

		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float? HeightValue { get; set; }				// 身長データ値
		public string HeightUnit { get; set; }				// 身長単位

		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float? ShittingHeightValue { get; set; }		// 座高データ値
		public string ShittingHeightUnit { get; set; }		// 座高単位

		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float? LowerLimbLengthValue { get; set; }	// 下肢長データ値
		public string LowerLimbLengthUnit { get; set; }		// 下肢長単位

		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float? WeightValue { get; set; }				// 体重データ値
		public string WeightUnit { get; set; }				// 体重単位


		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float? BodyFatValue { get; set; }			// 体脂肪データ値
		public string BodyFatUnit { get; set; }				// 体脂肪単位
	}
}
