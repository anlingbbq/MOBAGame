using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCheck : MonoBehaviour
{
    /// <summary>
    /// 当前炮塔的队伍
    /// </summary>
    private int m_Team;

    public void SetTeam(int team)
    {
        m_Team = team;
    }

    /// <summary>
    /// 检测到的敌人列表
    /// </summary>
    public List<BaseCtrl> EnemyList = new List<BaseCtrl>();

    private void OnTriggerEnter(Collider other)
    {
        BaseCtrl ctrl = other.GetComponent<BaseCtrl>();
        if (ctrl == null || ctrl.Model.Team == m_Team)
            return;

        EnemyList.Add(ctrl);
    }

    private void OnTriggerExit(Collider other)
    {
        BaseCtrl ctrl = other.GetComponent<BaseCtrl>();
        if (EnemyList.Contains(ctrl))
            EnemyList.Remove(ctrl);
    }
}
