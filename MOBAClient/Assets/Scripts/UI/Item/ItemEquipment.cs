using System;
using System.Collections;
using System.Collections.Generic;
using Common.Config;
using UnityEngine;
using UnityEngine.UI;

public class ItemEquipment : MonoBehaviour, IResourceListener
{
    [SerializeField]
    private Image ImageIcon;

    [SerializeField]
    private Text TextPrice;

    /// <summary>
    /// 道具id 由服务器传递
    /// </summary>
    public int ItemId;

    /// <summary>
    /// 描述信息
    /// </summary>
    public string Description;

    public void InitView(ItemModel model)
    {
        // 保存id
        ItemId = model.Id;

        // 加载物品图标
        ResourcesManager.Instance.Load(Paths.RES_EQUIPMENT_UI + ItemId, typeof(Sprite), this);
        // 显示价格
        TextPrice.text = model.Price.ToString();

        // 描述
        Description = model.Name + ":\n";
        if (model.Attack != 0)
        {
            Description += "攻击力+" + model.Attack + "  ";
        }
        if (model.Defense != 0)
        {
            Description += "防御力+" + model.Defense + "  ";
        }
        if (model.Hp != 0)
        {
            Description += "生命值+" + model.Hp + "  ";
        }

        // 设置ToggleGroup
        GetComponent<Toggle>().group = transform.parent.GetComponent<ToggleGroup>();
    }

    public void OnClick()
    {
        if (GetComponent<Toggle>().isOn)
        {
            ShopPanel panel = UIManager.Instance.GetPanel(UIPanelType.Shop) as ShopPanel;
            panel.OnEquipmentClick(ItemId);
        }
    }

    public void OnLoaded(string assetName, object asset)
    {
        ImageIcon.sprite = asset as Sprite;
    }
}
