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
    /// 保存游戏物体
    /// </summary>
    private Dictionary<int, GameObject> GameObjectDict = new Dictionary<int, GameObject>();

    /// <summary>
    /// 初始化游戏对象
    /// </summary>
    /// <param name="heros"></param>
    /// <param name="builds"></param>
    public void InitData(DtoHero[] heros, DtoBuild[] builds)
    {
        m_Heros = heros;
        Builds = builds;

        #region 英雄

        // 创建英雄
        GameObject go;
        foreach (DtoHero item in m_Heros)
        {
            // 先加载预设资源
            go = Instantiate(Resources.Load<GameObject>(Paths.RES_MODEL_HERO + item.Name));
            if (item.Team == 1)
            {
                go.transform.SetParent(Team1Parent);
                go.transform.position = Team1HeroPoint[0].position;
            }
            else if (item.Team == 2)
            {
                go.transform.SetParent(Team2Parent);
                go.transform.position = Team2HeroPoint[0].position;
            }
            // 判断这个英雄是不是自己
            if (item.Id == GameData.player.Id)
            {
                // 保存当前英雄
                Hero = item;   
            }

            GameObjectDict.Add(item.Id, go);
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
                GameObjectDict.Add(build.Id, go);
            }
            else if (build.Team == 2)
            {
                go = Team2Builds[build.TypeId - 1];
                go.SetActive(true);
                GameObjectDict.Add(build.Id, go);
            }
        }

        #endregion
    }
}
