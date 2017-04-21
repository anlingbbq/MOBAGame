using UnityEngine;
using System.Collections;

public class HeroInformation : MonoBehaviour {
	public HexNavMapManager hexNav;
	public static HexTile heroHex;
	public GameObject heroObject;
	public Hero hero;
	public  GameObject manager;
	public BattleManager battleManager;
	// Use this for initialization
	void Start () {
		battleManager = manager.GetComponent<BattleManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		hexNav = HexNavMapManager.GetInstance ();
		heroObject = GameObject.Find (hexNav.indexSelectedTile.ToString());
		if(heroObject != null)
		{
			heroHex = heroObject.GetComponent<HexTile> ();
		}
		if(heroHex.unit!=null)
		{
			hero = heroHex.unit.GetComponent<Hero>();
		}
	}
	void OnGUI()
	{
		GUILayout.BeginArea (new Rect (50, Screen.height - 100, Screen.width - 100, 100));
		GUILayout.BeginHorizontal ();
		if(heroHex.unit != null)
		{
			GUILayout.Label (heroHex.unit.name);
			if(hero!=null)
			{
			GUILayout.Label ("步数" + hero.step + "/" + hero.maxAP);
			}
			if (GUILayout.Button ("待机")) {
				hexNav.ClearMoveRange ();
				heroHex.Select ();
			}
			if (GUILayout.Button ("回合结束")) {
				battleManager.EndHeroRound ();
				heroHex.Idle ();
				hexNav.ClearAttackRange ();
				hexNav.ClearMoveRange ();
			}
			if (GUILayout.Button ("敌方回合结束")) {
				battleManager.EndEnemyRound ();
				for(int i=0;i<70;i++)
				{
					GameObject tile=GameObject.Find (i.ToString());
					HexTile hextile=tile.GetComponent<HexTile>();
					hextile.Start();
				}
			}
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();
	}
}
