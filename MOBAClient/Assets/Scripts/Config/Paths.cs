using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paths
{
    #region 音乐文件

    #region 界面

    /// <summary>
    /// UI声音资源文件路径 
    /// </summary>
    public const string RES_SOUND_UI = "Sound/UI/";

    public const string UI_LOGIN_BG = RES_SOUND_UI + "Welcome to Planet Urf";
    public const string UI_CLICK = RES_SOUND_UI + "Click";
    public const string UI_ENTERGAME = RES_SOUND_UI + "EnterGame";
    public const string UI_READY = RES_SOUND_UI + "Ready";
    public const string UI_BUY = RES_SOUND_UI + "Buy";
    public const string UI_SALE = RES_SOUND_UI + "Sale";
    public const string UI_SELECT = RES_SOUND_UI + "Select";

    /// <summary>
    /// 选择英雄的音效路径 
    /// </summary>
    public const string RES_SOUND_SELECT = "Sound/Select/";

    #endregion

    #region 战斗

    /// <summary>
    /// 战斗音效路径 
    /// </summary>
    public const string RES_SOUND_BATTLE = "Sound/Battle/";

    // 小兵
    public const string SOUND_MW_ATTACK = RES_SOUND_BATTLE + "MWAttack";
    public const string SOUND_MW_DEATH = RES_SOUND_BATTLE + "MWDeath";

    // 防御塔
    public const string SOUND_TOWER_ATTACK = RES_SOUND_BATTLE + "TowerAttack";

    // 战士英雄
    public const string SOUND_WARRIOR_ATTACK = RES_SOUND_BATTLE + "WarriorAttack";
    public const string SOUND_WARRIOR_DEATH = RES_SOUND_BATTLE + "WarriorDeath";

    #endregion

    #endregion

    /// <summary>
    /// 头像路径 
    /// </summary>
    public const string RES_HEAD_UI = "UI/Head/";
    public const string HEAD_NO_CONNECT = RES_HEAD_UI +　"no-Connect";

    /// <summary>
    /// 装备图标路径 
    /// </summary>
    public const string RES_EQUIPMENT_UI = "UI/Equipment/";

    /// <summary>
    /// 技能图片路径
    /// </summary>
    public const string RES_SKILL_UI = "UI/Skill/";

    /// <summary>
    /// 英雄预设路径 
    /// </summary>
    public const string RES_MODEL_HERO = "Model/Hero/";

    /// <summary>
    /// 小兵预设路径
    /// </summary>
    public const string RES_MODEL_MINION = "Model/Minion/";

    /// <summary>
    /// 小兵纹理 红
    /// </summary>
    public const string TEXTURE_MINION_RED = "Texture/Minion/Minion_Red";

    /// <summary>
    /// 小兵纹理 蓝
    /// </summary>
    public const string TEXTURE_MINION_BULE = "Texture/Minion/Minion_Blue";
}
