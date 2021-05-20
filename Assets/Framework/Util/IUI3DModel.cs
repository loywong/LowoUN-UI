using UnityEngine;

namespace LowoUN.Module.UI
{
	public interface IUI3DModel
	{
		//void UpdateModel<T> (T chara);
		//void UpdateModel(int charaClassTypID);
		GameObject CreateModel<T> (T chara, RenderTexture rtex);
		GameObject CreateModelByID (int charaid, RenderTexture rtex);
		//void RotateActor(float deltaX, bool dragging = true);
		//void PlayAnimation(string animName);
		void Release();
		void OnEnd();
		void SetActive(bool isActive);

		//TEMP
		void TriggerAnimation();
		void RotateActor (float r, bool dragging = true);
	}
}