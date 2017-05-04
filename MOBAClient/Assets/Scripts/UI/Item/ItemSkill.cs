using System;
using Common.Dto;
using MOBAClient;
using UnityEngine;
using UnityEngine.UI;

public class ItemSkill : MonoBehaviour, IResourceListener
{
    #region UI

    /// <summary>
    /// 技能图片
    /// </summary>
    [SerializeField]
    private Image ImgSkill;

    /// <summary>
    /// 技能遮罩
    /// </summary>
    [SerializeField]
    private Image ImgMask;

    /// <summary>
    /// 描述文本
    /// </summary>
    [SerializeField]
    private Text TextDesc;

    /// <summary>
    /// 描述图片
    /// </summary>
    [SerializeField]
    private Image ImgDesc;

    /// <summary>
    /// 升级按钮
    /// </summary>
    [SerializeField]
    private Button BtnUp;

    #endregion

    /// <summary>
    /// 描述内容
    /// </summary>
    private string Desc;

    /// <summary>
    /// 技能id
    /// </summary>
    private int SkillId;

    /// <summary>
    /// SkillData中的键
    /// </summary>
    private string SkillKey;

    /// <summary>
    /// 冷却时间
    /// </summary>
    private float CoolDown;

    /// <summary>
    /// 计时
    /// </summary>
    private float Timer;

    /// <summary>
    /// 是否学习
    /// </summary>
    private bool IsLearn;

    /// <summary>
    /// 是否准备好了
    /// </summary>
    private bool IsReady;

    public void Init(DtoSkill data)
    {
        SkillId = data.Id;
        SkillKey = "" + GameData.HeroData.Id + data.Id;
        CoolDown = (float)data.CoolDown;
        Desc = data.Name + "\n" + data.Description;
        // 加载图片
        ResourcesManager.Instance.Load(Paths.RES_SKILL_UI + data.Id, typeof(Sprite), this);
    }

    /// <summary>
    /// 点击升级按钮回掉
    /// </summary>
    public void OnBtnUpClick()
    {
        if (GameData.HeroData.SP < 1)
            return;

        // 发送升级技能的消息
        MOBAClient.BattleManager.Instance.RequestUpgradeSkill(SkillId, this);
    }

    /// <summary>
    /// 点击技能回掉
    /// </summary>
    public void OnSkillClick()
    {
        if (!IsLearn)
        {
            // 显示描述
            TextDesc.text = Desc;
            ImgDesc.gameObject.SetActive(true);
            TimerManager.Instance.AddTimer("ShowSkillDesc", 5, () =>
            {
                ImgDesc.gameObject.SetActive(false);
            });
        }
        else
        {
            // 使用技能
            if (IsReady)
            {
                IsReady = false;
                SkillData.Instance.RunSkill(SkillKey, GameData.HeroCtrl);
            }
        }
    }

    void Update()
    {
        if (IsLearn && !IsReady)
        {
            // 计算冷却时间
            Timer += Time.deltaTime;
            ImgMask.fillAmount = 1 - Timer / CoolDown;
            if (Timer >= CoolDown)
            {
                Timer = 0;
                IsReady = true;
                ImgMask.fillAmount = 0;
            }
        }
    }

    public void UpdateView(DtoSkill skill)
    {
        if (!IsLearn)
        {
            IsLearn = true;
            IsReady = true;
            ImgMask.fillAmount = 0;
        }

        BtnUp.gameObject.SetActive(false);
        CoolDown = (float)skill.CoolDown;
    }

    public void OnLoaded(string assetName, object asset)
    {
        ImgMask.sprite = asset as Sprite;
        ImgSkill.sprite = asset as Sprite;
    }
}
