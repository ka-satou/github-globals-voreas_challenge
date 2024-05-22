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

		public PersonalDataJoin PersonalData { get; set; }				// 個人データ
		public IList<MeasureHistory> MeasureHistorys { get; set; }		// 測定履歴データ
		public IList<PhysicalDataSet> PhysicalDatas { get; set; }		// 体格データ履歴リスト
		public IList<CapacityResultData> CapacityResults { get; set; }	// 体力・運動能力結果履歴リスト


		// ページ読み出しのときにコールされる。
		public void OnGet()
		{
			int? id = 0;		// ID入力値を取得

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
		}
	}
}
