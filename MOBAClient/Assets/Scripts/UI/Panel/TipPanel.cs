using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : UIBasePanel
{
    [SerializeField]
    private Text TextContent;

    public static string Content;
    // 点击确定按钮后回掉
    private static Action onCompleted;

    // ！！ 打开界面之前需要先设置内容
    public static void SetContent(string text, Action action = null)
    {
        Content = text;
        onCompleted = action;
    }

    public void OnBtnOkClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        if (onCompleted != null)
        {
            onCompleted();
            onCompleted = null;
        }
        UIManager.Instance.PopPanel();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        TextContent.text = Content;
    }
}
