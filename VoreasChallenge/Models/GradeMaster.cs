using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VoreasChallenge.Models
{
	/// <summary>
	/// 学年マスター
	/// </summary>
	public class GradeMaster
	{
		[Key]
		public int Id { get; set; }				// データID
		public string GradeName { get; set; }	// 学年名
	}
}
