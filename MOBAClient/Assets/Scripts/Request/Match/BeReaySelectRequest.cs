using Common.Code;
using ExitGames.Client.Photon;
using UnityEngine;

public class BeReaySelectRequest : BaseRequest
{
    private SelectPanel m_SelectPanel;

    protected override void Start()
    {
        base.Start();
        m_SelectPanel = GetComponent<SelectPanel>();
    }

    /// <summary>
    /// 接收确认选择的响应
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        if (response.ReturnCode != (short) ReturnCode.Falied)
        {
            int playerId = (int) response.Parameters[(byte) ParameterCode.PlayerId];
            // 刷新准备的数据
            SelectData.Instance.OnReady(playerId);
            m_SelectPanel.UpdateView();
        }
    }
}
