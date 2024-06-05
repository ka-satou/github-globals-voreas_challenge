using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VoreasChallenge.Models
{
	/// <summary>
	///  体力・運動能力測定平均データ
	/// </summary>
	public class CapacityResultAvg
	{
		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = false)]
		public float Run20mAvg { get; set; }			// 20m走値平均

		[DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = false)]
		public float ProAgilityAvg { get; set; }		// プロアジティ値平均

		[DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = false)]
		public float StandJumpAvg { get; set; }			// 立幅跳び値平均

		[DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = false)]
		public float RepetJumpAvg { get; set; }			// 反復横跳び値平均

		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float VerticalJumpAvg { get; set; }		// 垂直跳び値平均

		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float ReboundJumpIndexAvg { get; set; }	// リバウウンドジャンプ指数値平均

		[DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = false)]
		public float GCTimeAvg { get; set; }			// 接地時間値平均

		[DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = false)]
		public float JumpHeightAvg { get; set; }		// 跳躍高値平均

	}
}
