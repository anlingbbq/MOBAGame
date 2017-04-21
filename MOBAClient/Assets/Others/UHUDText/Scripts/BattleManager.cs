using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour {

	public GameObject[] heroList;
	public GameObject[] enemyList;
	public GameObject[] propList;

	public GameObject linePerfab;

	private HexNavMapManager hexNavMapManager;
	private HexTile currentHexTile = null;

	private ArrayList pathHexTileList = new ArrayList();
	private ArrayList tempPathHexTileList = new ArrayList();
	private ArrayList moveLineList = new ArrayList();

	private int tempStep=0;

	private GameObject heroMoveEnded; // 刚刚结束移动的英雄
	bool unitMoving = false;//移动开关(false是开着，true是关闭)
	public bool canAttack = false;

	// Use this for initialization
	void Start () {

		//设置屏幕自动旋转， 并置支持的方向
		Screen.orientation = ScreenOrientation.Portrait;
		Screen.autorotateToLandscapeLeft = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = false;


		hexNavMapManager = HexNavMapManager.GetInstance ();

		if (hexNavMapManager) {
			DeployHero (); //部署英雄，敌人，道具

			DeployEnemy ();//部署英雄，敌人，道具

			DeployProp ();//部署英雄，敌人，道具
		}

	
	}

	// Update is called once per frame
//	void FixedUpdate(){
	void Update () {

		if (unitMoving)
			return;

		#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_BLACKBERRY
		if(Input.touchCount==1){
			Touch touch=Input.touches[0];
			
			if(touch.phase == TouchPhase.Began){

				SelectTileByScreenPointToRay(touch.position);
			}

			if(touch.phase == TouchPhase.Moved && (currentHexTile!=null&&currentHexTile.unit!=null)){
				CreateMovePathHexTiles(touch.position);
			}

			if(touch.phase == TouchPhase.Ended){

				UnitMove(currentHexTile.unit);
				pathHexTileList.Clear();
			}
		}
		#endif
		
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if(Input.GetMouseButtonDown(0))
		{

			SelectTileByScreenPointToRay(Input.mousePosition);//选中网格
		}

		if(Input.GetMouseButton(0)&& (currentHexTile!=null&&currentHexTile.unit!=null))
		{
			CreateMovePathHexTiles(Input.mousePosition);
		}

		if(Input.GetMouseButtonUp(0)){

			if(currentHexTile!=null){
				UnitMove(currentHexTile.unit);
			}

			pathHexTileList.Clear();
		}
		#endif
	
	}

	void SelectTileByScreenPointToRay(Vector3 screenPoint){
		if (hexNavMapManager == null)
			return;

		GameObject hexTileObject = hexNavMapManager.SelectTileByScreenPointToRay(screenPoint, canAttack);

		if (hexTileObject != null) //不是空地
		{
			currentHexTile = hexTileObject.GetComponent<HexTile> ();
			pathHexTileList.Add(hexTileObject);
			if (currentHexTile.unit != null) {
				Enemy enemy = currentHexTile.unit.GetComponent<Enemy>();
				Hero hero = currentHexTile.unit.GetComponent<Hero>();

				if(canAttack && enemy!=null){
					PlayBattle();
					canAttack = false;
					return;
				}

				if(hero){
					tempStep = hero.step;
				}
			}
		} else {
			currentHexTile = null;
		}

	}

	void CreateMovePathHexTiles(Vector3 screenPoint){
		//Debug.Log ("********************CreateMovePathHexTiles********************");
		if (hexNavMapManager == null)
			return;

		int lastIndex = pathHexTileList.Count - 1;
		//Debug.Log ("********************pathHexTileList lastIndex:"+lastIndex);
		GameObject lastHexTileObject = pathHexTileList[lastIndex] as GameObject;
		//Debug.Log ("********************pathHexTileList lastHexTileObject:"+lastHexTileObject);
		GameObject nextHexTile = hexNavMapManager.SelectMovePathHexTilesByScreenPointToRay (screenPoint,lastHexTileObject);//若lastHexTileObject为有效节点，返回这个节点
		if (nextHexTile != null) 
		{

			GameObject prevHexTileObject = null;

			if(pathHexTileList.Count>1){
				prevHexTileObject = pathHexTileList[pathHexTileList.Count-2] as GameObject;
			}

//			if(HeroInformation.heroHex.unit==nextHexTile)
//			{
//				print (pathHexTileList[0]);
//			}
			//print(pathHexTileList[0]+"下一个点");
			if(nextHexTile.Equals(prevHexTileObject))
			{
				//撤销路线的场合
				int removeIndex = pathHexTileList.Count-1;
				pathHexTileList.RemoveAt(removeIndex);
				RemoveMoveLine(moveLineList.Count-1);
			}else{
				if (tempStep - pathHexTileList.Count + 1 == 0) return;
				//选择前进路线的场合
				pathHexTileList.Add(nextHexTile);

				// 绘制新的路线
				int count = pathHexTileList.Count;
				if(count> 1){
					GameObject endPointObject   =  pathHexTileList[count-1] as GameObject;
					GameObject startPointObject =  pathHexTileList[count-2] as GameObject;
					
					Vector3 startPoint = startPointObject.transform.position;
					Vector3 endPoint   = endPointObject.transform.position;
					moveLineList.Add(DrawMoveline(startPoint, endPoint));
				}
			}

			hexNavMapManager.ShowMoveRangeInMoving(nextHexTile,tempStep-pathHexTileList.Count+1);

		}
	}

	void PlayBattle(){
		//Debug.Log ("PlayBattlePlayBattlePlayBattlePlayBattlePlayBattle");
		heroMoveEnded.SendMessage ("PlayAttack");

		StartCoroutine(AttackEnemy());

	}

	IEnumerator AttackEnemy() {
		ArrayList canAttackList = hexNavMapManager.attackRangeList;
		yield return new WaitForSeconds(1.0f);


		foreach (GameObject enemyObject in enemyList) {
			Enemy enemy = enemyObject.GetComponent<Enemy> ();
			if (canAttackList.Contains(enemy.hexTileName)) {
					enemy.PlayDamage ();
				
			}
		}
	}

	void DeployHero(){
		int randomNum = -1;
		ArrayList randomNumList = new ArrayList ();
		if (heroList.Length > 0) {
			for (int i=0; i<heroList.Length; i++) {

				while(true){
					int columnNum = Random.Range (0, 6);
					int rownNum = Random.Range (0, 2);

					int temp = columnNum*10 + rownNum;

					if(!randomNumList.Contains(temp.ToString())){
						randomNumList.Add(temp.ToString());
						randomNum = temp;
						break;
					}
				}

				GameObject tile = hexNavMapManager.hexTileList[randomNum] as GameObject;
				GameObject hero = heroList[i] as GameObject;
				hero.transform.position = tile.transform.position;

				HexTile hexTile = tile.GetComponent<HexTile>();
				hexTile.unit = hero;

			}
		}

	}

	void DeployEnemy(){
		for (int i=0; i<enemyList.Length; i++) {
			
			GameObject enemyObject = enemyList[i] as GameObject;
			Enemy enemy = enemyObject.GetComponent<Enemy>();
			GameObject tile = null;
			if(i==0){
				tile = hexNavMapManager.hexTileList[38] as GameObject;
			}
			if(i==1){
				tile = hexNavMapManager.hexTileList[19] as GameObject;
			}
			if(i==2){
				tile = hexNavMapManager.hexTileList[49] as GameObject;
			}
			if(i==3){
				tile = hexNavMapManager.hexTileList[59] as GameObject;
			}
			
			
			enemyObject.transform.position = tile.transform.position;
			
			HexTile hexTile = tile.GetComponent<HexTile>();
			hexTile.unit = enemyObject;
			enemy.hexTileName = hexTile.gameObject.name;
			
		}
		
	}

	void DeployProp (){

		for (int i=0; i<propList.Length; i++) {

			GameObject prop = propList[i] as GameObject;

			GameObject tile = null;
			if(i==0){
				tile = hexNavMapManager.hexTileList[24] as GameObject;
			}
			if(i==1){
				tile = hexNavMapManager.hexTileList[35] as GameObject;
			}
			if(i==2){
				tile = hexNavMapManager.hexTileList[54] as GameObject;
			}


			prop.transform.position = tile.transform.position;
			
			HexTile hexTile = tile.GetComponent<HexTile>();
			hexTile.unit = prop;
			
		}

	}
	//划线
	private GameObject DrawMoveline(Vector3 startPoint, Vector3 endPoint)
	{
		GameObject aline = Instantiate (linePerfab);

		//通过游戏对象，GetComponent方法 传入LineRenderer
		//就是之前给line游戏对象添加的渲染器属性
		//有了这个对象才可以为游戏世界渲染线段
		LineRenderer lineRenderer = (LineRenderer)aline.GetComponent ("LineRenderer");
		lineRenderer.SetVertexCount(2);
		//pathLines.Add (aline);
		
		lineRenderer.SetPosition (0, startPoint);
		lineRenderer.SetPosition (1, endPoint);

		return aline;
	}

	private void RemoveMoveLine(int indexLine){
		GameObject lastLineObject = moveLineList[indexLine] as GameObject;
		if (indexLine >=0) 
		{
			moveLineList.RemoveAt (indexLine);
			Destroy (lastLineObject);
		}
	}

	void UnitMove(GameObject gameUnit)//选中目标时和开始移动时执行
	{

		if (gameUnit == null)
			return;
		if (moveLineList.Count < 1)
			return;

		Hero hero = gameUnit.GetComponent<Hero>();
		if (hero) {
			tempPathHexTileList.Clear();

			foreach (GameObject hexTileObject in pathHexTileList) {
				tempPathHexTileList.Add(hexTileObject);
			}

			unitMoving = true;
			hero.StartMove(tempPathHexTileList,moveLineList,this);
		}

	}

	public void UnitMoveEnd(GameObject gameUnit)//运动到目的地执行
	{
		heroMoveEnded = gameUnit;

		unitMoving = false;

		foreach (GameObject line in moveLineList) {
			Destroy(line);
		}

		moveLineList.Clear ();
		hexNavMapManager.ClearMoveRange ();
		hexNavMapManager.selectedTileObject = null;
		
		
	}

	public void ShowAttackRange(int propType, GameObject hexTileObjectStand){
		hexNavMapManager.ShowAttackRange (propType,hexTileObjectStand);
		this.canAttack = true;

	}
	public void EndHeroRound()
	{
		unitMoving = true;
	}
	public void EndEnemyRound()
	{
		foreach (GameObject copy in heroList) {
			Hero heroCopy= copy.GetComponent<Hero>();
			heroCopy.APControl();
		}
		unitMoving = false;
	}

}
