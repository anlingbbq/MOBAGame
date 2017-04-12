using System.Collections;
using System.Collections.Generic;
using Common.Config;
using Common.Dto;
using UnityEngine;

public class BattleData : Singleton<BattleData>
{
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
    private DtoBuild[] m_Builds;

    public DtoBuild[] Builds
    {
        get { return m_Builds; }
    }
    /// <summary>
    /// 初始化游戏对象
    /// </summary>
    /// <param name="heros"></param>
    /// <param name="builds"></param>
    public void InitData(DtoHero[] heros, DtoBuild[] builds)
    {
        m_Heros = heros;
        m_Builds = builds;

        return;
        // 创建英雄
        //GameObject go;
        //foreach (DtoHero item in m_Heros)
        //{
        //    // 先加载预设资源
        //    ResourcesManager.Instance.Load(Paths.RES_MODEL_HERO +　item.Name, typeof(GameObject));
        //}

        //// 创建建筑
        //foreach (DtoBuild item in m_Builds)
        //{
            
        //}
    }
}
