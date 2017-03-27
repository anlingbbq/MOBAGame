using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Code;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : UIBasePanel
{
    public InputField InputUsername;
    public InputField InputPassword;
    public InputField InputRepeat;
    public Text TextPrompt;

    private RegisterRequest m_RegisterRequest;

    void Start()
    {
        m_RegisterRequest = GetComponent<RegisterRequest>();
    }

    public void OnBtnRegisterClick()
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        TextPrompt.text = "";
        if (!string.IsNullOrEmpty(InputUsername.text)
            && !string.IsNullOrEmpty(InputPassword.text))
        {
            if (string.IsNullOrEmpty(InputRepeat.text)
                || InputRepeat.text == InputPassword.text)
            {
                m_RegisterRequest.Username = InputUsername.text;
                m_RegisterRequest.Password = InputPassword.text;
                m_RegisterRequest.DefalutRequest();

                ResetPanel();
            }
            else
            {
                TextPrompt.text = "两次密码不相同";
            }
        }
        else
        {
            TextPrompt.text = "用户名或密码不能为空";
        }
    }

    public void OnMaskLayerClick()
    {
        UIManager.Instance.PopPanel();
    }

    public void OnRegisterResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Suceess)
        {
            TipPanel.SetContent("注册成功");
            UIManager.Instance.PushPanel(UIPanelType.Tip);
        }
        else if (returnCode == ReturnCode.Falied)
        {
            TipPanel.SetContent("用户已存在");
            UIManager.Instance.PushPanel(UIPanelType.Tip);
        }
    }

    public void ResetPanel()
    {
        InputUsername.text = "";
        InputPassword.text = "";
        InputRepeat.text = "";
        TextPrompt.text = "";
    }
}
