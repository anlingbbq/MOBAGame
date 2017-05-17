using Common.Code;
using Common.Dto;
using ExitGames.Client.Photon;
using UnityEngine;

/// <summary>
/// 这里升级的数据直接在客户端写了
/// 升级的经验为 level * 300
/// 越写越随便。。
/// </summary>
public class GetRewardRequest : BaseRequest
{
    public override void OnOperationResponse(OperationResponse response)
    {
        // 获取击杀队伍和击杀者
        int teamId = (int)response[(byte) ParameterCode.TeamId];
        int fromId = (int)response[(byte)ParameterCode.FromId];

        // 获得经验
        if (teamId == GameData.HeroCtrl.Model.Team)
        {
            DtoHero hero = GameData.HeroCtrl.Model as DtoHero;
            hero.Exp += (int)response[(byte)ParameterCode.GetExp];
            if (hero.Exp >= hero.Level*300)
            {
                hero.Level++;
                hero.SP++;
                hero.Exp -= hero.Level*300;
            }

            // 获得金币
            if (fromId == hero.Id)
            {
                // 金币音效
                SoundManager.Instance.PlayEffectMusic(Paths.UI_BUY);
                // 更新数据
                hero.Money += (int)response[(byte)ParameterCode.GetCoins];
            }

            // 更新ui
            (UIManager.Instance.GetPanel(UIPanelType.Battle) as BattlePanel).UpdateView(hero);
        }
    }
}
