using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace VoreasChallenge.Models
{
	/// <summary>
	/// 入力データ
	/// </summary>
	public class InputData
	{
		// ID
		[Required]
		public int Id { get; set; }

		//--- 個人情報 ----//

		// 名前
		[Required]
		public string Name { get; set; }

		// スポーツタイプ
		public int SportsType { get; set; }

		// 測定日
		public DateTime? MeasureDay { get; set; }

		// 学年
		public int Grade { get; set; }

		// 性別
		public int Sex { get; set; }

		// 生年月日
		public DateTime BirthDay { get; set; }

		//---- 体格情報 ----//

		// 身長
		public float? Height { get; set; }

		// 座高
		public float? ShittingHeight { get; set; }

		// 下肢長
		public float? LowerLimbLength { get; set; }

		// 体重
		public float? Weight { get; set; }

		// 体脂肪
		public float? BodyFat { get; set; }


		//---- 計測情報 ----//

		// 20m走
		public float? Run20m { get; set; }

		// プロアジティ
		public float? ProAgility { get; set; }

		// 立幅跳び
		public float? StandJump { get; set; }

		// 反復横跳び
		public float? RepetJump { get; set; }

		// 垂直跳び
		public float? VerticalJump { get; set; }

		// 接地時間
		public float? GCTime { get; set; }

		// 跳躍高
		public float? JumpHeight { get; set; }
	}
}
