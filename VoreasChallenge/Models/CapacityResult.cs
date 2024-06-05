using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VoreasChallenge.Models
{
	/// <summary>
	/// 体力・運動能力結果
	/// </summary>
	public class CapacityResult
	{
		[Key, Column(Order = 1)]
		public int ID { get; set; }						// データID

		[Key, Column(Order = 2), DataType(DataType.Date)]
		public DateTime MeasureDay { get; set; }			// 測定日
		public int Grade { get; set; }						// 学年
		public int Sex { get; set; }						// 性別
		public int SportsType { get; set; }					// スポーツタイプ

		public float? Run20m { get; set; }					// 20m走
		public float? ProAgility { get; set; }				// プロアジティ
		public float? StandJump { get; set; }				// 立幅跳び
		public float? RepetJump { get; set; }				// 反復横跳び
		public float? VerticalJump { get; set; }			// 垂直跳び
		public float? ReboundJumpIndex { get; set; }		// リバウウンドジャンプ指数
		public float? GCTime { get; set; }					// 接地時間
		public float? JumpHeight { get; set; }				// 跳躍高
		public float? Run20mPoint { get; set; }				// 20m走得点
		public float? ProAgilityPoint { get; set; }			// プロアジティ得点
		public float? StandJumpPoint { get; set; }			// 立幅跳び得点
		public float? RepetJumpPoint { get; set; }			// 反復横跳び得点
		public float? VerticalJumpPoint { get; set; }		// 垂直跳び得点
		public float? ReboundJumpIndexPoint { get; set; }	// リバウウンドジャンプ指数得点
	}
}
