using System;
using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Dto;
using ExitGames.Client.Photon;
using LitJson;
using UnityEngine;

public class HeroMoveRequest : BaseRequest
{
    public override void OnOperationResponse(OperationResponse response)
    {
        // 获取id 玩家id同时也是英雄唯一标识id
        int heroId = (int)response.Parameters[(byte)ParameterCode.PlayerId];
        // 获取控制器
        BaseCtrl ctrl = BattleData.Instance.CtrlDict.ExTryGet(heroId);
    
        // 获取玩家移动的位置
        DtoVector3 point = JsonMapper.ToObject<DtoVector3>(
            response.Parameters[(byte)ParameterCode.DtoVector3] as string);

        ctrl.LostTarget();
        ctrl.Move(new Vector3((float)point.X, (float)point.Y, (float)point.Z));
    }

    public void SendHeroMove(Vector3 point)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        DtoVector3 dto = new DtoVector3(point.x, point.y, point.z);
        data.Add((byte)ParameterCode.DtoVector3, JsonMapper.ToJson(dto));
        SendRequest(data);
    }
}
