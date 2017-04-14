using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动画控制
/// </summary>
public class AnimeCtrl : MonoBehaviour
{
    /// <summary>
    /// 动画状态机
    /// </summary>
    [SerializeField]
    private Animator animator;

    /// <summary>
    /// 当前动画状态
    /// </summary>
    public AnimeState State { get; private set; }

    /// <summary>
    /// 播放闲置状态
    /// </summary>
    public void Free()
    {
        animator.SetBool("walk", false);
        State = AnimeState.free;
    }

    /// <summary>
    /// 播放攻击状态
    /// </summary>
    public void Attack()
    {
        animator.SetBool("walk", false);
        animator.SetTrigger("attack");
        State = AnimeState.attack;
    }

    /// <summary>
    /// 播放移动动画
    /// </summary>
    public void Walk()
    {
        animator.SetBool("walk", true);
        State = AnimeState.walk;
    }

    /// <summary>
    /// 播放死亡动画
    /// </summary>
    public void Death()
    {
        animator.SetBool("Death", true);
        State = AnimeState.death;
    }
}

public enum AnimeState
{
    free,
    walk,
    attack,
    death
}
