using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Config;
using UnityEngine;

public class KeyCtrl : MonoBehaviour
{
    /// <summary>
    /// 英雄移动的请求
    /// </summary>
    private HeroMoveRequest m_HeroMoveRequest;
    /// <summary>
    /// 英雄攻击的请求
    /// </summary>
    private AttackRequest m_AttackRequest;

    void Start()
    {
        m_HeroMoveRequest = GetComponent<HeroMoveRequest>();
        m_AttackRequest = GetComponent<AttackRequest>();
    }

    void Update()
    {
        #region 鼠标右键

        if (Input.GetMouseButtonDown(1))
        {
            // 判断自身死亡状态
            if (GameData.HeroCtrl.State == AnimeState.death)
                return;

            Vector2 mouse = Input.mousePosition;
            // 屏幕坐标转换为射线
            Ray ray = Camera.main.ScreenPointToRay(mouse);

            // 单体的射线检测 当中途有遮挡物时会无法去到后面的点击物体
            // RaycastHit hit;
            // Physics.Raycast(ray, out hit))

            RaycastHit[] hits = Physics.RaycastAll(ray);
            for (int i = hits.Length - 1; i >= 0; i--)
            {
                RaycastHit hit = hits[i];
                
                // 投射到敌人则攻击
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    int targetId = hit.collider.gameObject.GetComponent<BaseCtrl>().Model.Id;
                    RequestAttack(targetId);
                    break;
                }
                // 投射到地面则移动
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                {
                    RequestMove(hit.point);
                }
            }
        }

        #endregion


        #region 空格

        if (Input.GetKey(KeyCode.Space))
        {
            // 焦距到自己的英雄
            Camera.main.GetComponent<CameraCtrl>().FocusOn();
        }

        #endregion
    }

    /// <summary>
    /// 请求移动
    /// </summary>
    /// <param name="point"></param>
    private void RequestMove(Vector3 point)
    {
        // 显示点击特效
        GameObject go = PoolManager.Instance.GetObject("ClickMove");
        go.transform.position = point + Vector3.up * 2;

        // 发送移动的请求
        m_HeroMoveRequest.SendHeroMove(point);
    }

    /// <summary>
    /// 请求攻击
    /// </summary>
    /// <param name="targetId"></param>
    private void RequestAttack(int targetId)
    {
        // 请求攻击 发送参数 1.技能id, 2.攻击者的id, 3.目标id
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.SkillId, ServerConfig.SkillId);
        data.Add((byte)ParameterCode.AttackId, GameData.Player.Id);
        data.Add((byte)ParameterCode.TargetId, targetId);
        m_AttackRequest.SendRequest(data);
    }
}
