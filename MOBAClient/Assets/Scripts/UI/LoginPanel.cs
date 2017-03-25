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

    private LoginRequest m_LoginRequest;

	void Start ()
	{
	    m_LoginRequest = GetComponent<LoginRequest>();
        InputUsername.ActivateInputField();
	}

    public void OnLoginButtonClick()
    {
        TextPrompt.text = "";
        if (!string.IsNullOrEmpty(InputUsername.text)
            && !string.IsNullOrEmpty(InputPassword.text))
        {
            m_LoginRequest.Username = InputUsername.text;
            m_LoginRequest.Password = InputPassword.text;
            m_LoginRequest.DefalutRequest();
        }
        else
        {
            TextPrompt.text = "用户名或密码不能为空";
        }
    }

    public void OnRegisterButtonClick()
    {
        
    }

    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Suceess)
        {
            // 跳转场景
            TextPrompt.text = "登陆成功";
        }
        else
        {
            TextPrompt.text = "用户名或密码错误";
        }
    }
}
