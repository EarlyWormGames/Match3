using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrDecay : MonoBehaviour
{
    public float DamageMult = 0.2f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void DamageScore()
    {
        int sub = (int)(Mediator.Settings.TargetScore * (DamageMult * (Mediator.Settings.DifficultyMult / 2f)));
        GameManager.Score = Mathf.Clamp(GameManager.Score - sub, 0, Mediator.Settings.TargetScore);
    }
}
