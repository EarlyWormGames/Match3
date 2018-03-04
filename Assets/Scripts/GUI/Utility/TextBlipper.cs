using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextBlipper : MonoBehaviour
{
    [Tooltip("Time (in s) between each letter displayed")]
    public float BlipSpeed = 0.1f;
    public bool MouseHurries = true;
    public AudioSource BlipSound;
    [Tooltip("Should the sound play on space and new line characters?")]
    public bool NoiseOnSpace = false;

    public UnityEvent OnFinished;

    private TextMeshProUGUI display;
    private string textToBlip = "";
    private float timer;
    private int index;

    // Use this for initialization
    void Start()
    {
        display = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!string.IsNullOrEmpty(textToBlip))
        {
            timer += Time.deltaTime;
            if (timer >= BlipSpeed)
            {
                timer = 0;
                if (index < textToBlip.Length)
                {
                    int indexAdd = 1;
                    string textAdd = textToBlip[index].ToString();

                    //Check for rich text tags
                    if (textAdd == "<" && index + 1 < textToBlip.Length)
                    {
                        if (char.IsLetter(textToBlip[index + 1]) ||
                            textToBlip[index + 1] == '/')
                        {
                            int endIndex = textToBlip.IndexOf('>', index);
                            if (endIndex > -1)
                            {
                                for (int i = index + 1; i <= endIndex; ++i)
                                    textAdd += textToBlip[i];

                                indexAdd += endIndex - index;
                                timer = BlipSpeed;
                            }
                        }
                    }

                    //Add the text to the display
                    display.text += textAdd;

                    if ((textToBlip[index] != ' ' && textToBlip[index] != '\n' && indexAdd == 1) || NoiseOnSpace)
                    {
                        if (BlipSound != null)
                            BlipSound.PlayOneShot(BlipSound.clip);
                    }
                    index += indexAdd;
                }
                else
                {
                    Hurry();
                }
            }

            if (MouseHurries)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Hurry();
                }
            }
        }
    }

    public void Hurry()
    {
        display.text = textToBlip;
        textToBlip = "";

        OnFinished.Invoke();
    }

    public void ShowText(string text)
    {
        timer = 0;
        textToBlip = text;
        index = 0;
    }
}