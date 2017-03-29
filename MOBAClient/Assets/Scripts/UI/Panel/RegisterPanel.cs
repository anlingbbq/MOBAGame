using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Code;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : UIBasePanel
{
    public InputField InputUsername;
    public InputField InputPassword;
    public InputField InputRepeat;
    public Text TextPrompt;

    private UserRegisterRequest m_RegisterRequest;

    void Start()
    {
        m_RegisterRequest = GetComponent<UserRegisterRequest>();
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

                UIManager.Instance.PushPanel(UIPanelType.Mask);
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

    public void OnRegisterResponse(OperationResponse response)
    {
        // 关闭遮罩界面
        UIManager.Instance.PopPanel();

        if ((ReturnCode)response.ReturnCode == ReturnCode.Suceess)
        {
            TipPanel.SetContent("注册成功", () => UIManager.Instance.PopPanel());
            UIManager.Instance.PushPanel(UIPanelType.Tip);
        }
        else if ((ReturnCode)response.ReturnCode == ReturnCode.Falied)
        {
            TipPanel.SetContent(response.DebugMessage);
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
