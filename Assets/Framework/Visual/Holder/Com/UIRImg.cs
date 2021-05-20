#pragma warning disable 0649//ignore default value null
using UnityEngine;
using UnityEngine.UI;
using LowoUN.Util.Notify;
using UnityEngine.EventSystems;

namespace LowoUN.Module.UI.Com
{
	[RequireComponent(typeof(RawImage))]
	public class UIRImg : MonoBehaviour {
		[SerializeField]
		private bool isInteractiveEnable = true;
		[SerializeField]
		private UIEventRedirect evtHolder;

		private IUI3DModel _ui3DModle;

		private GameObject go;
		private string evtClickName;
		private string evtDragName;
		private string evtDragEndName;

		void Awake () {
			go = gameObject;

			if (isInteractiveEnable) {
				if (evtHolder == null) {
					evtHolder = new GameObject("Collider_3DModel").AddComponent<UIEventRedirect>();
					evtHolder.transform.SetParent (transform);
					evtHolder.GetComponent<RectTransform> ().anchorMin = new Vector2 (0f,0f);
					evtHolder.GetComponent<RectTransform> ().anchorMax = new Vector2 (1f,1f);
					evtHolder.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
					evtHolder.GetComponent<RectTransform> ().sizeDelta = Vector2.one;
				}

				if (evtHolder != null) {
					int hashcode = go.GetHashCode ();

					evtClickName = "UI_3DModel-Click-" + hashcode;
					evtDragName = "UI_3DModel-Drag-" + hashcode;
					evtDragEndName = "UI_3DModel-DragEnd-" + hashcode;

					evtHolder.SetEvtsName (evtClickName, evtDragName, evtDragEndName);

					NotifyMgr.AddListener<PointerEventData> (evtClickName, OnPointerClick_ItemCharaInfo);
					NotifyMgr.AddListener<PointerEventData> (evtDragName, OnDragModel_ItemCharaInfo);
					NotifyMgr.AddListener<PointerEventData> (evtDragEndName, OnDragModelEnd_ItemCharaInfo);
				} else {
					Debug.LogWarning (Util.Log.Format.UI() + "If you want to operate the ui 3D model, a UIEventRedirect obj is needed!!!");
				}
			}
		}

		void OnDestroy () {
			if (evtHolder != null) {
				NotifyMgr.RemoveListener<PointerEventData> (evtClickName, OnPointerClick_ItemCharaInfo);
				NotifyMgr.RemoveListener<PointerEventData> (evtDragName, OnDragModel_ItemCharaInfo);
				NotifyMgr.RemoveListener<PointerEventData> (evtDragEndName, OnDragModelEnd_ItemCharaInfo);
			}
		}

		private void OnPointerClick_ItemCharaInfo(UnityEngine.EventSystems.PointerEventData _data)
		{
			if (_ui3DModle != null) {
				_ui3DModle.TriggerAnimation();
			}		
		}
		private void OnDragModel_ItemCharaInfo(UnityEngine.EventSystems.PointerEventData _data)
		{
			if (_ui3DModle != null)
				_ui3DModle.RotateActor (-_data.delta.x);
		}
		private void OnDragModelEnd_ItemCharaInfo(UnityEngine.EventSystems.PointerEventData _data)
		{
			if (_ui3DModle != null)
				_ui3DModle.RotateActor (-_data.delta.x);
		}

		public GameObject SetInfo (object data, string assetName) {
			GameObject chara3D = null;

			RawImage img = go.GetComponent<RawImage>();
			RenderTexture m_Texture = CreateTexture(img.GetComponent<RectTransform>().sizeDelta);
			img.texture = m_Texture;

			chara3D = LowoUN.Module.Asset.Module_Asset.instance.LoadModel (assetName);
			IUI3DModel chara = chara3D.GetComponent<IUI3DModel>();
			if(chara != null)
				chara.CreateModel (data, m_Texture);

			if(chara3D != null)
				_ui3DModle = chara3D.GetComponent<IUI3DModel> ();

			return chara3D;
		}

		public GameObject SetInfo (int typid, string assetName) {
			GameObject chara3D = null;

			RawImage img = go.GetComponent<RawImage>();
			RenderTexture m_Texture = CreateTexture(img.GetComponent<RectTransform>().sizeDelta);
			img.texture = m_Texture;

			chara3D = LowoUN.Module.Asset.Module_Asset.instance.LoadModel (assetName);
			IUI3DModel chara = chara3D.GetComponent<IUI3DModel>();
			if(chara != null)
				chara.CreateModelByID (typid, m_Texture);

			return chara3D;
		}

		private RenderTexture CreateTexture(Vector2 v2)
		{
			RenderTexture m_Texture = null;
			m_Texture = new RenderTexture((int)v2.x, (int)v2.y, 16,RenderTextureFormat.ARGB32);
			m_Texture.autoGenerateMips = false;
			m_Texture.Create();

			return m_Texture;
		}
	}

}