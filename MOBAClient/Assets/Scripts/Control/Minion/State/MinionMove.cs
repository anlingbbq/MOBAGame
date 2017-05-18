using System.Collections;

public class MinionMove : MoveState
{
    private AIBaseRadar m_Radar;

    public override void EnterState()
    {
        base.EnterState();

        m_Radar = ((MinionCtrl)m_Ctrl).Radar;
    }

    public override IEnumerator RunLogic()
    {
        while (!m_Quit)
        {
            if (m_Ctrl.Model.CurHp > 0)
            {
                if (!m_Ctrl.IsMoving)
                    m_Ctrl.Move(((MinionCtrl)m_Ctrl).EndPoint);

                // 检测攻击目标
                if (m_Ctrl.Target == null || m_Ctrl.Target.Model == null || m_Ctrl.Target.Model.CurHp <= 0)
                {
                    AIBaseCtrl target = m_Radar.FindRecentlyEnemy();
                    if (target != null)
                    {
                        m_Ctrl.Target = target;
                        m_Ctrl.ChangeState(AIStateEnum.ATTACK);
                    }
                }
            }

            yield return null;
        }
    }
}
