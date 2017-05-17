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
    /// 技能数据
    /// </summary>
    private DtoSkill m_Data;

    /// <summary>
    /// 描述内容
    /// </summary>
    private string m_Desc;

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
        m_Data = data;
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
        MOBAClient.BattleManager.Instance.RequestUpgradeSkill(m_Data.Id, this);
    }

    /// <summary>
    /// 点击技能回掉
    /// </summary>
    public void OnSkillClick()
    {
        if (m_Data.Level >= 1)
        {
            // 使用技能
            if (m_IsReady)
            {
                m_IsReady = false;
                MOBAClient.BattleManager.Instance.RequestUseSkill(m_Data.Id, GameData.HeroData.Id, null, m_Data.Level);
            }
        }
    }

    void Update()
    {
        if (m_Data.Level >= 1 && !m_IsReady)
        {
            // 计算冷却时间
            m_Timer += Time.deltaTime;
            ImgMask.fillAmount = 1 - m_Timer / (float)m_Data.CoolDown;
            if (m_Timer >= m_Data.CoolDown)
            {
                m_Timer = 0;
                m_IsReady = true;
                ImgMask.fillAmount = 0;
            }
        }
    }

    /// <summary>
    /// 更新界面
    /// </summary>
    /// <param name="skill"></param>
    public void UpdateView(DtoSkill skill)
    {
        m_Data = skill;
        // 第一次学习技能
        if (m_Data.Level == 1)
        {
            m_IsReady = true;
            ImgMask.fillAmount = 0;
        }

        UpdataBtnUp();
    }

    /// <summary>
    /// 更新升级按钮状态
    /// </summary>
    /// <param name="active"></param>
    public void UpdataBtnUp()
    {
        // 没有技能点 或英雄等级不够
        if (GameData.HeroData.SP <= 0 || GameData.HeroData.Level < m_Data.UpgradeLevel)
            BtnUp.gameObject.SetActive(false);
        else
            BtnUp.gameObject.SetActive(true);
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
