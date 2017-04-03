using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendItem : MonoBehaviour
{
    [SerializeField]
    private Text TextName;
    [SerializeField]
    private Text TextState;

    // 好友项的名称
    public string Name
    {
        get { return TextName.text; }
    }

    public void SetItem(string name, bool isOnline)
    {
        TextName.text = name;
        if (isOnline)
        {
            TextState.text = "在线";
            TextState.color = Color.green;
        }
        else
        {
            TextState.text = "离线";
            TextState.color = Color.red;
        }
    }

    public void SetOnline(bool isOnline)
    {
        if (isOnline)
        {
            TextState.text = "在线";
            TextState.color = Color.green;
        }
        else
        {
            TextState.text = "离线";
            TextState.color = Color.red;
        }
    }
}
