using System.Collections.Generic;

namespace LowoUN.Module.UI 
{
	public interface ILst
	{
		List<int> SetItemList<T> (List<T> itemList);
		void SetItemFocused (int idx);
		void SetObjidOnHolder (int objid);
	}
}