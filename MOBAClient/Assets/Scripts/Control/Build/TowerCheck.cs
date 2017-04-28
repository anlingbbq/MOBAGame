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
    public List<AIBaseCtrl> EnemyList = new List<AIBaseCtrl>();

    private void OnTriggerEnter(Collider other)
    {
        AIBaseCtrl ctrl = other.GetComponent<AIBaseCtrl>();
        if (ctrl == null || ctrl.Model.Team == m_Team)
            return;

        EnemyList.Add(ctrl);
    }

    private void OnTriggerExit(Collider other)
    {
        AIBaseCtrl ctrl = other.GetComponent<AIBaseCtrl>();
        if (EnemyList.Contains(ctrl))
            EnemyList.Remove(ctrl);
    }
}
