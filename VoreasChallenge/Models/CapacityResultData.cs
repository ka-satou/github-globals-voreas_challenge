using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VoreasChallenge.Models
{
	/// <summary>
	/// 体力・運動能力測定結果データ
	/// </summary>
	public class CapacityResultData
	{
		public int ID { get; set; }						// データID

		[DataType(DataType.Date)]
		public DateTime? MeasureDay { get; set; }			// 測定日

		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = false)]
		public float? Run20mValue { get; set; }				// 20m走値
		public string Run20mUnit { get; set; }				// 20m走単位

		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = false)]
		public float? ProAgilityValue { get; set; }			// プロアジティ値
		public string ProAgilityUnit { get; set; }			// プロアジティ

		[DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = false)]
		public float? StandJumpValue { get; set; }			// 立幅跳び値
		public string StandJumpUnit { get; set; }			// 立幅跳び単位

		[DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = false)]
		public float? RepetJumpValue { get; set; }			// 反復横跳び値
		public string RepetJumpUnit { get; set; }			// 反復横跳び単位

		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float? VerticalJumpValue { get; set; }		// 垂直跳び値
		public string VerticalJumpUnit { get; set; }		// 垂直跳び単位

		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float? ReboundJumpIndexValue { get; set; }	// リバウウンドジャンプ指数値
		public string ReboundJumpIndexUnit { get; set; }	// リバウウンドジャンプ指数単位

		[DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = false)]
		public float? GCTimeValue { get; set; }				// 接地時間値
		public string GCTimeUnit { get; set; }				// 接地時間単位

		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float? JumpHeightValue { get; set; }			// 跳躍高値
		public string JumpHeightUnit { get; set; }			// 跳躍高単位
	}
}
