using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VoreasChallenge.Models
{
	/// <summary>
	/// スポーツタイプマスター
	/// </summary>
	public class SportsTypeMaster
	{
		[Key]
		public int Id { get; set; }					// データID
		public string SportsTypeName { get; set; }	// スポーツタイプ名
	}
}
