using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampCtrl : AIBaseCtrl
{
    public override void DeathResponse()
    {
        gameObject.SetActive(false);
    }

    public override void RebirthResponse()
    {
        gameObject.SetActive(true);
    }
}
