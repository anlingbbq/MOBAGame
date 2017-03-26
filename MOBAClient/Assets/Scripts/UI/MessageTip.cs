using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageTip : Singleton<MessageTip>
{
    [SerializeField]
    private Text TextConent;
    [SerializeField]
    private GameObject Tip;

    // 点击确定按钮后回掉
    private Action onCompleted;

    public void Show(string text, Action action = null)
    {
        Tip.SetActive(true);
        TextConent.text = text;
        onCompleted = action;
    }

    public void OnBtnOkClick()
    {
        if (onCompleted != null)
        {
            onCompleted();
            onCompleted = null;
        }
        Tip.SetActive(false);
        MaskLayer.Instance.Hide();
    }
}
