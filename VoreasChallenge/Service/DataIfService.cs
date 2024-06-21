using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="context"></param>
		public DataIfService(VoreasChallengeContext context)
		{
			_context = context;		// データコンテキスト設定
		}

		/// <summary>
		/// 個人データ取得
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
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

		/// <summary>
		/// 体格データ履歴リスト取得
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
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

		/// <summary>
		/// 体力・運動能力結果履歴リスト取得
		/// </summary>
		/// <param name="id"></param>
		/// <param name="MeasureHistorys"></param>
		/// <param name="CapacityResults"></param>
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

		/// <summary>
		/// 体力・運動能力結果平均値取得
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public CapacityResultAvg GetCapacityResultAvg(int? id)
		{
			CapacityResultAvg resultAvg = new CapacityResultAvg();

			var capAvg = _context.CapacityResult
			.Where(caprt => (caprt.Run20m != null && caprt.ProAgility != null && caprt.StandJump != null && caprt.RepetJump != null && caprt.VerticalJump != null && caprt.ReboundJumpIndex != null))	
			.GroupBy(cap => new { cap.Grade, cap.Sex })
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
			.Where(caprt => (caprt.Grade == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Grade).First())
			 && caprt.Sex == _context.CapacityResult.OrderByDescending(caprt => caprt.MeasureDay).Where(caprt => caprt.ID == id).Select(caprt => caprt.Sex).First())
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

		/// <summary>
		/// ★マークポイント取得(5段階評価)
		/// </summary>
		/// <param name="pointData"></param>
		/// <returns></returns>
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

		/// <summary>
		/// スポーツタイプリスト取得
		/// </summary>
		/// <returns></returns>
		public List<SportsTypeMaster> GetSportsTypeList() => _context.SportsTypeMaster.OrderBy(data => data.Id).ToList<SportsTypeMaster>();

		/// <summary>
		/// 学年リスト取得
		/// </summary>
		/// <returns></returns>
		public List<GradeMaster> GetGradeList() => _context.GradeMaster.OrderBy(data => data.Id).ToList<GradeMaster>();

		/// <summary>
		/// 性別リスト取得
		/// </summary>
		/// <returns></returns>
		public List<SexMaster> GetSexList() => _context.SexMaster.OrderBy(data => data.Id).ToList<SexMaster>();


		/// <summary>
		/// 個別データ取得
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public PersonalData GetPersonalData(int id) => _context.PersonalData.Where(pdata => pdata.ID == id).FirstOrDefault();

		/// <summary>
		/// 体格データ取得
		/// </summary>
		/// <param name="id"></param>
		/// <param name="date"></param>
		/// <returns></returns>
		public PhysicalData GetPhysicalData(int id, DateTime date) => _context.PhysicalData.Where(physdata => (physdata.ID == id && physdata.MeasureDay == date)).FirstOrDefault();

		/// <summary>
		///  体力・運動能力データ取得
		/// </summary>
		/// <param name="id"></param>
		/// <param name="date"></param>
		/// <returns></returns>
		public CapacityResult GetCapacityResults(int id, DateTime date) => _context.CapacityResult.Where(caprt => (caprt.ID == id && caprt.MeasureDay == date)).FirstOrDefault();

		/// <summary>
		/// 入力データ保存
		/// </summary>
		/// <param name="inputData"></param>
		/// <returns></returns>
		public bool SvaveInputData(InputData inputData)
		{
			bool bResult = false;

			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					int id = 0;
					if(inputData.Id == null)
					{	// 新規登録

						// 新規ID取得
						var idMaxData = _context.PersonalData.GroupBy(p => new { p.ID })
						.Select(p => new
							{
								idMax = p.Max(p => p.ID)
							}
						).FirstOrDefault();
						id = idMaxData.idMax + 1;

						// 個人データ挿入
						PersonalData personal = new PersonalData
						{
							ID = id,
							SportsType = inputData.SportsType,
							Name = inputData.Name,
							Grade = inputData.Grade,
							Sex = inputData.Sex,
							BirthDay = inputData.BirthDay
						};
						_context.PersonalData.Add(personal);
						_context.SaveChanges();
					}
					else
					{	// 登録済み更新

						// 個人データ更新
						id = (int)inputData.Id;
						PersonalData personal = new PersonalData
						{
							ID = id,
							SportsType = inputData.SportsType,
							Name = inputData.Name,
							Grade = inputData.Grade,
							Sex = inputData.Sex,
							BirthDay = inputData.BirthDay
						};
						_context.PersonalData.Update(personal);
						_context.SaveChanges();
					}

					// 体格データ挿入
					bool phyContains =_context.PhysicalData.Any(phy => (phy.ID == id && phy.MeasureDay == inputData.MeasureDay));
					PhysicalData physical = new PhysicalData
					{
						ID = id,
						MeasureDay = (DateTime)inputData.MeasureDay,
						Height = inputData.Height,
						ShittingHeight = inputData.ShittingHeight,
						LowerLimbLength = inputData.LowerLimbLength,
						Weight = inputData.Weight,
						BodyFat = inputData.BodyFat
					};
					if (!phyContains)
					{
						_context.PhysicalData.Add(physical);
					}
					else
					{
						_context.PhysicalData.Update(physical);
					}
					_context.SaveChanges();

					// 体力・運動能力結果データ挿入
					bool capContains =_context.CapacityResult.Any(phy => (phy.ID == id && phy.MeasureDay == inputData.MeasureDay));
					CapacityResult capacity = new CapacityResult
					{
						ID = id,
						MeasureDay = (DateTime)inputData.MeasureDay,
						Grade = inputData.Grade,
						Sex = inputData.Sex,
						SportsType = inputData.SportsType,
						Run20m = inputData.Run20m,
						ProAgility = inputData.ProAgility,
						StandJump = inputData.StandJump,
						RepetJump = inputData.RepetJump,
						VerticalJump = inputData.VerticalJump,
						GCTime = inputData.GCTime,
						JumpHeight = inputData.JumpHeight
					};

					// 体力・運動能力ポイント設定
					capacity = SetCapacityPoint(capacity);

					if (!capContains)
					{
						_context.CapacityResult.Add(capacity);
					}
					else
					{
						_context.CapacityResult.Update(capacity);
					}
					_context.SaveChanges();

					transaction.Commit();
					bResult = true;
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex);
					transaction.Rollback();
				}
			}
			return bResult;
		}

		/// <summary>
		/// 体力・運動能力ポイント設定
		/// </summary>
		/// <param name="capacity"></param>
		/// <returns></returns>
		CapacityResult SetCapacityPoint(CapacityResult capacity)
		{
			// リバウウンドジャンプ指数計算
			capacity.ReboundJumpIndex = ((capacity.JumpHeight != null) && (capacity.GCTime != null))? capacity.JumpHeight / capacity.GCTime : null;

			// 平均値/2乗和/データ数取得
			var capAvgSqrSum = _context.CapacityResult
			.Where(caprt => (caprt.Run20m != null && caprt.ProAgility != null && caprt.StandJump != null && caprt.RepetJump != null && caprt.VerticalJump != null && caprt.ReboundJumpIndex != null))
			.GroupBy(cap => new { cap.Grade, cap.Sex })
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
					Run20mSqrSum = caprt.Sum(capss => capss.Run20m * capss.Run20m),
					ProAgilitySqrSum = caprt.Sum(capss => capss.ProAgility * capss.ProAgility),
					StandJumpSqrSum = caprt.Sum(capss => capss.StandJump * capss.StandJump),
					RepetJumpSqrSum = caprt.Sum(capss => capss.RepetJump * capss.RepetJump),
					VerticalJumpSqrSum = caprt.Sum(capss => capss.VerticalJump * capss.VerticalJump),
					ReboundJumpIndexSqrSum = caprt.Sum(capss => capss.ReboundJumpIndex * capss.ReboundJumpIndex),
					Count = caprt.Count()
				}
			)
			.Where(caprt => (caprt.Grade == capacity.Grade && caprt.Sex == capacity.Sex))
			.FirstOrDefault();

			// 標準偏差計算
			float Run20mStdevp = (float)Math.Sqrt((double)capAvgSqrSum.Run20mSqrSum / (double)capAvgSqrSum.Count - (double)capAvgSqrSum.Run20mAvg * (double)capAvgSqrSum.Run20mAvg);
			float ProAgilityStdevp = (float)Math.Sqrt((double)capAvgSqrSum.ProAgilitySqrSum / (double)capAvgSqrSum.Count - (double)capAvgSqrSum.ProAgilityAvg * (double)capAvgSqrSum.ProAgilityAvg);
			float StandJumpStdevp = (float)Math.Sqrt((double)capAvgSqrSum.StandJumpSqrSum / (double)capAvgSqrSum.Count - (double)capAvgSqrSum.StandJumpAvg * (double)capAvgSqrSum.StandJumpAvg);
			float RepetJumpStdevp = (float)Math.Sqrt((double)capAvgSqrSum.RepetJumpSqrSum / (double)capAvgSqrSum.Count - (double)capAvgSqrSum.RepetJumpAvg * (double)capAvgSqrSum.RepetJumpAvg);
			float VerticalJumpStdevp = (float)Math.Sqrt((double)capAvgSqrSum.VerticalJumpSqrSum / (double)capAvgSqrSum.Count - (double)capAvgSqrSum.VerticalJumpAvg * (double)capAvgSqrSum.VerticalJumpAvg);
			float ReboundJumpIndexStdevp = (float)Math.Sqrt((double)capAvgSqrSum.ReboundJumpIndexSqrSum / (double)capAvgSqrSum.Count - (double)capAvgSqrSum.ReboundJumpIndexAvg * (double)capAvgSqrSum.ReboundJumpIndexAvg);

			// 体力・運動能力ポイント計算
			capacity.Run20mPoint = (capacity.Run20m != null)? (float)(((capacity.Run20m - capAvgSqrSum.Run20mAvg) / Run20mStdevp) * (-1) + 2.5) : null;
			capacity.ProAgilityPoint = (capacity.ProAgility != null)? (float)(((capacity.ProAgility - capAvgSqrSum.ProAgilityAvg) / ProAgilityStdevp) * (-1) + 2.5) : null;
			capacity.StandJumpPoint = (capacity.StandJump != null)? (float)(((capacity.StandJump - capAvgSqrSum.StandJumpAvg) / StandJumpStdevp) + 2.5) : null;
			capacity.RepetJumpPoint = (capacity.RepetJump != null)? (float)(((capacity.RepetJump - capAvgSqrSum.RepetJumpAvg) / RepetJumpStdevp) + 2.5) : null;
			capacity.VerticalJumpPoint = (capacity.VerticalJump != null)? (float)(((capacity.VerticalJump - capAvgSqrSum.VerticalJumpAvg) / VerticalJumpStdevp) + 2.5) : null;
			capacity.ReboundJumpIndexPoint = (capacity.ReboundJumpIndex != null)? (float)(((capacity.ReboundJumpIndex - capAvgSqrSum.ReboundJumpIndexAvg) / ReboundJumpIndexStdevp) + 2.5) : null;

			return capacity;
		}
	}
}
