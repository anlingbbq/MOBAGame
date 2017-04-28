using System.Collections;
using UnityEngine;

public class HeroMove : MoveState
{
    public override void EnterState()
    {
        base.EnterState();
        // 播放移动动画
        m_Ctrl.AnimeCtrl.Move();
    }

    public override IEnumerator RunLogic()
    {
        while (!m_Quit)
        {
            // 检测寻路是否终止
            if (!m_Ctrl.IsMoving)
            {
                m_Ctrl.ChangeState(AIStateEnum.IDLE);
            }

            yield return null;
        }
    }
}
