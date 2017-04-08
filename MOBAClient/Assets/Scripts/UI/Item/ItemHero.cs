using System;
using System.Collections;
using System.Collections.Generic;
using Common.Config;
using UnityEngine;
using UnityEngine.UI;

public class ItemHero : MonoBehaviour, IResourceListener
{
    [SerializeField]
    private Image ImageHead;
    [SerializeField]
    private Button BtnHead;
    public int HeroId;
    
    private AudioClip Sound;

    void Start()
    {
        BtnHead.onClick.AddListener(OnClick);
    }

    public void InitView(HeroModel hero)
    {
        // 保存英雄id
        HeroId = hero.Id;

        // 加载选择的音效资源
        ResourcesManager.Instance.Load(Paths.RES_SOUND_SELECT + hero.Name,
            typeof(AudioClip), this, AssetType.SoundEffect);

        // 获取头像资源
        string headPath = Paths.RES_HEAD_UI + hero.Name;
        ResourcesManager.Instance.Load(headPath, typeof(Sprite), this, AssetType.Sprite);
    }

    /// <summary>
    /// 英雄是否可选择
    /// </summary>
    public bool Interactable
    {
        get { return BtnHead.interactable; }
        set { BtnHead.interactable = value; }
    }

    /// <summary>
    /// 选择英雄事件
    /// </summary>
    public void OnClick()
    {
        // 播放音效
        SoundManager.Instance.PlayEffectMusic(Sound);

        // 发送选人的请求
        SelectPanel panel = UIManager.Instance.GetPanel(UIPanelType.Select) as SelectPanel;
        panel.OnSelectHeroClick(HeroId);
    }

    public void OnLoaded(string assetName, object asset, AssetType assetType)
    {
        if (assetType == AssetType.SoundEffect)
        {
            Sound = asset as AudioClip;
        }
        else if (assetType == AssetType.Sprite)
        {
            ImageHead.sprite = asset as Sprite;
        }
    }
}
