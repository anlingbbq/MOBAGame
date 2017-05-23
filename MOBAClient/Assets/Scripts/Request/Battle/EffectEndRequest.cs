using System;
using System.Collections.Generic;
using Common.Code;
using ExitGames.Client.Photon;

public class EffectEndRequest : BaseRequest
{
    public void SendEffectEnd(string effectKey)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.EffectKey, effectKey);

        SendRequest(data);
    }

    public override void OnOperationResponse(OperationResponse response)
    {

    }
}
