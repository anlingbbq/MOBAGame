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
    /// 播放闲置状态
    /// </summary>
    public void Idle()
    {
        animator.SetBool("walk", false);
        animator.SetBool("attack", false);
    }

    /// <summary>
    /// 播放攻击状态
    /// </summary>
    public void Attack()
    {
        animator.SetBool("walk", false);
        animator.SetBool("attack", true);
    }

    /// <summary>
    /// 播放移动动画
    /// </summary>
    public void Move()
    {
        animator.SetBool("attack", false);
        animator.SetBool("walk", true);
    }

    /// <summary>
    /// 播放死亡动画
    /// </summary>
    public void Death()
    {
        animator.SetTrigger("death");
    }
}