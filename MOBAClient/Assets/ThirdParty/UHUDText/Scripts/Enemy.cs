using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public BattleManager battleManager;
	public string hexTileName;
	public GameObject hpText;

	private Animation enemyAnimation;
	
	public enum EnemyState{
		Idle, 
		Attack, 
		Damage, 
		Dead, 
		Walk,
		Skill
	};

	public EnemyState state = EnemyState.Idle;

	// Use this for initialization
	void Start () {
		enemyAnimation = GetComponent<Animation>();
		Invoke("EnemyInit",Random.Range (0,5));
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void EnemyInit(){
		enemyAnimation["idle"].wrapMode = WrapMode.Loop;
		
		UpdateEnemyState (state);

		StartCoroutine(Timer());

	}

	IEnumerator Timer() {
		while (true) {
			yield return new WaitForSeconds(6.0f);
//			Debug.Log(string.Format("Enemy  Timer2 is up !!! time=${0}", Time.time));
			EnemyRandomState();
		}
	}

	void EnemyRandomState(){
		if ((state == EnemyState.Walk) || (state == EnemyState.Attack)) {
			return;
		}

		int randomNum = Random.Range(0,100);
		
		if(randomNum<40){
			state = EnemyState.Idle;
		}else if(randomNum>40 && randomNum < 80){
			state = EnemyState.Attack;
		}else{
			state = EnemyState.Idle;
		}
		
		UpdateEnemyState(state);
		
	}

	void UpdateEnemyState(EnemyState enemyState){
		if (enemyState == EnemyState.Idle) {
			enemyAnimation.CrossFade ("idle");
			state = EnemyState.Idle;
			
		} else if (enemyState == EnemyState.Walk) {
			enemyAnimation.CrossFade ("walk");
			state = EnemyState.Walk;
			
		} else if (enemyState == EnemyState.Damage) {
			enemyAnimation.CrossFade ("damage");
			state = EnemyState.Damage;
		} else if (enemyState == EnemyState.Attack) {
			enemyAnimation.CrossFade ("attack");
			state = EnemyState.Attack;
			
		} else if (enemyState == EnemyState.Skill) {
			enemyAnimation.CrossFade ("skill");
			state = EnemyState.Skill;
		} else if (enemyState == EnemyState.Dead) {
			enemyAnimation.CrossFade("dead");

		}
	}

	public void PlayDamage(){
		UpdateEnemyState(EnemyState.Damage);

		if (hpText) {
			hpText.SendMessage("HPText");
		}
	}
}
