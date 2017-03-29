using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.DTO;
using Common.OpCode;
using DG.Tweening;
using ExitGames.Client.Photon;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : UIBasePanel
{
    public InputField InputUsername;
    public InputField InputPassword;
    public Text TextPrompt;
    public Transform LoginLayer;

    public GameObject RegisterPanel;

    private UserLoginRequest m_LoginRequest;

	void Start ()
	{
	    m_LoginRequest = GetComponent<UserLoginRequest>();
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

    // 获取登陆响应
    public void OnLoginResponse(OperationResponse response)
    {
        if ((ReturnCode)response.ReturnCode == ReturnCode.Suceess)
        {
            // 登陆音效
            SoundManager.Instance.PlayEffectMusic(Paths.UI_ENTERGAME);

            UIManager.Instance.PushPanel(UIPanelType.MainMenu);
        }
        else
        {
            // 关闭遮罩界面
            UIManager.Instance.PopPanel();

            TipPanel.SetContent(response.DebugMessage);
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

        //  TODO 这里有时间改成闪光效果
        Vector3 temp = LoginLayer.localPosition;
        temp.y = Screen.height / 2 + GetComponent<RectTransform>().sizeDelta.y / 2;
        LoginLayer.localPosition = temp;
        LoginLayer.DOLocalMoveY(0, 1.5f).SetEase(Ease.OutBack);
    }
}
