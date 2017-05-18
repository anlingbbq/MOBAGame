using System.Collections;
using System.Collections.Generic;
using Common.Dto;
using UnityEngine;

public class TestHeroData : MonoBehaviour
{
    public int TeamId = 0;
    public int Hp = 5000;
    public int Attack = 50;
    public int Defense = 30;
	void Start () {
        DtoMinion data = new DtoMinion(0, 0, TeamId, Hp, Attack, Defense, 5, 1.0f, 8, "Hero");
        AIBaseCtrl ctrl = GetComponent<AIBaseCtrl>();
	    ctrl.Model = data;
        ctrl.Init(data, true);
	}
}
