using System.Collections;
using System.Collections.Generic;
using Common.Code;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 回应添加好友的界面
/// </summary>
public class AddToClientPanel : UIBasePanel
{
    [SerializeField]
    private Text TextName;

    // 请求的玩家名称
    [HideInInspector]
    public string FromName;

    // 请求的玩家id
    [HideInInspector]
    public int FromId;

    private PlayerAddToClientRequest m_AddRequest;

    void Start()
    {
        m_AddRequest = GetComponent<PlayerAddToClientRequest>();
    }

    /// <summary>
    /// 按钮回调
    /// </summary>
    /// <param name="isAccept"></param>
    public void OnBtnClick(bool isAccept)
    {
        SoundManager.Instance.PlayEffectMusic(Paths.UI_CLICK);

        IsAcceptFriend(isAccept);
    }

    /// <summary>
    /// 这里处理的是 服务器对PlayerAddRequest的响应
    /// 将请求添加好友的玩家数据发送过来
    /// </summary>
    /// <param name="response"></param>
    public void OnOperationResponse(OperationResponse response)
    {
        this.ShowPanel();

        FromName = response.Parameters.ExTryGet((byte)(ParameterCode.PlayerName)) as string;
        FromId = (int) response.Parameters.ExTryGet((byte) (ParameterCode.PlayerId));
        TextName.text = FromName;

        StartCoroutine(AutoRefuse());
    }

    /// <summary>
    /// 是否添加好友
    /// </summary>
    /// <param name="isAccept"></param>
    public void IsAcceptFriend(bool isAccept)
    {
        m_AddRequest.isAccept = isAccept;
        m_AddRequest.FromName = FromName;
        m_AddRequest.FromId = FromId;
        m_AddRequest.DefalutRequest();

        this.HidePanel();
    }

    /// <summary>
    /// 5秒后自动拒绝
    /// </summary>
    /// <returns></returns>
    IEnumerator AutoRefuse()
    {
        yield return new WaitForSeconds(5);

        IsAcceptFriend(false);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        TextName.text = "";
        StopCoroutine(AutoRefuse());
    }

    public override void OnExit()
    {
        base.OnExit();

        StopCoroutine(AutoRefuse());
    }
}
