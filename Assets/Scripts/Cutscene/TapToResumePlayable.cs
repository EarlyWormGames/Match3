using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class TapToResumePlayable : PlayableBehaviour
{
    public CutsceneActivator activator;
    public PlayableDirector director;
    public bool Pause;

    private bool hasPaused = false;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (activator != null)
            activator.Next();

        if (Pause && !hasPaused)
            director.Pause();

        hasPaused = true;
    }

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);

        if (Application.isPlaying)
        {
            Cutscene.ClickEvent += UnPause;
        }
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);

        if (Application.isPlaying)
        {
            Cutscene.ClickEvent -= UnPause;
        }
    }

    void UnPause()
    {
        director.Resume();
    }
}
