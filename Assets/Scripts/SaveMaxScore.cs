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
        //������ ��������������
        if (!File.Exists(path))
        {
            Debug.Log("�ش� ���� ������������");
            saveData.maxScore = totalScore;
            JsonSave();
        }
        else //������ �����ϸ�
        {
            //�ҷ�����
            string JsonData = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(JsonData);

            if (saveData != null) //saveData�� ���� ������
            {
                //�ش� ������ ������ ������
                saveData.maxScore = saveData.maxScore > totalScore ? saveData.maxScore : totalScore;
            }
        }
    }

    private void JsonSave()
    {
        //�����ϱ�
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }
}