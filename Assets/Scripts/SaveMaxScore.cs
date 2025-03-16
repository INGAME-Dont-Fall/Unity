using System;
using System.IO;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;


[Serializable]
public class SaveData
{
    public int maxScore;
    public SaveData(int _maxScore)
    {
        maxScore = _maxScore;
    }
}

public class SaveMaxScore : MonoBehaviour
{
    [SerializeField] private TMP_Text maxScore_Text;

    private SaveData saveData = new SaveData(0);

    private string path;
    private int totalScore;
    public void ExcuteSave(int _totalScore)
    {
        totalScore = _totalScore;
        path = Path.Combine(Application.persistentDataPath, "SaveData.json");
        JsonLoad();
        maxScore_Text.text = string.Format("{0:D6}", saveData.maxScore);
        JsonSave();
    }

    private void JsonLoad()
    {
        //파일이 존재하지않으면
        if (!File.Exists(path))
        {
            Debug.Log("해당 파일 존재하지않음");
            saveData.maxScore = totalScore;
            JsonSave();
        }
        else //파일이 존재하면
        {
            //불러오기
            string JsonData = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(JsonData);

            if (saveData != null) //saveData에 뭐가 있으면
            {
                //해당 내용을 저장할 변수들
                saveData.maxScore = saveData.maxScore > totalScore ? saveData.maxScore : totalScore;
            }
        }
    }

    private void JsonSave()
    {
        //저장하기
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }
}