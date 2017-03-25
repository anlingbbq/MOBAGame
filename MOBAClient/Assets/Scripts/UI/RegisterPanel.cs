using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Code;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class RegisterPanel : MonoBehaviour
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

        public void OnRegisterButtonClick()
        {
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

        public void OnRegisterResponse(ReturnCode returnCode)
        {
            if (returnCode == ReturnCode.Suceess)
            {
                TextPrompt.text = "注册成功";
            }
            else if (returnCode == ReturnCode.Falied)
            {
                TextPrompt.text = "用户已存在";
            }
        }
    }
}
