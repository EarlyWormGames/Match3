using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialPlayer : MonoBehaviour
{
    public Tutorial Asset;
    public UnityEvent OnEventComplete;

    private int currentIndex = -1;
    private bool playing;

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            int score = GameManager.instance.Score;
            if (Asset.Events[currentIndex].ScoreIsPercentage)
                score = score / Mediator.Settings.TargetScore;

            if (Asset.Events[currentIndex].WaitForScoreAbove)
            {
                if (score >= Asset.Events[currentIndex].WaitForScore)
                    NextEvent();
            }
            else if (Asset.Events[currentIndex].WaitForScoreBelow)
            {
                if (score <= Asset.Events[currentIndex].WaitForScore)
                    NextEvent();
            }
        }
    }

    public void NextEvent()
    {
        ++currentIndex;
        if (currentIndex != 0)
            OnEventComplete.Invoke();

        GameManager.instance.m_bAllowRegularGameOver = !Asset.PreventGameOver;

        if (currentIndex < Asset.Events.Length)
        {
            playing = true;
            //Lock/unlock all the nodes!
            for (int x = 0; x < GameManager.instance.m_Grid.m_GridWidth; ++x)
            {
                for (int y = 0; y < GameManager.instance.m_Grid.m_GridHeight; ++y)
                {
                    if (Asset.Events[currentIndex].UnlockedNodes.Contains(new Vector2(x, y)) ||
                        !Asset.Events[currentIndex].LockNodes)
                        GameManager.instance.m_Grid.m_Nodes[x, y].AllowSwap = true;
                    else
                        GameManager.instance.m_Grid.m_Nodes[x, y].AllowSwap = false;
                }
            }
        }
        else
        {
            playing = false;
        }
    }

    private void OnEnable()
    {
        GameManager.onScored += OnScored;
    }

    private void OnDisable()
    {
        GameManager.onScored -= OnScored;
    }

    void OnScored(ItemColour a_colour, bool a_swapped, GridNode a_node)
    {
        if (!playing)
            return;

        if (Asset.Events[currentIndex].WaitForSwap)
            NextEvent();
    }
}