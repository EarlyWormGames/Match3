using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using RijndaelManagedEncryption;
using System;

public class SavedData : MonoBehaviour
{
    public static int FreeTurns;

    public static void Load()
    {
        string contents = File.ReadAllText(Application.dataPath + "/SaveData.txt");
        contents = Encryption.DecryptRijndael(contents, "B9FCCD1BF5772EF9");

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(contents);

        XmlElement freeTurns = xmlDoc.GetElementById("FreeTurns");
        FreeTurns = Convert.ToInt32(freeTurns.InnerText);
    }

    public static void Save()
    {
        XmlDocument xmlDoc = new XmlDocument();;

        XmlElement freeTurns = xmlDoc.CreateElement("FreeTurns");
        freeTurns.InnerText = FreeTurns.ToString();

        string contents = "";
        using (var stringWriter = new StringWriter())
        using (var xmlTextWriter = XmlWriter.Create(stringWriter))
        {
            xmlDoc.WriteTo(xmlTextWriter);
            xmlTextWriter.Flush();
            contents = stringWriter.GetStringBuilder().ToString();
        }

        contents = Encryption.EncryptRijndael(contents, "B9FCCD1BF5772EF9");

        File.WriteAllText(Application.dataPath + "/SaveData.txt", contents);
    }
}
