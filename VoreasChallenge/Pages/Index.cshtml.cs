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

		// コンストラクタ
		public IndexModel(VoreasChallengeContext context)
		{
			_context = context;
		}

		public PersonalDataJoin PersonalData { get; set; }				// 個人データ
		public IList<MeasureHistory> MeasureHistorys { get; set; }		// 測定履歴データ
		public IList<PhysicalData> PhysicalDatas { get; set; }			// 体格データ履歴リスト
		public IList<CapacityResultData> CapacityResults { get; set; }	// 体力・運動能力結果履歴リスト

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

		// 体格データ履歴リスト取得
		private void GetPhysicalData(int? id)
		{
			var phydata = _context.PhysicalData
			.Where(physdata => physdata.ID == id)
			.OrderByDescending(physdata => physdata.MeasureDay)
			.Select(physdata => physdata).Take(5)
			.ToList();

			if((phydata != null) && (phydata.Count > 0))
			{
				PhysicalDatas = new List<PhysicalData>();
				foreach(var phyitem in phydata)
				{
					PhysicalData physicalData = new PhysicalData();
					physicalData.ID = phyitem.ID;
					physicalData.MeasureDay = phyitem.MeasureDay;
					physicalData.Height = (float)Math.Round(phyitem.Height, 1, MidpointRounding.AwayFromZero);
					physicalData.ShittingHeight = (float)Math.Round(phyitem.ShittingHeight, 1, MidpointRounding.AwayFromZero);
					physicalData.LowerLimbLength = (float)Math.Round(phyitem.LowerLimbLength, 1, MidpointRounding.AwayFromZero);
					physicalData.Weight = (float)Math.Round(phyitem.Weight, 1, MidpointRounding.AwayFromZero);
					physicalData.BodyFat= (float)Math.Round(phyitem.BodyFat, 1, MidpointRounding.AwayFromZero);
					PhysicalDatas.Add(physicalData);
				}
			}
		}

		// 体力・運動能力結果履歴リスト取得
		private void GetCapacityResults(int? id)
		{
			var caprslt = _context.CapacityResult
			.Join(_context.GradeMaster,
				caprt => caprt.Grade,
				grade => grade.Id,
				(caprt, grade) => new {caprt, grade}
			)
			.Join(_context.SportsTypeMaster,
				caprt_grade => caprt_grade.caprt.SportsType,
				sports => sports.Id,
				(caprt_grade, sports) => new {caprt_grade.caprt,caprt_grade.grade, sports}
			)
			.Where(caprt_grade_sports => caprt_grade_sports.caprt.ID == id)
			.OrderByDescending(caprt_grade_sports => caprt_grade_sports.caprt.MeasureDay)
			.Select(caprt_grade_sports => caprt_grade_sports).Take(5)
			.ToList();

			if((caprslt != null) && (caprslt.Count > 0))
			{
				MeasureHistorys = new List<MeasureHistory>();
				CapacityResults = new List<CapacityResultData>();
				foreach(var caprtitem in caprslt)
				{
					MeasureHistory measureHistory = new MeasureHistory();
					measureHistory.ID = caprtitem.caprt.ID;
					measureHistory.MeasureDay = caprtitem.caprt.MeasureDay;
					measureHistory.Grade = caprtitem.grade.GradeName;
					measureHistory.SportsType = caprtitem.sports.SportsTypeName;
					MeasureHistorys.Add(measureHistory);

					CapacityResultData capacityResultData = new CapacityResultData();
					capacityResultData.ID = caprtitem.caprt.ID;
					capacityResultData.MeasureDay = caprtitem.caprt.MeasureDay;
					capacityResultData.Run20m = (caprtitem.caprt.Run20m != null)? (float)Math.Round((float)caprtitem.caprt.Run20m, 1, MidpointRounding.AwayFromZero) : null;
					capacityResultData.ProAgility = (caprtitem.caprt.ProAgility != null)? (float)Math.Round((float)caprtitem.caprt.ProAgility, 1, MidpointRounding.AwayFromZero) : null;
					capacityResultData.StandJump = (caprtitem.caprt.StandJump != null) ? (float)Math.Round((float)caprtitem.caprt.StandJump, 1, MidpointRounding.AwayFromZero) : null;
					capacityResultData.RepetJump = (caprtitem.caprt.RepetJump !=null)? (float)Math.Round((float)caprtitem.caprt.RepetJump, 1, MidpointRounding.AwayFromZero) : null;
					capacityResultData.VerticalJump = (caprtitem.caprt.VerticalJump != null)? (float)Math.Round((float)caprtitem.caprt.VerticalJump, 1, MidpointRounding.AwayFromZero) : null;
					capacityResultData.ReboundJumpIndex = (caprtitem.caprt.ReboundJumpIndex != null)? (float)Math.Round((float)caprtitem.caprt.ReboundJumpIndex, 1, MidpointRounding.AwayFromZero) : null;
					capacityResultData.GCTime = (caprtitem.caprt.GCTime != null)? (float)Math.Round((float)caprtitem.caprt.GCTime, 1, MidpointRounding.AwayFromZero) : null;
					capacityResultData.JumpHeight = (caprtitem.caprt.JumpHeight != null)? (float)Math.Round((float)caprtitem.caprt.JumpHeight, 1, MidpointRounding.AwayFromZero) : null;
					CapacityResults.Add(capacityResultData);
				}
			}

			// 空の行を追加
			if(caprslt.Count< 5)
			{
				for(int index = caprslt.Count; index < 5; index++)
				{
					MeasureHistory measureHistory = new MeasureHistory();
					MeasureHistorys.Add(measureHistory);
					CapacityResultData capacityResultData = new CapacityResultData();
					CapacityResults.Add(capacityResultData);
				}
			}
		}

		// ページ読み出しのときにコールされる。
		public void OnGet()
		{
			int? id = 1;		// ID入力値を取得

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
			GetPhysicalData(id);
			if (PhysicalDatas == null)
			{
				// データなしポップアップ画面表示
				return;
			}

			// 体力・運動能力結果履歴リスト取得
			GetCapacityResults(id);
			if ((MeasureHistorys == null) || ( CapacityResults == null))
			{
				// データなしポップアップ画面表示
				return;
			}
		}
	}
}
