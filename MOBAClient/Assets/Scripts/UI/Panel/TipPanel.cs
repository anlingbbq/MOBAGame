using System;
using System.Collections;
using System.Collections.Generic;
using Common.Config;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 提示界面
/// 之前的UIManager只能用栈管理面板，对于这样的面板不太方便
/// 后来在UIBasePanel加入和hide和show，脱离了栈
/// 这里还是用之前的方式，懒得改了
/// </summary>
public class TipPanel : UIBasePanel
{
    [SerializeField]
    private Text TextContent;

    /// <summary>
    /// 提示内容
    /// </summary>
    public static string Content;
    /// <summary>
    /// 点击按钮的回掉函数
    /// </summary>
    public static Action OnCompleted;
    /// <summary>
    /// 定时关闭
    /// </summary>
    private static int TimeOff = -1;


    /// <summary>
    /// 打开界面之前需要先设置内容
    /// </summary>
    /// <param name="text">内容</param>
    /// <param name="action">回掉函数</param>
    public static void SetContent(string text, Action action = null, int timeOff = -1)
    {
        Content = text;
        OnCompleted = action;

        if (timeOff != -1)
        {
            TimeOff = timeOff;
            TipPanel panel = UIManager.Instance.GetPanel(UIPanelType.Tip) as TipPanel;
            panel.StartCoroutine(panel.AutoHide(TimeOff));
        }
    }

    public void OnBtnOkClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        // 先隐藏界面
        UIManager.Instance.PopPanel();
        if (TimeOff != -1)
            StopAllCoroutines();

        // 再处理回掉函数
        if (OnCompleted != null)
        {
            OnCompleted();
            OnCompleted = null;
        }
    }

    /// <summary>
    /// 自动隐藏
    /// </summary>
    /// <returns></returns>
    IEnumerator AutoHide(int timeOff)
    {
        yield return new WaitForSeconds(timeOff);
        UIManager.Instance.PopPanel();
    }


    public override void OnEnter()
    {
        base.OnEnter();

        TextContent.text = Content;
    }
}
