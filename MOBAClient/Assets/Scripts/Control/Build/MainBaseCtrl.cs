using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBaseCtrl : AIBaseCtrl
{
    public override void DeathResponse()
    {
        base.DeathResponse();

        // 播放死亡动画
        GetComponent<Animation>().CrossFade("death");
    }
}
