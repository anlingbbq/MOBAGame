//#define TEST

using System.Collections;

public class MinionDead : DeadState
{
    public override void EnterState()
    {
        base.EnterState();
        // 禁用寻路组件
        m_Ctrl.SetAgent(false);
        // 动画
        m_Ctrl.AnimeCtrl.Death();
        // 音效
        m_Ctrl.PlayAudio("death");
        // 关闭雷达
        ((MinionCtrl)m_Ctrl).Radar.Close();

        // 延时回收
        TimerManager.Instance.AddTimer("PoolHide" + m_Ctrl.Model.Id, 10, () =>
        {

#if !TEST
            // 清理数据
            BattleData.Instance.CtrlDict.Remove(m_Ctrl.Model.Id);
#endif
            m_Ctrl.Model = null;

            PoolManager.Instance.HideObjet(m_Ctrl.gameObject);
        });
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
