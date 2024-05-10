using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VoreasChallenge.Models
{
	/// <summary>
	/// 体力・運動能力結果
	/// </summary>
	public class CapacityResult
	{
		public int ID { get; set; }						// データID

		[DataType(DataType.Date)]
		public DateTime MeasureDay { get; set; }		// 測定日

		public double Run20m { get; set; }				// 20m走
		public double ProAgility { get; set; }			// プロアジティ
		public double StandJump { get; set; }			// 立幅跳び
		public double RepetJump { get; set; }			// 反復横跳び
		public double VerticalJump { get; set; }		// 垂直跳び
		public double ReboundJumpIndex { get; set; }	// リバウウンドジャンプ指数
	}
}
