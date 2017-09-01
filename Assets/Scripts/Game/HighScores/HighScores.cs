using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class HighScores : MonoBehaviour
{
    public List<int> Scores;
    string currentDirectory;
    public string scoreFileName = "Scores.txt";

    void Start()
    {
        currentDirectory = Application.dataPath;
        // Load the scores by default.
        LoadScoresFromFile();
    }
    void Update()
    {
    }
    public void LoadScoresFromFile()
    {
        // Check if the file exists
        bool fileExists = File.Exists(currentDirectory + "\\" + scoreFileName);
        if (!fileExists)
        {
            Debug.LogWarning("The file " + scoreFileName +
           " does not exist. No scores will be loaded.", this);
            return;
        }
        // new List to store our scores
        Scores = new List<int>();
        // Create a new File Reader from the Scores file directory
        StreamReader StrReader = new StreamReader(currentDirectory +
       "\\" + scoreFileName);
        int scoreCount = 0;
        // look at the next part of the file - IF we can
        while (StrReader.Peek() != 0 && scoreCount < Scores.Count)
        {
            int OutScore = -1;
            string fileLine = StrReader.ReadLine();
            bool Try = int.TryParse(fileLine, out OutScore);
            // if we could read the line
            if (Try)
            {
                // Add it to the scores list
                Scores[scoreCount] = OutScore;
            }
            else
            {
                // if we couldnt read the line Set a default value to 0
                Debug.Log("Invalid line in scores file at " + scoreCount +
               ", using default value.", this);
                Scores[scoreCount] = 0;
            }
            // keep iterating
            scoreCount++;
        }

        //Close the StreamReader
        StrReader.Close();
        Debug.Log("High scores read from " + scoreFileName);    }

    public void AddScore(int newScore)
    {
        int CurrIndex = -1;

        // loop to incert our next score in order of highest to lowest
        for (int i = 0; i < Scores.Count; i++)
        {
            if (Scores[i] > newScore || Scores[i] == 0)
            {
                CurrIndex = i;
                break;
            }
        }

        if (CurrIndex < 0)
        {
            return;
        }

        //Sort the list
        for (int i = Scores.Count - 1; i > CurrIndex; i--)
        {
            Scores[i] = Scores[i - 1];
        }
        // Insert our new score in its place
        Scores[CurrIndex] = newScore;
    }

    public void SaveToFile()
    {
        // Create a StreamWriter for our file path.
        StreamWriter StrWriter = new StreamWriter(currentDirectory + "\\"
       + scoreFileName);
        // Write the lines to the file
        for (int i = 0; i < Scores.Count; i++)
        {
            StrWriter.WriteLine(Scores[i]);
        }
        // Close the stream
        StrWriter.Close();
    }

}