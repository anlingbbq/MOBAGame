using System.Collections;
using System.Collections.Generic;
using Common.Code;
using Common.Extension;
using Common.OpCode;
using ExitGames.Client.Photon;
using UnityEngine;

public class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{
    // 代表客户端
    private static PhotonPeer m_Peer;

    public static PhotonPeer Peer
    {
        get { return m_Peer; }
    }
    // IP地址
    private string m_ServerAddress = "127.0.0.1:5055";
    // 服务器端应用名称
    private string m_ApplicationName = "MOBA";
    // 使用协议
    private ConnectionProtocol m_Protocol = ConnectionProtocol.Udp;

    public static PhotonEngine Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

	void Start () {
		m_Peer = new PhotonPeer(this, m_Protocol);
	    m_Peer.Connect(m_ServerAddress, m_ApplicationName);
	}
	
	void Update () {
		m_Peer.Service();
	}

    void OnDestroy()
    {
        m_Peer.Disconnect();
    }

    #region Photon相关接口

    public void DebugReturn(DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        BaseRequest request = m_RequestDict.TryGetEx((OperationCode) operationResponse.OperationCode);
        if (request)
        {
            request.OnOperationResponse(operationResponse);
        }
        else
        {
            Debug.LogError("找不到响应的对相应处理对象");
        }
    }

    public void OnEvent(EventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log("当前连接状态： " + statusCode);
    }

    #endregion


    #region 管理请求，事件

    private Dictionary<OperationCode, BaseRequest> m_RequestDict = new Dictionary<OperationCode, BaseRequest>();
    private Dictionary<EventCode, BaseEvent> m_EventDict = new Dictionary<EventCode, BaseEvent>();

    public void AddRequest(BaseRequest request)
    {
        if (!m_RequestDict.ContainsKey(request.OpCode))
            m_RequestDict.Add(request.OpCode, request);
    }

    public void RemoveRequest(BaseRequest request)
    {
        m_RequestDict.Remove(request.OpCode);
    }

    public void AddEvent(BaseEvent baseEvent)
    {
        if (!m_EventDict.ContainsKey(baseEvent.EventCode))
            m_EventDict.Add(baseEvent.EventCode, baseEvent);
    }

    public void RemoveEvent(BaseEvent baseEvent)
    {
        m_EventDict.Remove(baseEvent.EventCode);
    }

    #endregion
}
