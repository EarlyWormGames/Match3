using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using RijndaelManagedEncryption;
using System;

public class SaveData
{
    public static int FreeTurns;
    public static int[] LevelScores;
    public static int LevelCount = 25;
    public static bool IsDev = false;
    public static int LastArcade = -1;
    public static long ArcadeScore;

    private static XmlDocument loadedDoc;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        Load(LevelCount);
    }

    public static void Load(int count)
    {
        LevelCount = count;
        LevelScores = new int[LevelCount];
        if (File.Exists(Application.persistentDataPath + "/SaveData.txt"))
        {
            //Load and decrypt the file
            string contents = File.ReadAllText(Application.persistentDataPath + "/SaveData.txt");
            contents = Encryption.DecryptRijndael(contents, "B9FCCD1BF5772EF9");

            //Begin reading the contents
            loadedDoc = new XmlDocument();
            loadedDoc.LoadXml(contents);

            //Grab our free turns value
            XmlElement freeTurns = (XmlElement)loadedDoc.SelectSingleNode("/SaveData/FreeTurns");
            FreeTurns = Convert.ToInt32(freeTurns.InnerText);

            XmlElement arcade = (XmlElement)loadedDoc.SelectSingleNode("/SaveData/Arcade");
            if (arcade == null)
            {
                arcade = loadedDoc.CreateElement("Arcade");
                loadedDoc.DocumentElement.AppendChild(arcade);
                arcade.InnerText = LastArcade.ToString();
            }
            LastArcade = Convert.ToInt32(arcade.InnerText);

            XmlElement arcadeScore = (XmlElement)loadedDoc.SelectSingleNode("/SaveData/ArcadeScore");
            if (arcadeScore == null)
            {
                arcadeScore = loadedDoc.CreateElement("ArcadeScore");
                loadedDoc.DocumentElement.AppendChild(arcadeScore);
                arcadeScore.InnerText = ArcadeScore.ToString();
            }
            ArcadeScore = Convert.ToInt64(arcadeScore.InnerText);

            //Grab the scores (1,2,3 stars) for the levels
            XmlElement levels = (XmlElement)loadedDoc.SelectSingleNode("/SaveData/Levels");

            int i = 0;
            foreach (var child in levels.ChildNodes)
            {
                LevelScores[i] = Convert.ToInt32((child as XmlElement).InnerText);
                ++i;
            }
        }
        else
        {
            FreeTurns = 0;
            LastArcade = -1;
            ArcadeScore = 0;

            File.Create(Application.persistentDataPath + "/SaveData.txt").Close();

            //Create an empty xmldoc
            loadedDoc = new XmlDocument();

            XmlElement root = loadedDoc.CreateElement("SaveData");
            loadedDoc.AppendChild(root);

            //Save our free turns as an xmlelement
            XmlElement freeTurns = loadedDoc.CreateElement("FreeTurns");
            root.AppendChild(freeTurns);

            //Create the parent of all the level elements
            XmlElement levels = loadedDoc.CreateElement("Levels");
            root.AppendChild(levels);

            //Create the parent of all the level elements
            XmlElement arcade = loadedDoc.CreateElement("Arcade");
            root.AppendChild(arcade);

            XmlElement arcadeScore = loadedDoc.CreateElement("ArcadeScore");
            root.AppendChild(arcadeScore);

            Save();
        }
    }

    public static void Save()
    {
        XmlElement freeTurns = (XmlElement)loadedDoc.SelectSingleNode("/SaveData/FreeTurns");
        freeTurns.InnerText = FreeTurns.ToString();

        XmlElement arcade = (XmlElement)loadedDoc.SelectSingleNode("/SaveData/Arcade");
        arcade.InnerText = LastArcade.ToString();

        XmlElement arcadeScore = (XmlElement)loadedDoc.SelectSingleNode("/SaveData/ArcadeScore");
        arcadeScore.InnerText = ArcadeScore.ToString();

        //Create the parent of all the level elements
        XmlElement levels = (XmlElement)loadedDoc.SelectSingleNode("/SaveData/Levels");

        int i = 0;
        //Add all of the level scores to the document
        foreach (var item in levels.ChildNodes)
        {
            var child = (item as XmlElement);
            child.InnerText = LevelScores[i].ToString();

            ++i;
        }

        if (i < LevelScores.Length - 1)
        {
            for (int x = i; x < LevelScores.Length; ++x)
            {
                XmlElement child = loadedDoc.CreateElement("level");
                child.InnerText = LevelScores[x].ToString();
                levels.AppendChild(child);
            }
        }

        //Begin file saving code
        string contents = "";
        using (var stringWriter = new StringWriter())
        using (var xmlTextWriter = XmlWriter.Create(stringWriter))
        {
            //Write the xmldoc to a writer
            loadedDoc.WriteTo(xmlTextWriter);
            xmlTextWriter.Flush();

            //Build it out as a string so we can use it
            contents = stringWriter.GetStringBuilder().ToString();
        }

        //Encrypt and save the file
        contents = Encryption.EncryptRijndael(contents, "B9FCCD1BF5772EF9");
        File.WriteAllText(Application.persistentDataPath + "/SaveData.txt", contents);
    }
}
