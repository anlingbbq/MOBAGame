using System.Collections.Generic;
using Common.Config;
using Common.Dto;
using MOBAClient;
using UnityEngine;

public class BattleData : Singleton<BattleData>
{
    [Header("队伍1")]
    [SerializeField]
    private Transform Team1Parent;
    [SerializeField]
    private Transform[] Team1HeroPoint;
    [SerializeField]
    public Transform[] Team1MinionPoint;
    [SerializeField]
    private GameObject[] Team1Builds;

    [Header("队伍2")]
    [SerializeField]
    private Transform Team2Parent;
    [SerializeField]
    private Transform[] Team2HeroPoint;
    [SerializeField]
    public Transform[] Team2MinionPoint;
    [SerializeField]
    private GameObject[] Team2Builds;

    /// <summary>
    /// 保存游戏控制器
    /// </summary>
    public Dictionary<int, AIBaseCtrl> CtrlDict = new Dictionary<int, AIBaseCtrl>();

    /// <summary>
    /// 初始化游戏对象
    /// </summary>
    /// <param name="heros"></param>
    /// <param name="builds"></param>
    public void InitData(DtoHero[] heros, DtoBuild[] builds, SkillModel[] skills)
    {
        int myTeam = GetMyTeamId(heros, GameData.Player.Id);

        // 初始化技能数据
        SkillManager.Instance.Init(heros, skills);

        #region 英雄

        // 创建英雄
        GameObject go = null;
        foreach (DtoHero item in heros)
        {
            if (item.Team == 1)
            {
                go = Instantiate(Resources.Load<GameObject>(Paths.RES_MODEL_HERO + item.Name),
                    Team1HeroPoint[0].position, Quaternion.AngleAxis(180, Vector3.up));
                go.transform.SetParent(Team1Parent);
            }
            else
            {
                go = Instantiate(Resources.Load<GameObject>(Paths.RES_MODEL_HERO + item.Name),
                    Team2HeroPoint[0].position, Quaternion.AngleAxis(180, Vector3.up));
                go.transform.SetParent(Team2Parent);
            }

            // 初始化控制器
            AIBaseCtrl ctrl = go.GetComponent<AIBaseCtrl>();
            ctrl.Init(item, item.Team == myTeam);
            CtrlDict.Add(item.Id, ctrl);

            // 判断这个英雄是不是自己
            if (item.Id == GameData.Player.Id)
            {
                // 保存自己英雄的控制器
                GameData.HeroCtrl = ctrl;
                // 保存自己英雄的数据
                GameData.HeroData = ctrl.Model as DtoHero;
            }
        }

        #endregion

        #region 建筑

        // 创建建筑
        for (int i = 0; i < builds.Length; i++)
        {
            DtoBuild build = builds[i];
            if (build.Team == 1)
            {
                go = Team1Builds[build.TypeId - 1];
                go.SetActive(true);
            }
            else if (build.Team == 2)
            {
                go = Team2Builds[build.TypeId - 1];
                go.SetActive(true);
            }

            // 初始化控制器
            AIBaseCtrl ctrl = go.GetComponent<AIBaseCtrl>();
            ctrl.Init(build, build.Team == myTeam);
            if (build.Team == myTeam)
            {
                ctrl.Init(build, true);
                ctrl.MiniMapHead.color = Color.blue;
            }
            else
            {
                ctrl.Init(build, false);
                ctrl.MiniMapHead.color = Color.red;
            }
 
            CtrlDict.Add(build.Id, ctrl);
        }

        #endregion

        // 发送初始化完成的消息
        MOBAClient.BattleManager.Instance.InitComplete();
    }

    /// <summary>
    /// 产生小兵
    /// </summary>
    /// <param name="minions"></param>
    public void SpawnMinion(DtoMinion[] minions)
    {
        // 每0.5秒生产一个小兵
        for (int i = 0; i < minions.Length; i++)
        {
            DtoMinion minion = minions[i];
            TimerManager.Instance.AddTimer("SpawnMinion" + minion.Id, i * 0.5f, CreateMinion, minion);
        }
    }

    /// <summary>
    /// 创建小兵
    /// </summary>
    /// <param name="minion"></param>
    public void CreateMinion(params object[] args)
    {
        DtoMinion minion = (DtoMinion)args[0];

        // 创建实例
        GameObject go = null;
        //go = Instantiate(Resources.Load<GameObject>(Paths.RES_MODEL_MINION + minion.Name),
        //Team1MinionPoint[0].position, Quaternion.AngleAxis(180, Vector3.up));
        go = PoolManager.Instance.GetObject("Minion");
        if (minion.Team == 1)
        {
            go.transform.position = Team1MinionPoint[0].position;
            go.transform.SetParent(Team1Parent);
        }
        else
        {
            go.transform.position = Team2MinionPoint[0].position;
            go.transform.SetParent(Team2Parent);
        }
        // 初始化控制器
        AIBaseCtrl ctrl = go.GetComponent<AIBaseCtrl>();
        ctrl.Init(minion, minion.Team == GameData.HeroData.Team);
        CtrlDict.Add(minion.Id, ctrl);
    }

    /// <summary>
    /// 获取自身队伍的id
    /// </summary>
    /// <param name="heros"></param>
    /// <param name="playerId"></param>
    /// <returns></returns>
    private int GetMyTeamId(DtoHero[] heros, int playerId)
    {
        foreach (DtoHero hero in heros)
        {
            if (hero.Id == playerId)
            {
                return hero.Team;
            }
        }
        return -1;
    }

    public void RebirthHero(int heroId)
    {
        if (!CtrlDict.ContainsKey(heroId))
            return;

        HeroCtrl hero = CtrlDict[heroId] as HeroCtrl;
        // 设置位置
        if (hero.Model.Team == 1)
            hero.transform.position = Team1HeroPoint[0].position;
        else if (hero.Model.Team == 2)
            hero.transform.position = Team2HeroPoint[0].position;

        // 设置层
        if (hero.Model.Team == GameData.HeroData.Team)
            hero.gameObject.layer = LayerMask.NameToLayer("Friend");
        else
            hero.gameObject.layer = LayerMask.NameToLayer("Enemy");

        hero.RebirthResponse();
    }
}
