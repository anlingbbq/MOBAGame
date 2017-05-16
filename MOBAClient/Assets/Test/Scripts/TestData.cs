using System.Collections;
using System.Collections.Generic;
using Common.Config;
using Common.Dto;
using UnityEngine;

public class TestData : MonoBehaviour
{
    public AIBaseCtrl Ctrl;

    void Start()
    {
        DtoMinion data = new DtoMinion(0, 0, 1, 1000, 21, 20, 3, 1.2, 4, "Minion");
        Ctrl.Model = data;
    }
}
