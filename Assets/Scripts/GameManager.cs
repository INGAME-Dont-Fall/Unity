using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    private int point; //초기에 주어지는 포인
    private int score; //선반 위에 올려진 물체에 매겨진 점수 합산
    private bool isOver; //게임 오버 제어
    private bool activeDead;

    private Object lastObject;
    [SerializeField] private GameObject objectGroup;
    [SerializeField] private List<ObjectInfo> gamePrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject square; //인벤토리 한 칸
    [SerializeField] int[] initObj = { 0, 1, 1, 0, 1 };
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text pointText;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private float maxTime = 30.0f;

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
        for (int i = 0; i < initObj.Length; i++)
        {
            GameObject go = Instantiate(square, inventory.transform);

            int index = initObj[i];
            Instantiate(gamePrefab[index].gameUI, go.transform);
        }
    }

    //게임 시작 버튼 누를 시
    public void GamePlay()
    {
        //모든 오브젝트의 그래비티 스케일 변경
        var objs = FindObjectsByType<DragObj>(FindObjectsSortMode.None);
        foreach (var obj in objs)
        {
            obj.GetComponent<Rigidbody2D>().gravityScale = 1;
        }


        //데드존을 활성화하여 닿는지 검사가 가능해짐(트리거 함수는 게임 오브젝트에서)
        activeDead = true;
    }

    public void GameOver()
    {
        if (isOver)
        {
            return;
        }
        isOver = true;

        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        //활성화 된 모든 오브젝트 가져오기
        Object[] objs = FindObjectsByType<Object>(FindObjectsSortMode.None);

        //모든 오브젝트에 접근해서 하나씩 지우기
        foreach (var obj in objs)
        {
            //물체가 가진 고유 포인트 합산

            //오브젝트 삭제
            Destroy(obj);
            yield return new WaitForSeconds(0.1f);
        }

        //마지막으로 남은 포인트까지 합산
        score += point;
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
        SceneManager.LoadScene("PlayScnene");
    }

    public void ScoreUpdate()
    {
        scoreText.text = "" + score;
    }

    public void PointUpdate()
    {
        pointText.text = "" + point;
    }

    public void TimerUpdate()
    {
        timer.text = "" + maxTime;
    }
}
