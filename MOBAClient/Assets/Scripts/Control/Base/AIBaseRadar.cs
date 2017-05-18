using System.Collections.Generic;
using UnityEngine;

public class AIBaseRadar : MonoBehaviour
{

    /// <summary>
    /// AI控制器
    /// </summary>
    private AIBaseCtrl m_Ctrl;
    /// <summary>
    /// 开启
    /// </summary>
    /// <param name="ctrl"></param>
    public void Open(AIBaseCtrl ctrl)
    {
        m_Ctrl = ctrl;
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 关闭
    /// </summary>
    public void Close()
    {
        EnemyList.Clear();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 检测到的敌人列表
    /// </summary>
    public List<AIBaseCtrl> EnemyList = new List<AIBaseCtrl>();

    /// <summary>
    /// 进入范围添加敌人
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        AIBaseCtrl ctrl = other.GetComponent<AIBaseCtrl>();
        if (ctrl == null || ctrl.Model.Team == m_Ctrl.Model.Team)
            return;

        EnemyList.Add(ctrl);
    }

    /// <summary>
    /// 离开范围移除敌人
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        AIBaseCtrl ctrl = other.GetComponent<AIBaseCtrl>();
        if (EnemyList.Contains(ctrl))
        {
            EnemyList.Remove(ctrl);
            if (m_Ctrl.Target == ctrl)
                m_Ctrl.Target = null;
        }
    }

    /// <summary>
    /// 寻找一个敌人
    /// </summary>
    /// <returns></returns>
    public AIBaseCtrl FindEnemy()
    {
        AIBaseCtrl enemy = null;
        for (int i = EnemyList.Count - 1; i >= 0; i--)
        {
            enemy = EnemyList[i];

            if (enemy == null || enemy.Model.CurHp <= 0)
            {
                EnemyList.Remove(enemy);
                continue;
            }

            return enemy;
        }
        return null;
    }

    /// <summary>
    /// 寻找最近的敌人
    /// </summary>
    /// <returns></returns>
    public AIBaseCtrl FindRecentlyEnemy()
    {
        AIBaseCtrl enemy = null;
        float rDistance = 1000;
        foreach (AIBaseCtrl item in EnemyList)
        {
            float distance = Vector2.Distance(transform.position, item.transform.position);
            if (rDistance > distance)
            {
                rDistance = distance;
                enemy = item;
            }
        }
        return enemy;
    }

}
