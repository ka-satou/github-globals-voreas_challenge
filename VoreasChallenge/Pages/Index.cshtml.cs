using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoreasChallenge.Data;
using VoreasChallenge.Models;

namespace VoreasChallenge.Pages
{
	public class IndexModel : PageModel
	{
//		private readonly ILogger<IndexModel> _logger;

		private readonly VoreasChallengeContext _context;		// データインタフェース


//		public IndexModel(ILogger<IndexModel> logger)
//		{
//			_logger = logger;
//		}

		public IndexModel(VoreasChallengeContext context)
		{
			_context = context;
		}


//		public void OnGet()
//		{
//		}

		public PersonalDataJoin PersonalData { get; set; }			// 個人データ
		public IList<PhysicalData> PhysicalDatas { get; set; }		// 体格データ履歴リスト
		public IList<CapacityResult> CapacityResults { get; set; }	// 体力・運動能力結果履歴リスト

		// 個人データ取得
		private void GetPersonalData(int? id)
		{
			var pjoin = _context.PersonalData
			.Join(_context.SportsTypeMaster,
				personal => personal.SportsType,
				sports => sports.Id,
				(personal, sports) => new {personal, sports}
			)
			.Join(_context.GradeMaster,
				personal_sports => personal_sports.personal.Grade,
				grade => grade.Id,
				(personal_sports, grade) => new {personal_sports.personal, personal_sports.sports, grade}
			)
			.Join(_context.SexMaster,
				personal_sports_grade => personal_sports_grade.personal.Sex,
				sex => sex.Id,
				(personal_sports_grade, sex) => new {personal_sports_grade.personal, personal_sports_grade.sports, personal_sports_grade.grade, sex}
			)
			.Select(personal_sports_grade_sex => 
				new
				{ 
					ID = personal_sports_grade_sex.personal.ID,
					SportsType = personal_sports_grade_sex.sports.SportsTypeName,
					Name = personal_sports_grade_sex.personal.Name,
					Grade = personal_sports_grade_sex.grade.GradeName,
					Sex = personal_sports_grade_sex.sex.SexName,
					BirthDay =  personal_sports_grade_sex.personal.BirthDay
				}
			).FirstOrDefault(personal_sports_grade_sex => personal_sports_grade_sex.ID == id);

			// 結果データ取得
			if (pjoin != null) { 
				PersonalData = new PersonalDataJoin();
				PersonalData.ID = pjoin.ID;
				PersonalData.SportsType = pjoin.SportsType;
				PersonalData.Name = pjoin.Name;
				PersonalData.Grade = pjoin.Grade;
				PersonalData.Sex = pjoin.Sex;
				PersonalData.BirthDay = pjoin.BirthDay;
			}
		}

		// ページ読み出しのときにコールされる。
		public async Task OnGetAsync()
		{
			int? id = 0;		// ID入力値を取得

			if (id == null)
			{
				// データなしポップアップ画面表示
				return;
			}

			// 個人データ取得
			GetPersonalData(id);
			if (PersonalData == null)
			{
				// データなしポップアップ画面表示
				return;
			}

			// 体格データ履歴リスト取得
			PhysicalDatas = await _context.PhysicalData.Where(m => m.ID == id).ToListAsync();

			// 体力・運動能力結果履歴リスト取得
			CapacityResults = await _context.CapacityResult.Where(m => m.ID == id).ToListAsync();
		}
	}
}
