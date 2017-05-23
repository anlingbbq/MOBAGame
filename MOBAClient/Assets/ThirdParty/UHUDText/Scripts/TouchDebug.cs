using UnityEngine;
using System.Collections;

public class TouchDebug : MonoBehaviour {

	public string groundLayerName = "NavMapLayer";

	private Vector3 cursorPosition;
	private Vector3 hitPoint;
	private HexNavMapManager navMapManager;

	private string touchType  = "";
	// Use this for initialization
	void Start () {
		navMapManager = HexNavMapManager.GetInstance ();
	
	}

	void OnGUI () 
	{
		GUI.Label(new Rect(10, 25, 800, 20), "mapIndex("+navMapManager.indexSelectedTile+")");//显示选中的块的标记

		float x = hitPoint.x;
		float y = hitPoint.y;
		float z = hitPoint.z;
		GUI.Label(new Rect(10, 45, 800, 20), "("+x+","+y+","+z+")");

		GUI.Label(new Rect(10, 65, 800, 20), touchType);
	}
	
	// Update is called once per frame
	void Update () {

		#if UNITY_IOS || UNITY_ANDROID
		if(Input.touchCount==1){
			Touch touch=Input.touches[0];

			if(touch.phase == TouchPhase.Began){
				cursorPosition=touch.position;
			}
		}

		if(Input.touchCount==1){
			Touch touch=Input.touches[0];
			
			if(touch.phase == TouchPhase.Began){
				touchType = "TouchPhase.Began    touch.phase = "+touch.phase;
			}
			
			if(touch.phase == TouchPhase.Moved){
				touchType = "TouchPhase.Moved    touch.phase = "+touch.phase;
			}
			
			if(touch.phase == TouchPhase.Ended){
				touchType = "TouchPhase.Ended    touch.phase = "+touch.phase;
			}
		}
		#endif

		#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if(Input.GetMouseButton(0))
		{
			cursorPosition=Input.mousePosition;
		}
		#endif


		LayerMask mask = LayerNameToIntMask (groundLayerName);
		Ray ray = Camera.main.ScreenPointToRay (cursorPosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, mask)) {
			hitPoint = hit.point;
		}
	
	}

	static int LayerNameToIntMask(string layerName)
	{
		int layer = LayerMask.NameToLayer(layerName);
		if(layer == 0)
			return int.MaxValue;
		
		return 1 << layer;
	}
}
