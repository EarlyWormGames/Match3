using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberScroller : MonoBehaviour
{
    public float Speed;
    public TextMeshProUGUI text;

    private int Number;
    private int count;

    public void BeginScroll(int num)
    {
        Number = num;
        count = 0;
        StartCoroutine(Tick());
    }

    IEnumerator Tick()
    {
        yield return new WaitForSeconds(Speed);
        int add = 1;

        if (Number - count >= 10)
            add = 10;
        if (Number - count >= 100)
            add = 100;

        count += add;
        text.text = count.ToString();

        if (count < Number)
        {
            StartCoroutine(Tick());
        }
    }
}
