using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObject : ReuseableObject
{
    public override void BeforeGetObject()
    {
        
    }

    public override void BeforeHideObject()
    {
        GetComponent<EllipsoidParticleEmitter>().ClearParticles();
        GetComponent<EllipsoidParticleEmitter>().emit = false;
    }
}
