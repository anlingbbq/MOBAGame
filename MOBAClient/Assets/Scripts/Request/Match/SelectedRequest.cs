using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Dto;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class SelectedRequest : BaseRequest
{
    private SelectPanel m_SelectPanel;

    protected override void Start()
    {
        base.Start();
        m_SelectPanel = GetComponent<SelectPanel>();
    }

    /// <summary>
    /// 发送选择的请求
    /// </summary>
    public void SendSelectedRequest(int heroId)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.HeroId, heroId);
        SendRequest(data);
    }

    /// <summary>
    /// 接收服务器是否选择成功的响应
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        if (response.ReturnCode != (short)ReturnCode.Falied)
        {
            int playerId = (int)response.Parameters[(byte)ParameterCode.PlayerId];
            int heroId = (int)response.Parameters[(byte)ParameterCode.HeroId];
            // 刷新队伍数据
            m_SelectPanel.SelectData.OnSelected(playerId, heroId);
            m_SelectPanel.UpdateView();
        }
    }
}
