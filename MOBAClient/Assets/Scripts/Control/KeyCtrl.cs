using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCtrl : MonoBehaviour
{
    /// <summary>
    /// 英雄移动的请求
    /// </summary>
    private HeroMoveRequest m_HeroMoveRequest;

    void Start()
    {
        m_HeroMoveRequest = GetComponent<HeroMoveRequest>();
    }

    void Update()
    {
        #region 鼠标右键

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mouse = Input.mousePosition;
            // 屏幕坐标转换为射线
            Ray ray = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hit;
            // 判断是没有投射到物体
            if (!Physics.Raycast(ray, out hit))
                return;

            // 投射到地面则移动
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                Move(hit.point);
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
    /// 移动
    /// </summary>
    /// <param name="point"></param>
    private void Move(Vector3 point)
    {
        // 显示点击特效
        GameObject go = PoolManager.Instance.GetObject("ClickMove");
        go.transform.position = point + Vector3.up;

        // 发送移动的请求
        m_HeroMoveRequest.SendHeroMove(point);
    }
}
