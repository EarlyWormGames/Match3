using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneLevel
    {
        public Cutscene cutscene;
        public int level;
    }
    public CutsceneLevel[] Cutscenes;


    private void Awake()
    {
        foreach (var item in Cutscenes)
        {
            if (item.level != Mediator.Settings.Level + 1 || Mediator.Settings.isArcade)
                item.cutscene.gameObject.SetActive(false);
            else
                item.cutscene.gameObject.SetActive(true);
        }
    }
}
