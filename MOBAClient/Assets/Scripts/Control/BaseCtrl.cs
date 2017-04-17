using System.Collections;
using System.Collections.Generic;
using Common.Dto;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 战斗模型的基类
/// </summary>
public class BaseCtrl : MonoBehaviour
{
    /// <summary>
    /// 保存数据对象
    /// </summary>
    public DtoMinion Model { get; set; }

    /// <summary>
    /// 目标
    /// </summary>
    [SerializeField]
    protected Transform m_Target;
    /// <summary>
    /// 失去目标
    /// </summary>
    public void LostTarget()
    {
        m_Target = null;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="model"></param>
    /// <param name="friend"></param>
    public void Init(DtoMinion model, bool friend)
    {
        Model = model;

        // 设置血条颜色
        m_HpCtrl.SetColor(friend);

        // 设置层
        if (friend)
            gameObject.layer = LayerMask.NameToLayer("Friend");
        else
            gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    #region 动画

    /// <summary>
    /// 动画控制器
    /// </summary>
    [SerializeField]
    protected AnimeCtrl m_AnimeCtrl;

    #endregion

    #region 生命值

    /// <summary>
    /// 血条控制器
    /// </summary>
    [SerializeField]
    protected HpCtrl m_HpCtrl;

    /// <summary>
    /// 血量变化
    /// </summary>
    public void OnHpChange()
    {
        m_HpCtrl.SetHp((float)Model.CurHp / Model.MaxHp);
    }

    #endregion

    #region 移动

    /// <summary>
    /// 寻路
    /// </summary>
    [SerializeField]
    protected NavMeshAgent m_Agent;

    /// <summary>
    /// 是否在移动
    /// </summary>
    protected bool IsMoving
    {
        get
        {
            return m_Agent.pathPending
                || m_Agent.remainingDistance > m_Agent.stoppingDistance
                || m_Agent.velocity != Vector3.zero
                || m_Agent.pathStatus != NavMeshPathStatus.PathComplete;
        }
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="point"></param>
    public void Move(Vector3 target)
    {
        target.y = transform.position.y;
        // 寻路
        m_Agent.ResetPath();
        m_Agent.SetDestination(target);
        // 播放动画
        m_AnimeCtrl.Walk();
    }

    protected virtual void Update()
    {
        // 检测寻路是否终止
        if (m_AnimeCtrl.State == AnimeState.walk && !IsMoving)
        {
            m_Target = null;
            m_AnimeCtrl.Free();
        }
    }

    #endregion

    #region 攻击

    /// <summary>
    /// 攻击
    /// </summary>
    public virtual void Attack()
    {
        
    }

    /// <summary>
    /// 响应攻击
    /// </summary>
    public virtual void AttackResponse(params Transform[] target)
    {
        
    }

    #endregion

    /// <summary>
    /// 死亡
    /// </summary>
    public virtual void DeathResponse()
    {
        
    }
}
