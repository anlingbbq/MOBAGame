using System.Collections;
using System.Collections.Generic;
using Common.Dto;
using UnityEngine;

public class Test2Manager : Singleton<Test2Manager>
{
    public Transform TeamOne;
    public Transform TeamTwo;

    private int m_MinionNum = 3;

    private int m_MinionId = -1000;

    void Start()
    {
        SpawnMinion();
        // 每20秒产生一批小兵
        TimerManager.Instance.AddTimerRepeat("StartSpawnMinion", 20, SpawnMinion);
    }

    /// <summary>
    /// 产生小兵
    /// </summary>
    /// <param name="minions"></param>
    private void SpawnMinion()
    {
        // 每0.5秒创建一个小兵
        for (int i = 0; i < m_MinionNum; i++)
        {
            DtoMinion teamOneMinion = new DtoMinion(m_MinionId--, 0, 1, 220, 40, 20, 3, 1.5, 5, "Minion");
            TimerManager.Instance.AddTimer("SpawnMinion" + teamOneMinion.Id, i * 0.5f, CreateMinion, teamOneMinion);

            DtoMinion teamTwoMinion = new DtoMinion(m_MinionId--, 0, 2, 220, 40, 20, 3, 1.5, 5, "Minion");
            TimerManager.Instance.AddTimer("SpawnMinion" + teamTwoMinion.Id, i * 0.5f, CreateMinion, teamTwoMinion);
        }
    }

    /// <summary>
    /// 创建小兵
    /// </summary>
    /// <param name="minion"></param>
    private void CreateMinion(params object[] args)
    {
        DtoMinion minion = (DtoMinion)args[0];

        // 创建实例
        GameObject go = null;
        go = PoolManager.Instance.GetObject("Minion");
        if (minion.Team == 1)
        {
            go.transform.position = TeamOne.position;
            go.transform.SetParent(TeamOne);
        }
        else
        {
            go.transform.position = TeamTwo.position;
            go.transform.SetParent(TeamTwo);
        }
        // 初始化控制器
        AIBaseCtrl ctrl = go.GetComponent<AIBaseCtrl>();
        ctrl.Init(minion, false);
        if (minion.Team == 1)
        {
            (ctrl as MinionCtrl).EndPoint = TeamTwo.position;
            ctrl.ChangeState(AIStateEnum.IDLE);
        }
        else
        {
            (ctrl as MinionCtrl).EndPoint = TeamOne.position;
            ctrl.ChangeState(AIStateEnum.IDLE);
        }
    }
}
