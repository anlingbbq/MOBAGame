using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyBattleRequest : BaseRequest
{
    /// <summary>
    /// 收到准备开始战斗的消息
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        SceneManager.LoadScene("Battle");
    }
}
