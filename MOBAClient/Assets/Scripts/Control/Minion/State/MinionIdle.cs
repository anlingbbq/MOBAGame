using System.Collections;

public class MinionIdle : IdleState
{
    public override IEnumerator RunLogic()
    {
        yield return null;
        // 移动到终点
        m_Ctrl.Move(((MinionCtrl)m_Ctrl).EndPoint);
    }
}
