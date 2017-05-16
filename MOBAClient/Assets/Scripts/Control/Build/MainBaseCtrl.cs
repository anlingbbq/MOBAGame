using System.Collections;
using System.Collections.Generic;
using Common.Dto;
using UnityEngine;

public class MainBaseCtrl : AIBaseCtrl
{
    public override void Init(DtoMinion model, bool friend)
    {
        base.Init(model, friend);
        // 设置小地图头像颜色
        if (friend)
            MiniMapHead.color = Color.blue;
        else
            MiniMapHead.color = Color.red;
    }

    public override void DeathResponse()
    {
        base.DeathResponse();

        // 播放死亡动画
        GetComponent<Animation>().CrossFade("death");
    }
}
