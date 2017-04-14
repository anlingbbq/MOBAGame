using System.Collections;
using System.Collections.Generic;
using Common.Config;
using Common.Dto;
using UnityEngine;

public class BattleData : Singleton<BattleData>
{
    [Header("队伍1")]
    [SerializeField]
    private Transform Team1Parent;
    [SerializeField]
    private Transform[] Team1HeroPoint;
    [SerializeField]
    private GameObject[] Team1Builds;

    [Header("队伍2")]
    [SerializeField]
    private Transform Team2Parent;
    [SerializeField]
    private Transform[] Team2HeroPoint;
    [SerializeField]
    private GameObject[] Team2Builds;

    /// <summary>
    /// 英雄数据
    /// </summary>
    private DtoHero[] m_Heros;

    public DtoHero[] Heros
    {
        get { return m_Heros; }
    }
    /// <summary>
    /// 建筑数据
    /// </summary>
    public DtoBuild[] Builds { get; private set; }

    /// <summary>
    /// 自己的英雄数据
    /// </summary>
    public DtoHero Hero { get; private set; }

    /// <summary>
    /// 自己的英雄控制器
    /// </summary>
    public BaseCtrl HeroCtrl { get; private set; }

    /// <summary>
    /// 保存游戏物体
    /// </summary>
    public Dictionary<int, BaseCtrl> CtrlDict = new Dictionary<int, BaseCtrl>();

    /// <summary>
    /// 初始化游戏对象
    /// </summary>
    /// <param name="heros"></param>
    /// <param name="builds"></param>
    public void InitData(DtoHero[] heros, DtoBuild[] builds)
    {
        m_Heros = heros;
        Builds = builds;

        int myTeam = GetMyTeamId(heros, GameData.player.Id);

        #region 英雄

        // 创建英雄
        GameObject go = null;
        foreach (DtoHero item in m_Heros)
        {
            if (item.Team == 1)
            {
                go = Instantiate(Resources.Load<GameObject>(Paths.RES_MODEL_HERO + item.Name),
                    Team1HeroPoint[0].position, Quaternion.AngleAxis(90, Vector3.up));
                go.transform.SetParent(Team1Parent);
            }
            else if (item.Team == 2)
            {
                go = Instantiate(Resources.Load<GameObject>(Paths.RES_MODEL_HERO + item.Name),
                    Team2HeroPoint[0].position, Quaternion.AngleAxis(-90, Vector3.up));
                go.transform.SetParent(Team2Parent);
            }

            // 初始化控制器
            BaseCtrl ctrl = go.GetComponent<BaseCtrl>();
            ctrl.Init(item, item.Team == myTeam);
            CtrlDict.Add(item.Id, ctrl);

            // 判断这个英雄是不是自己
            if (item.Id == GameData.player.Id)
            {
                // 保存当前英雄
                Hero = item;
                // 保存英雄的控制器
                HeroCtrl = ctrl;
            }
        }

        #endregion
        return;
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
            BaseCtrl ctrl = go.GetComponent<BaseCtrl>();
            ctrl.Init(build, build.Team == myTeam);
            CtrlDict.Add(build.Id, ctrl);
        }

        #endregion
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
}
