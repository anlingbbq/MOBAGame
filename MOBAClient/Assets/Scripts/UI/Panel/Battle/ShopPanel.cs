using System.Collections.Generic;
using Common.Config;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : UIBasePanel
{
    /// <summary>
    /// 物品预制体
    /// </summary>
    [SerializeField]
    private GameObject ItemEquipment;

    /// <summary>
    /// 装备项父对象
    /// </summary>
    [SerializeField]
    private Transform GridEquipment;

    /// <summary>
    /// 装备描述
    /// </summary>
    [SerializeField]
    private Text TextDescription;

    /// <summary>
    /// 保存所有的道具脚本
    /// </summary>
    private Dictionary<int, ItemEquipment> m_ItemDict = new Dictionary<int, ItemEquipment>();

    /// <summary>
    /// 当前选择的道具id
    /// </summary>
    private int m_SelectedId = -1;

    public override void Awake()
    {
        base.Awake();

        InitView(GameData.Items);
    }

    public void InitView(ItemModel[] models)
    {
        GameObject go;
        for (int i = 0; i < models.Length; i++)
        {
            go = Instantiate(ItemEquipment);
            go.transform.SetParent(GridEquipment);
            go.transform.localScale = Vector3.one;
            ItemEquipment item = go.GetComponent<ItemEquipment>();
            item.InitView(models[i]);

            m_ItemDict.Add(item.ItemId, item);
        }
    }

    public void OnEquipmentClick(int itemId)
    {
        // 双击购买
        if (m_SelectedId == itemId)
        {
            MOBAClient.BattleManager.Instance.RequestBuyItem(itemId);
        }
        // 单击显示描述
        m_SelectedId = itemId;
        TextDescription.text = m_ItemDict.ExTryGet(itemId).Description;
    }

    public void OnCloseClick()
    {
        HidePanel();
    }

    public override void OnEnter()
    {
        UIManager.Instance.ShopPanel(UIPanelType.Mask);

        base.OnEnter();
        SoundManager.Instance.PlayEffectMusic(Paths.UI_BUY);
    }

    public override void OnExit()
    {
        base.OnExit();
        UIManager.Instance.HidePanel(UIPanelType.Mask);
    }
}
