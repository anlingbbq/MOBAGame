using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexTile : MonoBehaviour {

	public enum HexTileState{
		Default, 
		Selected, 
		Walkable, 
		Hostile, 
		Range
	};

	public HexTileState state = HexTileState.Default;
	private HexNavMapManager navMapManager;
	// 是什么占用这个位置，可能是英雄，可能是敌人，可能是阻挡物，等等
	public GameObject unit = null;
	public int weightValue = 0;
	public int weightValueOne = 0;
	public int weightValueTwo = 0;
	public int weightValueThree = 0;
	private Weight heroWeight;


	// Use this for initialization
	public void Start () {
		navMapManager = HexNavMapManager.GetInstance ();
		GameObject weight = GameObject.Find ("HexNavMap");
		heroWeight = weight.GetComponent<Weight> ();
		StartCoroutine (SetAllValue ());//给每个网格的WeightValue赋值
	}
	//给每个网格的WeightValue赋值
	IEnumerator SetAllValue ()
	{
		yield return new WaitForSeconds (0);
		for(int i=2;i<navMapManager.person.Count;i++)
		{
			if(unit!=null)
			{
				if(unit.Equals(navMapManager.person[2]))
				{
					heroWeight.AddWeightRange (this.gameObject);
					heroWeight.SetWeightValueOne ();
				}
				if(unit.Equals(navMapManager.person[3]))
				{
					heroWeight.AddWeightRange (this.gameObject);
					heroWeight.SetWeightValueTwo ();
				}
				if(unit.Equals(navMapManager.person[4]))
				{
					heroWeight.AddWeightRange (this.gameObject);
					heroWeight.SetWeightValueThree ();
				}
			}
		}
	}
	void SetWeightValue()
	{
		if (navMapManager.selectedTileObject != null) {
			HexTile heroHex = navMapManager.selectedTileObject.GetComponent<HexTile> ();
			if(heroHex.unit!=null)
			{
				if(heroHex.unit.Equals(navMapManager.person[2]))
				{
					weightValue=weightValueOne*3+weightValueTwo+weightValueThree;
				}
				if(heroHex.unit.Equals(navMapManager.person[3]))
				{
					weightValue=weightValueOne+weightValueTwo*3+weightValueThree;
				}
				if(heroHex.unit.Equals(navMapManager.person[4]))
				{
					weightValue=weightValueOne+weightValueTwo+weightValueThree*3;
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
		SetWeightValue ();
	}

	public void Select()
	{ 
		SetState(HexTileState.Selected); 
	}
	public void Idle()
	{
		SetState (HexTileState.Default);
	}
	
	public void SetState(HexTileState tileState){
		state=tileState;

//		
//		if(!walkable){
//			GetComponent<Renderer>().material=GridManager.GetMatUnwalkable();
//			gameObject.SetActive(false);
//			return;
//		}
//		else{
//			gameObject.SetActive(true);
//		}
		
		if(state==HexTileState.Default) GetComponent<Renderer>().material=HexNavMapManager.GetMatNormal();
		else if(state==HexTileState.Selected) GetComponent<Renderer>().material=HexNavMapManager.GetMatSelected();
		else if(state==HexTileState.Walkable) GetComponent<Renderer>().material=HexNavMapManager.GetMatWalkable();
		else if(state==HexTileState.Hostile) GetComponent<Renderer>().material=HexNavMapManager.GetMatHostile();
		else if(state==HexTileState.Range) GetComponent<Renderer>().material=HexNavMapManager.GetMatRange();
		
		//if(Application.isPlaying){
		//if(state==_TileState.Default) renderer.enabled=false;
		//else renderer.enabled=true;
		//renderer.enabled=true;
		//}
	}

	public void SetMaterial(Material mat){
		GetComponent<Renderer>().material=mat;
		GetComponent<Renderer>().enabled=true;
	}
}
