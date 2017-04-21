using UnityEngine;
using System.Collections;

public class HP : MonoBehaviour {

	public bl_HUDText HUDRoot;

	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}

	public void HPText(){
		HUDRoot.NewText("- " + Random.Range(50, 100).ToString(), base.transform, Color.red, 8, 20f, -1f, 2.2f, bl_Guidance.Up);
	}
}
