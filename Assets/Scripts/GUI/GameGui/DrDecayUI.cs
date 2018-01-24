using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrDecayUI : BadGuyUI
{
    public ParticleSystem m_ZapParticle;

    public void EndSlide()
    {
        m_ZapParticle.Play();
    }
}
