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
        if (!GameData.HeroCtrl)
            return;

        #region 空格

        if (Input.GetKey(KeyCode.Space))
        {
            // 焦距到自己的英雄
            Camera.main.GetComponent<CameraCtrl>().FocusOn();
        }

        #endregion

        #region 鼠标右键

        // 判断自身死亡状态
        if (GameData.HeroCtrl.State.Type == AIStateEnum.DEAD)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mouse = Input.mousePosition;
            // 屏幕坐标转换为射线
            Ray ray = Camera.main.ScreenPointToRay(mouse);

            // 单体的射线检测 当中途有遮挡物时会无法去到后面的点击物体
            // RaycastHit hit;
            // Physics.Raycast(ray, out hit))

            
            HitState state = HitState.INVALID;
            RaycastHit hit = new RaycastHit();
            RaycastHit[] hits = Physics.RaycastAll(ray);
            for (int i = hits.Length - 1; i >= 0; i--)
            {
                // 投射到敌人则攻击
                if (hits[i].collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    hit = hits[i];
                    state = HitState.ATTACK;
                    break;
                }
                // 投射到地面则移动
                if (hits[i].collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                {
                    hit = hits[i];
                    state = HitState.MOVE;
                }
            }

            /*
             * 这里这样处理是为了
             * 在先投射到移动层再投射到攻击层的时候
             * 不处理移动，只处理攻击
             */
            if (state == HitState.MOVE)
            {
                RequestMove(hit.point);
            }
            else if (state == HitState.ATTACK)
            {
                int targetId = hit.collider.gameObject.GetComponent<AIBaseCtrl>().Model.Id;
                // 发送攻击请求
                m_AttackRequest.SendAttack(ServerConfig.SkillId, GameData.Player.Id, targetId);
            }
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
        m_HeroMoveRequest.SendMove(point);
    }

    /// <summary>
    /// 射线状态
    /// </summary>
    enum HitState
    {
        INVALID,
        ATTACK,
        MOVE,
    }
}
