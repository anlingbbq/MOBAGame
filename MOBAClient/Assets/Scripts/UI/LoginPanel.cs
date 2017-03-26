using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
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
            MaskLayer.Instance.Show();
        }
        else
        {
            TextPrompt.text = "用户名或密码不能为空";
        }
    }

    public void OnBtnRegisterClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        RegisterPanel.SetActive(true);
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
            MessageTip.Instance.Show("用户名或密码错误");
        }
    }

    public void ResetPanel()
    {
        InputUsername.text = "";
        InputPassword.text = "";
        TextPrompt.text = "";
    }
}
