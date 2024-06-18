/****************************************************************
 * File			: Assets\Entry\Global.cs
 * Author		: www.loywong.com
 * COPYRIGHT	: (C)
 * Date			: 2018/04/24
 * Description	: 全局枚举，包含如游戏场景状态
 * Version		: 1.0
 * Maintain		: //[date] desc
 ****************************************************************/

namespace LowoUN {
	public enum Enum_GameState {
		None = -1,
		Login = 0,
		World,
		Battle,

		Test = 999,
	}
}