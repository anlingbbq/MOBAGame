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

/// <summary>
/// 登陆用户的界面
/// </summary>
public class LoginPanel : UIBasePanel, IResourceListener
{
    [SerializeField]
    private InputField InputUsername;
    [SerializeField]
    private InputField InputPassword;
    [SerializeField]
    private Text TextPrompt;
    [SerializeField]
    private Transform LoginLayer;
    [SerializeField]
    private GameObject RegisterPanel;

    private UserLoginRequest m_LoginRequest;

	void Start ()
	{
	    m_LoginRequest = GetComponent<UserLoginRequest>();

        LoadUiSound();
	}

    /// <summary>
    /// 加载UI界面的声音文件 
    /// </summary>
    public void LoadUiSound()
    {
        ResourcesManager.Instance.Load(Paths.UI_ENTERGAME, typeof(AudioClip));
        ResourcesManager.Instance.Load(Paths.UI_CLICK, typeof(AudioClip));
        ResourcesManager.Instance.Load(Paths.UI_READY, typeof(AudioClip));
        ResourcesManager.Instance.Load(Paths.UI_LOGIN_BG, typeof(AudioClip), this, AssetType.SoundBGM);
    }

    /// <summary>
    /// 点击登陆
    /// </summary>
    public void OnBtnLoginClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        if (!string.IsNullOrEmpty(InputUsername.text)
            && !string.IsNullOrEmpty(InputPassword.text))
        {
            m_LoginRequest.SendLoginRequest(InputUsername.text, InputPassword.text);

            ResetPanel();

            UIManager.Instance.PushPanel(UIPanelType.Mask);
        }
        else
        {
            TextPrompt.text = "用户名或密码不能为空";
        }
    }

    /// <summary>
    /// 点击注册
    /// </summary>
    public void OnBtnRegisterClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        UIManager.Instance.PushPanel(UIPanelType.Register);
    }

    /// <summary>
    /// 登陆响应 
    /// </summary>
    /// <param name="response"></param>
    public void OnLoginResponse(OperationResponse response)
    {
        // 关闭遮罩界面
        UIManager.Instance.PopPanel();

        if ((ReturnCode)response.ReturnCode == ReturnCode.Suceess)
        {
            // 登陆音效
            SoundManager.Instance.PlayEffectMusic(Paths.UI_ENTERGAME);

            UIManager.Instance.PushPanel(UIPanelType.MainMenu);
        }
        else
        {
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

        InputUsername.ActivateInputField();

        //  TODO 这里有时间改成闪光效果
        Vector3 temp = LoginLayer.localPosition;
        temp.y = Screen.height / 2 + GetComponent<RectTransform>().sizeDelta.y / 2;
        LoginLayer.localPosition = temp;
        LoginLayer.DOLocalMoveY(0, 1.5f).SetEase(Ease.OutBack);
    }

    public void OnLoaded(string assetName, object asset, AssetType assetType)
    {
        // 播放背景音乐
        if (assetType == AssetType.SoundBGM)
        {
            SoundManager.Instance.PlayBgMusic(asset as AudioClip);
        }
    }
}
