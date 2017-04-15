using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavPathGizmo : MonoBehaviour
{
    void Start()
    {
        
    }

    void OnDrawGizmos()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        NavMeshPath path = agent.path;

        Color color = Color.green;
        switch (path.status)
        {
            case NavMeshPathStatus.PathComplete:
                color = Color.green;
                break;
            case NavMeshPathStatus.PathPartial:
                color = Color.red;
                break;
            case NavMeshPathStatus.PathInvalid:
                color = Color.yellow;
                break;
        }

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], color);
        }
    }
}
