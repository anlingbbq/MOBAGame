using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Dto;
using ExitGames.Client.Photon;
using LitJson;
using UnityEngine;

public class SpawnMinionRequest : BaseRequest
{
    /// <summary>
    /// 接收创建的小兵数据
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        DtoMinion[] minions = JsonMapper.ToObject<DtoMinion[]>(response[(byte)ParameterCode.MinionArray] as string);
        BattleData.Instance.SpawnMinion(minions);
    }
}
