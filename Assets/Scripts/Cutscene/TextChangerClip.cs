using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class TextChangerClip : PlayableAsset, ITimelineClipAsset
{
    public ExposedReference<TMPro.TextMeshProUGUI> TextObject;
    public string Text = "";

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TextChangerPlayable>.Create(graph, new TextChangerPlayable());
        var clone = playable.GetBehaviour();
        clone.TextObject = TextObject.Resolve(graph.GetResolver());
        clone.Text = Text;
        return playable;
    }
}
