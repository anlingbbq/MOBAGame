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

        public void OnBtnRegisterClick()
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

                    ResetPanel();
                    MaskLayer.Instance.Show();
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
                MessageTip.Instance.Show("注册成功");
            }
            else if (returnCode == ReturnCode.Falied)
            {
                MessageTip.Instance.Show("用户已存在");
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
}
