using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class TextChangerPlayable : PlayableBehaviour
{
    public TextMeshProUGUI TextObject;
    public string Text;

    private string oldText;
    private bool set = false;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (TextObject != null)
        {
            if (!set)
            {
                set = true;
                oldText = TextObject.text;
            }
            TextObject.text = Text;
        }
    }

    
}
