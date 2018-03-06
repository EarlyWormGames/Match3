using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;

[System.Serializable]
public class SaveData
{
    public static SaveData instance;
    private static int keySize = 256, ivSize = 16;
    public static string key = "AE3FD4635F726D121568AEED2885FI21";
    public static bool IsDev = false;

    public int FreeTurns;
    public List<int> LevelScores = new List<int>();
    public int LastArcade = -1;
    public int ArcadeScore;
    public SaveableVector2 ScrollPoint = Vector2.zero;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        Load();
    }

    public static void Load()
    {
        if (!File.Exists(Application.persistentDataPath + "/save.data"))
        {
            instance = new SaveData();
            return;
        }

        instance = null;
        try
        {
            byte[] bytes;
            bytes = Encoding.UTF8.GetBytes(key);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.data", FileMode.Open);
            var crypto = CreateDecryptionStream(bytes, file);

            instance = (SaveData)bf.Deserialize(crypto);
            crypto.Close();
            file.Close();
        }
        catch { }

        if (instance == null)
            instance = new SaveData();
    }

    public static void Save()
    {
        byte[] bytes;
        bytes = Encoding.UTF8.GetBytes(key);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.data");
        var crypto = CreateEncryptionStream(bytes, file);

        bf.Serialize(crypto, instance);
        crypto.Close();
        file.Close();
    }

    public static CryptoStream CreateEncryptionStream(byte[] key, Stream outputStream)
    {
        byte[] iv = new byte[ivSize];

        var rng = new RNGCryptoServiceProvider();
        // Using a cryptographic random number generator
        rng.GetNonZeroBytes(iv);

        // Write IV to the start of the stream
        outputStream.Write(iv, 0, iv.Length);

        Rijndael rijndael = new RijndaelManaged();
        rijndael.KeySize = keySize;

        CryptoStream encryptor = new CryptoStream(
            outputStream,
            rijndael.CreateEncryptor(key, iv),
            CryptoStreamMode.Write);
        return encryptor;
    }

    public static CryptoStream CreateDecryptionStream(byte[] key, Stream inputStream)
    {
        byte[] iv = new byte[ivSize];

        if (inputStream.Read(iv, 0, iv.Length) != iv.Length)
        {
            Debug.LogError("Failed to read IV from stream.");
        }

        Rijndael rijndael = new RijndaelManaged();
        rijndael.KeySize = keySize;

        CryptoStream decryptor = new CryptoStream(
            inputStream,
            rijndael.CreateDecryptor(key, iv),
            CryptoStreamMode.Read);
        return decryptor;
    }
}

[System.Serializable]
public class SaveableVector2
{
    public float x, y;

    public static implicit operator SaveableVector2(Vector2 val)
    {
        if (val == null)
            val = new Vector2();

        var v2 = new SaveableVector2();
        v2.x = val.x;
        v2.y = val.y;
        return v2;
    }

    public static implicit operator Vector2(SaveableVector2 val)
    {
        if (val == null)
            val = new SaveableVector2();

        var v2 = new Vector2();
        v2.x = val.x;
        v2.y = val.y;
        return v2;
    }
}