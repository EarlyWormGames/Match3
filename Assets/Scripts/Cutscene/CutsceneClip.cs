using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[Serializable]
public class CutsceneClip : PlayableAsset, ITimelineClipAsset
{
    public ExposedReference<CutsceneActivator> activator;
    public bool Pause = false;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CutscenePlayable>.Create(graph, new CutscenePlayable());
        var clone = playable.GetBehaviour();
        clone.activator = activator.Resolve(graph.GetResolver());
        clone.director = owner.GetComponent<PlayableDirector>();
        clone.Pause = Pause;
        return playable;
    }
}
