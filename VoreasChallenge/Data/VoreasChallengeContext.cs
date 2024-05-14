using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VoreasChallenge.Models;

namespace VoreasChallenge.Data
{
	public class VoreasChallengeContext : DbContext
	{
		public VoreasChallengeContext (DbContextOptions<VoreasChallengeContext> options)
			: base(options)
		{
		}

		public DbSet<PersonalData> PersonalData { get; set; }			// 個人データクラステーブルインターフェース
		public DbSet<PhysicalData> PhysicalData { get; set; }			// 体格データクラステーブルインターフェース
		public DbSet<CapacityResult> CapacityResult { get; set; }		// 体力・運動能力結果クラステーブルインターフェース
		public DbSet<SportsTypeMaster> SportsTypeMaster { get; set; }	// スポーツタイプマスターインターフェース
		public DbSet<GradeMaster> GradeMaster { get; set; }				// 学年マスターインターフェース
		public DbSet<SexMaster> SexMaster { get; set; }					// 性別マスターインターフェース
	}
}
