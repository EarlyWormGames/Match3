using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tutorial", menuName = "Tutorial")]
public class Tutorial : ScriptableObject
{
    [System.Serializable]
    public class TutorialEvent
    {
        public bool LockNodes = true;
        public List<Vector2> UnlockedNodes = new List<Vector2>();
        public bool WaitForSwap;
        public bool WaitForCustomEvent;
        public bool WaitForScoreAbove, WaitForScoreBelow;
        public int WaitForScore = 0;
        public bool ScoreIsPercentage = false;
    }

    public bool PreventGameOver = false;
    public TutorialEvent[] Events;
}