using System;
using System.Collections.Generic;
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
		public double Height { get; set; }					// 身長
		public double ShittingHeight { get; set; }			// 座高
		public double LowerLimbLength { get; set; }			// 下肢長
		public double Weight { get; set; }					// 体重
		public double BodyFat { get; set; }					// 体脂肪
	}
}
