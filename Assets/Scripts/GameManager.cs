using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;
using System.Linq;
using DontFall.UI;
using DontFall.Transition;
using DontFall.Board;

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
    private int totalScore = 0; //전체 스코어
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

    [SerializeField] private ScoreSO scoreData;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject square; //인벤토리 한 칸
    [SerializeField] private ScoreToggle scoreToggle;
    [SerializeField] private TransitionManager transitionManager;
    [SerializeField] private BoardController boardController;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text pointText;
    [SerializeField] private float maxTime = 30.00f;
    [SerializeField] private int maxPoint; //초기에 주어지는 포인트
    [SerializeField] private GameObject destroyPrefab;
    [SerializeField] private int targetScore;

    [SerializeField] private DontFall.PlaySound playSound;
    [SerializeField] private AudioClip popSound;
    [SerializeField] private AudioClip dropSound;

    public static GameManager Instance => instance;

    public ObjectList[] Objects => objects;

    public Canvas Canvas => canvas;
    public List<GameObject> emptyInventory;
    public GameObject objectGroup;

    private void Awake()
    {
        curObjectsList = new List<GameObject>();
        instance = this;
        ObjectInit();
    }

    private void Start()
    {
        RoundStart();
    }

    private void RoundStart()
    {
        boardController.Moving = false;
        maxPoint = 50 + (currentRound - 1) * 20;
        curObjectsList.Clear();
        DeadLine.GetComponent<BoxCollider2D>().isTrigger = false;

        //인벤토리를 다 비운다.
        foreach (Transform child in inventory.transform)
        {
            Destroy(child.gameObject);
        }
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
        if (isStart && !isOver)
        {
            TimerDecrease();
        }
    }

    private void SetStart()
    {
        isStart = true;
        boardController.Moving = true;
        DeadLine.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void ObjectInit()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            for (int j = 0; j < objects[i].objectList.Count; j++)
            {
                ObjectData data = objects[i].objectList[j];

                data.go.GetComponent<DragObj>().index = data.Index;
                data.ui.GetComponent<DragUI>().index = data.Index;

                data.go.GetComponent<DragObj>().size = data.Size;
                data.ui.GetComponent<DragUI>().size = data.Size;

                data.go.GetComponent<Rigidbody2D>().mass = data.Mass;
                data.go.GetComponent<DragObj>().DifficultyLevel = data.difficultyLevel;
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
    /// 인벤토리에 물체를 생성하는 함수
    /// </summary>
    public void CreateObj()
    {
        if(point <= 0)
        {
            point = 0;
            return;
        }
        point -= 10;

        PointUpdate();

        RoundData curData;
        int small = 0;
        int medium = 0;
        int high = 0;
        int special = 0;

        if (currentRound <= 10) //Low
        {
            curData = roundSO[0];
            small -= (currentRound - 1);
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

        Debug.Log(small);
        int smallTargetScore = Mathf.RoundToInt((maxPoint / 10) * (small / 100));
        int mediumTargetScore = Mathf.RoundToInt((maxPoint / 10) * (medium / 100));
        int highTargetScore = Mathf.RoundToInt((maxPoint / 10) * (high / 100));
        int specialTargetScore = Mathf.RoundToInt((maxPoint / 10) * (special / 100));

        //targetScore = currentRound * (smallTargetScore + 10 * 5 * mediumTargetScore + 30 * 15 * highTargetScore);
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
        var ui = Instantiate(curObj[index].ui, go.transform).GetComponent<DragUI>();
        ui.playSound = playSound;
        ui.dropSound = dropSound;

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


    IEnumerator GameOverRoutine(bool clear)
    {
        //활성화 된 모든 오브젝트 가져오기
        DragObj[] objs = FindObjectsByType<DragObj>(FindObjectsSortMode.None);

        foreach (var obj in objs)
        {
            obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }

        //모든 오브젝트에 접근해서 하나씩 지우기
        foreach (var obj in objs)
        {
            //물체가 가진 고유 포인트 합산
            score += ((int)obj.GetComponent<Rigidbody2D>().mass * obj.GetComponent<DragObj>().DifficultyLevel * currentRound);
            //오브젝트 삭제
            Vector3 transform = obj.gameObject.transform.position;
            obj.GetComponent<DragObj>().InputDisable();
            Destroy(obj.gameObject);
            playSound.Play(popSound);

            Instantiate(destroyPrefab, transform, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }

        //마지막으로 남은 포인트까지 합산
        score += (point * currentRound * 10);
   
        ScoreUpdate();
        yield return new WaitForSeconds(1f);

        clear = clear && score > targetScore;
        if (clear) //다음 라운드 진행
        {
            transitionManager.StartTransition(true, true, () => {
                Debug.Log("클리어");
                GameManager.Instance.emptyInventory.Clear();
                totalScore += score;
                score = 0;
                currentRound++;
                RoundStart();
                transitionManager.StartTransition(false, true, () => { });
            });
        }
        else //게임오버 씬으로 이동 or 종료창
        {
            transitionManager.StartTransition(true, true, () => {
                scoreData.totalScore = totalScore;
                scoreData.roundScore = score;
                SceneManager.LoadScene("GameOverScene");
            });
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
        scoreToggle.Score = totalScore;
        scoreToggle.Point = (score, targetScore);
    }

    public void PointUpdate()
    {
        string textPoint = string.Format("{0:D6}", point);

        pointText.text = textPoint;
    }

    public void TimerUpdate()
    {
        string textTime = currentTime.ToString("F2");
        string[] parts = textTime.Split('.');
        int integerPart = Int32.Parse(parts[0]);
        int decimalPart = Int32.Parse(parts[1]);

        textTime = string.Format("{0:D2}:{1:D2}", integerPart, decimalPart);

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
