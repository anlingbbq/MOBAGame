using System;
using System.Collections.Generic;
using Common.Code;
using ExitGames.Client.Photon;
using UnityEngine;

public abstract class BaseEvent : MonoBehaviour
{
    public EventCode Code;
    public abstract void OnEvent(EventData eventData);

    /// <summary>
    /// 需要在start或之前给EventCode赋值
    /// </summary>
    public virtual void Start()
    {
        PhotonEngine.Instance.AddEvent(this);
    }

    public virtual void OnDestroy()
    {
        PhotonEngine.Instance.RemoveEvent(this);
    }
}
