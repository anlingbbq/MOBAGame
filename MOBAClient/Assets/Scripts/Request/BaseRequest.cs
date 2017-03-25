using System.Collections;
using System.Collections.Generic;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public abstract class BaseRequest : MonoBehaviour
{
    [HideInInspector]
    public OperationCode OpCode;
    public abstract void DefalutRequest();
    public abstract void OnOperationResponse(OperationResponse response);

	public virtual void Start () {
		PhotonEngine.Instance.AddRequest(this);
	}

    public virtual void OnDestroy()
    {
        PhotonEngine.Instance.RemoveRequest(this);
    }
}
