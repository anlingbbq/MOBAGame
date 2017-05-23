using System.Collections.Generic;
using Common.Code;
using Common.Dto;
using ExitGames.Client.Photon;
using LitJson;
using UnityEngine;

public class BuyItemRequest : BaseRequest
{
    public void SendBuyItem(int itemId)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.ItemId, itemId);
        SendRequest(data);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        if (response.ReturnCode == (byte) ReturnCode.Falied)
            return;

        // 更新数据
        DtoHero hero = JsonMapper.ToObject<DtoHero>(response[(byte) ParameterCode.DtoHero] as string);
        BattleData.Instance.CtrlDict[hero.Id].Model = hero;

        // 如果时自己购买
        if (hero.Id == GameData.HeroCtrl.Model.Id)
        {
            SoundManager.Instance.PlayEffectMusic(Paths.UI_BUY);
            GameData.HeroData = hero;
            (UIManager.Instance.GetPanel(UIPanelType.Battle) as BattlePanel).UpdateView();
        }
    }
}
