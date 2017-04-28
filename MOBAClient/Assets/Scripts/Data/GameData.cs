using System.Collections;
using System.Collections.Generic;
using Common.Dto;
using Common.DTO;
using UnityEngine;

public class GameData
{
    /// <summary>
    /// 保存自身的玩家数据 
    /// </summary>
    public static DtoPlayer Player;

    /// <summary>
    /// 保存自身的玩家控制器 
    /// </summary>
    public static AIBaseCtrl HeroCtrl;

    /// <summary>
    /// 自己的英雄数据
    /// </summary>
    public static DtoHero HeroData;
}
