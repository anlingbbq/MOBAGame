using UnityEngine;
using System.Collections;

public class PlayMp4 : MonoBehaviour {

	// Use this for initialization
	void Start () {

		#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_BLACKBERRY
		//Handheld.PlayFullScreenMovie("CG_TLBB.mp4",Color.black, FullScreenMovieControlMode.Hidden);
		Handheld.PlayFullScreenMovie("logo.mp4",Color.black, FullScreenMovieControlMode.CancelOnInput);
		//iPhoneUtils.PlayMovie("logo.mp4", Color.black, iPhoneMovieControlMode.Hidden,iPhoneMovieScalingMode.AspectFit);
		
		Application.LoadLevelAsync(1);
		#endif

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		//if (GUI.Button (new Rect (20, 10, 200, 50), "PLAY ControlMode.CancelOnTouch")) {
		//	Handheld.PlayFullScreenMovie ("logo.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
		//}
	}
}
