using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class ObjectInfo
{
    public GameObject gameObj;
    public GameObject gameUI;
}

//게임의 시작, 실패, 초기화 관리
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private int Point; //초기에 주어지는 포인
    private int Score; //선반 위에 올려진 물체에 매겨진 점수 합산
    private bool IsOver; //게임 오버 제어

    private Object lastObject;
    [SerializeField] private GameObject ObjectGroup;
    [SerializeField] List<ObjectInfo> gamePrefab;
    [SerializeField] Canvas canvas;

    public static GameManager Instance => instance;
    public List<ObjectInfo> GamePrefab => gamePrefab;
    public Canvas Canvas => canvas;
    public List<GameObject> EmptyInventory;


    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60; //프레임 60으로 고정
    }

    private void Start()
    {
        GameStart();
    }

    public void GameStart()
    {

    }

    public void GameOver()
    {
        if (IsOver)
        {
            return;
        }
        IsOver = true;

        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        //활성화 된 모든 오브젝트 가져오기
        Object[] objs = FindObjectsOfType<Object>();

        ////모든 오브젝트의 물리효과 제거
        //foreach(var obj in objs)
        //{
        //    obj.Rb2d.simulated = false;
        //}

        //모든 오브젝트에 접근해서 하나씩 지우기
        foreach(var obj in objs)
        {
            //obj.GetComponent<Animator>().SetTrigger("Destroy");
            Destroy(obj);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        //게임오버 씬으로 이동
    }

    //초기화 버튼 이벤트
    public void Restart()
    {
        StartCoroutine(ResetCorutine());
    }


    IEnumerator ResetCorutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
}
