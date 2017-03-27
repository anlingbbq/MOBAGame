using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : UIBasePanel
{
    public InputField InputUsername;
    public InputField InputPassword;
    public Text TextPrompt;

    public GameObject RegisterPanel;

    private LoginRequest m_LoginRequest;

	void Start ()
	{
	    m_LoginRequest = GetComponent<LoginRequest>();
        InputUsername.ActivateInputField();

        SoundManager.Instance.LoadLoginSound();
	}

    public void OnBtnLoginClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        if (!string.IsNullOrEmpty(InputUsername.text)
            && !string.IsNullOrEmpty(InputPassword.text))
        {
            m_LoginRequest.Username = InputUsername.text;
            m_LoginRequest.Password = InputPassword.text;
            m_LoginRequest.DefalutRequest();

            ResetPanel();

            UIManager.Instance.PushPanel(UIPanelType.Mask);
        }
        else
        {
            TextPrompt.text = "用户名或密码不能为空";
        }
    }

    public void OnBtnRegisterClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        UIManager.Instance.PushPanel(UIPanelType.Register);
    }

    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Suceess)
        {
            // 跳转场景
            SoundManager.Instance.PlayEffectMusic(Paths.UI_ENTERGAME);
        }
        else
        {
            // 关闭遮罩界面
            UIManager.Instance.PopPanel();

            TipPanel.SetContent("用户名或密码错误");
            UIManager.Instance.PushPanel(UIPanelType.Tip);
        }
    }

    public void ResetPanel()
    {
        InputUsername.text = "";
        InputPassword.text = "";
        TextPrompt.text = "";
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // 这里有时间改成闪光效果 TODO
        Vector3 temp = transform.localPosition;
        temp.y = Screen.height / 2 + GetComponent<RectTransform>().sizeDelta.y / 2;
        transform.localPosition = temp;
        transform.DOLocalMoveY(0, 1.5f).SetEase(Ease.OutBack);
    }
}
