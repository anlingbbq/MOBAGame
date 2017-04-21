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
    [Header("UI控件")]
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

    #region 文字浮动效果

    [Header("浮动文字")]
    [SerializeField]
    private bl_HUDText m_HUDText;

    public void FloatDamage(int damage, Transform trans)
    {
        m_HUDText.NewText("- " + damage, trans, Color.red, 28, 20f, -1f, 2.2f, bl_Guidance.Up);
    }

    #endregion

    public void InitView()
    {
        // 获取数据
        DtoHero hero = GameData.HeroData;
        // 加载头像
        ResourcesManager.Instance.Load(Paths.RES_HEAD_UI + hero.Name, typeof(Sprite), this);
        // 默认状态
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

    /// <summary>
    /// 刷新界面
    /// </summary>
    /// <param name="hero"></param>
    public void UpdateView(DtoHero hero)
    {

        // 更新状态条
        BarExp.value = (float) hero.Exp / (hero.Level * 100);
        BarHp.value = (float) hero.CurHp / hero.MaxHp;
        BarMp.value = (float) hero.CurMp / hero.MaxMp;

        // 更新文本
        TextAttack.text = hero.Attack.ToString();
        TextDefense.text = hero.Defense.ToString();
        TextLevel.text = "Lv." + hero.Level;
        TextHp.text = hero.CurHp + "/" + hero.MaxHp;
        TextMp.text = hero.CurMp + "/" + hero.MaxMp;
        TextCoins.text = hero.Money.ToString();
        TextKDA.text = hero.Kill + "/" + hero.Death;
    }

    void IResourceListener.OnLoaded(string assetName, object asset, AssetType assetType)
    {
        ImgHead.sprite = asset as Sprite;
    }
}
