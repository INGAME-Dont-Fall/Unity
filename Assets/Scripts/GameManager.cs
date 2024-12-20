using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Mathematics;
using Unity.VisualScripting;
using System.Text;
using UnityEngine.UIElements;

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

    private int score; //선반 위에 올려진 물체에 매겨진 점수 합산
    private int point; //현재 포인트
    private bool isOver; //게임 오버 제어
    private float currentTime;
    private bool isStart; //스타트 시 활성화
    private Transform boardTransform;
    private int[] initObj = { 0, 1, 1 }; //초기 인벤토리 할당 값

    private Object lastObject;
    [SerializeField] private List<ObjectInfo> gamePrefab; //게임 오브젝트들
    [SerializeField] private List<ObjectInfo> assistPrefab; //벽 같은 생성 오브젝트
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject square; //인벤토리 한 칸
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text pointText;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private float maxTime = 30.00f;
    [SerializeField] private int maxPoint; //초기에 주어지는 포인트
    [SerializeField] private GameObject destroyPrefab;
    [SerializeField] private GameObject board; //중심 판

    public static GameManager Instance => instance;
    public List<ObjectInfo> GamePrefab => gamePrefab;
    public List<ObjectInfo> AssistPrefab => assistPrefab;
    public Canvas Canvas => canvas;
    public List<GameObject> emptyInventory;
    public GameObject objectGroup;

    private void Awake()
    {
        instance = this;
        boardTransform = board.transform;
    }

    private void Start()
    {
        RoundStart();
    }

    private void RoundStart()
    {
        //인벤토리를 다 비운다.
        foreach (Transform child in inventory.transform)
        {
            Destroy(child.gameObject);
        }
        board.transform.position = boardTransform.position;
        board.transform.rotation = boardTransform.rotation;
        GameInit();
        isStart = false;
        isOver = false;
        point = maxPoint;
        ScoreUpdate();
        PointUpdate();
        currentTime = maxTime;
        TimerUpdate();
    }

    private void Update()
    {
        if(isStart && !isOver)
        {
            TimerDecrease();
        }
    }

    private void SetStart()
    {
        isStart = true;
    }

    public void GameInit()
    {
        for (int i = 0; i < initObj.Length; i++)
        {
            GameObject go = Instantiate(square, inventory.transform);

            int index = initObj[i];
            Instantiate(gamePrefab[index].gameUI, go.transform);
        }
    }

    public void CreateAssistObj()
    {
        if(point > 0)
        {
            GameObject go = Instantiate(square, inventory.transform);

            int index = UnityEngine.Random.Range(0, AssistPrefab.Count);
            Instantiate(AssistPrefab[index].gameUI, go.transform);
            point -= 10;
            PointUpdate();
        }
    }

    //게임 시작 버튼 누를 시
    public void GamePlay()
    {
        SetStart();
        //모든 오브젝트의 그래비티 스케일 변경
        var objs = FindObjectsByType<DragObj>(FindObjectsSortMode.None);
        foreach (var obj in objs)
        {
            obj.GetComponent<Rigidbody2D>().gravityScale = 1;
        }


        //데드존을 활성화하여 닿는지 검사가 가능해짐(트리거 함수는 게임 오브젝트에서)
    }

    public void GameOver()
    {
        if (isOver)
        {
            return;
        }
        isOver = true;

        StartCoroutine(GameOverRoutine(false));
    }

    private void nextRound()
    {
        
    }

    IEnumerator GameOverRoutine(bool clear)
    {
        //활성화 된 모든 오브젝트 가져오기
        DragObj[] objs = FindObjectsByType<DragObj>(FindObjectsSortMode.None);

        foreach (var obj in objs)
        {
            obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
        board.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        //모든 오브젝트에 접근해서 하나씩 지우기
        foreach (var obj in objs)
        {
            //물체가 가진 고유 포인트 합산

            //오브젝트 삭제
            Vector3 transform = obj.gameObject.transform.position;
            obj.GetComponent<DragObj>().InputDisable();
            Destroy(obj.gameObject);

            Instantiate(destroyPrefab, transform, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }

        //마지막으로 남은 포인트까지 합산
        score += point;
        ScoreUpdate();
        yield return new WaitForSeconds(1f);

        if(clear) //다음 라운드 진행
        {
            nextRound();
            RoundStart();
        }
        else //게임오버 씬으로 이동 or 종료창
        {
            SceneManager.LoadScene("GameManager");
        }
    }

    //초기화 버튼 이벤트
    public void Restart()
    {
        foreach (Transform obj in objectGroup.transform)
        {
            obj.GetComponent<DragObj>().InputDisable();
            Destroy(obj.gameObject);
        }

        //인벤토리를 다 비운다.
        foreach (Transform child in inventory.transform)
        {
            Destroy(child.gameObject);
        }

        //다시 인벤토리를 초기 상태로 채워 넣음
        GameInit();

        point = maxPoint;
        PointUpdate();
    }

    public void ScoreUpdate()
    {
        String textScore = "" + score;

        int Length = textScore.Length;
        for (int i = Math.Max(6, Length); i > Length; --i)
        {
            textScore = "0" + textScore;
        }

        scoreText.text = textScore;
    }

    public void PointUpdate()
    {
        String textPoint = "" + point;

        int Length = textPoint.Length;
        for (int i = Math.Max(6, Length); i > Length; --i)
        {
            textPoint = "0" + textPoint;
        }

        pointText.text = textPoint;
    }

    public void TimerUpdate()
    {
        String textTime = currentTime.ToString("F2");
        if(textTime.Length - 3 > 0)
        {
            StringBuilder sb = new StringBuilder(textTime);
            int index = textTime.Length - 3;
            sb[index] = ':';  // 해당 인덱스를 수정

            textTime = sb.ToString();  // 수정된 문자열을 다시 할당

            if(textTime.Length == 4)
            {
                textTime = "0" + textTime;
            }

            //아니 인덱스 접근이 안되는게 이게 언어입니까?
        }


        timer.text = textTime;
    }

    public void TimerDecrease()
    {
        if(currentTime > 0.0f)
        {
            currentTime -= Time.deltaTime;
            TimerUpdate();
        }
        else
        {
            currentTime = 0.00f;
            TimerUpdate();
            isStart = false;
            StartCoroutine(GameOverRoutine(true));
        }
    }
}
