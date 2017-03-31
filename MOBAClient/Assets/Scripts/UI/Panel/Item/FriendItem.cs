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

    public void InitItem(string name, bool isOnline)
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
}
