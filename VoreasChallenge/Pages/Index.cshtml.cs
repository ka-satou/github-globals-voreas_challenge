using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoreasChallenge.Data;
using VoreasChallenge.Models;
using VoreasChallenge.Service;

namespace VoreasChallenge.Pages
{
	/// <summary>
	/// Indexビハンドコードクラス
	/// </summary>
	public class IndexModel : PageModel
	{
//		private readonly ILogger<IndexModel> _logger;

//		private readonly VoreasChallengeContext _context;		// データインタフェース

		private static int? IdSave{ get; set; }					// 保存Id
		private static DateTime BirthDayDefault = new DateTime(2010,1,1);	// 誕生日デフォルト

		/// <summary>
		/// データインターフェースサービスクラス
		/// </summary>
		private DataIfService dataIfService { get; set; }

//		public IndexModel(ILogger<IndexModel> logger)
//		{
//			_logger = logger;
//		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="context"></param>
		public IndexModel(VoreasChallengeContext context)
		{
			dataIfService = new DataIfService(context);
		}

		/// <summary>
		/// データ入力パラメータ
		/// </summary>
		[BindProperty]
		public int? InputID { get; set; }

		/// <summary>
		/// 画面表示プロパティ
		/// </summary>
		public PersonalDataJoin PersonalData { get; set; }				// 個人データ
		public IList<MeasureHistory> MeasureHistorys { get; set; }		// 測定履歴データ
		public IList<PhysicalDataSet> PhysicalDatas { get; set; }		// 体格データ履歴リスト
		public IList<CapacityResultData> CapacityResults { get; set; }	// 体力・運動能力結果履歴リスト
		public CapacityResultAvg CapacityResultAvg { get; set; }		// 体力・運動能力結果平均

		/// <summary>
		/// 画面表示データ取得
		/// </summary>
		/// <param name="inputId"></param>
		private void GetViewData(int? inputId = 0)
		{
			int? id = inputId;		// ID入力値を取得

			if (id == null)
			{
				// データなしポップアップ画面表示
				return;
			}

			// 個人データ取得
			PersonalData = dataIfService.GetPersonalData(id);
			if (PersonalData == null)
			{
				// データなしポップアップ画面表示
				return;
			}

			// 体格データ履歴リスト取得
			PhysicalDatas = dataIfService.GetPhysicalData(id);
			if (PhysicalDatas == null)
			{
				// データなしポップアップ画面表示
				return;
			}

			// 体力・運動能力結果履歴リスト取得
			List<MeasureHistory> measureHistorys = null;
			List<CapacityResultData> capacityResults = null;
			dataIfService.GetCapacityResults(id,out measureHistorys,out capacityResults);
			if ((measureHistorys == null) || ( capacityResults == null))
			{
				// データなしポップアップ画面表示
				return;
			}
			MeasureHistorys = new List<MeasureHistory>(measureHistorys);
			CapacityResults = new List<CapacityResultData>(capacityResults);

			// 体力・運動能力結果平均取得
			CapacityResultAvg = dataIfService.GetCapacityResultAvg(id);

			IdSave = id;		// IDを保存
		}

		/// <summary>
		/// ページ初回表示
		/// </summary>
		/// <returns></returns>
		public IActionResult OnGet()
		{
			
			int? id = (InputID != null)? InputID : (IdSave != null)? InputID = IdSave : InputID = 0;
			GetViewData(id);
			return Page();
		}

		/// <summary>
		/// 更新ボタン押下/再表示処理
		/// </summary>
		/// <returns></returns>
		public IActionResult OnPostUpdate()
		{
			int? id = (InputID != null)? InputID : (IdSave != null)? InputID = IdSave : InputID = 0;
			GetViewData(id);
			return Page();
		}

		/// <summary>
		/// データ入力モーダル画面表示
		/// </summary>
		/// <returns></returns>
		public PartialViewResult OnGetInputDataModal()
		{
			int? id = (InputID != null)? InputID : (IdSave != null)? InputID = IdSave : InputID = null;
			PartialViewResult viewResult = null;

			PersonalData personal = null;
			PhysicalData physical = null;
			CapacityResult capacity = null;

			if (id != null)
			{
				personal = dataIfService.GetPersonalData((int)id);
				physical = dataIfService.GetPhysicalData((int)id, DateTime.Now.Date);
				if (physical == null)
				{
					physical = new PhysicalData();
				}
				capacity = dataIfService.GetCapacityResults((int)id, DateTime.Now.Date);
				if(capacity == null)
				{
					capacity = new CapacityResult();
				}
			}

			if(personal == null)		// 新規の場合
			{
				viewResult = new PartialViewResult
				{
					ViewName = "_InputDataModal",
					ViewData = new ViewDataDictionary<InputData>
						(
							ViewData, 
							new InputData
							{
								Id = null,
								SportsTypeSelect = new SelectList(
												dataIfService.GetSportsTypeList(),
												nameof(SportsTypeMaster.Id),
												nameof(SportsTypeMaster.SportsTypeName)
											),	
								GradeSelect = new SelectList(
										dataIfService.GetGradeList(),
										nameof(GradeMaster.Id),
										nameof(GradeMaster.GradeName)
									),
								SexSelect = new SelectList(
										dataIfService.GetSexList(),
										nameof(SexMaster.Id),
										nameof(SexMaster.SexName)
									),
								MeasureDay = DateTime.Now,
								BirthDay = BirthDayDefault
							}
						)
				};
			}
			else	// 登録済みの場合
			{
				viewResult = new PartialViewResult
				{
					ViewName = "_InputDataModal",
					ViewData = new ViewDataDictionary<InputData>
						(
							ViewData, 
							new InputData
							{
								Id = personal.ID,
								Name = personal.Name,
								SportsType = personal.SportsType,
								SportsTypeSelect = new SelectList(
												dataIfService.GetSportsTypeList(),
												nameof(SportsTypeMaster.Id),
												nameof(SportsTypeMaster.SportsTypeName)
											),	
								Grade = personal.Grade,
								GradeSelect = new SelectList(
										dataIfService.GetGradeList(),
										nameof(GradeMaster.Id),
										nameof(GradeMaster.GradeName)
									),
								Sex = personal.Sex,
								SexSelect = new SelectList(
										dataIfService.GetSexList(),
										nameof(SexMaster.Id),
										nameof(SexMaster.SexName)
									),
								MeasureDay = DateTime.Now,
								BirthDay = personal.BirthDay,
								Height = physical.Height,
								ShittingHeight = physical.ShittingHeight,
								LowerLimbLength = physical.LowerLimbLength,
								Weight = physical.Weight,
								BodyFat = physical.BodyFat,
								Run20m = capacity.Run20m,
								ProAgility = capacity.ProAgility,
								StandJump = capacity.StandJump,
								RepetJump = capacity.RepetJump,
								VerticalJump = capacity.VerticalJump,
								GCTime = capacity.GCTime,
								JumpHeight = capacity.JumpHeight
							}
						)
				};
			}

			return viewResult;
		}

		/// <summary>
		/// データ入力モーダル画面保存押下/データ挿入・更新処理 or 入力エラー表示処理
		/// </summary>
		/// <param name="model"></param>
		public PartialViewResult OnPostInputDataModal(InputData model)
		{
			if (ModelState.IsValid)	// データ有効の場合のみ保存
			{
				IdSave = model.Id;
				dataIfService.SvaveInputData(model);
			}

			model.SportsTypeSelect = new SelectList(
											dataIfService.GetSportsTypeList(),
											nameof(SportsTypeMaster.Id),
											nameof(SportsTypeMaster.SportsTypeName)
										);

			model.GradeSelect = new SelectList(
									dataIfService.GetGradeList(),
									nameof(GradeMaster.Id),
									nameof(GradeMaster.GradeName)
								);

			model.SexSelect = new SelectList(
									dataIfService.GetSexList(),
									nameof(SexMaster.Id),
									nameof(SexMaster.SexName)
								);

			return new PartialViewResult
			{
				ViewName = "_InputDataModal",
				ViewData = new ViewDataDictionary<InputData>(ViewData, model)
			};
		}
	}
}
