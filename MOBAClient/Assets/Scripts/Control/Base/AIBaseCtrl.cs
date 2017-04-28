using System;
using System.Collections;
using System.Collections.Generic;
using Common.Dto;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// AI控制器的基类
/// 包括数据、攻击、血条、动画、移动，模块
/// 需要重写攻击，攻击、死亡、复活响应
/// 
/// 状态机功能
/// 状态添加和切换
/// </summary>
public class AIBaseCtrl : MonoBehaviour
{
    /// <summary>
    /// 保存数据对象
    /// </summary>
    public DtoMinion Model { get; set; }

    /// <summary>
    /// 目标
    /// </summary>
    public AIBaseCtrl Target;

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
    public AnimeCtrl AnimeCtrl;

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

    #region 攻击

    /// <summary>
    /// 接收服务器攻击响应
    /// 同步攻击动画 不计算伤害
    /// </summary>
    public virtual void AttackResponse(params Transform[] target)
    {
    }

    /// <summary>
    /// 发送计算伤害的请求
    /// </summary>
    public virtual void AttackRequest()
    {
    }

    #endregion

    #region 音效

    [SerializeField]
    protected AudioSource m_AudioSource;

    protected virtual void Start()
    {
        if (m_AudioSource)
        {
            m_AudioSource.playOnAwake = false;
            m_AudioSource.loop = false;
        }
    }

    /// <summary>
    /// 保存音效文件
    /// </summary>
    protected Dictionary<string, AudioClip> m_ClipDict = new Dictionary<string, AudioClip>();

    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlayAudio(string name)
    {
        if (m_AudioSource == null)
            return;

        AudioClip clip = m_ClipDict.ExTryGet(name);
        if (clip == null)
            return;

        m_AudioSource.clip = clip;
        m_AudioSource.Play();
    }

    #endregion

    #region 死亡

    /// <summary>
    /// 死亡响应
    /// </summary>
    public virtual void DeathResponse()
    {
        // 失去目标
        Target = null;
        // 隐藏血条
        m_HpCtrl.SetBarActive(false);
    }

    /// <summary>
    /// 复活响应
    /// </summary>
    public virtual void RebirthResponse()
    {

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
    public bool IsMoving
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
    /// 切换移动状态
    /// </summary>
    /// <param name="point"></param>
    public void Move(Vector3 point)
    {
        MoveTo(point);
        // 移动状态
        ChangeState(AIStateEnum.MOVE);
    }

    /// <summary>
    /// 移动至指定位置
    /// </summary>
    /// <param name="point"></param>
    public void MoveTo(Vector3 point)
    {
        point.y = transform.position.y;
        // 寻路
        m_Agent.ResetPath();
        m_Agent.SetDestination(point);
        // 动画
        AnimeCtrl.Move();
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMove()
    {
        m_Agent.Stop();
    }

    #endregion

    #region 状态机

    /// <summary>
    /// 保存枚举和状态类的映射
    /// </summary>
    protected Dictionary<AIStateEnum, AIState> StateDict = new Dictionary<AIStateEnum, AIState>();

    /// <summary>
    /// 当前状态
    /// </summary>
    public AIState State { get; set; }

    /// <summary>
    /// 添加状态
    /// </summary>
    /// <param name="state"></param>
    public void AddState(AIState state)
    {
        if (StateDict.ContainsKey(state.Type))
        {
            Log.Error("状态已添加");
            return;
        }

        StateDict.Add(state.Type, state);
        state.SetCtrl(this);
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="type"></param>
    /// <returns>是否可以进入新状态</returns>
    public bool ChangeState(AIStateEnum type)
    {
        // 无法进入新状态
        if (State != null && !State.CheckNextState(type))
        {
            return false;
        }

        // 不包含该状态
        if (!StateDict.ContainsKey(type))
        {
            Log.Error("找不到状态 ：" + Enum.GetName(typeof(AIStateEnum), type));
            return false;
        }

        // 已为当前状态
        if (State != null && State.Type == type)
        {
            return false;
        }

        // 退出当前状态
        if (State != null)
        {
            State.ExitState();
        }

        // 改变状态
        State = StateDict[type];
        State.EnterState();
        StartCoroutine(State.RunLogic());

        return true;
    }

    #endregion
}
