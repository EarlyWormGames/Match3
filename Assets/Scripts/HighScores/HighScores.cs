using UnityEngine;
using System.Collections;
using System.IO;
public class HighScores : MonoBehaviour
{
    public int[] scores = new int[10];
    string currentDirectory;

    public string scoreFileName = "highscores.txt";

    void Start()
    {
        currentDirectory = Application.dataPath;

        // TESTING
        LoadScoresFromFile();
    }

    void Update()
    {

    }

    public void LoadScoresFromFile()
    {
        //Check if the file exsists
        bool fileExists = File.Exists(currentDirectory + "\\" + scoreFileName);
        if (fileExists == false)
        {
            // Error if the file cannot be found
            Debug.LogError("The file " + scoreFileName +
           " does not exist. No scores will be loaded.", this);
            return;
        }
        // Create the aray to store
        scores = new int[scores.Length];
        StreamReader fileReader = new StreamReader(currentDirectory +
       "\\" + scoreFileName);

        // A counter to make sure we don't go past the end of our scores
        int scoreCount = 0;
        
        // loop to bring our scores in from our file
        while (fileReader.Peek() != 0 && scoreCount < scores.Length)
        {
            // Read that line into a variable
            string fileLine = fileReader.ReadLine();
            // Try to parse that variable into an int
            // make a variable to put it in
            int readScore = -1;
            // Try to parse it
            bool didParse = int.TryParse(fileLine, out readScore);
            if (didParse)
            {
                // Successfully read a number, put it in the array.
                scores[scoreCount] = readScore;
            }
            else
            {
                // If the number couldn't be parsed then we probably had
                // junk in our file. Print an error, and then use
                // a default value.
                Debug.Log("Invalid line in scores file at " + scoreCount +
               ", using default value.", this);
                scores[scoreCount] = 0;
            }
            // Incrememt the counter!
            scoreCount++;
        }
        // Close the stream!
        fileReader.Close();
        Debug.Log("High scores read from " + scoreFileName);
    }
    public void SaveScoresToFile()
    {
        // Create a StreamWriter for our file path.
        StreamWriter fileWriter = new StreamWriter(currentDirectory + "\\"
       + scoreFileName);
        // Write the lines to the file
        for (int i = 0; i < scores.Length; i++)
        {
            fileWriter.WriteLine(scores[i]);
        }
        // Close the stream
        fileWriter.Close();
        Debug.Log("High scores written to " + scoreFileName);

    }
    public void AddScore(int newScore)
    {
        // First up we find out what index it belongs at.
        // This will be the first index with a score lower than
        // the new score.
        int desiredIndex = -1;
        for (int i = 0; i < scores.Length; i++)
        {
            // Instead of checking the value of desiredIndex
            // use 'break' to stop the loop.
            if (scores[i] > newScore || scores[i] == 0)
            {
                desiredIndex = i;
                break;
            }
        }
        // If no desired index was found then the score
        // isn't high enough to get on the table
        if (desiredIndex < 0)
        {
            Debug.Log("Score of " + newScore +
            " not high enough for high scores list.", this);
            return;
        }
        // Move all of the scores after that index
        // back by one position. Do this by looping from
        // the back of the array to our desired index.
        for (int i = scores.Length - 1; i > desiredIndex; i--)
        {
            scores[i] = scores[i - 1];
        }
        // Insert our new score in its place
        scores[desiredIndex] = newScore;
        Debug.Log("Score of " + newScore +
        " entered into high scores at position " + desiredIndex, this);
    }
}