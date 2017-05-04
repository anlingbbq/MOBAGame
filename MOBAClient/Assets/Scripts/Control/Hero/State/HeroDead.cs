using System.Collections;
using UnityEngine;

public class HeroDead : DeadState
{
    public override void EnterState()
    {
        base.EnterState();
        // 动画
        m_Ctrl.AnimeCtrl.Death();
        // 音效
        m_Ctrl.PlayAudio("death");
        // 隐藏小地图头像
        m_Ctrl.MiniMapHead.gameObject.SetActive(false);
        // 添加到死亡层
        m_Ctrl.gameObject.layer = LayerMask.NameToLayer("Death");
    }

    public override IEnumerator RunLogic()
    {
        yield return null;
    }
}
