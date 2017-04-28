using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MOBAClient;

public class AIState
{
    /// <summary>
    /// ai控制器
    /// </summary>
    protected AIBaseCtrl m_Ctrl;
    /// <summary>
    /// 是否退出
    /// </summary>
    protected bool m_Quit = false;

    /// <summary>
    /// 当前状态
    /// </summary>
    public AIStateEnum Type = AIStateEnum.INVALID;

    /// <summary>
    /// 设置控制器
    /// </summary>
    /// <param name="ai"></param>
    public void SetCtrl(AIBaseCtrl ctrl)
    {
        m_Ctrl = ctrl;
    }

    /// <summary>
    /// 进入状态调用
    /// </summary>
    public virtual void EnterState()
    {
        m_Quit = false;
    }

    /// <summary>
    /// 离开状态调用
    /// </summary>
    public virtual void ExitState()
    {
        m_Quit = true;
    }

    /// <summary>
    /// 是否可以进入下一个状态
    /// </summary>
    /// <param name="next"></param>
    /// <returns></returns>
    public virtual bool CheckNextState(AIStateEnum next)
    {
        return true;
    }

    /// <summary>
    /// 执行该状态下的逻辑
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator RunLogic()
    {
        // 这个用法很有趣 类似于update
        while (!m_Quit)
        {
            // Logic
            yield return null;
        }
    }
}

public enum AIStateEnum
{
    INVALID = -1,
    IDLE,
    ATTACK,
    DEAD,
    MOVE,
}

public class IdleState : AIState
{
    public IdleState()
    {
        Type = AIStateEnum.IDLE;
    }

}

public class AttackState : AIState
{
    public AttackState()
    {
        Type = AIStateEnum.ATTACK;
    }
}

public class MoveState : AIState
{
    public MoveState()
    {
        Type = AIStateEnum.MOVE;
    }

}

public class DeadState : AIState
{
    public DeadState()
    {
        Type = AIStateEnum.DEAD;
    }

    public override bool CheckNextState(AIStateEnum next)
    {
        return false;
    }
}
