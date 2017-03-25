using System;
using System.Collections.Generic;
using Common.Code;
using ExitGames.Client.Photon;
using UnityEngine;

public abstract class BaseEvent : MonoBehaviour
{
    public EventCode EventCode;
    public abstract void OnEvent(EventData eventData);

    public virtual void Start()
    {
        PhotonEngine.Instance.AddEvent(this);
    }

    public virtual void OnDestroy()
    {
        PhotonEngine.Instance.RemoveEvent(this);
    }
}
