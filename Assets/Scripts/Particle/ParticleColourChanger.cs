using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColourChanger : MonoBehaviour {


    public Material CapsicumMat;
    public Material CakeMat;
    public Material BroccoliMat;
    public Material CupcakeMat;
    public Material IcecreamMat;
    public Material CarrotMat;
    internal ParticleSystem[] m_childParticles;
    internal NodeItem m_gemParent;
    internal ItemColour m_gemColour;
	// Use this for initialization
	void Start () {
        m_gemParent = GetComponentInParent<NodeItem>();
        if (m_gemParent != null)
        {
            m_gemColour = m_gemParent.m_Colour;
        }
        m_childParticles = GetComponentsInChildren<ParticleSystem>();
        switch (m_gemColour)
        {
            case ItemColour.Capsicum:
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ps.GetComponent<ParticleSystemRenderer>().material = CapsicumMat;
                }
                break;

            case ItemColour.Cake:
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ps.GetComponent<ParticleSystemRenderer>().material = CakeMat;
                }
                break;

            case ItemColour.Broccoli:
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ps.GetComponent<ParticleSystemRenderer>().material = BroccoliMat;
                }
                break;

            case ItemColour.Cupcake:
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ps.GetComponent<ParticleSystemRenderer>().material = CupcakeMat;
                }
                break;

            case ItemColour.Icecream:
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ps.GetComponent<ParticleSystemRenderer>().material = IcecreamMat;
                }
                break;

            case ItemColour.Carrot:
                foreach (ParticleSystem ps in m_childParticles)
                {
                    ps.GetComponent<ParticleSystemRenderer>().material = CarrotMat;
                }
                break;

            default:
                break;
        }
    }
}
