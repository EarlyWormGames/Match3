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
    public static bool IsDev = true;

    private static XmlDocument loadedDoc;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        Load(LevelCount);
    }

    public static void Load(int LevelCount)
    {
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
            string query = string.Format("//*[@id='{0}']", "FreeTurns");
            XmlElement freeTurns = (XmlElement)loadedDoc.SelectSingleNode(query);
            FreeTurns = Convert.ToInt32(freeTurns.InnerText);

            //Grab the scores (1,2,3 stars) for the levels
            query = string.Format("//*[@id='{0}']", "Levels");
            XmlElement levels = (XmlElement)loadedDoc.SelectSingleNode(query);

            int i = 0;
            foreach (var child in levels.ChildNodes)
            {
                LevelScores[i] = Convert.ToInt32((child as XmlElement).InnerText);
                ++i;
            }
        }
        else
        {
            File.Create(Application.persistentDataPath + "/SaveData.txt").Close();

            //Create an empty xmldoc
            loadedDoc = new XmlDocument();

            XmlElement root = loadedDoc.CreateElement("SaveData");
            loadedDoc.AppendChild(root);

            //Save our free turns as an xmlelement
            XmlElement freeTurns = loadedDoc.CreateElement("FreeTurns");
            freeTurns.SetAttribute("id", "FreeTurns");
            freeTurns.InnerText = FreeTurns.ToString();
            root.AppendChild(freeTurns);

            //Create the parent of all the level elements
            XmlElement levels = loadedDoc.CreateElement("Levels");
            levels.SetAttribute("id", "Levels");
            root.AppendChild(levels);

            Save();
        }
    }

    public static void Save()
    {
        //Save our free turns as an xmlelement
        string query = string.Format("//*[@id='{0}']", "FreeTurns"); // or "//book[@id='{0}']"
        XmlElement freeTurns = (XmlElement)loadedDoc.SelectSingleNode(query);
        //XmlElement freeTurns = loadedDoc.GetElementById("FreeTurns");
        freeTurns.InnerText = FreeTurns.ToString();

        //Create the parent of all the level elements
        query = string.Format("//*[@id='{0}']", "Levels");
        XmlElement levels = (XmlElement)loadedDoc.SelectSingleNode(query);

        int i = 0;
        //Add all of the level scores to the document
        foreach (var item in levels.ChildNodes)
        {
            var child = (item as XmlElement);
            child.InnerText = LevelScores[i].ToString();
            levels.AppendChild(child);

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
