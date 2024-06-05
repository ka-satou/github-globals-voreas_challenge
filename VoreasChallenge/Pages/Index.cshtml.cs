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
using VoreasChallenge.Service;

namespace VoreasChallenge.Pages
{
	public class IndexModel : PageModel
	{
//		private readonly ILogger<IndexModel> _logger;

//		private readonly VoreasChallengeContext _context;		// データインタフェース

		private DataIfService dataIfService { get; set; }

//		public IndexModel(ILogger<IndexModel> logger)
//		{
//			_logger = logger;
//		}

		// コンストラクタ
		public IndexModel(VoreasChallengeContext context)
		{
			dataIfService = new DataIfService(context);
		}


		[BindProperty]
		public int? InputID { get; set; } = 0;

		public PersonalDataJoin PersonalData { get; set; }				// 個人データ
		public IList<MeasureHistory> MeasureHistorys { get; set; }		// 測定履歴データ
		public IList<PhysicalDataSet> PhysicalDatas { get; set; }		// 体格データ履歴リスト
		public IList<CapacityResultData> CapacityResults { get; set; }	// 体力・運動能力結果履歴リスト
		public CapacityResultAvg CapacityResultAvg { get; set; }		// 体力・運動能力結果平均

		// ページ読み出しのときにコールされる。
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
		}

		// ページ初期表示
		public IActionResult OnGet()
		{
			int? id = InputID;
			GetViewData(id);
			return Page();
		}

		// 更新・データ入力ボタン押下
		public IActionResult OnPostUpdate()
		{
			int? id = InputID;
			GetViewData(id);
			return Page();
		}

		// 更新・データ入力ボタン押下
		public IActionResult  OnPostInput()
		{
			// データ入力ポップアップ画面表示

			int? id = InputID;
			GetViewData(id);
			return Page();
		}
	}
}
