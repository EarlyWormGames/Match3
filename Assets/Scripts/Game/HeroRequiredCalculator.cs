using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HeroRequiredCalculator : MonoBehaviour
{
    [Serializable]
    public class EnemyCounters
    {
        [TypeName(typeof(BadNode))]
        public string EnemyType;

        [Tooltip("How many we have to find to use this")]
        public int RequiredCount = 1;

        [Tooltip("Higher priority = better chance of use")]
        public int Priority = 0;

        [AssetsOnly]
        public GameObject[] Counters;
    }

    [Tooltip("Show the prompt when below X percent moves left")]
    public float ShowAtPercent = 0.2f;
    public EnemyCounters[] EnemiesCounters;
    public UnityEvent OnShowPrompt;
    public UnityEvent OnClosePrompt;

    Dictionary<string, EnemyCounters> CountersDic = new Dictionary<string, EnemyCounters>();
    bool showing;
    GameObject counterPrefab;

    // Use this for initialization
    void Start()
    {
        foreach(var item in EnemiesCounters)
        {
            CountersDic.Add(item.EnemyType, item);
        }

        GameManager.onScored += Score;
    }

    void Score(ItemColour a_colour, bool a_swapped, GridNode a_node)
    {
        if ((GameManager.instance.m_TurnsLeft / (float)Mediator.Settings.Turns <= ShowAtPercent) && !showing)
        {
            //Make a counting dictionary
            Dictionary<string, int> countersCount = new Dictionary<string, int>();
            EnemyCounters useCounter = null;

            //Go through all the tiles (except the top row)
            for (int x = 0; x < GameManager.instance.m_Grid.m_GridWidth; ++x)
            {
                for (int y = 1; y < GameManager.instance.m_Grid.m_GridHeight; ++y)
                {
                    var tile = GameManager.instance.m_Grid.m_Nodes[x, y];

                    //Check what type they are
                    var type = tile.m_Shape.GetType().ToString();
                    if (CountersDic.ContainsKey(type))
                    {
                        if (!countersCount.ContainsKey(type))
                            countersCount.Add(type, 1);
                        ++countersCount[type];

                        //if we've reached the required count, do a thing
                        if (countersCount[type] >= CountersDic[type].RequiredCount)
                        {
                            if (useCounter == null)
                                useCounter = CountersDic[type];
                            else if (CountersDic[type].Priority > useCounter.Priority)
                                useCounter = CountersDic[type];
                        }
                    }
                }
            }

            if (useCounter != null)
            {
                //We did it! Show a prefab
                int rand = UnityEngine.Random.Range(0, useCounter.Counters.Length);
                counterPrefab = useCounter.Counters[rand];

                showing = true;
                OnShowPrompt.Invoke();
            }
        }
    }
    
    public void ClosePrompt()
    {
        //Close the prompt, but don't show again
        OnClosePrompt.Invoke();
    }

    public void ShowAd()
    {
        AdManager.AdForHero(counterPrefab);
    }
}
