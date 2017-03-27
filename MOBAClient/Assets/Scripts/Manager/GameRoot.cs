using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
	void Start ()
	{
	    UIManager.Instance.PushPanel(UIPanelType.MainMenu);
	    UIManager.Instance.PushPanel(UIPanelType.Login);
	}
}
