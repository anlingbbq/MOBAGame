using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObject : MonoBehaviour, IPoolReuseable
{
    public void BeforeGetObject()
    {
        
    }

    public void BeforeHideObject()
    {
        GetComponent<EllipsoidParticleEmitter>().ClearParticles();
        GetComponent<EllipsoidParticleEmitter>().emit = false;
    }
}
