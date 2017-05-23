using System.Collections;
using Common.Config;
using Common.Dto;
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
    /// <summary>
    /// 复活倒计时
    /// </summary>
    [SerializeField]
    private Text TextRebirth;
    /// <summary>
    /// 购买按钮
    /// </summary>
    [SerializeField]
    private Button BtnShop;

    /// <summary>
    /// 装备栏
    /// </summary>
    [SerializeField]
    private Image[] Equipments;

    /// <summary>
    /// 技能栏
    /// </summary>
    [SerializeField]
    private Button[] Skills;

    #endregion

    #region 文字浮动效果

    [Header("浮动文字")]
    [SerializeField]
    private bl_HUDText HUDText;

    public void FloatDamage(int damage, Transform trans)
    {
        HUDText.NewText("- " + damage, trans, Color.red, 28, 20f, -1f, 2.2f, bl_Guidance.Up);
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

        // 飘字设置
        HUDText.CanvasParent = GameObject.Find("Canvas").transform;

        // 复活倒计时
        TextRebirth.text = ServerConfig.HeroRebirthCD.ToString();
        TextRebirth.gameObject.SetActive(false);

        // 技能设置
        DtoSkill skill = null;
        for (int i = 0; i < hero.Skills.Length; i++)
        {
            skill = hero.Skills[i];
            if (skill != null)
            {
                Skills[i].gameObject.SetActive(true);
                Skills[i].GetComponent<ItemSkill>().Init(skill);
            }
        }
    }

    /// <summary>
    /// 刷新界面
    /// </summary>
    /// <param name="hero"></param>
    public void UpdateView()
    {
        DtoHero hero = GameData.HeroData;

        // 更新状态条
        BarExp.value = (float) hero.Exp / (hero.Level * 300);
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

        // 更新装备栏
        for (int i = 0; i < hero.Equipments.Length; i++)
        {
            if (hero.Equipments[i] != -1)
            {
                Equipments[i].sprite = ResourcesManager.Instance.GetAsset(Paths.RES_EQUIPMENT_UI + hero.Equipments[i]) as Sprite;
                Equipments[i].color = Color.white;
            }
        }

        // 更新技能栏的升级状态
        if (GameData.HeroData.SP > 0)
        {
            DtoSkill skill = null;
            for (int i = 0; i < hero.Skills.Length; i++)
            {
                skill = hero.Skills[i];
                if (skill != null)
                {
                    ItemSkill item = Skills[i].GetComponent<ItemSkill>();
                    item.UpdataBtnUp();
                }
            }
        }
    }

    #region 复活倒计时

    /// <summary>
    /// 是否开始计时
    /// </summary>
    private bool m_StartRebirth;

    /// <summary>
    /// 开始复活计时
    /// </summary>
    public void StartRebirthCD()
    {
        m_StartRebirth = true;
        m_RebirthCount = ServerConfig.HeroRebirthCD;
        TextRebirth.gameObject.SetActive(true);
        //StartCoroutine(RebirthCD());
    }

    private float m_RebirthCount;
    void Update()
    {
        if (m_StartRebirth)
        {
            m_RebirthCount -= Time.deltaTime;
            TextRebirth.text = Mathf.RoundToInt(m_RebirthCount).ToString();

            if (m_RebirthCount <= 0)
            {
                m_StartRebirth = false;
                TextRebirth.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator RebirthCD()
    {
        while (m_StartRebirth)
        {
            m_RebirthCount -= Time.deltaTime;
            TextRebirth.text = Mathf.RoundToInt(m_RebirthCount).ToString();

            if (m_RebirthCount <= 0)
            {
                m_StartRebirth = false;
                TextRebirth.gameObject.SetActive(false);
            }
        }
        yield return null;
    }

    #endregion

    public void OnBtnShopClick()
    {
        UIManager.Instance.ShopPanel(UIPanelType.Shop);
    }

    void IResourceListener.OnLoaded(string assetName, object asset)
    {
        ImgHead.sprite = asset as Sprite;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        InitView();
    }
}
