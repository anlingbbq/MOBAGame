using System.Collections.Generic;
using Common.Code;
using ExitGames.Client.Photon;
using LitJson;
using MOBAClient;

public class UseSkillRequest : BaseRequest
{
    public void SendUseSkill(int skillId, int level, int from, int[] target)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.SkillId, skillId);
        data.Add((byte)ParameterCode.SkillLevel, level);
        data.Add((byte)ParameterCode.FromId, from);
        if (target != null)
        {
            data.Add((byte)ParameterCode.TargetArray, JsonMapper.ToJson(target));
        }
            

        SendRequest(data);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        // 获取使用者数据
        AIBaseCtrl from = BattleData.Instance.CtrlDict.ExTryGet((int)response[(byte)ParameterCode.FromId]);
        if (from == null)
            return;

        // 获取目标数据
        AIBaseCtrl[] to = null;
        string toStr = response[(byte)ParameterCode.TargetArray] as string;
        if (toStr != null)
        {
            int[] toIds = JsonMapper.ToObject<int[]>(toStr);
            to = new AIBaseCtrl[toIds.Length];
            for (int i = 0; i < toIds.Length; i++)
            {
                to[i] = BattleData.Instance.CtrlDict[toIds[i]];
            }
        }

        // 使用技能
        int skillId = (int) response[(byte)ParameterCode.SkillId];
        int level = (int)response[(byte)ParameterCode.SkillLevel];
        SkillManager.Instance.UseSkill(skillId, level, from, to);
    }
}
