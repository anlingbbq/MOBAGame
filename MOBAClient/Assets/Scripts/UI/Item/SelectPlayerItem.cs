using System.Collections;
using System.Collections.Generic;
using Common.Dto;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerItem : MonoBehaviour
{
    [SerializeField]
    private Text TextName;
    [SerializeField]
    private Image ImageBg;
    [SerializeField]
    private Text TextState;
    [SerializeField]
    private Image ImageHead;

    /// <summary>
    /// 更新视图
    /// </summary>
    /// <param name="model"></param>
    public void UpdateView(SelectModel model)
    {
        TextName.text = model.PlayerName;
        // 是否进入
        if (!model.IsEnter)
        {
            ImageBg.color = Color.white;
            ImageHead.sprite = (Sprite) ResourcesManager.Instance.
                GetAsset(Paths.HEAD_NO_CONNECT);
            return;
        }
        // 是否有选择的英雄
        if (model.HeroId == -1)
        {
            // 隐藏头像
            ImageHead.color = new Color(1, 1, 1, 0);
        }
        else
        {
            // 显示头像
            ImageHead.color = Color.white;
            //ImageHead.sprite.a
        }
        // 是否准备
        if (model.IsReady)
        {
            ImageBg.color = Color.green;
            TextState.text = "已选择";
        }
        else
        {
            ImageBg.color = Color.white;
            TextState.text = "选择中";
        }
    }
}
