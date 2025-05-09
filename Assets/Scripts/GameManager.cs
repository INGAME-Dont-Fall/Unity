using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DontFall.Board;
using DontFall.Transition;
using DontFall.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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

    private int itemsCount;
    private int targetItemsCount;
    private float small = 0;
    private float medium = 0;
    private float high = 0;
    private float special = 0;
    private int currentRound = 1;
    private int totalScore = 0; //전체 스코어
    private int score; //현재 라운드에 선반 위에 올려진 물체에 매겨진 점수 합산
    private int point; //현재 포인트
    private bool isOver; //게임 오버 제어
    private float currentTime;
    private bool isStart; //스타트 시 활성화
    private Transform boardTransform;
    private List<ObjectData> curObjectsList;
    private OverlayController overlayController;

    [SerializeField] private PlayData playData;
    [SerializeField] private TMP_Text objectCountText;
    [SerializeField] private Slider zoom;
    [SerializeField] private GameObject DeadLine;
    [SerializeField] private List<RoundData> roundSO;
    [SerializeField] private ObjectList[] objects;
    [SerializeField] private int objectCount;

    [SerializeField] private GameObject startButton;
    [SerializeField] private ScoreSO scoreData;
    [SerializeField] private GameObject itmes;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject inventoryArea;
    [SerializeField] private GameObject square; //인벤토리 한 칸
    [SerializeField] private ScoreToggle scoreToggle;
    [SerializeField] private TransitionManager transitionManager;
    [SerializeField] private BoardController boardController;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text pointText;
    [SerializeField] private float maxTime = 30.00f;
    [SerializeField] private int maxPoint; //초기에 주어지는 포인트
    [SerializeField] private GameObject destroyPrefab;

    [SerializeField] private DontFall.PlaySound playSound;
    [SerializeField] private AudioClip popSound;
    [SerializeField] private AudioClip dropSound;

    [SerializeField] private Settings settings;

    public static GameManager Instance => instance;

    public ObjectList[] Objects => objects;

    public Canvas Canvas => canvas;
    public GameObject objectGroup;
    public CanvasGroup CanvasGroup => canvasGroup;
    public GameObject InventoryArea => inventoryArea;

    public event Action GameStart;
    public event Action GameEnd;

    private void Awake()
    {
        overlayController = InventoryArea.GetComponent<OverlayController>();
        canvasGroup = canvas.GetComponent<CanvasGroup>();
        curObjectsList = new List<ObjectData>();
        instance = this;
        ObjectInit();
    }

    private void Start()
    {
        RoundStart();
    }

    private void RoundStart()
    {
        currentRound = playData.currentRound;
        totalScore = playData.totalScore;
        score = 0;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        zoom.value = 13;
        startButton.SetActive(false);
        inventoryArea.SetActive(true);
        boardController.Moving = false;
        maxPoint = 30 + (currentRound - 1) * 5;
        curObjectsList.Clear();
        DeadLine.GetComponent<BoxCollider2D>().isTrigger = false;
        scoreToggle.Round = currentRound;

        itemsCount = 0;
        targetItemsCount = ((currentRound - 1) / 5) + 2;
        UpdateObjectCount();

        //인벤토리를 다 비운다.
        foreach (Transform child in inventory.transform)
        {
            Destroy(child.gameObject);
        }
        isStart = false;
        isOver = false;
        point = maxPoint;
        SetRoundProbability();
        ScoreUpdate();
        PointUpdate();
        currentTime = maxTime;
        TimerUpdate();
    }

    private void SetRoundProbability()
    {
        int adjustmentValue;
        RoundData curData;

        if (currentRound < 10) //Low 1~9
        {
            curData = roundSO[0];
            adjustmentValue = currentRound - 1;
            small = curData.lowProbability - adjustmentValue;
            medium = curData.mediumProbability + adjustmentValue;
            high = curData.highProbability;
            special = curData.specialProbability;
        }
        else if (currentRound < 20) //Medium 10~14
        {
            curData = roundSO[1];
            adjustmentValue = currentRound - 10;
            small = curData.lowProbability - adjustmentValue;
            medium = curData.mediumProbability + adjustmentValue / 2.0f;
            high = curData.highProbability + adjustmentValue / 2.0f;
            special = curData.specialProbability;
        }
        else //High 15 ~
        {
            curData = roundSO[2];
            small = curData.lowProbability;
            medium = curData.mediumProbability;
            high = curData.highProbability;
            special = curData.specialProbability;
        }

        medium += small;
        high += medium;
        special += high;

        Debug.Log(string.Format("small : {0}%", small));
        Debug.Log(string.Format("meditum : {0}%", medium));
        Debug.Log(string.Format("high : {0}%", high));
        Debug.Log(string.Format("special : {0}%", special));
    }

    private void Update()
    {
        if (isStart && !isOver)
        {
            TimerDecrease();
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (settings.Openned)
            {
                settings.CloseOption();
            }
            else
            {
                settings.OpenOption();
            }
        }
    }

    private void UpdateObjectCount()
    {
        objectCountText.text = string.Format("{0}/{1}", itemsCount, targetItemsCount);
    }

    private void SetStart()
    {
        inventoryArea.SetActive(false);
        isStart = true;
        boardController.Moving = true;
        DeadLine.GetComponent<BoxCollider2D>().isTrigger = true;

        GameStart?.Invoke();
    }

    public void SkipRound()
    {
        transitionManager.StartTransition(true, true, () =>
        {
            Debug.Log("클리어");
            score = 0;
            playData.currentRound++;
            playData.totalScore = totalScore;
            SceneManager.LoadScene("PlayScene");
        });
    }

    private void ObjectInit()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            for (int j = 0; j < objects[i].objectList.Count; j++)
            {
                ObjectData data = objects[i].objectList[j];

                data.go.GetComponent<DragObj>().index = j;
                data.ui.GetComponent<DragUI>().index = j;

                data.go.GetComponent<DragObj>().size = data.Size;
                data.ui.GetComponent<DragUI>().size = data.Size;

                data.go.GetComponent<Rigidbody2D>().mass = data.Mass;
                data.go.GetComponent<DragObj>().mass = data.Mass;
                data.go.GetComponent<DragObj>().DifficultyLevel = data.difficultyLevel;
            }
        }
    }

    //특정 오브젝트 삭제
    public void RemoveCurrentList(ObjectData data)
    {
        for (int i = 0; i < curObjectsList.Count; ++i)
        {
            if (curObjectsList[i] == data)
            {
                curObjectsList.RemoveAt(i);
                break;
            }
        }
    }

    //특정 오브젝트 추가
    public void AddObject(ObjectData data, bool isAdd)
    {
        GameObject go = Instantiate(square, inventory.transform);
        if (isAdd)
        {
            curObjectsList.Add(data);
        }
        var ui = Instantiate(data.ui, go.transform).GetComponent<DragUI>();
        ui.go = data.go;
        ui.playSound = playSound;
        ui.dropSound = dropSound;
    }
    public void IncreaseItemsCount()
    {
        itemsCount++;
        UpdateObjectCount();
        if (itemsCount >= targetItemsCount)
        {
            startButton.SetActive(true);
        }
    }

    public void DecreaseItemsCount()
    {
        itemsCount--;
        UpdateObjectCount();
        if (itemsCount >= targetItemsCount)
        {
            startButton.SetActive(true);
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

            var ui = Instantiate(curObjectsList[i].ui, go.transform).GetComponent<DragUI>();
            ui.playSound = playSound;
            ui.dropSound = dropSound;
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
        if (point < 10)
        {
            return;
        }

        point -= 10;

        PointUpdate();

        float random = UnityEngine.Random.Range(0.0f, 100.0f);
        List<ObjectData> curObj = null;

        Debug.Log(string.Format("random : {0}%", random));
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

        curObjectsList.Add(curObj[index]);
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
        GameEnd?.Invoke();

        //활성화 된 모든 오브젝트 가져오기
        DragObj[] objs = FindObjectsByType<DragObj>(FindObjectsSortMode.None);

        foreach (var obj in objs)
        {
            obj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }

        //모든 오브젝트에 접근해서 하나씩 지우기
        foreach (var obj in objs)
        {
            if (obj.transform.childCount >= 1)
            {
                foreach (Transform child in obj.transform)
                {
                    if (child.gameObject.GetComponent<DragObj>() != null)
                    {
                        float mass = child.gameObject.GetComponent<DragObj>().mass;
                        score += ((int)mass * child.GetComponent<DragObj>().DifficultyLevel * currentRound);
                    }
                }
            }
            //물체가 가진 고유 포인트 합산
            score += ((int)obj.GetComponent<DragObj>().mass * obj.GetComponent<DragObj>().DifficultyLevel * currentRound);
            //오브젝트 삭제
            Vector3 transform = obj.gameObject.transform.position;
            obj.GetComponent<DragObj>().InputDisable();
            Destroy(obj.gameObject);
            playSound.Play(popSound);

            Instantiate(destroyPrefab, transform, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }

        //마지막으로 남은 포인트까지 합산
        score += (point * currentRound);
        if (clear)
            totalScore += score;
        ScoreUpdate();
        yield return new WaitForSeconds(1f);

        if (clear) //다음 라운드 진행
        {
            Debug.Log("클리어");
            score = 0;
            playData.currentRound++;
            totalScore += score;
            playData.totalScore = totalScore;
            transitionManager.StartTransition(true, true, () =>
            {
                SceneManager.LoadScene("WinScene");
            });
        }
        else //게임오버 씬으로 이동 or 종료창
        {
            transitionManager.StartTransition(true, true, () =>
            {
                scoreData.totalScore = totalScore;
                scoreData.round = currentRound;
                SceneManager.LoadScene("GameOverScene");
            });
        }
    }

    //초기화 버튼 이벤트
    public void Restart()
    {
        if (objectGroup.transform.childCount == 0)
        {
            return;
        }

        if (point < 5)
        {
            return;
        }

        itemsCount = 0;
        UpdateObjectCount();
        startButton.SetActive(false);

        point -= 5;
        PointUpdate();

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
