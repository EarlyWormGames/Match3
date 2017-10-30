using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EyePanel : MonoBehaviour
{
    public static EyePanel instance;

    public Sprite OnImage;
    public Sprite OffImage;

    private Image image;

    private void Start()
    {
        instance = this;
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }

    public void SetImage(bool On)
    {
        if (On)
            image.sprite = OnImage;
        else
            image.sprite = OffImage;
    }
}
