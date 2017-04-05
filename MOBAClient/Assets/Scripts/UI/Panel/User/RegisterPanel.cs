using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Code;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 注册用户的界面
/// </summary>
public class RegisterPanel : UIBasePanel
{
    [SerializeField]
    private InputField InputUsername;
    [SerializeField]
    private InputField InputPassword;
    [SerializeField]
    private InputField InputRepeat;
    [SerializeField]
    private Text TextPrompt;

    private UserRegisterRequest m_RegisterRequest;

    void Start()
    {
        m_RegisterRequest = GetComponent<UserRegisterRequest>();
    }

    #region 点击回掉

    /// <summary>
    /// 点击注册
    /// </summary>
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

    /// <summary>
    /// 点击屏蔽层
    /// </summary>
    public void OnMaskLayerClick()
    {
        UIManager.Instance.PopPanel();
    }

    #endregion

    #region 服务器响应

    /// <summary>
    /// 注册响应
    /// </summary>
    /// <param name="response"></param>
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

    #endregion

    public void ResetPanel()
    {
        InputUsername.text = "";
        InputPassword.text = "";
        InputRepeat.text = "";
        TextPrompt.text = "";
    }
}
