using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 血条控制
/// </summary>
public class HpCtrl : MonoBehaviour
{
    /// <summary>
    /// 血条
    /// </summary>
    [SerializeField]
    private Slider BarHp;
    /// <summary>
    /// 填充图片
    /// </summary>
    [SerializeField]
    private Image ImgFill;

    /// <summary>
    /// 设置血条状态
    /// </summary>
    public void SetBarActive(bool isActive)
    {
        BarHp.gameObject.SetActive(isActive);
    }

    /// <summary>
    /// 设置颜色
    /// </summary>
    public void SetColor(bool friend)
    {
        ImgFill.color = friend ? Color.green : Color.red;
    }

    /// <summary>
    /// 设置血量
    /// </summary>
    /// <param name="value"></param>
    public void SetHp(float value)
    {
        BarHp.value = value;
    }

    void LateUpdate()
    {
        // 始终朝向摄像机
        transform.forward = Camera.main.transform.forward;    
    }
}
