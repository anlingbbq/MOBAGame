using System.Collections;
using UnityEngine;

/// <summary>
/// 攻击状态视为一个持续的状态
/// 当没有到达攻击距离时 移动至目标再攻击，移动过程同样视为攻击状态
/// </summary>
public class HeroAttack : AttackState
{
    public override IEnumerator RunLogic()
    {
        while(!m_Quit)
        {
            if (m_Ctrl.Target == null || m_Ctrl.Target.Model.CurHp <= 0)
            {
                m_Ctrl.ChangeState(AIStateEnum.IDLE);
            }
            else
            {
                // 获取与目标的距离
                float distance = Vector2.Distance(m_Ctrl.transform.position, m_Ctrl.Target.transform.position);

                // 超过攻击距离
                if (distance > m_Ctrl.Model.AttackDistance)
                {
                    // 移动至目标位置
                    m_Ctrl.MoveTo(m_Ctrl.Target.transform.position);
                }
                // 直接攻击
                else
                {
                    m_Ctrl.StopMove();
                    m_Ctrl.transform.LookAt(m_Ctrl.Target.transform);
                    m_Ctrl.AnimeCtrl.Attack();
                }
            }

            yield return null;
        }
    }
}
