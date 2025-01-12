using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;
using System.Linq;


[Serializable]
public class ObjectList
{
    public Size size;
    public List<ObjectData> objectList;
}

//게임의 시작, 실패, 초기화 관리
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private int currentRound = 1;
    private int totalScore; //전체 스코어
    private int score; //현재 라운드에 선반 위에 올려진 물체에 매겨진 점수 합산
    private int point; //현재 포인트
    private bool isOver; //게임 오버 제어
    private float currentTime;
    private bool isStart; //스타트 시 활성화
    private Transform boardTransform;
    private List<GameObject> curObjectsList;

    [SerializeField] GameObject DeadLine;
    [SerializeField] private List<RoundData> roundSO;
    [SerializeField] private ObjectList[] objects;
    [SerializeField] private int objectCount;

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
    [SerializeField] private int targetScore;

    public static GameManager Instance => instance;

    public ObjectList[] Objects => objects;

    public Canvas Canvas => canvas;
    public List<GameObject> emptyInventory;
    public GameObject objectGroup;

    private void Awake()
    {
        curObjectsList = new List<GameObject>();
        instance = this;
        boardTransform = board.transform;
        ObjectInit();
    }

    private void Start()
    {
        RoundStart();
    }

    private void RoundStart()
    {
        curObjectsList.Clear();
        DeadLine.GetComponent<BoxCollider2D>().isTrigger = false;
        //인벤토리를 다 비운다.
        foreach (Transform child in inventory.transform)
        {
            Destroy(child.gameObject);
        }
        board.transform.position = boardTransform.position;
        board.transform.rotation = boardTransform.rotation;
        CreateObj();
        isStart = false;
        isOver = false;
        point = maxPoint + (currentRound-1) * 20;
        ScoreUpdate();
        PointUpdate();
        currentTime = maxTime;
        TimerUpdate();
    }

    private void Update()
    {
        if (isStart && !isOver)
        {
            TimerDecrease();
        }
    }

    private void SetStart()
    {
        isStart = true;
        DeadLine.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void ObjectInit()
    {
        for (int i = 0; i < objects.Length; i++) 
        {
            for (int j = 0; j < objects[i].objectList.Count; j++)
            {
                ObjectData data = objects[i].objectList[j];

                data.go.GetComponent<DragObj>().index = data.index;
                data.ui.GetComponent<DragUI>().index = data.index;

                data.go.GetComponent<DragObj>().size = data.size;
                data.ui.GetComponent<DragUI>().size = data.size;

                data.go.GetComponent<Rigidbody2D>().mass = data.mass;
            }
        }
    }

    /// <summary>
    /// 게임 초기화 시 원 상태로 복구하는 함수
    /// </summary>
    public void GameInit()
    {
        //인벤토리를 다 비운다.
        foreach (Transform child in inventory.transform)
        {
            Destroy(child.gameObject);
        }
        board.transform.position = boardTransform.position;
        board.transform.rotation = boardTransform.rotation;
        for (int i = 0; i < curObjectsList.Count; i++)
        {
            GameObject go = Instantiate(square, inventory.transform);

            Instantiate(curObjectsList[i], go.transform);
        }
        isStart = false;
        isOver = false;
        ScoreUpdate();
        PointUpdate();
        currentTime = maxTime;
        TimerUpdate();
    }

    /// <summary>
    /// 인벤토리에 플레이어에게 도움을 주는 물체를 생성하는 함수
    /// </summary>
    public void CreateAssistObj()
    {
        if (point > 0)
        {
            GameObject go = Instantiate(square, inventory.transform);

            int index = UnityEngine.Random.Range(0, objects[(int)Size.Assist].objectList.Count);
            curObjectsList.Add(objects[(int)Size.Assist].objectList[index].ui);
            Instantiate(objects[(int)Size.Assist].objectList[index].ui, go.transform);
            point -= 10;
            PointUpdate();
        }
    }

    /// <summary>
    /// 인벤토리에 물체를 생성하는 함수
    /// </summary>
    public void CreateObj()
    {
        RoundData curData;
        int small = 0;
        int medium = 0;
        int high = 0;
        int special = 0;

        if (currentRound <= 10) //Low
        {
            curData = roundSO[0];
            small -= (currentRound-1);
            medium += (currentRound - 1);
        }
        else if (currentRound <= 15) //Medium
        {
            curData = roundSO[1];
        }
        else //High
        {
            curData = roundSO[2];
        }

        small += curData.lowProbability;
        medium += curData.mediumProbability + small;
        high += curData.highProbability + medium;
        special += curData.specialProbability + high;

        for (int i = 0; i < objectCount; i++)
        {
            int random = UnityEngine.Random.Range(0, 100);
            List<ObjectData> curObj = null;

            if (random < small)
            {
                curObj = objects[(int)Size.Small].objectList.ToList();
            }
            else if (random < medium)
            {
                curObj = objects[(int)Size.Medium].objectList.ToList();
            }
            else if (random < high)
            {
                curObj = objects[(int)Size.Large].objectList.ToList();
            }
            else if (random < special)
            {
                curObj = objects[(int)Size.Special].objectList.ToList();
            }

            GameObject go = Instantiate(square, inventory.transform);

            int index = UnityEngine.Random.Range(0, curObj.Count);

            curObjectsList.Add(curObj[index].ui);
            Instantiate(curObj[index].ui, go.transform);
        }

    }

    //게임 시작 버튼 누를 시
    public void GamePlay()
    {
        SetStart();
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
            score += ((int)obj.GetComponent<Rigidbody2D>().mass * 10 + currentRound * 100);
            //오브젝트 삭제
            Vector3 transform = obj.gameObject.transform.position;
            obj.GetComponent<DragObj>().InputDisable();
            Destroy(obj.gameObject);

            Instantiate(destroyPrefab, transform, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }

        //마지막으로 남은 포인트까지 합산
        score += (point * currentRound * 10);
        totalScore += score;
        ScoreUpdate();
        yield return new WaitForSeconds(1f);

        clear = clear && score > targetScore;
        if (clear) //다음 라운드 진행
        {
            Debug.Log("클리어");
            score = 0;
            currentRound++;
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

        //다시 인벤토리를 초기 상태로 채워 넣음
        GameInit();
    }

    public void ScoreUpdate()
    {
        String textScore = "" + totalScore;

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
        if (textTime.Length - 3 > 0)
        {
            StringBuilder sb = new StringBuilder(textTime);
            int index = textTime.Length - 3;
            sb[index] = ':';  // 해당 인덱스를 수정

            textTime = sb.ToString();  // 수정된 문자열을 다시 할당

            if (textTime.Length == 4)
            {
                textTime = "0" + textTime;
            }

            //아니 인덱스 접근이 안되는게 이게 언어입니까?
        }


        timer.text = textTime;
    }

    public void TimerDecrease()
    {
        if (currentTime > 0.0f)
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
