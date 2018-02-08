using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    public class MusicLevel
    {
        public VolumeController Track;
        public List<string> Levels = new List<string>();
    }

    public static MusicManager instance;

    public MusicLevel[] Tracks;

    // Use this for initialization
    void Awake()
    {
        instance = this;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene a_scene, LoadSceneMode a_mode)
    {
        foreach (var track in Tracks)
        {
            if (track.Levels.Contains(a_scene.name))
                track.Track.FadeIn();
            else
                track.Track.FadeOut();
        }
    }
}