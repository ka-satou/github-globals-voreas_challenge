using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VoreasChallenge.Models
{
	/// <summary>
	/// 性別マスター
	/// </summary>
	public class SexMaster
	{
		[Key]
		public int Id { get; set; }				// データID
		public string SexName { get; set; }		// 性別名
	}
}
