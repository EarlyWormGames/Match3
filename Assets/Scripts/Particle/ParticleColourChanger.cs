using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColourChanger : MonoBehaviour {

    internal ParticleSystem[] m_childParticles;
    internal NodeItem m_gemParent;
	// Use this for initialization
	void Start () {
        m_gemParent = GetComponentInParent<NodeItem>();
        m_childParticles = GetComponentsInChildren<ParticleSystem>();
        switch (m_gemParent.m_Colour)
        {
            case ItemColour.Capsicum:
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ParticleSystem.MainModule main = ps.main;
                    main.startColor = new Color(1, 0, 0, main.startColor.color.a);
                }
                break;

            //case ItemColour.Orange:
            //    foreach (ParticleSystem ps in m_childParticles)
            //    {
            //        ps.startColor = new Color(1, 0.5f, 0, ps.startColor.a);
            //    }
            //    break;

            case ItemColour.Cake:
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ParticleSystem.MainModule main = ps.main;
                    main.startColor = new Color(1, 1, 0, main.startColor.color.a);
                }
                break;

            case ItemColour.Broccoli:
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ParticleSystem.MainModule main = ps.main;
                    main.startColor = new Color(0, 1, 0, main.startColor.color.a);
                }
                break;

            case ItemColour.Cupcake:
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ParticleSystem.MainModule main = ps.main;
                    main.startColor = new Color(0, 1, 0.8f, main.startColor.color.a);
                }
                break;

            case ItemColour.Icecream:
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ParticleSystem.MainModule main = ps.main;
                    main.startColor = new Color(0.5f, 0, 1, main.startColor.color.a);
                }
                break;

            //case ItemColour.Pink:
            //    foreach (ParticleSystem ps in m_childParticles)
            //    {
            //        ps.startColor = new Color(1, 0, 0.5f, ps.startColor.a);
            //    }
            //    break;
        }
	}
}
