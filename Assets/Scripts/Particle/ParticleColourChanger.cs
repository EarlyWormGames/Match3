using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColourChanger : MonoBehaviour {

    internal ParticleSystem m_mainParticle;
    internal ParticleSystem[] m_childParticles;
    internal NodeItem m_gemParent;
	// Use this for initialization
	void Start () {
        m_mainParticle = GetComponent<ParticleSystem>();
        m_gemParent = GetComponentInParent<NodeItem>();
        m_childParticles = GetComponentsInChildren<ParticleSystem>();
        switch (m_gemParent.m_Colour)
        {
            case ItemColour.Red:
                m_mainParticle.startColor = new Color(1, 0, 0, m_mainParticle.startColor.a);
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ps.startColor = new Color(1, 0, 0, ps.startColor.a);
                }
                break;

            case ItemColour.Orange:
                m_mainParticle.startColor = new Color(1, 0.5f, 0, m_mainParticle.startColor.a);
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ps.startColor = new Color(1, 0.5f, 0, ps.startColor.a);
                }
                break;

            case ItemColour.Yellow:
                m_mainParticle.startColor = new Color(1, 1, 0, m_mainParticle.startColor.a);
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ps.startColor = new Color(1, 1, 0, ps.startColor.a);
                }
                break;

            case ItemColour.Green:
                m_mainParticle.startColor = new Color(0, 1, 0, m_mainParticle.startColor.a);
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ps.startColor = new Color(0, 1, 0, ps.startColor.a);
                }
                break;

            case ItemColour.Blue:
                m_mainParticle.startColor = new Color(0, 0, 1, m_mainParticle.startColor.a);
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ps.startColor = new Color(0, 0, 1, ps.startColor.a);
                }
                break;

            case ItemColour.Purple:
                m_mainParticle.startColor = new Color(0.5f, 0, 1, m_mainParticle.startColor.a);
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ps.startColor = new Color(0.5f, 0, 1, ps.startColor.a);
                }
                break;

            case ItemColour.Pink:
                m_mainParticle.startColor = new Color(1, 0, 0.5f, m_mainParticle.startColor.a);
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ps.startColor = new Color(1, 0, 0.5f, ps.startColor.a);
                }
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
