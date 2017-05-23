using UnityEngine;
using System.Collections;

public class EnemyAniTest : MonoBehaviour {
	private Animator myAni;
	// Use this for initialization
	void Start () {
		myAni = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo myAniinfor = myAni.GetCurrentAnimatorStateInfo (0);

		if (!Input.anyKey) {
			myAni.SetInteger("num",0);
		}
		if (Input.GetMouseButtonDown (0)) {
			myAni.SetInteger("num",1);
		}
	}
}
