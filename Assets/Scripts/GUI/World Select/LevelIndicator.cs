using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIndicator : MonoBehaviour {

    public Sprite Unlocked;
    public Sprite Locked;

    Image DisplayImage;
    LevelSettings MyLevel;

    LevelSettings PreviousLevel;
    // Use this for initialization
    void Start () {

        //The GUI Image must be the first in the children list
        DisplayImage = GetComponentInChildren<Image>();

        //the level setting component exsists on the parent object.
        MyLevel = GetComponentInParent<LevelSettings>();
    }
	
	// Update is called once per frame
	void Update () {

        DisplayImage.sprite = UnlockedOrLocked();

    }

    Sprite UnlockedOrLocked()
    {
        //NOT REDUNDANT, curse you intelesence
        if (MyLevel.LevelNum - 1 >= 0)
        {
            //if previous level has a score, Current level is unlocked
            if (SaveData.LevelScores[MyLevel.LevelNum - 1] != 0)
            {
                return Unlocked;
            }
        }

        // if Current level has no score return locked
        if (SaveData.LevelScores[MyLevel.LevelNum] == 0)
        {
            return Locked;
        }
        else
        {
            return Unlocked;
        }
    }
}
