using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[Serializable]
public class TapToResumeClip : PlayableAsset, ITimelineClipAsset
{
    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TapToResumePlayable>.Create(graph, new TapToResumePlayable());
        var clone = playable.GetBehaviour();
        return playable;
    }
}
