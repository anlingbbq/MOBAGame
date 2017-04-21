using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexNavMapManager : MonoBehaviour {

	public GameObject[] hexTileList;

	private static HexNavMapManager instance;


	public static void SetInstance()
	{ 
		if(instance==null) instance=(HexNavMapManager)FindObjectOfType(typeof(HexNavMapManager)); 
	}
	public static HexNavMapManager GetInstance()
	{ 
		return instance; 
	}

	private string groundLayerName = "NavMapLayer";
	private Vector3 cursorPosition;//光标的位置
	private Vector3 hitPoint;
	public GameObject selectedTileObject = null;//选中的格子，包括划线途中选中的
	public int indexSelectedTile;

	//material for each individual tile
	public Material hexMatNormal;
	public Material hexMatSelected;
	public Material hexMatWalkable;
	public Material hexMatUnwalkable;
	public Material hexMatHostile;
	public Material hexMatRange;
	public Material hexMatAbilityAll;
	public Material hexMatAbilityHostile;
	public Material hexMatAbilityFriendly;
	public Material hexMatInvisible;
	
	public static Material GetMatNormal(){return instance.hexMatNormal;}//默认透明块
	public static Material GetMatSelected(){return instance.hexMatSelected;}//选中状态
	public static Material GetMatWalkable(){return instance.hexMatWalkable;}//可以行走
	public static Material GetMatUnwalkable(){return instance.hexMatUnwalkable;}//不能行走
	public static Material GetMatHostile(){return instance.hexMatHostile;}//敌方的
	public static Material GetMatRange(){return instance.hexMatRange;}//范围
	public static Material GetMatAbilityAll(){return instance.hexMatAbilityAll;}
	public static Material GetMatAbilityHostile(){return instance.hexMatAbilityHostile;}
	public static Material GetMatAbilityFriendly(){return instance.hexMatAbilityFriendly;}
	public static Material GetMatInvisible(){return instance.hexMatInvisible;}

	public GameObject linePerfab;
	public ArrayList moveRangeList   = new ArrayList();
	public ArrayList attackRangeList = new ArrayList();
	public List<GameObject> person = new List<GameObject>();
	public List<GameObject> props = new List<GameObject>();
	public List<GameObject> effectiveTileObject=new List<GameObject>();//有效节点数组
	
	void Awake(){
		if(instance==null) instance=this;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	// 获取击中的网格
	public GameObject SelectTileByScreenPointToRay(Vector3 screenPoint, bool canAttack){
		cursorPosition = screenPoint;
		LayerMask mask = LayerNameToIntMask (groundLayerName);
		Ray ray = Camera.main.ScreenPointToRay (cursorPosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, mask)) {
			
			GameObject hexTilObject = hit.transform.gameObject;//射线击中的目标

			HexTile selectedTile = hexTilObject.GetComponent<HexTile>();
			if (selectedTile.unit != null) {

				Enemy enemy = selectedTile.unit.GetComponent<Enemy>();

				if(canAttack&&enemy!=null){
					return hexTilObject;
				}
			}

			OnTileSelected(hexTilObject);//选中的网格

			return hexTilObject;
		}
		return null;
	}

	// 如果是个有效的节点，则返回这个的节点
	public GameObject SelectMovePathHexTilesByScreenPointToRay(Vector3 screenPoint, GameObject currentHexTileObject)
	{
		//有效节点数组
		effectiveTileObject.Add (selectedTileObject);
		//有效节点选中状态
		HexTile hex=selectedTileObject.GetComponent<HexTile>();
		hex.Select();
		//Debug.Log ("HexNavMapManager::SelectMovePathHexTilesByScreenPointToRay");
		if (currentHexTileObject == null)
			return null;

		cursorPosition = screenPoint;
		LayerMask mask = LayerNameToIntMask (groundLayerName);
		Ray ray = Camera.main.ScreenPointToRay (cursorPosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, mask)) 
		{
			
			GameObject nextHexTileObject = hit.transform.gameObject;
			HexTile hextile=nextHexTileObject.GetComponent<HexTile>();

			if(MovePathHexTilesValidCheck(currentHexTileObject, nextHexTileObject))
			{
				//判断节点是否有效(若有人在此节点，视为无效)
				foreach(GameObject people in person)
				{
					if(hextile.unit == people)
					{
						if(nextHexTileObject.Equals(effectiveTileObject[0]))
						{
							return nextHexTileObject;
						}else{
							return null;
						}
					}
					if(hextile.unit == null)
					{
						if(Vector3.Distance(hit.point, nextHexTileObject.transform.position)<0.4f)
						{
							return nextHexTileObject;
						}
						
					}
				}

				foreach(GameObject propcopy in props)
				{
					if(hextile.unit == propcopy)
					{
						if(Vector3.Distance(hit.point, nextHexTileObject.transform.position)<0.4f)
						{
							return nextHexTileObject;
						}
					}
				}

			}

		}

		return null;

	}

	//选中有效
	bool MovePathHexTilesValidCheck(GameObject currentHexTileObject, GameObject nextHexTileObject){
		int currentIndex = int.Parse (currentHexTileObject.name);
		int nextIndex = int.Parse (nextHexTileObject.name);
		int columnNum = currentIndex / 10;
		//Debug.Log("currentIndex:"+currentIndex+"    nextIndex:"+nextIndex);
		if (columnNum == 0) {
			//向上，向下
			if(nextIndex == currentIndex+1){
				return true;
			}
			if(nextIndex == currentIndex-1){
				return true;
			}
			
			//右斜上，右斜下
			if(nextIndex == currentIndex+10+1){
				return true;
			}
			if(nextIndex == currentIndex+10){
				return true;
			}
			
		} else if (columnNum == 6) {
			//向上，向下
			if(nextIndex == currentIndex+1){
				return true;
			}
			if(nextIndex == currentIndex-1){
				return true;
			}
			//左斜上，左斜下
			if(nextIndex == currentIndex-10+1){
				return true;
			}
			if(nextIndex == currentIndex-10){
				return true;
			}
			
		} else if (columnNum == 2 || columnNum == 4) {
			//向上，向下
			if(nextIndex == currentIndex+1){
				return true;
			}
			if(nextIndex == currentIndex-1){
				return true;
			}
			
			//右斜上，右斜下
			if(nextIndex == currentIndex+10+1){
				return true;
			}
			if(nextIndex == currentIndex+10){
				return true;
			}
			
			//左斜上，左斜下
			if(nextIndex == currentIndex-10+1){
				return true;
			}
			if(nextIndex == currentIndex-10){
				return true;
			}
			
			
		}else if(columnNum == 1 || columnNum == 3 ||columnNum == 5){
			//向上，向下
			if(nextIndex == currentIndex+1){
				return true;
			}
			if(nextIndex == currentIndex-1){
				return true;
			}
			
			//右斜上，右斜下
			if(nextIndex == currentIndex+10){
				return true;
			}
			if(nextIndex == currentIndex+10-1){
				return true;
			}
			
			//左斜上，左斜下
			if(nextIndex == currentIndex-10){
				return true;
			}
			if(nextIndex == currentIndex-10-1){
				return true;
			}
			
		}

		return false;
	} 

	//清除移动范围
	public void ClearMoveRange(){
		foreach (string indexStr in moveRangeList ) {
			GameObject rangeTileObject = hexTileList[int.Parse(indexStr)] as GameObject;
			HexTile rangeTile = rangeTileObject.GetComponent<HexTile>();
			rangeTile.SetState(HexTile.HexTileState.Default);
		}
		moveRangeList.Clear();

	}
	//清除攻击范围
	public void ClearAttackRange(){
		
		foreach (string indexStr in attackRangeList ) {
			GameObject rangeTileObject = hexTileList[int.Parse(indexStr)] as GameObject;
			HexTile rangeTile = rangeTileObject.GetComponent<HexTile>();
			rangeTile.SetState(HexTile.HexTileState.Default);
		}
		attackRangeList.Clear ();
	}
	//显示移动范围
	public void ShowMoveRangeInMoving(GameObject hexTilObject, int step){
		ClearMoveRange();

		selectedTileObject = hexTilObject;
		indexSelectedTile = int.Parse(hexTilObject.name);

//		AddMoveRangeList (indexSelectedTile);
//		ShowMoveRange (step);

		// 根据英雄的活动能力显示活动范围
		ShowMoveRange (indexSelectedTile, step);

	}
	//清除英雄的选中状态
	public void ClearEarlyState(GameObject a)
	{
		HeroInformation.heroHex.Idle ();

	}
	// 当格子被用户选中,如果有战斗单位，还要显示战斗单位的活动范围
	void OnTileSelected(GameObject hexTilObject){
		//Debug.Log ("******************* HexNavMapManager::OnTileSelected *******************");

		// 如果重复选择，则不作处理
		//if (hexTilObject.Equals (selectedTileObject))
		//	return;

		// 清除前一个格子的效果
		ClearMoveRange();
		ClearAttackRange ();
		ClearEarlyState (hexTilObject);

		// 解析当前选中的格子，获得格子的位置，和格子中的游戏单元（英雄，敌人，物品什么的）
		HexTile selectedTile = hexTilObject.GetComponent<HexTile>();
		selectedTileObject = hexTilObject;
		indexSelectedTile = int.Parse(hexTilObject.name);


		// 如果选中的格子中有Hero，则显示Hero的移动范围(MoveRangeList)
		if (selectedTile.unit != null) {
			Hero hero = selectedTile.unit.GetComponent<Hero>();

//			Prop prop = selectedTile.unit.GetComponent<Prop>();

			if(hero){
				//AddMoveRangeList (indexSelectedTile);
				//ShowMoveRange(hero.step);

				// 根据英雄的活动能力显示活动范围
				ShowMoveRange (indexSelectedTile, hero.step);
			}
//			else if(prop){
//				selectedTile.unit.SetActive(false);
//			}

		} else {
			AddMoveRangeList (indexSelectedTile);
			selectedTile.SetState(HexTile.HexTileState.Selected);
		}
	}

	#region 计算活动范围的方法

	// 显示活动范围
	void ShowMoveRange(int TileNum, int step){

		//Debug.Log ("ShowMoveRange Start:"+ System.DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + "##############################################");

		List<string> neighbourList = GetNeighbourTlieList (TileNum);
		
		List<string> closeList   = new List<string> ();
		List<string> openList    = new List<string> ();
		List<string> newOpenList = new List<string> ();

		for(int m=0; m<neighbourList.Count; m++){
			string neighbour=neighbourList[m];
			if(!newOpenList.Contains(neighbour)) newOpenList.Add(neighbour);
		}
		
		
		for (int i=0; i < step; i++) 
		{
			//Debug.Log ("ShowMoveRange Start step = " + (step-i));
			openList = newOpenList;
			//Debug.Log ("========================openList.count = " + openList.Count);
			newOpenList = new List<string> ();

			for(int n=0; n<openList.Count; n++)
			{

				neighbourList=GetNeighbourTlieList(int.Parse(openList[n]));
				for(int m=0; m<neighbourList.Count; m++)
				{
					string neighbour=neighbourList[m];
					if(!closeList.Contains(neighbour) && !openList.Contains(neighbour) && !newOpenList.Contains(neighbour))
					{
						newOpenList.Add(neighbour);
						//Debug.Log ("*********************************newOpenList Add: " + neighbour);
					}
				}
			}

			for(int n=0; n<openList.Count; n++)
			{
				string tile=openList[n];
				if(!tile.Equals(TileNum.ToString()) && !closeList.Contains(tile))
				{
					closeList.Add(tile);
					//Debug.Log ("++++++++++++++++++++++++++++++++++++++vcloseList Add: " + tile);
				}
			}
		}

		// 将原点加入列表中
		closeList.Add (TileNum.ToString());
		moveRangeList = new ArrayList(closeList);
		
		// 最后修改显示范围的状态
		foreach (string indexStr in moveRangeList) {
			//Debug.Log("Can Move Range Index = "+indexStr);
			GameObject rangeTileObject = hexTileList[int.Parse(indexStr)] as GameObject;
			HexTile rangeTile = rangeTileObject.GetComponent<HexTile>();
			rangeTile.SetState(HexTile.HexTileState.Range);
		}


		//Debug.Log ("ShowMoveRange End:"+System.DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + "##############################################");
	}

//	// 显示活动范围
//	void ShowMoveRange(int step){
//		Debug.Log ("++ShowMoveRange Start:"+ System.DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + "##############################################");
//
//
//		for (int i=0; i < step; i++) {
//			Debug.Log ("ShowMoveRange Start step = " + (step-i));
//			
//			ArrayList temp = new ArrayList();
//			
//			for(int j=0;j< moveRangeList.Count;j++){
//				string str = moveRangeList[j].ToString();
//				temp.Add(str);
//			}
//			
//			foreach(string indexStr in temp){
//				GetMoveRange(int.Parse(indexStr));
//			}
//		}
//		
//		// 最后修改显示范围的状态
//		foreach (string indexStr in moveRangeList ) {
//			Debug.Log("Can Move Range Index = "+indexStr);
//			GameObject rangeTileObject = hexTileList[int.Parse(indexStr)] as GameObject;
//			HexTile rangeTile = rangeTileObject.GetComponent<HexTile>();
//			rangeTile.SetState(HexTile.HexTileState.Range);
//		}
//		
//		Debug.Log ("++ShowMoveRange End:"+System.DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + "##############################################");
//	}
#endregion

	private List<string> GetNeighbourTlieList(int indexCenter){
		//Debug.Log ("GetNeighbourTlieList:"+indexCenter);
		int columnNum = indexCenter / 10;
		//Debug.Log ("GetNeighbourTlieList columnNum:"+indexCenter / 10);

		List<string> neighbourTlieList = new List<string>();
		
		if (columnNum == 0) {
			//向上，向下
			int up = indexCenter+1;
			if(up/10==0){
				AddNeighbourTlieList(up,neighbourTlieList);
			}
			int down = indexCenter-1;
			if(down/10==0){
				AddNeighbourTlieList(down,neighbourTlieList);
			}
			
			//右斜上，右斜下
			int upR = indexCenter+10+1;
			if(upR/10==1){
				AddNeighbourTlieList(upR,neighbourTlieList);
			}
			int downR = indexCenter+10;
			if(downR/10==1){
				AddNeighbourTlieList(downR,neighbourTlieList);
			}
			
		} else if (columnNum == 6) {
			//向上，向下
			int up = indexCenter+1;
			if(up/10==6){
				AddNeighbourTlieList(up,neighbourTlieList);
			}
			int down = indexCenter-1;
			if(down/10==6){
				AddNeighbourTlieList(down,neighbourTlieList);
			}
			//左斜上，左斜下
			int upL = indexCenter-10+1;
			if(upL/10==5){
				AddNeighbourTlieList(upL,neighbourTlieList);
			}
			int downL = indexCenter-10;
			if(downL/10==5){
				AddNeighbourTlieList(downL,neighbourTlieList);
			}
			
		} else if (columnNum == 2 || columnNum == 4) {
			//向上，向下
			int up = indexCenter+1;
			if(up/10==columnNum){
				AddNeighbourTlieList(up,neighbourTlieList);
			}
			int down = indexCenter-1;
			if(down/10==columnNum){
				AddNeighbourTlieList(down,neighbourTlieList);
			}
			
			//右斜上，右斜下
			int upR = indexCenter+10+1;
			if(upR/10==columnNum+1){
				AddNeighbourTlieList(upR,neighbourTlieList);
			}
			int downR = indexCenter+10;
			if(downR/10==columnNum+1){
				AddNeighbourTlieList(downR,neighbourTlieList);
			}
			
			//左斜上，左斜下
			int upL = indexCenter-10+1;
			if(upL/10==columnNum-1){
				AddNeighbourTlieList(upL,neighbourTlieList);
			}
			int downL = indexCenter-10;
			if(downL/10==columnNum-1){
				AddNeighbourTlieList(downL,neighbourTlieList);
			}
			
			
		}else if(columnNum == 1 || columnNum == 3 ||columnNum == 5){
			//向上，向下
			int up = indexCenter+1;
			if(up/10==columnNum){
				AddNeighbourTlieList(up,neighbourTlieList);
			}
			int down = indexCenter-1;
			if(down/10==columnNum){
				AddNeighbourTlieList(down,neighbourTlieList);
			}
			
			//右斜上，右斜下
			int upR = indexCenter+10;
			if(upR/10==columnNum+1){
				AddNeighbourTlieList(upR,neighbourTlieList);
			}
			int downR = indexCenter+10-1;
			if(downR/10==columnNum+1){
				AddNeighbourTlieList(downR,neighbourTlieList);
			}
			
			//左斜上，左斜下
			int upL = indexCenter-10;
			if(upL/10==columnNum-1){
				AddNeighbourTlieList(upL,neighbourTlieList);
			}
			int downL = indexCenter-10-1;
			if(downL/10==columnNum-1){
				AddNeighbourTlieList(downL,neighbourTlieList);
			}
			
		}

		return neighbourTlieList;
	}


	void GetMoveRange(int indexCenter)
	{
		//Debug.Log ("GetMoveRange:"+indexCenter);
		int columnNum = indexCenter / 10;
		//Debug.Log ("GetMoveRange columnNum:"+indexCenter / 10);

		if (columnNum == 0) {
			//向上，向下
			int up = indexCenter+1;
			if(up/10==0){
				AddMoveRangeList(up);
			}
			int down = indexCenter-1;
			if(down/10==0){
				AddMoveRangeList(down);
			}

			//右斜上，右斜下
			int upR = indexCenter+10+1;
			if(upR/10==1){
				AddMoveRangeList(upR);
			}
			int downR = indexCenter+10;
			if(downR/10==1){
				AddMoveRangeList(downR);
			}

		} else if (columnNum == 6) {
			//向上，向下
			int up = indexCenter+1;
			if(up/10==6){
				AddMoveRangeList(up);
			}
			int down = indexCenter-1;
			if(down/10==6){
				AddMoveRangeList(down);
			}
			//左斜上，左斜下
			int upL = indexCenter-10+1;
			if(upL/10==5){
				AddMoveRangeList(upL);
			}
			int downL = indexCenter-10;
			if(downL/10==5){
				AddMoveRangeList(downL);
			}

		} else if (columnNum == 2 || columnNum == 4) {
			//向上，向下
			int up = indexCenter+1;
			if(up/10==columnNum){
				AddMoveRangeList(up);
			}
			int down = indexCenter-1;
			if(down/10==columnNum){
				AddMoveRangeList(down);
			}

			//右斜上，右斜下
			int upR = indexCenter+10+1;
			if(upR/10==columnNum+1){
				AddMoveRangeList(upR);
			}
			int downR = indexCenter+10;
			if(downR/10==columnNum+1){
				AddMoveRangeList(downR);
			}

			//左斜上，左斜下
			int upL = indexCenter-10+1;
			if(upL/10==columnNum-1){
				AddMoveRangeList(upL);
			}
			int downL = indexCenter-10;
			if(downL/10==columnNum-1){
				AddMoveRangeList(downL);
			}


		}else if(columnNum == 1 || columnNum == 3 ||columnNum == 5){
			//向上，向下
			int up = indexCenter+1;
			if(up/10==columnNum){
				AddMoveRangeList(up);
			}
			int down = indexCenter-1;
			if(down/10==columnNum){
				AddMoveRangeList(down);
			}
			
			//右斜上，右斜下
			int upR = indexCenter+10;
			if(upR/10==columnNum+1){
				AddMoveRangeList(upR);
			}
			int downR = indexCenter+10-1;
			if(downR/10==columnNum+1){
				AddMoveRangeList(downR);
			}
			
			//左斜上，左斜下
			int upL = indexCenter-10;
			if(upL/10==columnNum-1){
				AddMoveRangeList(upL);
			}
			int downL = indexCenter-10-1;
			if(downL/10==columnNum-1){
				AddMoveRangeList(downL);
			}

		}
	}

	void AddMoveRangeList(int index){
		//Debug.Log ("**********AddMoveRangeList Index = "+index);
		if (index < 0) return;
		if (index > 69) return;
		if (!moveRangeList.Contains (index.ToString ())) 
		{
			moveRangeList.Add (index.ToString ());
		}

	}
	//相邻的格子列表
	void AddNeighbourTlieList(int index, List<string> neighbourTlieList)
	{
		//Debug.Log ("**********AddNeighbourTlieList Index = "+index);
		if (index < 0) return;
		if (index > 69) return;

		neighbourTlieList.Add (index.ToString ());
	}


#region Draw Line on map
	void DrawMoveline(Vector3 startPoint, Vector3 endPoint){
		GameObject aline = Instantiate (linePerfab);
		
		//通过游戏对象，GetComponent方法 传入LineRenderer
		//就是之前给line游戏对象添加的渲染器属性
		//有了这个对象才可以为游戏世界渲染线段
		LineRenderer lineRenderer = (LineRenderer)aline.GetComponent ("LineRenderer");
		lineRenderer.SetVertexCount(2);
		//pathLines.Add (aline);
		
		lineRenderer.SetPosition (0, startPoint);
		lineRenderer.SetPosition (1, endPoint);
	}
#endregion

	static int LayerNameToIntMask(string layerName)
	{
		int layer = LayerMask.NameToLayer(layerName);
		if(layer == 0)
			return int.MaxValue;
		
		return 1 << layer;
	}


	public void ShowAttackRange(int propType,GameObject hexTileObjectStand){
		// Sword
		if(propType == 1){
			ShowSwordAttackRange(hexTileObjectStand);
		}
		
		// Axe
		if(propType == 2){
			ShowAxeAttackRange(hexTileObjectStand);
		}
		
		// Bow
		if(propType == 3){
			ShowBowAttackRange(hexTileObjectStand);
		}
	}
	//剑的攻击范围
	void ShowSwordAttackRange(GameObject basehexTileObject){
		int baseTileIndex = int.Parse (basehexTileObject.name);
		List<string> swordAttackRangeList = GetNeighbourTlieList (baseTileIndex);
		DrawAttackRange (swordAttackRangeList);
	}
	//斧子的攻击范围
	void ShowAxeAttackRange(GameObject basehexTileObject){
		int baseTile = int.Parse (basehexTileObject.name);

		List<string> axeAttackRangeList = new List<string> ();

		List<string> keyTileList = GetKeyTileListAxeAttackRange (baseTile);
		int keyTileCount = keyTileList.Count;

		for (int i=0; i<keyTileCount; i++) {
			int keyTileIndex = int.Parse(keyTileList[i]);

			List<string> keyTileNeighbourList = GetNeighbourTlieList (keyTileIndex);
			keyTileNeighbourList.Add(keyTileIndex.ToString());

			for(int j=0; j<keyTileNeighbourList.Count; j++){
				if(!axeAttackRangeList.Contains(keyTileNeighbourList[j])){
					axeAttackRangeList.Add(keyTileNeighbourList[j]);
				}
			}
		}//End for (int i=0... 

		axeAttackRangeList.Remove (baseTile.ToString());

		DrawAttackRange (axeAttackRangeList);

	}
	//弓箭的攻击范围
	void ShowBowAttackRange(GameObject basehexTileObject){
		int baseTile = int.Parse (basehexTileObject.name);
		List<string> bowAttackRangeList = GetBowAttackRangeList (baseTile);
		bowAttackRangeList.Remove (baseTile.ToString());
		DrawAttackRange (bowAttackRangeList);
	}
	//显示攻击范围
	void DrawAttackRange(List<string> rangeList){

		attackRangeList = new ArrayList(rangeList);
		
		// 最后修改显示范围的状态
		foreach (string indexStr in attackRangeList) {
			GameObject rangeTileObject = hexTileList[int.Parse(indexStr)] as GameObject;
			HexTile rangeTile = rangeTileObject.GetComponent<HexTile>();
			rangeTile.SetState(HexTile.HexTileState.Hostile);
		}
		
	}

	private List<string> GetKeyTileListAxeAttackRange(int baseTile)
	{
		List<string> keyTileList = new List<string>();
		AddNeighbourTlieList(baseTile,keyTileList);

		int columnNum = baseTile / 10;

		int up = baseTile + 1;
		if(up/10==columnNum){
			AddNeighbourTlieList(up,keyTileList);
		}

		int down = baseTile-1;
		if(down/10==columnNum){
			AddNeighbourTlieList(down,keyTileList);
		}

		int right = baseTile + 20;
		AddNeighbourTlieList(right,keyTileList);

		int lift  = baseTile - 20;
		AddNeighbourTlieList(lift,keyTileList);

		return keyTileList;
	}

	private List<string> GetBowAttackRangeList(int baseTile)
	{
		List<string> keyTileList = new List<string>();
		
		int columnNum = baseTile / 10;

		//向上,向下
		for(int i=0;i<10;i++){
			AddNeighbourTlieList(columnNum*10+i,keyTileList);
		}

		//右斜
		int rightStepCount = 6 - columnNum;
		int upR = baseTile;
		for(int i=0;i<rightStepCount;i++){
			upR = AddTileObliqueIndex(upR, keyTileList,"UpRight");
			if(upR == -1) break;
		}

		int downR = baseTile;
		for(int i=0;i<rightStepCount;i++){
			downR = AddTileObliqueIndex(downR, keyTileList,"DownRight");
			if(downR == -1) break;
		}

		int liftStepCount = columnNum;
		int upL = baseTile;
		for(int i=0;i<liftStepCount;i++){
			upL = AddTileObliqueIndex(upL, keyTileList,"UpLift");
			if(upL == -1) break;
		}

		int downL = baseTile;
		for(int i=0;i<liftStepCount;i++){
			downL = AddTileObliqueIndex(downL, keyTileList,"DownLift");
			if(downL == -1) break;
		}
		
		return keyTileList;
	}
	//斜向的格子下标
	int AddTileObliqueIndex(int baseTile, List<string> keyTileList, string direction){

		int columnNum = baseTile / 10;

		if (columnNum == 1 || columnNum == 3 || columnNum == 5) {

			if(direction.Equals("UpRight")){
				int upR = baseTile + 10;
				if (upR / 10 == columnNum + 1) {
					AddNeighbourTlieList(upR,keyTileList);
					return upR;
				}
			}

			if(direction.Equals("DownRight")){
				int downR = baseTile + 10 - 1;
				if (downR / 10 == columnNum + 1) {
					AddNeighbourTlieList(downR,keyTileList);
					return downR;
				}
			}

			if(direction.Equals("UpLift")){
				int upL = baseTile - 10;
				if (upL / 10 == columnNum - 1) {
					AddNeighbourTlieList(upL,keyTileList);
					return upL;
				}
			}

			if(direction.Equals("DownLift")){
				int downL = baseTile - 10 - 1;
				if (downL / 10 == columnNum - 1) {
					AddNeighbourTlieList(downL,keyTileList);
					return downL;
				}
			}
		} else {

			if(direction.Equals("UpRight")){
				int upR = baseTile+10+1;
				if(upR/10==columnNum+1){
					AddNeighbourTlieList(upR,keyTileList);
					return upR;
				}
			}

			
			if(direction.Equals("DownRight")){
				int downR = baseTile+10;
				if(downR/10==columnNum+1){
					AddNeighbourTlieList(downR,keyTileList);
					return downR;
				}
			}
			
			if(direction.Equals("UpLift")){
				int upL = baseTile-10+1;
				if(upL/10==columnNum-1){
					AddNeighbourTlieList(upL,keyTileList);
					return upL;
				}
			}
			
			if(direction.Equals("DownLift")){
				int downL = baseTile-10;
				if(downL/10==columnNum-1){
					AddNeighbourTlieList(downL,keyTileList);
					return downL;
				}
			}


		}

		return -1;
	}
}
