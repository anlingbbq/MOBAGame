using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 屏蔽界面 UIManager中会屏蔽上一个界面的控制
/// </summary>
public class MaskPanel : UIBasePanel
{
    [SerializeField]
    private GameObject Layer;

    // 遮罩界面的点击事件
    public static Action OnClickHandler;

    public void OnClick()
    {
        if (OnClickHandler != null)
        {
            OnClickHandler();
            OnClickHandler = null;
        }
    }
}
