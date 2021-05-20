using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace LowoUN.Module.UI
{
	public class UIGraphicCollider : MaskableGraphic
	{

	    [SerializeField]
	    public override Texture mainTexture
	    {
	        get
	        {
	            return s_WhiteTexture;
	        }
	    }
	        
	    public override void SetNativeSize()
	    {

	    }

	    protected override void OnPopulateMesh(VertexHelper vh)
	    {
	        vh.Clear();

	        var r = GetPixelAdjustedRect();
	        var v = new Vector4(r.x, r.y, r.x + r.width, r.y + r.height);

	        var color32 = color;
	        vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(0, 0));
	        vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(0, 1));
	        vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(1, 1));
	        vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(1, 0));

	        //vh.AddTriangle(0, 1, 2);
	        //vh.AddTriangle(2, 3, 0);
	        vh.AddTriangle(0, 0, 0);
	        vh.AddTriangle(0, 0, 0);
	    }
	}
}