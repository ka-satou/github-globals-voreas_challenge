﻿using System;
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
		public DateTime MeasureDay { get; set; }			// 測定日

		public float? Run20m { get; set; }					// 20m走
		public float? ProAgility { get; set; }				// プロアジティ
		public float? StandJump { get; set; }				// 立幅跳び
		public float? RepetJump { get; set; }				// 反復横跳び
		public float? VerticalJump { get; set; }			// 垂直跳び
		public float? ReboundJumpIndex { get; set; }		// リバウウンドジャンプ指数
		public float? GCTime { get; set; }					// 接地時間
		public float? JumpHeight { get; set; }				// 跳躍高
	}
}