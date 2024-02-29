namespace Tgs.GameWagers.Common.Enums;

/// <summary>
/// 投注狀態
/// </summary>
public enum WagerState
{
	/// <summary>
	/// 等待中: 我們交易員因為可能因為賠率的轉換等因素，還未接受這張註單。
	/// </summary>
	Waiting = 0,

	/// <summary>
	/// 進行中: 此註單還沒有結算。(注單還沒有結算的狀態有可能是這場比賽還沒有結算之類的情形。)
	/// </summary>
	Running = 1,

	/// <summary>
	/// 作廢: 在註單為 running 的狀態下，玩家下註註金返回。原因可能為我們交易員對此場賽事有些疑慮。可與我們聯繫詢問發生什麼狀況。
	/// </summary>
	Void = 2,

	/// <summary>
	/// 退款: 在註單為 running 的狀態下，玩家下註註金返回。原因有可能是賽事取消或發生什麼意外。
	/// </summary>
	Refund = 3,

	/// <summary>
	/// 已取消: 在註單為 waiting 的狀態下，玩家下註註金返回。可能狀況很多。
	/// </summary>
	Reject = 4,

	/// <summary>
	/// 輸: 此註單已結算且玩家輸了此註單。
	/// </summary>
	Lose = 5,

	/// <summary>
	/// 贏: 此注单已结算且玩家赢了此注单。
	/// </summary>
	Win = 6,

	/// <summary>
	/// 和局: 此註單已結算且此註單為和局。
	/// </summary>
	Draw = 7,

	/// <summary>
	/// 半赢: 此註單已結算且玩家贏了下注額一半。
	/// </summary>
	HalfWin = 8,

	/// <summary>
	/// 半輸:  此註單已結算且玩家輸了下注額一半。
	/// </summary>
	HalfLose = 9
}
