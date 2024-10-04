using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public int Score;
    public bool IsOver;

    public Object lastObject;
    public GameObject ObjectPrefab;
    public Transform ObjectGroup;

    [Range(1, 30)]
    public int PoolSize;
    public int PoolCursor;
    public List<Object> ObjectPool;

    public GameObject[] HiddenObj;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60; //������ 60���� ����

        ObjectPool = new List<Object>();


    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Object MakeObject()
    {
        //���ο� ������ ���� �׷쿡 ����Ͽ� ����
        GameObject instantObject = Instantiate(ObjectPrefab, ObjectGroup);
        instantObject.name = "Object" + ObjectPool.Count;
        Object instantObj = instantObject.GetComponent<Object>();
        ObjectPool.Add(instantObj);

        return instantObj;
    }

    Object GetObject()
    {
        for (int index = 0; index < ObjectPool.Count; index++)
        {
            PoolCursor = (PoolCursor + 1) % ObjectPool.Count;
            if (!ObjectPool[PoolCursor].gameObject.activeSelf)
            {
                return ObjectPool[PoolCursor];
            }
        }

        //�� Ȱ��ȭ �Ǿ��־ �Ѱ��� ���� ������?
        return MakeObject();
    }
    void NextObject()
    {
        if (IsOver)
        {
            return;
        }

        lastObject = GetObject();
        //���⼭ ������ �����������

        //������Ʈ Ȱ��ȭ
        lastObject.gameObject.SetActive(true);

        StartCoroutine("WaitNext");
    }

    //�ڷ�ƾ ����
    IEnumerator WaitNext()
    {
        while (lastObject != null)
        {
            yield return null;
        }
        //2.5�� �޽�
        yield return new WaitForSeconds(2.5f);

        NextObject();
    }

    public void TouchDown()
    {
        if (lastObject == null)
        {
            return;
        }

        //lastObject.Drag();
    }
    public void TouchUp()
    {
        if (lastObject == null)
        {
            return;
        }

        //lastObject.Drop();
        lastObject = null;
    }

    public void GameOver()
    {
        if (IsOver)
        {
            return;
        }
        IsOver = true;

        StartCoroutine("GameOverRoutine");
    }
    IEnumerator GameOverRoutine()
    {
        //Ȱ��ȭ �� ��� ���� ��������
        Object[] Obj = FindObjectsOfType<Object>();

        //��� �������� ����ȿ�� ����
        for (int index = 0; index < Obj.Length; ++index)
        {
            Obj[index].rb2d.simulated = false;
        }

        //��� ���ۿ� �����ؼ� �ϳ��� �����
        for (int index = 0; index < Obj.Length; ++index)
        {
            //���� ���� �� ���� ���� �ѱ��.
            Obj[index].Hide(Vector3.up * 100);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        // �ְ� ���� ����
        int maxScore = Mathf.Max(Score, PlayerPrefs.GetInt("MaxScore"));
        PlayerPrefs.SetInt("MaxScore", maxScore);

        //���ӿ��� ������ �̵�
    }

    public void Restart()
    {
        StartCoroutine("ResetCorutine");
    }

    IEnumerator ResetCorutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
}
