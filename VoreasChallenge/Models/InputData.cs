using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using VoreasChallenge.Service;

namespace VoreasChallenge.Models
{
	/// <summary>
	/// 入力データ
	/// </summary>
	public class InputData
	{
		// ID
		[Required]
		[Display(Name = "個人ID")]
		public int? Id { get; set; }

		//--- 個人情報 ----//

		// 名前
		[Required]
		[Display(Name = "名前")]
		public string Name { get; set; }

		// スポーツタイプ
		[Required]
		[Display(Name = "種目")]
		public int SportsType { get; set; }
		public SelectList SportsTypeSelect { get; set; }

		// 測定日
		[Required]
		[Display(Name = "測定日")]
		[DataType(DataType.Date)]
		public DateTime? MeasureDay { get; set; }

		// 学年
		[Required]
		[Display(Name = "学年")]
		public int Grade { get; set; }
		public SelectList GradeSelect { get; set; }

		// 性別
		[Required]
		[Display(Name = "性別")]
		public int Sex { get; set; }
		public SelectList SexSelect { get; set; }

		// 生年月日
		[Required]
		[Display(Name = "生年月日")]
		[DataType(DataType.Date)]
		public DateTime BirthDay { get; set; }

		//---- 体格情報 ----//

		// 身長
		[Display(Name = "身長")]
		public float? Height { get; set; }

		// 座高
		[Display(Name = "座高")]
		public float? ShittingHeight { get; set; }

		// 下肢長
		[Display(Name = "下肢長")]
		public float? LowerLimbLength { get; set; }

		// 体重
		[Display(Name = "体重")]
		public float? Weight { get; set; }

		// 体脂肪
		[Display(Name = "体脂肪")]
		public float? BodyFat { get; set; }


		//---- 計測情報 ----//

		// 20m走
		[Display(Name = "20m走")]
		public float? Run20m { get; set; }

		// プロアジティ
		[Display(Name = "プロアジティ")]
		public float? ProAgility { get; set; }

		// 立幅跳び
		[Display(Name = "立幅跳び")]
		public float? StandJump { get; set; }

		// 反復横跳び
		[Display(Name = "反復横跳び")]
		public float? RepetJump { get; set; }

		// 垂直跳び
		[Display(Name = "垂直跳び")]
		public float? VerticalJump { get; set; }

		// 接地時間
		[Display(Name = "接地時間")]
		public float? GCTime { get; set; }

		// 跳躍高
		[Display(Name = "跳躍高")]
		public float? JumpHeight { get; set; }
	}
}
