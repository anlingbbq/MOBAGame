using System.Collections;
using System.Collections.Generic;
using Common.DTO;
using UnityEngine;
using UnityEngine.UI;

public class FriendListPanel : UIBasePanel
{
    [SerializeField]
    private GameObject ItemFriend;

    [SerializeField]
    private Transform GridLayout;

    // 保存好友列表
    private List<FriendItem> m_ItemList = new List<FriendItem>();

    void Start()
    {
        List<DtoFriend> friends = GameData.player.Friends;
        GameObject go = null;
        foreach (DtoFriend friend in friends)
        {
            go = Instantiate(ItemFriend);
            go.transform.SetParent(GridLayout);
            FriendItem item = go.GetComponent<FriendItem>();
            item.InitItem(friend.Name, friend.IsOnline);

            m_ItemList.Add(item);
        }
    }
}
