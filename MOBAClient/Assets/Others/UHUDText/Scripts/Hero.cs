using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	private ArrayList moveHexTileList;
	private ArrayList moveLineList;

	public BattleManager battleManager;


	public Prop heroProp = null;
	Vector3 moveForward;//英雄移动后的朝向

	public enum HeroState{
		Idle, 
		Attack, 
		Damage, 
		Dead, 
		Walk,
		Skill
	};

	public HeroState state = HeroState.Idle;
	//AP的定义部分
	public int step;//当前能走的步数
	public int minAP;
	public int maxAP;
	int usedAp;

	private Animation heroAnimation;

	private Vector3 moveTarget;
	float moveSpeed = 0;
	void Awake()
	{
		heroAnimation = GetComponent<Animation>();
	}
	// Use this for initialization
	void Start () {

		Invoke("HeroInit",Random.Range (0,2));
	}

	void HeroInit(){
		heroAnimation["idle"].wrapMode = WrapMode.Loop;
		
		UpdateHeroState (state);
		
		StartCoroutine(Timer());
	}

	IEnumerator Timer() {
		while (true) {
			yield return new WaitForSeconds(5.0f);
			//Debug.Log(string.Format("Hero Timer2 is up !!! time=${0}", Time.time));
			HeroRandomState();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	//随机出现待机动作
	void HeroRandomState(){

		if ((state == HeroState.Walk) || (state == HeroState.Attack)) {
			return;
		}

		int randomNum = Random.Range(0,100);
		
		if(randomNum<40){
			state = HeroState.Idle;
		}else if(randomNum>40 && randomNum < 80){
			state = HeroState.Idle;
		}else{
			state = HeroState.Attack;
		}
		
		UpdateHeroState(state);

	}
	//播放动画控制
	void UpdateHeroState(HeroState heroState){

		if (heroState == HeroState.Idle) {
			heroAnimation.CrossFade ("idle");
			state = HeroState.Idle;

		}else if(heroState == HeroState.Walk){
			heroAnimation.CrossFade ("walk");
			heroAnimation["walk"].wrapMode= WrapMode.Loop;
			state = HeroState.Walk;

		}else if(heroState == HeroState.Attack){
			heroAnimation.CrossFade ("attack");
			state = HeroState.Attack;

		}else if(heroState == HeroState.Damage){
			heroAnimation.CrossFade ("damage");
			state = HeroState.Damage;
			
		}else if(heroState == HeroState.Skill){
			heroAnimation.CrossFade ("skill");
			state = HeroState.Skill;
			
		}else if(heroState == HeroState.Dead){
			heroAnimation.CrossFade ("dead");
			state = HeroState.Dead;
			
		}
	}
	//攻击
	public void PlayAttack(){
		UpdateHeroState(HeroState.Attack);
	}

	// 两个参数，一个是移动点的列表，一个是移动线段的列表
	//开始移动
	public void StartMove(ArrayList tileList, ArrayList lineList, BattleManager battleManagerScript){

		if (tileList == null || lineList == null||battleManagerScript==null)
			return;

		moveHexTileList = tileList;
		moveLineList    = lineList;
		battleManager = battleManagerScript;

		UpdateHeroState (HeroState.Walk);
		moveSpeed = 0.15f;
		LetIsGo();

		GameObject hexTileObject = moveHexTileList [0] as GameObject;
		HexTile hextile=hexTileObject.GetComponent<HexTile>();
		hextile.Select();

	}

	void LetIsGo(){
		
		GameObject tileStop  = moveHexTileList[1] as GameObject;

		Vector3 targetPoint = tileStop.transform.position;

		iTween.MoveTo(gameObject, iTween.Hash("position", targetPoint, "time",moveSpeed,"easeType", "linear", "looktarget", targetPoint, "oncomplete","MoveToTarget"));

		//iTween.MoveTo(gameObject, iTween.Hash("position", targetPoint, "easeType", "linear", "looktarget", targetPoint, "oncomplete","MoveToTarget","delay", .1));
	}
	//移动到目标
	void MoveToTarget(){
		GameObject tileStart = moveHexTileList[0] as GameObject;//第一块格子（包括英雄本身的格子）
		GameObject tileStop  = moveHexTileList[1] as GameObject;//下一块格子
		//移动后的朝向
		moveForward =tileStop.transform.position-tileStart.transform.position;//移动时的方向
		this.gameObject.transform.rotation = Quaternion.FromToRotation (Vector3.forward, moveForward);
		if (this.gameObject.transform.up!=Vector3.up) {
			this.gameObject.transform.Rotate(0,0,180);
			
		}
		//this.gameObject.transform.rotation = Quaternion.FromToRotation (Vector3.forward, this.gameObject.transform.right);
		usedAp++;
		APShow ();//显示AP变化
		//移动中网格的状态变化
		HexTile theStepTileFirst = tileStop.GetComponent<HexTile> ();
		HexTile theStepTileSecond = tileStart.GetComponent<HexTile> ();
		theStepTileFirst.Select ();
		theStepTileSecond.Idle ();

		HexTile hexTileStart = tileStart.GetComponent<HexTile> ();
		hexTileStart.unit = null;

		HexTile hexTileStop = tileStop.GetComponent<HexTile> ();

		if (hexTileStop.unit != null) {
			Hero hero = hexTileStop.unit.GetComponent<Hero> ();
			Prop prop = hexTileStop.unit.GetComponent<Prop> ();

			if (prop) {
				hexTileStop.unit.SetActive (false);
				heroProp = prop;
				hexTileStop.unit = gameObject;
			}

			if(hero){

			}
		} else {
			hexTileStop.unit = gameObject;
		}


		moveHexTileList.RemoveAt (0);//移除第一个元素

		GameObject line = moveLineList [0] as GameObject;
		moveLineList.RemoveAt (0);
		Destroy (line);


		if (moveHexTileList.Count > 1) {
			LetIsGo ();
		} else {
			theStepTileFirst.Select();
			StopMove();
		}

	}

	public void StopMove(){

		UpdateHeroState (HeroState.Idle);

		battleManager.UnitMoveEnd (gameObject);

		if (heroProp != null) {
			int lastStepIndex = moveHexTileList.Count - 1; 
			GameObject hexTileObject = moveHexTileList [lastStepIndex] as GameObject;
			ShowAttackRange (hexTileObject);
		} else {
			int lastStepIndex = moveHexTileList.Count - 1; 
			GameObject hexTileObject = moveHexTileList [lastStepIndex] as GameObject;
			HexTile hextile=hexTileObject.GetComponent<HexTile>();
			hextile.Select();
		}

		moveHexTileList = null;
		moveLineList    = null;
	}

	public void ShowAttackRange(GameObject hexTileObject){
		battleManager.ShowAttackRange (heroProp.typeProp,hexTileObject);
	}
	//AP控制
	public void APShow()
	{
		step = maxAP - usedAp;
		if (step < 0) {
			step = 0;
		}
	}
	public void APControl()
	{
		step += minAP;
		if (step>maxAP) {
			step = maxAP;
		}
	}
}
