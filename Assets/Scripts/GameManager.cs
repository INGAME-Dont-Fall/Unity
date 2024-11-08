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
    private bool ActiveDead;

    private Object lastObject;
    [SerializeField] private GameObject ObjectGroup;
    [SerializeField] List<ObjectInfo> gamePrefab;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject Square; //인벤토리 한 칸
    [SerializeField] int[] InitObj = { 0, 1, 1, 0, 1 };

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
        for(int i = 0; i < InitObj.Length; i++) 
        {
            GameObject go = Instantiate(Square, Inventory.transform);

            int index = InitObj[i];
            Instantiate(gamePrefab[index].gameUI, go.transform);
        }
    }

    //게임 시작 버튼 누를 시
    public void GamePlay()
    {
        //모든 오브젝트의 그래비티 스케일 변경


        //데드존을 활성화하여 닿는지 검사가 가능해짐(트리거 함수는 게임 오브젝트에서)
        ActiveDead = true;
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

        //모든 오브젝트에 접근해서 하나씩 지우기
        foreach(var obj in objs)
        {
            //물체가 가진 고유 포인트 합산
            
            //오브젝트 삭제
            Destroy(obj);
            yield return new WaitForSeconds(0.1f);
        }

        //마지막으로 남은 포인트까지 합산
        Score += Point;
        yield return new WaitForSeconds(1f);

        //게임오버 씬으로 이동 or 종료창
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
