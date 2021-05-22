using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace LowoUN.Module.UI.HudAction 
{
	public class UHUDAction : MonoBehaviour
	{
		private static UHUDAction _instance = null;
		public static UHUDAction instance {
			get {
				//if (_instance == null) {
				//	_instance = GameObject.FindObjectOfType<UHUDAction>();
				//	if (_instance == null)
				//		_instance = new GameObject("HUDText").AddComponent<UHUDAction>();
				//}
				return _instance;
			}
		}

		public Transform CanvasParent = null;
		public GameObject ImagePrefab = null;

		public float FadeSpeed = 30f;
		public float FloatingSpeed = 10f;
		public float HideDistance = 75f;
		public float MaxViewAngle = 180;
		public bool DestroyTextOnDeath = true;
		private List<UAction> textList = new List<UAction>();
		private Camera _camera = null;

//		public void Awake()
//		{
//			_instance = this;
//
//			Init();
//		}
//
//		private void Init()
//		{
//			if(LowoUN.Module.Cameras.Module_Camera.instance != null)
//				_camera = LowoUN.Module.Cameras.Module_Camera.instance.GetCurCamera ();
//			
//			if (this.CanvasParent == null)
//				//UGUI.UGUIUtil.CreateCanvas().transform;
//				Debug.LogWarning("====== LowoUN-UI ===> HUD / Action: Don't forget to set CanvasRoot");
//			
//			if (this.ImagePrefab == null)
//				//UGUIUtil.CreateImage();
//				Debug.LogWarning("====== LowoUN-UI ===> HUD / Action: Don't forget to set ImagePrefab");
//		}

		public void SetCamera(Camera cam)
		{
			this._camera = cam;
		}

		/// <summary>
		/// set label font
		/// </summary>
		/// <param name="font"></param>
		public void SetFont(Font font)
		{
			this.ImagePrefab.GetComponent<Text>().font = font;
		}

		/// <summary>
		/// set outline color
		/// </summary>
		/// <param name="color"></param>
		public void SetOutlineColor(Color color)
		{
			this.ImagePrefab.GetComponent<Outline>().effectColor = color;
		}

		/// <summary>
		/// Set the outline distance
		/// </summary>
		/// <param name="distance"></param>
		public void SetOutLineOffset(Vector2 distance)
		{
			this.ImagePrefab.GetComponent<Outline>().effectDistance = distance;
		}

		/// <summary>
		/// Disable all text.
		/// </summary>
		public void OnEnd()
		{
			for (int i = 0; i < this.textList.Count; i++)
			{
				if (this.textList[i] != null)
				{
					Destroy(this.textList[i].gameObject);
				}
				this.textList.Remove(this.textList[i]);
			}
			this.textList.Clear();
			if (this.ImagePrefab != null)
			{
				Destroy(this.ImagePrefab);
			}
			if (this.CanvasParent != null)
			{
				Destroy(this.CanvasParent);
			}
		}

		void OnDestroy()
		{
	    	OnEnd();
		}

		/// <summary>
		/// send a new event, to create a new floating text
		/// </summary>
		/// <param name="text">text to show</param>
		/// <param name="pos">start position of the text</param>
		/// <param name="color">color of the text</param>
		/// <param name="size">size of the text font</param>
		/// <param name="speed">move speed of the text</param>
		/// <param name="xStartAcce">x direction start acceleraction</param>
		/// <param name="yStartAcce">y direction start acceleraction</param>
		/// <param name="yAcceScaleFactor"></param>
		/// <param name="movement">direction type of movement</param>
		/// //Color color, int size, 
		public void CreateImage(string uiSpriteName, Vector3 pos, float speed, float xStartAcce, float yStartAcce, float yAcceScaleFactor, UIEnum_Hud_UDriectionType movement)
		{
			GameObject obj = Instantiate(ImagePrefab) as GameObject;
			UAction item = obj.GetComponent<UAction>();
			item.speed = speed;
			item.color = Color.white;
			item.pos = pos;

			//item.text = text;
			Sprite s = UIAsset.instance.LoadSprite (uiSpriteName);
			if (s != null)
			{
				obj.SetActive(true);
				obj.GetComponent<Image>().sprite = s;
			}
			else{
				obj.SetActive(false);
			}


			//item.size = size;
			item.movement = movement;
			item.xAcceleration = xStartAcce;
			item.yAcceleration = yStartAcce;
			item.yQuicknessScaleFactor = yAcceScaleFactor;
			obj.transform.SetParent(CanvasParent, false);
			obj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			this.textList.Add(item);
		}

		private void DrawHud()
		{
			//if (Event.current.type == EventType.Repaint)
			//{
				for (int i = 0; i < this.textList.Count; i++)
				{
					//when target is destroyed then remove it from list.
					if (this.textList[i].rect.gameObject == null)
					{
						//When player / Object death, destroy all last text.
						if (this.DestroyTextOnDeath)
						{
							Destroy(this.textList[i].rect.gameObject);
							this.textList[i] = null;
						}
						this.textList.Remove(this.textList[i]);
						return;
					}
					UAction temporal = this.textList[i];

					//fade text
	                temporal.color -= new Color(0f, 0f, 0f, (Time.deltaTime * (this.FadeSpeed + this.textList[i].speed)) / 100f);

					//if Text have more than a target graphic
					//add a canvas group in the root for fade all
					if (this.textList[i].layoutRoot != null)
					{
						this.textList[i].layoutRoot.alpha = this.textList[i].color.a;
					}
					//if complete fade remove and destroy text
					if (this.textList[i].color.a <= 0f)
					{
						Destroy(this.textList[i].rect.gameObject);
						this.textList[i] = null;
						this.textList.Remove(this.textList[i]);
					}
					else//if UI visible
					{
						//Convert Word Position in screen position for UI
						float mov = UIAdaptScreen.instance.GetScaleValue() * this._camera.WorldToScreenPoint(this.textList[i].pos + Vector3.up).y - this._camera.WorldToScreenPoint(this.textList[i].pos - Vector3.up).y;
						UAction text = textList[i];
						text.yAcceleration += Time.deltaTime * this.textList[i].yQuicknessScaleFactor;
						switch (this.textList[i].movement)
						{
						case UIEnum_Hud_UDriectionType.Up:
							text.yOffset += (((Time.deltaTime * this.FloatingSpeed) * mov) * 0.25f) * this.textList[i].yAcceleration;
							break;
						case UIEnum_Hud_UDriectionType.Down:
							text.yOffset -= (((Time.deltaTime * this.FloatingSpeed) * mov) * 0.25f) * this.textList[i].yAcceleration;
							break;
						case UIEnum_Hud_UDriectionType.Left:
							text.xOffset -= (((Time.deltaTime * this.FloatingSpeed) * mov) * 0.25f) * this.textList[i].xAcceleration;
							break;
						case UIEnum_Hud_UDriectionType.Right:
							text.xOffset += (((Time.deltaTime * this.FloatingSpeed) * mov) * 0.25f) * this.textList[i].xAcceleration;
							break;
						case UIEnum_Hud_UDriectionType.RightUp:
							text.yOffset += (((Time.deltaTime * this.FloatingSpeed) * mov) * 0.25f) * this.textList[i].yAcceleration;
							text.xOffset += (((Time.deltaTime * this.FloatingSpeed) * mov) * 0.25f);
							break;
						case UIEnum_Hud_UDriectionType.RightDown:
							text.yOffset -= (((Time.deltaTime * this.FloatingSpeed) * mov) * 0.25f) * this.textList[i].yAcceleration;
							text.xOffset += (((Time.deltaTime * this.FloatingSpeed) * mov) * 0.25f) * this.textList[i].xAcceleration;
							break;
						case UIEnum_Hud_UDriectionType.LeftUp:
							text.yOffset += (((Time.deltaTime * this.FloatingSpeed) * mov) * 0.25f) * this.textList[i].yAcceleration;
							text.xOffset -= (((Time.deltaTime * this.FloatingSpeed) * mov) * 0.25f) * this.textList[i].xAcceleration;
							break;
						case UIEnum_Hud_UDriectionType.LeftDown:
							text.yOffset -= (((Time.deltaTime * this.FloatingSpeed) * mov) * 0.25f) * this.textList[i].yAcceleration;
							text.xOffset -= (((Time.deltaTime * this.FloatingSpeed) * mov) * 0.25f) * this.textList[i].xAcceleration;
							break;
						}

						//Get center up of target
						Vector3 position = this.textList[i].pos;
						Vector3 front = position - this._camera.transform.position;
						//its in camera view
						if ((front.magnitude <= this.HideDistance) && (Vector3.Angle(this._camera.transform.forward, position - this._camera.transform.position) <= this.MaxViewAngle))
						{
							//Convert position to view port
							Vector2 v = this._camera.WorldToViewportPoint(position);

							this.textList[i].rect.anchorMax = v;
							this.textList[i].rect.anchorMin = v;
						}
					}
				}
			//}
		}

		void Update () {
			DrawHud();
		}
	}
}