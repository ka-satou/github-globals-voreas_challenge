using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoreasChallenge.Data;
using VoreasChallenge.Models;

namespace VoreasChallenge.Service
{
	/// <summary>
	/// データインターフェースサービス
	/// </summary>
	public class DataIfService
	{

		private VoreasChallengeContext _context { get; set; }		// データインタフェース


		// コンストラクタ
		public DataIfService(VoreasChallengeContext context)
		{
			_context = context;		// データコンテキスト設定
		}

		// 個人データ取得
		public PersonalDataJoin GetPersonalData(int? id)
		{
			PersonalDataJoin PersonalData = null;

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

			return PersonalData;
		}

		// 体格データ履歴リスト取得
		public List<PhysicalDataSet> GetPhysicalData(int? id)
		{
			List<PhysicalDataSet> PhysicalDatas = null;

			var phydata = _context.PhysicalData
			.Where(physdata => physdata.ID == id)
			.OrderByDescending(physdata => physdata.MeasureDay)
			.Select(physdata => physdata).Take(5)
			.ToList();

			if((phydata != null) && (phydata.Count > 0))
			{
				PhysicalDatas = new List<PhysicalDataSet>();
				foreach(var phyitem in phydata)
				{
					PhysicalDataSet physicalData = new PhysicalDataSet();
					physicalData.ID = phyitem.ID;
					physicalData.MeasureDay = phyitem.MeasureDay;
					physicalData.HeightValue = (phyitem.Height != null)? (float)Math.Round((float)phyitem.Height, 1, MidpointRounding.AwayFromZero) : null;
					physicalData.HeightUnit = (phyitem.Height != null)? "cm" : null;
					physicalData.ShittingHeightValue = (phyitem.ShittingHeight != null)? (float)Math.Round((float)phyitem.ShittingHeight, 1, MidpointRounding.AwayFromZero) : null;
					physicalData.ShittingHeightUnit = (phyitem.ShittingHeight != null)? "cm" : null;
					physicalData.LowerLimbLengthValue = (phyitem.LowerLimbLength != null)? (float)Math.Round((float)phyitem.LowerLimbLength, 1, MidpointRounding.AwayFromZero) : null;
					physicalData.LowerLimbLengthUnit = (phyitem.LowerLimbLength != null)? "cm" : null;
					physicalData.WeightValue = (phyitem.Weight != null)? (float)Math.Round((float)phyitem.Weight, 1, MidpointRounding.AwayFromZero) : null;
					physicalData.WeightUnit = (phyitem.Weight != null)? "kg" : null;
					physicalData.BodyFatValue= (phyitem.BodyFat != null)? (float)Math.Round((float)phyitem.BodyFat, 1, MidpointRounding.AwayFromZero) : null;
					physicalData.BodyFatUnit = (phyitem.BodyFat != null)? "%" : null;
					PhysicalDatas.Add(physicalData);
				}

				// 空の行を追加
				if(PhysicalDatas.Count< 5)
				{
					for(int index = PhysicalDatas.Count; index < 5; index++)
					{
						PhysicalDataSet physicalData = new PhysicalDataSet();
						PhysicalDatas.Add(physicalData);
					}
				}
			}

			return PhysicalDatas;
		}

		// 体力・運動能力結果履歴リスト取得
		public void GetCapacityResults(int? id,out List<MeasureHistory> MeasureHistorys, out List<CapacityResultData> CapacityResults)
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
					capacityResultData.Run20mValue = (caprtitem.caprt.Run20m != null)? (float)Math.Round((float)caprtitem.caprt.Run20m, 2, MidpointRounding.AwayFromZero) : null;
					capacityResultData.Run20mUnit = (caprtitem.caprt.Run20m != null)? "秒" : null;
					capacityResultData.Run20mPoint = (caprtitem.caprt.Run20mPoint != null)? GetStarPoint((float)caprtitem.caprt.Run20mPoint) : null;
					capacityResultData.ProAgilityValue = (caprtitem.caprt.ProAgility != null)? (float)Math.Round((float)caprtitem.caprt.ProAgility, 2, MidpointRounding.AwayFromZero) : null;
					capacityResultData.ProAgilityUnit = (caprtitem.caprt.ProAgility != null)? "秒" : null;
					capacityResultData.ProAgilityPoint = (caprtitem.caprt.ProAgilityPoint != null)? GetStarPoint((float)caprtitem.caprt.ProAgilityPoint) : null;
					capacityResultData.StandJumpValue = (caprtitem.caprt.StandJump != null)? (float)Math.Round((float)caprtitem.caprt.StandJump, 1, MidpointRounding.AwayFromZero) : null;
					capacityResultData.StandJumpUnit = (caprtitem.caprt.StandJump != null)? "cm" : null;
					capacityResultData.StandJumpPoint = (caprtitem.caprt.StandJumpPoint != null)? GetStarPoint((float)caprtitem.caprt.StandJumpPoint) : null;
					capacityResultData.RepetJumpValue = (caprtitem.caprt.RepetJump !=null)? (float)Math.Round((float)caprtitem.caprt.RepetJump, 0, MidpointRounding.AwayFromZero) : null;
					capacityResultData.RepetJumpUnit = (caprtitem.caprt.RepetJump !=null)? "回" : null;
					capacityResultData.RepetJumpPoint = (caprtitem.caprt.RepetJumpPoint != null)? GetStarPoint((float)caprtitem.caprt.RepetJumpPoint) : null;
					capacityResultData.VerticalJumpValue = (caprtitem.caprt.VerticalJump != null)? (float)Math.Round((float)caprtitem.caprt.VerticalJump, 0, MidpointRounding.AwayFromZero) : null;
					capacityResultData.VerticalJumpUnit = (caprtitem.caprt.VerticalJump != null)? "cm" : null;
					capacityResultData.VerticalJumpPoint = (caprtitem.caprt.VerticalJumpPoint != null)? GetStarPoint((float)caprtitem.caprt.VerticalJumpPoint) : null;
					capacityResultData.ReboundJumpIndexValue = (caprtitem.caprt.ReboundJumpIndex != null)? (float)Math.Round((float)caprtitem.caprt.ReboundJumpIndex, 1, MidpointRounding.AwayFromZero) : null;
					capacityResultData.ReboundJumpIndexUnit = (caprtitem.caprt.ReboundJumpIndex != null)? "cm/s" : null;
					capacityResultData.ReboundJumpIndexPoint = (caprtitem.caprt.ReboundJumpIndexPoint != null)? GetStarPoint((float)caprtitem.caprt.ReboundJumpIndexPoint) : null;
					capacityResultData.GCTimeValue = (caprtitem.caprt.GCTime != null)? (float)Math.Round((float)caprtitem.caprt.GCTime, 3, MidpointRounding.AwayFromZero) : null;
					capacityResultData.GCTimeUnit = (caprtitem.caprt.GCTime != null)? "秒" : null;
					capacityResultData.JumpHeightValue = (caprtitem.caprt.JumpHeight != null)? (float)Math.Round((float)caprtitem.caprt.JumpHeight, 1, MidpointRounding.AwayFromZero) : null;
					capacityResultData.JumpHeightUnit = (caprtitem.caprt.JumpHeight != null)? "cm" : null;
					CapacityResults.Add(capacityResultData);
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
			else
			{
				MeasureHistorys = null;
				CapacityResults = null;
			}
		}

		// 体力・運動能力結果平均値取得
		public CapacityResultAvg GetCapacityResultAvg(int? id)
		{
			CapacityResultAvg resultAvg = new CapacityResultAvg();

			var capAvg = _context.CapacityResult.GroupBy(cap => new { cap.Grade, cap.Sex })
			.Select(caprt => new
				{
					Grade = caprt.Key.Grade,
					Sex =  caprt.Key.Sex,
					Run20mAvg = caprt.Average(capav => capav.Run20m),
					ProAgilityAvg = caprt.Average(capav => capav.ProAgility),
					StandJumpAvg = caprt.Average(capav => capav.StandJump),
					RepetJumpAvg = caprt.Average(capav => capav.RepetJump),
					VerticalJumpAvg = caprt.Average(capav => capav.VerticalJump),
					ReboundJumpIndexAvg = caprt.Average(capav => capav.ReboundJumpIndex),
					GCTimeAvg = caprt.Average(capav => capav.GCTime),
					JumpHeightAvg = caprt.Average(capav => capav.JumpHeight)
				}
			)
			.Where(caprt => caprt.Grade == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Grade).First())
			.Where(caprt => caprt.Sex == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Sex).First())
			.FirstOrDefault();

			resultAvg.Run20mAvg = (float)capAvg.Run20mAvg;
			resultAvg.ProAgilityAvg = (float)capAvg.ProAgilityAvg;
			resultAvg.StandJumpAvg = (float)capAvg.StandJumpAvg;
			resultAvg.RepetJumpAvg = (float)capAvg.RepetJumpAvg;
			resultAvg.VerticalJumpAvg = (float)capAvg.VerticalJumpAvg;
			resultAvg.ReboundJumpIndexAvg = (float)capAvg.ReboundJumpIndexAvg;
			resultAvg.GCTimeAvg = (float)capAvg.GCTimeAvg;
			resultAvg.JumpHeightAvg = (float)capAvg.JumpHeightAvg;

			return resultAvg;
		}

		// ★マークポイント取得(5段階評価)
		private string GetStarPoint(float pointData)
		{
			string starPnt = null;
			if((pointData >= 0.0) && (pointData < 1.0)){
				starPnt = "★";
			}
			else if((pointData >= 1.0) && (pointData < 2.0)){
				starPnt = "★★";
			}
			else if((pointData >= 2.0) && (pointData < 3.0)){
				starPnt = "★★★";
			}
			else if((pointData >= 3.0) && (pointData < 4.0)){
				starPnt = "★★★★";
			}
			else if(pointData >= 4.0){
				starPnt = "★★★★★";
			}

			return starPnt;
		}
	}
}
