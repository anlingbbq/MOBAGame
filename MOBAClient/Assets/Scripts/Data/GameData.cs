using System.Collections.Generic;
using Common.Config;
using Common.Dto;
using Common.DTO;

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
    /// 保存自身的英雄数据
    /// </summary>
    public static DtoHero HeroData;

    /// <summary>
    /// 保存道具数据
    /// </summary>
    public static ItemModel[] Items;
}
