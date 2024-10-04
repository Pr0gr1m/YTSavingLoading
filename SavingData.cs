using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SavingData : MonoBehaviour
{
    [SerializeField] private int Integer;

    public string path;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SaveGame();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
    }

    void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream;
        if (!File.Exists(Application.persistentDataPath + path))
        {
            stream = File.Create(Application.persistentDataPath + path);
        } else
        {
            stream = File.Open(Application.persistentDataPath + path, FileMode.Open);
        }

        SaveData savedData = new SaveData()
        {
            saveInt = Integer
        };

        formatter.Serialize(stream, savedData);
        stream.Close();
    }

    void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(Application.persistentDataPath + path, FileMode.Open);

            SaveData data = (SaveData)formatter.Deserialize(stream);
            Integer = data.saveInt;

            stream.Close();
        }
        else
        {
            Debug.LogError("No stream exists");
        }
    }
}

[Serializable]
class SaveData
{
    public int saveInt;
}