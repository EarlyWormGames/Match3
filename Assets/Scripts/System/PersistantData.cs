using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class PersistantData : MonoBehaviour
{
    public static PersistantData instance;

    public GameObject Prefab;

    private float Timer;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        if (instance != null)
            return;

        instance = new GameObject().AddComponent<PersistantData>();
        DontDestroyOnLoad(instance.gameObject);

        DontDestroyOnLoad(Instantiate(instance.Prefab));
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            //App switched
            Analytics.CustomEvent("App switched", new Dictionary<string, object>
                {
                    { "Time Opened", Timer }
                });

            Timer = 0;
        }
        else
        {
            //App opened
            Timer = 0;
        }
    }
}
