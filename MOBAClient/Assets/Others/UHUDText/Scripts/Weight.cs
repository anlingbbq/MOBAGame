using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weight : MonoBehaviour {
	public List<int> theGroupOne;
	public List<int> theGroupTwo;
	public List<int> theGroupThree;
	public List<int> theGroupFour;
	public List<int> theGroupFive;
	
	private HexNavMapManager navMapManager;
	private HexTile hero;
	// Use this for initialization
	void Start () {
		navMapManager = HexNavMapManager.GetInstance ();
	}
	
	// Update is called once per frame
	void Update () {
//		if (navMapManager.selectedTileObject != null) {
//			hero=navMapManager.selectedTileObject.GetComponent<HexTile>();
//			if(hero != null)
//			{
//				print (hero.unit);
//				AddWeightRange (navMapManager.selectedTileObject);
//				SetWeightValue ();
//			}
//			else{
//				print ("1");
//			}
//		}
	}
	public void AddWeightRange(GameObject theTile)//参数是网格
	{
		int theIndex = int.Parse (theTile.name);
		int columnNum = theIndex / 10;//十位
		int rowNum = theIndex % 10;//个位

		theGroupOne.Add ((columnNum)*10+(rowNum+1));
		theGroupOne.Add ((columnNum-1)*10+(rowNum+1));
		theGroupOne.Add ((columnNum+1)*10+(rowNum+1));
		theGroupOne.Add ((columnNum-1)*10+rowNum);
		theGroupOne.Add ((columnNum+1)*10+rowNum);
		theGroupOne.Add ((columnNum)*10+(rowNum-1));


		theGroupTwo.Add ((columnNum) * 10 + (rowNum+2));
		theGroupTwo.Add ((columnNum-1) * 10 + (rowNum+2));
		theGroupTwo.Add ((columnNum+1) * 10 + (rowNum+2));
		theGroupTwo.Add ((columnNum+2) * 10 + (rowNum+1));
		theGroupTwo.Add ((columnNum-2) * 10 + (rowNum+1));
		theGroupTwo.Add ((columnNum-2) * 10 + (rowNum));
		theGroupTwo.Add ((columnNum+2) * 10 + (rowNum));
		theGroupTwo.Add ((columnNum-2) * 10 + (rowNum-1));
		theGroupTwo.Add ((columnNum+2) * 10 + (rowNum-1));
		theGroupTwo.Add ((columnNum+1) * 10 + (rowNum-1));
		theGroupTwo.Add ((columnNum-1) * 10 + (rowNum-1));
		theGroupTwo.Add ((columnNum) * 10 + (rowNum-2));

		theGroupThree.Add ((columnNum)*10 + (rowNum+3));
		theGroupThree.Add ((columnNum-1)*10+(rowNum+3));
		theGroupThree.Add ((columnNum+1)*10+(rowNum+3));
		theGroupThree.Add ((columnNum+2)*10+(rowNum+2));
		theGroupThree.Add ((columnNum-2)*10+(rowNum+2));
		theGroupThree.Add ((columnNum+3)*10+(rowNum+2));
		theGroupThree.Add ((columnNum-3)*10+(rowNum+2));
		theGroupThree.Add ((columnNum+3)*10+(rowNum+1));
		theGroupThree.Add ((columnNum-3)*10+(rowNum+1));
		theGroupThree.Add ((columnNum+3)*10+(rowNum));
		theGroupThree.Add ((columnNum-3)*10+(rowNum));
		theGroupThree.Add ((columnNum+3)*10+(rowNum-1));
		theGroupThree.Add ((columnNum-3)*10+(rowNum-1));
		theGroupThree.Add ((columnNum+2)*10+(rowNum-2));
		theGroupThree.Add ((columnNum-2)*10+(rowNum-2));
		theGroupThree.Add ((columnNum-1)*10+(rowNum-2));
		theGroupThree.Add ((columnNum+1)*10+(rowNum-2));
		theGroupThree.Add ((columnNum)*10 + (rowNum-3));

		theGroupFour.Add ((columnNum)*10 + (rowNum+4));
		theGroupFour.Add ((columnNum-1)*10+(rowNum+4));
		theGroupFour.Add ((columnNum+1)*10+(rowNum+4));
		theGroupFour.Add ((columnNum+2)*10+(rowNum+3));
		theGroupFour.Add ((columnNum-2)*10+(rowNum+3));
		theGroupFour.Add ((columnNum+3)*10+(rowNum+3));
		theGroupFour.Add ((columnNum-3)*10+(rowNum+3));
		theGroupFour.Add ((columnNum+4)*10+(rowNum+2));
		theGroupFour.Add ((columnNum-4)*10+(rowNum+2));
		theGroupFour.Add ((columnNum+4)*10+(rowNum+1));
		theGroupFour.Add ((columnNum-4)*10+(rowNum+1));
		theGroupFour.Add ((columnNum+4)*10+(rowNum));
		theGroupFour.Add ((columnNum-4)*10+(rowNum));
		theGroupFour.Add ((columnNum+4)*10+(rowNum-1));
		theGroupFour.Add ((columnNum-4)*10+(rowNum-1));
		theGroupFour.Add ((columnNum+4)*10+(rowNum-2));
		theGroupFour.Add ((columnNum-4)*10+(rowNum-2));
		theGroupFour.Add ((columnNum+3)*10+(rowNum-2));
		theGroupFour.Add ((columnNum-3)*10+(rowNum-2));
		theGroupFour.Add ((columnNum+2)*10+(rowNum-3));
		theGroupFour.Add ((columnNum-2)*10+(rowNum-3));
		theGroupFour.Add ((columnNum-1)*10+(rowNum-3));
		theGroupFour.Add ((columnNum+1)*10+(rowNum-3));
		theGroupFour.Add ((columnNum)*10 + (rowNum-4));

		theGroupFive.Add ((columnNum)*10 + (rowNum+5));
		theGroupFive.Add ((columnNum+1)*10 + (rowNum+5));
		theGroupFive.Add ((columnNum-1)*10 + (rowNum+5));
		theGroupFive.Add ((columnNum+2)*10 + (rowNum+4));
		theGroupFive.Add ((columnNum-2)*10 + (rowNum+4));
		theGroupFive.Add ((columnNum+3)*10 + (rowNum+4));
		theGroupFive.Add ((columnNum-3)*10 + (rowNum+4));
		theGroupFive.Add ((columnNum+4)*10 + (rowNum+3));
		theGroupFive.Add ((columnNum-4)*10 + (rowNum+3));
		theGroupFive.Add ((columnNum+5)*10 + (rowNum+3));
		theGroupFive.Add ((columnNum-5)*10 + (rowNum+3));
		theGroupFive.Add ((columnNum+5)*10 + (rowNum+2));
		theGroupFive.Add ((columnNum-5)*10 + (rowNum+2));
		theGroupFive.Add ((columnNum+5)*10 + (rowNum+1));
		theGroupFive.Add ((columnNum-5)*10 + (rowNum+1));
		theGroupFive.Add ((columnNum+5)*10 + (rowNum));
		theGroupFive.Add ((columnNum-5)*10 + (rowNum));
		theGroupFive.Add ((columnNum+5)*10 + (rowNum-1));
		theGroupFive.Add ((columnNum-5)*10 + (rowNum-1));
		theGroupFive.Add ((columnNum+5)*10 + (rowNum-2));
		theGroupFive.Add ((columnNum-5)*10 + (rowNum-2));
		theGroupFive.Add ((columnNum+4)*10 + (rowNum-3));
		theGroupFive.Add ((columnNum-4)*10 + (rowNum-3));
		theGroupFive.Add ((columnNum+3)*10 + (rowNum-3));
		theGroupFive.Add ((columnNum-3)*10 + (rowNum-3));
		theGroupFive.Add ((columnNum+2)*10 + (rowNum-4));
		theGroupFive.Add ((columnNum-2)*10 + (rowNum-4));
		theGroupFive.Add ((columnNum+1)*10 + (rowNum-4));
		theGroupFive.Add ((columnNum-1)*10 + (rowNum-4));
		theGroupFive.Add ((columnNum)*10 + (rowNum-5));


	}
	//以第一个英雄为中心赋值
	public void SetWeightValueOne()
	{
		foreach (int i in theGroupOne) {
			if(i>-1&&i<70)
			{
				GameObject a = GameObject.Find (i.ToString());
				HexTile b = a.GetComponent<HexTile> ();
				b.weightValueOne = 20;
			}
		}
		foreach (int i in theGroupTwo) {
			if(i>-1&&i<70)
			{
				GameObject a = GameObject.Find (i.ToString());
				HexTile b = a.GetComponent<HexTile> ();
				b.weightValueOne = 10;
			}
		}
		foreach (int i in theGroupThree) {
			if(i>-1&&i<70)
			{
				GameObject a = GameObject.Find (i.ToString());
				HexTile b = a.GetComponent<HexTile> ();
				b.weightValueOne = 5;
			}

		}
		foreach (int i in theGroupFour) {
			if(i>-1&&i<70)
			{
				GameObject a = GameObject.Find (i.ToString());
				HexTile b = a.GetComponent<HexTile> ();
				b.weightValueOne = 2;
			}
		}
		foreach (int i in theGroupFive) {
			if(i>-1&&i<70)
			{
				GameObject a = GameObject.Find (i.ToString());
				HexTile b = a.GetComponent<HexTile> ();
				b.weightValueOne = 1;
			}
		}
		theGroupOne.Clear ();
		theGroupTwo.Clear ();
		theGroupThree.Clear ();
		theGroupFour.Clear ();
		theGroupFive.Clear ();
	}
	//以第二个英雄为中心赋值
	public void SetWeightValueTwo()
	{
		foreach (int i in theGroupOne) {
			if(i>-1&&i<70)
			{
				GameObject a = GameObject.Find (i.ToString());
				HexTile b = a.GetComponent<HexTile> ();
				b.weightValueTwo = 20;
			}
		}
		foreach (int i in theGroupTwo) {
			if(i>-1&&i<70)
			{
				GameObject a = GameObject.Find (i.ToString());
				HexTile b = a.GetComponent<HexTile> ();
				b.weightValueTwo = 10;
			}
		}
		foreach (int i in theGroupThree) {
			if(i>-1&&i<70)
			{
				GameObject a = GameObject.Find (i.ToString());
				HexTile b = a.GetComponent<HexTile> ();
				b.weightValueTwo = 5;
			}
			
		}
		foreach (int i in theGroupFour) {
			if(i>-1&&i<70)
			{
				GameObject a = GameObject.Find (i.ToString());
				HexTile b = a.GetComponent<HexTile> ();
				b.weightValueTwo = 2;
			}
		}
		foreach (int i in theGroupFive) {
			if(i>-1&&i<70)
			{
				GameObject a = GameObject.Find (i.ToString());
				HexTile b = a.GetComponent<HexTile> ();
				b.weightValueTwo = 1;
			}
		}
		theGroupOne.Clear ();
		theGroupTwo.Clear ();
		theGroupThree.Clear ();
		theGroupFour.Clear ();
		theGroupFive.Clear ();
		
	}
	//以第三个英雄为中心赋值
	public void SetWeightValueThree()
	{
		foreach (int i in theGroupOne) {
			if(i>-1&&i<70)
			{
				GameObject a = GameObject.Find (i.ToString());
				HexTile b = a.GetComponent<HexTile> ();
				b.weightValueThree = 20;
			}
		}
		foreach (int i in theGroupTwo) {
			if(i>-1&&i<70)
			{
				GameObject a = GameObject.Find (i.ToString());
				HexTile b = a.GetComponent<HexTile> ();
				b.weightValueThree = 10;
			}
		}
		foreach (int i in theGroupThree) {
			if(i>-1&&i<70)
			{
				GameObject a = GameObject.Find (i.ToString());
				HexTile b = a.GetComponent<HexTile> ();
				b.weightValueThree = 5;
			}
			
		}
		foreach (int i in theGroupFour) {
			if(i>-1&&i<70)
			{
				GameObject a = GameObject.Find (i.ToString());
				HexTile b = a.GetComponent<HexTile> ();
				b.weightValueThree = 2;
			}
		}
		foreach (int i in theGroupFive) {
			if(i>-1&&i<70)
			{
				GameObject a = GameObject.Find (i.ToString());
				HexTile b = a.GetComponent<HexTile> ();
				b.weightValueThree = 1;
			}
		}
		theGroupOne.Clear ();
		theGroupTwo.Clear ();
		theGroupThree.Clear ();
		theGroupFour.Clear ();
		theGroupFive.Clear ();

		
	}
	void GroupOneCSRandom()
	{

	}
	void GroupTwoCSRandom()
	{
		
	}
	void GroupThreeCSRandom()
	{
		
	}
	void GroupFourCSRandom()
	{
		
	}
	void GroupFiveCSRandom()
	{
		
	}
}
