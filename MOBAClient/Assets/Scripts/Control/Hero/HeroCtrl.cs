#define TEST

using Common.Dto;
using UnityEngine;

public class HeroCtrl : AIBaseCtrl
{
    public override void Init(DtoMinion model, bool friend)
    {
        base.Init(model, friend);

        // 设置移动速度
        Speed = (float)Model.Speed;

#if !TEST
        // 调整角度
        MiniMapHead.transform.rotation = Quaternion.Euler(90, 0, 0);
        transform.rotation = Model.Team == 1 ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 1);
#endif

        // 初始化状态机
        AddState(new HeroIdel());
        AddState(new HeroMove());
        AddState(new HeroAttack());
        AddState(new HeroDead());
        ChangeState(AIStateEnum.IDLE);
    }

    /// <summary>
    /// 接收攻击响应 保存攻击目标
    /// </summary>
    /// <param name="target"></param>
    public override void AttackResponse(params AIBaseCtrl[] target)
    {
        Target = target[0];
        // 攻击状态
        ChangeState(AIStateEnum.ATTACK);
    }

    /// <summary>
    /// 死亡响应
    /// </summary>
    public override void DeathResponse()
    {
        base.DeathResponse();
        // 死亡状态
        ChangeState(AIStateEnum.DEAD);
    }

    /// <summary>
    /// 复活响应
    /// </summary>
    public override void RebirthResponse()
    {
        base.RebirthResponse();

        Model.CurHp = Model.MaxHp;
        OnHpChange();

        SetAgent(true);

        AnimeCtrl.Rebirth();
        ChangeState(AIStateEnum.IDLE);

        if (GameData.HeroData.Id == Model.Id)
        {
            // 焦距到自己的英雄
            Camera.main.GetComponent<CameraCtrl>().FocusOn(GameData.HeroCtrl.transform);
        }
    }

    void Update()
    {
        if (MiniMapHead.gameObject.activeSelf)
        {
            // 固定小地图头像角度
            MiniMapHead.transform.rotation = Quaternion.Euler(90, 180, 0);
        }
    }
}
