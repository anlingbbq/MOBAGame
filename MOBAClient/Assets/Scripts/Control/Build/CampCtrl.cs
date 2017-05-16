using System.Collections;
using System.Collections.Generic;
using Common.Dto;
using UnityEngine;

public class CampCtrl : AIBaseCtrl
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
        gameObject.SetActive(false);
    }

    public override void RebirthResponse()
    {
        gameObject.SetActive(true);
    }
}
