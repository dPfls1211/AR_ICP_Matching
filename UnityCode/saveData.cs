using System.Collections;
using System.Collections.Generic;// let us use lists
using UnityEngine;
using System.IO;                //file management
using System;


public class saveData : MonoBehaviour
{
    static GameObject _container;
    static GameObject Contaner
    {
        get
        {
            return _container;
        }
    }
    static saveData _instance;
    public static saveData Instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "savedata";
                _instance = _container.AddComponent(typeof(saveData)) as saveData;
                //NotDestroyOnLoad(_container);
            }
            return _instance;
        }
    }
    public string GameDataFileName = "data.json"; //이름 변경 XX

    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            if(_gameData == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }
    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;
        if (File.Exists(filePath))
        {
            Debug.Log("불러오기");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            Debug.Log("새파일");
            _gameData = new GameData();
        }
    }
    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;
        File.WriteAllText(filePath, ToJsonData);
        Debug.Log("데이터 저장");
    }
    private void OnApplicationQuit()
    {
        Debug.Log("새파일");
        SaveGameData();
    }
}