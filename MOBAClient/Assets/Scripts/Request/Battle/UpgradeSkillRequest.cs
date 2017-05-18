using Common.Code;
using Common.Dto;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using LitJson;
using MOBAClient;

public class UpgradeSkillRequest : BaseRequest
{
    /// <summary>
    /// 发送请求的ui
    /// </summary>
    private ItemSkill m_ItemSkill;

    /// <summary>
    /// 发送技能升级的请求
    /// </summary>
    /// <param name="skillId">技能id</param>
    /// <param name="item">发送请求的ui</param>
    public void SendUpgradeSkill(int skillId, ItemSkill item)
    {
        Dictionary<byte, object> data = new Dictionary <byte, object>();
        data.Add((byte)ParameterCode.SkillId, skillId);
        SendRequest(data);

        m_ItemSkill = item;
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        int heroId = (int) response[(byte) ParameterCode.PlayerId];

        // 是自身才有更新的必要
        if (GameData.HeroData.Id == heroId)
        {
            // 更新数据
            DtoSkill skill = JsonMapper.ToObject<DtoSkill>(response[(byte)ParameterCode.DtoSkill] as string);
            GameData.HeroData.SP--;
            // 更新ui
            m_ItemSkill.UpdateView(skill);
        }
    }
}
