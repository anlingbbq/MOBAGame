using System.Collections;

public class HeroDead : DeadState
{
    public override void EnterState()
    {
        base.EnterState();
        // 停止寻路
        m_Ctrl.StopMove();
        // 禁用寻路组件
        m_Ctrl.SetAgent(false);
        // 动画
        m_Ctrl.AnimeCtrl.Death();
        // 音效
        m_Ctrl.PlayAudio("death");
    }

    public override IEnumerator RunLogic()
    {
        yield return null;
    }

    public override bool CheckNextState(AIStateEnum next)
    {
        return true;
    }
}
