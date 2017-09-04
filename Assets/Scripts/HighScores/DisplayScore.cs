using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour {

    GameObject SceneManager;
    HighScores m_HS;
    string BuiltString;

    // Use this for initialization
    void Start () {
        SceneManager = GameObject.FindGameObjectWithTag("SceneManager");
        m_HS = SceneManager.GetComponent<HighScores>();
    }
	
	// Update is called once per frame
	void Update () {
	}

    private void OnGUI()
    {
        UpdateText();
    }

    void UpdateText()
    {
        BuiltString = "";
        Text t = GetComponent<Text>();
        
        for (int i = m_HS.scores.Length; i < 0; i--)
        {
            BuiltString += m_HS.scores[i].ToString() + "\n";
        }
        t.text = BuiltString;
    }
}
