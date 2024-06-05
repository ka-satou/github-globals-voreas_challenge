﻿using System;
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
					physicalData.HeightUnit = "cm";
					physicalData.ShittingHeightValue = (phyitem.ShittingHeight != null)? (float)Math.Round((float)phyitem.ShittingHeight, 1, MidpointRounding.AwayFromZero) : null;
					physicalData.ShittingHeightUnit = "cm";
					physicalData.LowerLimbLengthValue = (phyitem.LowerLimbLength != null)? (float)Math.Round((float)phyitem.LowerLimbLength, 1, MidpointRounding.AwayFromZero) : null;
					physicalData.LowerLimbLengthUnit = "cm";
					physicalData.WeightValue = (phyitem.Weight != null)? (float)Math.Round((float)phyitem.Weight, 1, MidpointRounding.AwayFromZero) : null;
					physicalData.WeightUnit = "kg";
					physicalData.BodyFatValue= (phyitem.BodyFat != null)? (float)Math.Round((float)phyitem.BodyFat, 1, MidpointRounding.AwayFromZero) : null;
					physicalData.BodyFatUnit = "%";
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
					capacityResultData.Run20mUnit = "秒";
					capacityResultData.Run20mPoint = (caprtitem.caprt.Run20mPoint != null)? GetStarPoint((float)caprtitem.caprt.Run20mPoint) : null;
					capacityResultData.ProAgilityValue = (caprtitem.caprt.ProAgility != null)? (float)Math.Round((float)caprtitem.caprt.ProAgility, 2, MidpointRounding.AwayFromZero) : null;
					capacityResultData.ProAgilityUnit = "秒";
					capacityResultData.ProAgilityPoint = (caprtitem.caprt.ProAgilityPoint != null)? GetStarPoint((float)caprtitem.caprt.ProAgilityPoint) : null;
					capacityResultData.StandJumpValue = (caprtitem.caprt.StandJump != null) ? (float)Math.Round((float)caprtitem.caprt.StandJump, 1, MidpointRounding.AwayFromZero) : null;
					capacityResultData.StandJumpUnit = "cm";
					capacityResultData.StandJumpPoint = (caprtitem.caprt.StandJumpPoint != null)? GetStarPoint((float)caprtitem.caprt.StandJumpPoint) : null;
					capacityResultData.RepetJumpValue = (caprtitem.caprt.RepetJump !=null)? (float)Math.Round((float)caprtitem.caprt.RepetJump, 0, MidpointRounding.AwayFromZero) : null;
					capacityResultData.RepetJumpUnit = "回";
					capacityResultData.RepetJumpPoint = (caprtitem.caprt.RepetJumpPoint != null)? GetStarPoint((float)caprtitem.caprt.RepetJumpPoint) : null;
					capacityResultData.VerticalJumpValue = (caprtitem.caprt.VerticalJump != null)? (float)Math.Round((float)caprtitem.caprt.VerticalJump, 0, MidpointRounding.AwayFromZero) : null;
					capacityResultData.VerticalJumpUnit = "cm";
					capacityResultData.VerticalJumpPoint = (caprtitem.caprt.VerticalJumpPoint != null)? GetStarPoint((float)caprtitem.caprt.VerticalJumpPoint) : null;
					capacityResultData.ReboundJumpIndexValue = (caprtitem.caprt.ReboundJumpIndex != null)? (float)Math.Round((float)caprtitem.caprt.ReboundJumpIndex, 1, MidpointRounding.AwayFromZero) : null;
					capacityResultData.ReboundJumpIndexUnit = "cm/s";
					capacityResultData.ReboundJumpIndexPoint = (caprtitem.caprt.ReboundJumpIndexPoint != null)? GetStarPoint((float)caprtitem.caprt.ReboundJumpIndexPoint) : null;
					capacityResultData.GCTimeValue = (caprtitem.caprt.GCTime != null)? (float)Math.Round((float)caprtitem.caprt.GCTime, 3, MidpointRounding.AwayFromZero) : null;
					capacityResultData.GCTimeUnit = "秒";
					capacityResultData.JumpHeightValue = (caprtitem.caprt.JumpHeight != null)? (float)Math.Round((float)caprtitem.caprt.JumpHeight, 1, MidpointRounding.AwayFromZero) : null;
					capacityResultData.JumpHeightUnit = "cm";
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

			resultAvg.Run20mAvg = (float)_context.CapacityResult
			.Where(caprt => caprt.Grade == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Grade).First())
			.Where(caprt => caprt.Sex == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Sex).First())
			.Average(caprt => caprt.Run20m);

			resultAvg.ProAgilityAvg = (float)_context.CapacityResult
			.Where(caprt => caprt.Grade == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Grade).First())
			.Where(caprt => caprt.Sex == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Sex).First())
			.Average(caprt => caprt.ProAgility);

			resultAvg.StandJumpAvg = (float)_context.CapacityResult
			.Where(caprt => caprt.Grade == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Grade).First())
			.Where(caprt => caprt.Sex == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Sex).First())
			.Average(caprt => caprt.StandJump);

			resultAvg.RepetJumpAvg = (float)_context.CapacityResult
			.Where(caprt => caprt.Grade == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Grade).First())
			.Where(caprt => caprt.Sex == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Sex).First())
			.Average(caprt => caprt.RepetJump);

			resultAvg.VerticalJumpAvg = (float)_context.CapacityResult
			.Where(caprt => caprt.Grade == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Grade).First())
			.Where(caprt => caprt.Sex == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Sex).First())
			.Average(caprt => caprt.VerticalJump);

			resultAvg.ReboundJumpIndexAvg = (float)_context.CapacityResult
			.Where(caprt => caprt.Grade == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Grade).First())
			.Where(caprt => caprt.Sex == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Sex).First())
			.Average(caprt => caprt.ReboundJumpIndex);

			resultAvg.GCTimeAvg = (float)_context.CapacityResult
			.Where(caprt => caprt.Grade == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Grade).First())
			.Where(caprt => caprt.Sex == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Sex).First())
			.Average(caprt => caprt.GCTime);

			resultAvg.JumpHeightAvg = (float)_context.CapacityResult
			.Where(caprt => caprt.Grade == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Grade).First())
			.Where(caprt => caprt.Sex == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Sex).First())
			.Average(caprt => caprt.JumpHeight);

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
