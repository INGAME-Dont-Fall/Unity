using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int Count;
    public int Score;
    public bool IsOver;

    public Object lastObject;
    public GameObject[] ObjectPrefab;
    public GameObject effectPrefab;
    public Transform ObjectGroup;

    [Range(1, 30)]
    public int PoolSize;
    public int PoolCursor;

    public GameObject[] HiddenObj;

    //�̱���
    public static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        Application.targetFrameRate = 60; //������ 60���� ����
    }

    private void Start()
    {
        GameStart();
    }

    public void GameStart()
    {
        Invoke("NextObject", 1.5f);
    }

    Object MakeObject()
    {
        int index = Random.Range(0, 3);

        //���ο� ������Ʈ�� ������Ʈ �׷쿡 ����Ͽ� ����
        GameObject instantObject = Instantiate(ObjectPrefab[index], ObjectGroup);
        instantObject.name = "Object" + ++Count;
        Object instantObj = instantObject.GetComponent<Object>();

        return instantObj;
    }

    void NextObject()
    {
        if (IsOver)
        {
            return;
        }

        lastObject = MakeObject();

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

        lastObject.Drag();
    }
    public void TouchUp()
    {
        if (lastObject == null)
        {
            return;
        }

        lastObject.Drop();
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
        //Ȱ��ȭ �� ��� ������Ʈ ��������
        Object[] Obj = FindObjectsOfType<Object>();

        //��� ������Ʈ�� ����ȿ�� ����
        for (int index = 0; index < Obj.Length; ++index)
        {
            Obj[index].rb2d.simulated = false;
        }

        //��� ������Ʈ�� �����ؼ� �ϳ��� �����
        for (int index = 0; index < Obj.Length; ++index)
        {
            Obj[index].GetComponent<Effect>().GetComponent<Animator>().SetTrigger("Destroy");
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

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
