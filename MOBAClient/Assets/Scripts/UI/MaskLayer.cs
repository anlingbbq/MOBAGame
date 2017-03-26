using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 屏蔽层 发送消息时先启用屏蔽按钮等事件
/// </summary>
public class MaskLayer : Singlton<MaskLayer>
{
    [SerializeField]
    private GameObject Layer;

    private Action OnClickCallback;

    public void Show(Action action = null)
    {
        Layer.SetActive(true);
        OnClickCallback = action;
    }

    public void Hide()
    {
        Layer.SetActive(false);
    }

    public void OnClick()
    {
        if (OnClickCallback != null)
        {
            OnClickCallback();
            OnClickCallback = null;
        }
    }
}
