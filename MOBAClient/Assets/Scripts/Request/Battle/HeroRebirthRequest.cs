using Common.Code;
using ExitGames.Client.Photon;
using UnityEngine;

public class HeroRebirthRequest : BaseRequest
{
    public override void OnOperationResponse(OperationResponse response)
    {
        int heroId = (int) response[(byte)ParameterCode.HeroId];
        if (heroId == GameData.HeroData.Id)
        {
            // 隐藏遮罩
            UIManager.Instance.HidePanel(UIPanelType.Mask);
        }

        // 复活英雄
        BattleData.Instance.RebirthHero(heroId);
    }
}
