using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorCtrl : BaseCtrl, IResourceListener
{
    protected override void Start()
    {
        base.Start();
        ResourcesManager.Instance.Load(Paths.RES_SOUND_BATTLE + "WarriorAttack", typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_SOUND_BATTLE + "WarriorDeath", typeof(AudioClip), this);
    }

    public override void Attack()
    {
        base.Attack();

        // 只发送自己的攻击
        if (m_Target == null || this != GameData.HeroCtrl)
            return;

        // 获取目标id
        int targetId = m_Target.GetComponent<BaseCtrl>().Model.Id;
        // 请求计算伤害
        BattleRoot.Instance.RequestDamage(targetId);
    }

    /// <summary>
    /// 接收攻击响应 保存攻击目标
    /// </summary>
    /// <param name="target"></param>
    public override void AttackResponse(params Transform[] target)
    {
        m_Target = target[0].GetComponent<BaseCtrl>();
    }

    public override void DeathResponse()
    {
        base.DeathResponse();

        // 停止寻路
        m_Agent.Stop();
        // 播放死亡动画
        m_AnimeCtrl.Death();
        // 播放音效
        PlayAudio("death");

        // 添加到死亡层
        gameObject.layer = LayerMask.NameToLayer("Death");
    }

    protected override void Update()
    {
        base.Update();

        if (m_Target != null)
        {
            // 获取与目标的距离
            float distance = Vector2.Distance(transform.position, m_Target.transform.position);

            // 超过攻击距离
            if (distance > Model.AttackDistance)
            {
                // 移动至目标位置
                Move(m_Target.transform.position);
            }
            // 直接攻击
            else
            {
                // 停止寻路
                m_Agent.Stop();
                // 面向目标
                transform.LookAt(m_Target.transform);
                // 播放动画
                m_AnimeCtrl.Attack();
            }
        }
    }

    public void OnLoaded(string assetName, object asset, AssetType assetType)
    {
        switch (assetName)
        {
            case Paths.RES_SOUND_BATTLE + "WarriorAttack":
                m_ClipDict.Add("attack", asset as AudioClip);
                break;
            case Paths.RES_SOUND_BATTLE + "WarriorDeath":
                m_ClipDict.Add("death", asset as AudioClip);
                break;
        }
    }
}
    