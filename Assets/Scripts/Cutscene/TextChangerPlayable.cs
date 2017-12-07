using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class TextChangerPlayable : PlayableBehaviour
{
    public TextMeshProUGUI TextObject;
    public string Text;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (TextObject != null)
        {
            TextObject.text = Text;
        }
    }
}
