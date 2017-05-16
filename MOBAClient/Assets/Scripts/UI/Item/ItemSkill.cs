using System;
using Common.Dto;
using MOBAClient;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BattleManager = MOBAClient.BattleManager;

public class ItemSkill : MonoBehaviour, IResourceListener, IPointerEnterHandler, IPointerExitHandler
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

    #region 属性

    /// <summary>
    /// 描述内容
    /// </summary>
    private string m_Desc;

    /// <summary>
    /// 技能id
    /// </summary>
    private int m_SkillId;

    /// <summary>
    /// 冷却时间
    /// </summary>
    private float m_CoolDown;

    /// <summary>
    /// 当前等级
    /// </summary>
    private int m_Level = -1;

    /// <summary>
    /// 计时
    /// </summary>
    private float m_Timer;

    /// <summary>
    /// 是否准备好了
    /// </summary>
    private bool m_IsReady;

    #endregion

    public void Init(DtoSkill data)
    {
        m_SkillId = data.Id;
        m_Level = data.Level;
        m_CoolDown = (float)data.CoolDown;
        m_Desc = data.Name + "\n" + data.Description;
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
        MOBAClient.BattleManager.Instance.RequestUpgradeSkill(m_SkillId, this);
    }

    /// <summary>
    /// 点击技能回掉
    /// </summary>
    public void OnSkillClick()
    {
        if (m_Level >= 0)
        {
            // 使用技能
            if (m_IsReady)
            {
                m_IsReady = false;
                MOBAClient.BattleManager.Instance.RequestUseSkill(m_SkillId, GameData.HeroData.Id, null, m_Level);
            }
        }
    }

    void Update()
    {
        if (m_Level >= 0 && !m_IsReady)
        {
            // 计算冷却时间
            m_Timer += Time.deltaTime;
            ImgMask.fillAmount = 1 - m_Timer / m_CoolDown;
            if (m_Timer >= m_CoolDown)
            {
                m_Timer = 0;
                m_IsReady = true;
                ImgMask.fillAmount = 0;
            }
        }
    }

    public void UpdateView(DtoSkill skill)
    {
        if (m_Level < 0)
        {
            m_IsReady = true;
            ImgMask.fillAmount = 0;
        }
        m_Level = skill.Level;
        BtnUp.gameObject.SetActive(false);
        m_CoolDown = (float)skill.CoolDown;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 隐藏描述
        ImgDesc.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 显示描述
        TextDesc.text = m_Desc;
        ImgDesc.gameObject.SetActive(true);
    }

    public void OnLoaded(string assetName, object asset)
    {
        ImgMask.sprite = asset as Sprite;
        ImgSkill.sprite = asset as Sprite;
    }
}
