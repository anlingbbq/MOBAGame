using System.Collections;

public class HeroIdel : IdleState
{
    public override void EnterState()
    {
        base.EnterState();
        // 失去目标
        m_Ctrl.Target = null;
        // 空闲动画
        m_Ctrl.AnimeCtrl.Idle();
    }

    public override IEnumerator RunLogic()
    {
        yield return null;
    }
}
