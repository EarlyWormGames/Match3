using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coroutiner : MonoBehaviour
{
    private static List<IEnumerator> routines = new List<IEnumerator>();
    private static Coroutiner instance;

    // Use this for initialization
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (routines.Count > 0)
        {
            IEnumerator[] frameRoutines = routines.ToArray();
            foreach (var item in frameRoutines)
            {
                StartCoroutine(item);
                routines.RemoveAt(0);
            }
        }
    }

    public static void BeginCoroutine(IEnumerator routine)
    {
        routines.Add(routine);
    }
}
