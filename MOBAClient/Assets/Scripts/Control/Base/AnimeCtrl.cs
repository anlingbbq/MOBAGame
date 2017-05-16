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
    private Animator m_Animator;

    #region 动画速度

    /// <summary>
    /// 攻击动画的长度
    /// </summary>
    private float m_AttackLength;
    /// <summary>
    /// 攻击动画的速度
    /// </summary>
    private float m_AttackSpeed = 1;
    /// <summary>
    /// 根据攻击速度计算攻击动画的速度
    /// </summary>
    /// <param name="speed">攻击速度</param>
    public void SetAttackSpeed(float speed)
    {
        if (m_AttackLength == 0)
        {
            // 获取攻击动画的长度
            var clips = m_Animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == "attack")
                {
                    m_AttackLength = clip.length;
                    break;
                }
            }
        }
        // 计算动画速度
        m_AttackSpeed = m_AttackLength / speed;
    }

    #endregion

    #region 播放动画

    /// <summary>
    /// 播放闲置状态
    /// </summary>
    public void Idle()
    {
        m_Animator.speed = 1;

        m_Animator.SetBool("run", false);
        m_Animator.SetBool("attack", false);
    }

    /// <summary>
    /// 播放攻击状态
    /// </summary>
    public void Attack()
    {
        m_Animator.speed = m_AttackSpeed;

        m_Animator.SetBool("run", false);
        m_Animator.SetBool("attack", true);
    }

    /// <summary>
    /// 播放移动动画
    /// </summary>
    public void Move()
    {
        m_Animator.speed = 1;

        m_Animator.SetBool("attack", false);
        m_Animator.SetBool("run", true);
    }

    /// <summary>
    /// 播放死亡动画
    /// </summary>
    public void Death()
    {
        m_Animator.speed = 1;

        m_Animator.SetTrigger("death");
    }

    #endregion
}