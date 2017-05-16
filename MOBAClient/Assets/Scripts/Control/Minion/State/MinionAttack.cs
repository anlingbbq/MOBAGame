using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAttack : AttackState
{
    private float m_WaitTime;
    private AIBaseRadar m_Radar;

    public override void EnterState()
    {
        base.EnterState();
        m_WaitTime = 0;
        m_Radar = ((MinionCtrl) m_Ctrl).Radar;
    }

    /// <summary>
    /// 小兵攻击逻辑
    /// 这里有个问题：自身死亡的消息是异步的，
    /// 在这个while循环内的任何地方都有可能经过了死亡的处理，导致一些问题
    /// 最好是在死亡处理时能退出这个循环，但是没想到解决办法，只能见一个解决一个。。
    /// </summary>
    /// <returns></returns>
    public override IEnumerator RunLogic()
    {
        while (!m_Quit)
        {
            if (m_Ctrl.Model.CurHp > 0)
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
                        m_WaitTime += Time.deltaTime;
                        if (m_WaitTime >= 1)
                        {
                            m_WaitTime = 0;
                            // 重新寻找最近的敌人
                            m_Radar.FindRecentlyEnemy();
                        }
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
            }
         
            yield return null;
        }
    }
}
