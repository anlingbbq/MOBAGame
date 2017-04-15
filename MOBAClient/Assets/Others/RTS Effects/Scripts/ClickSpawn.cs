using UnityEngine;
using System.Collections;

public class ClickSpawn : MonoBehaviour {

	public GameObject Effect;
	
	void Start () {

	}

	void Update () {
		if (Input.GetButtonDown ("Fire1")) 
		{
			StartCoroutine("Spawner");
		}
	}

	IEnumerator Spawner()
	{
		var mousePos = Input.mousePosition;
//		mousePos.y = 0.25f;
		mousePos.z = 5f;
		var objectPos = Camera.main.ScreenToWorldPoint(mousePos);

		Instantiate(Effect, objectPos, Quaternion.identity);
		yield return new WaitForSeconds(1);
		//Destroy (Effect);
	}
}