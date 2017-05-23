using Common.Code;
using Common.Dto;
using ExitGames.Client.Photon;
using LitJson;
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
        // 获取修改的数据
        DtoHero[] heros = JsonMapper.ToObject<DtoHero[]>(response[(byte)ParameterCode.HerosArray] as string);

        // 更新数据
        for (int i = 0; i < heros.Length; i++)
        {
            DtoHero hero = heros[i];
            AIBaseCtrl ctrl = BattleData.Instance.CtrlDict.ExTryGet(hero.Id);
            if (ctrl == null)
                return;

            ctrl.Model = hero;
            // 更新自己的数据
            if (ctrl.Model.Id == GameData.HeroData.Id)
            {
                GameData.HeroData = hero;
                (UIManager.Instance.GetPanel(UIPanelType.Battle) as BattlePanel).UpdateView();
            }
        }

        // 自己获得金币
        int heroId = (int)response[(byte)ParameterCode.HeroId];
        if (heroId == GameData.HeroData.Id)
        {
            // 金币音效
            SoundManager.Instance.PlayEffectMusic(Paths.UI_BUY);
        }
    }
}
