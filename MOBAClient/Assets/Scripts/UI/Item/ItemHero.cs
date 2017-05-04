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
    
    private AudioClip m_HeroSound;

    /// <summary>
    /// 英雄名称
    /// </summary>
    private string HeroName;

    void Start()
    {
        BtnHead.onClick.AddListener(OnClick);
    }

    public void InitView(HeroModel hero)
    {
        // 保存英雄数据
        HeroId = hero.TypeId;
        HeroName = hero.Name;

        // 加载选择的音效资源
        ResourcesManager.Instance.Load(Paths.RES_SOUND_SELECT + HeroName, typeof(AudioClip));

        // 获取头像资源
        ResourcesManager.Instance.Load(Paths.RES_HEAD_UI + HeroName, typeof(Sprite), this);
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
        // 播放英雄音效
        SoundManager.Instance.PlayEffectMusic(m_HeroSound);

        // 发送选人的请求
        SelectPanel panel = UIManager.Instance.GetPanel(UIPanelType.Select) as SelectPanel;
        panel.OnSelectHeroClick(HeroId);
    }

    public void OnLoaded(string assetName, object asset)
    {
        if (assetName == Paths.RES_SOUND_SELECT + HeroName)
        {
            m_HeroSound = asset as AudioClip;
        }
        else if (assetName == Paths.RES_HEAD_UI + HeroName)
        {
            ImageHead.sprite = asset as Sprite;
        }
    }
}
