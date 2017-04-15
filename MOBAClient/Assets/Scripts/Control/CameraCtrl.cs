using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    // 相机边界
    [Header("摄像机边界")]
    [SerializeField]
    private float X_MAX;
    [SerializeField]
    private float X_MIN;
    [SerializeField]
    private float Z_MAX;
    [SerializeField]
    private float Z_MIN;

    /// <summary>
    /// 相机移动速度
    /// </summary>
    [Header("移动速度")]
    [SerializeField]
    private float m_Speed = 25;

    /// <summary>
    /// 敏感区域
    /// </summary>
    private float m_Area = 0.1f;

    /// <summary>
    /// 是否有焦点
    /// </summary>
    public static bool IsFocus = true;
    void OnApplicationFocus(bool focus)
    {
        IsFocus = focus;
    }

    void Awake()
    {
        // 鼠标锁定在屏幕内
        Cursor.lockState = CursorLockMode.Confined;
    }

    void LateUpdate()
    {
        if (!IsFocus)
            return;

        // 目标点
        Vector3 target = Vector3.zero;

        // 限制鼠标的坐标区域
        Vector3 mousePos = Input.mousePosition;
        float x = Mathf.Clamp(mousePos.x, 0, Screen.width);
        float y = Mathf.Clamp(mousePos.y, 0, Screen.height);

        // 检测鼠标是否在敏感区域 在则移动摄像机
        if (y > Screen.height*(1 - m_Area))
        {
            target.z = 1;
        }
        else if (y < Screen.height*m_Area)
        {
            target.z = -1;
        }

        if (x > Screen.width*(1 - m_Area))
        {
            target.x = 1;
        }
        else if (x < Screen.width*m_Area)
        {
            target.x = -1;
        }

        // 当摄像机斜向移动时 统一移动速度
        if (target.x != 0 && target.z != 0)
        {
            target = target.normalized;
        }

        // 移动
        transform.position += target * Time.deltaTime * m_Speed;

        // 限制区域
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, X_MIN, X_MAX),
            transform.position.y, 
            Mathf.Clamp(transform.position.z, Z_MIN, Z_MAX));
    }

    /// <summary>
    /// 焦点在英雄上
    /// </summary>
    public void FocusOn()
    {
        if (GameData.Hero == null)
            return;

        Vector3 pos = GameData.Hero.transform.position;
        transform.position = new Vector3(pos.x, transform.position.y, pos.z - 10);
    }
}
