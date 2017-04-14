using System;
using System.Collections;
using System.Collections.Generic;
using Common.Dto;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : UIBasePanel, IResourceListener
{
    #region UI控件

    /// <summary>
    /// 头像图片
    /// </summary>
    [SerializeField]
    private Image ImgHead;
    /// <summary>
    /// 经验条
    /// </summary>
    [SerializeField]
    private Slider BarExp;
    /// <summary>
    /// 生命条
    /// </summary>
    [SerializeField]
    private Slider BarHp;
    /// <summary>
    /// 魔法条
    /// </summary>
    [SerializeField]
    private Slider BarMp;
    /// <summary>
    /// 生命值
    /// </summary>
    [SerializeField]
    private Text TextHp;
    /// <summary>
    /// 魔法值
    /// </summary>
    [SerializeField]
    private Text TextMp;
    /// <summary>
    /// 等级
    /// </summary>
    [SerializeField]
    private Text TextLevel;
    /// <summary>
    /// 攻击力
    /// </summary>
    [SerializeField]
    private Text TextAttack;
    /// <summary>
    /// 防御力
    /// </summary>
    [SerializeField]
    private Text TextDefense;
    /// <summary>
    /// KDA
    /// </summary>
    [SerializeField]
    private Text TextKDA;
    /// <summary>
    /// 金币
    /// </summary>
    [SerializeField]
    private Text TextCoins;

    #endregion

    /// <summary>
    /// 进入游戏的请求
    /// </summary>
    private EnterBattleRequest m_EnterRequest;

    void Start()
    {
        // 释放资源
        ResourcesManager.Instance.ReleaseAll();
        m_EnterRequest = GetComponent<EnterBattleRequest>();
        OnEnter();
    }

    public void InitView()
    {
        // 获取数据
        DtoHero hero = BattleData.Instance.Hero;
        // 加载头像
        ResourcesManager.Instance.Load(Paths.RES_HEAD_UI + hero.Name, typeof(Sprite), this);
        // 默认状体
        BarExp.value = 0;
        BarHp.value = 1;
        BarMp.value = 1;
        // 赋值
        TextAttack.text = hero.Attack.ToString();
        TextDefense.text = hero.Defense.ToString();
        TextLevel.text = "Lv." + hero.Level;
        TextHp.text = hero.CurHp + "/" + hero.MaxHp;
        TextMp.text = hero.CurMp + "/" + hero.MaxMp;
        TextCoins.text = hero.Money.ToString();
        TextKDA.text = hero.Kill + "/" + hero.Death;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        m_EnterRequest.SendRequest();
    }

    void IResourceListener.OnLoaded(string assetName, object asset, AssetType assetType)
    {
        ImgHead.sprite = asset as Sprite;
    }
}
