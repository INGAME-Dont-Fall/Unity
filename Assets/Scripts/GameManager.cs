using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private int Count;
    private int Score;
    private bool IsOver;

    private Object lastObject;
    [SerializeField] private GameObject[] ObjectPrefab;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private Transform ObjectGroup;

    [Range(1, 30)]
    [SerializeField] private int PoolSize;
    [SerializeField] private int PoolCursor;

    private GameObject[] HiddenObj;

    //싱글톤
    private static GameManager instance;
    public static GameManager Instance => instance;

    private void Awake()
    {
        Application.targetFrameRate = 60; //프레임 60으로 고정
    }

    private void Start()
    {
        GameStart();
    }

    public void GameStart()
    {
        Invoke(nameof(NextObject), 1.5f);
    }

    Object MakeObject()
    {
        int index = Random.Range(0, ObjectPrefab.Length);

        //새로운 오브젝트를 오브젝트 그룹에 상속하여 생성
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

        StartCoroutine(WaitNext());
    }

    //코루틴 생성
    IEnumerator WaitNext()
    {
        yield return new WaitWhile(() => lastObject != null);
        //2.5초 휴식
        yield return new WaitForSeconds(2.5f);

        NextObject();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData data)
    {
        if (lastObject == null)
        {
            return;
        }

        lastObject.Drag();
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData data)
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

        StartCoroutine(GameOverRoutine());
    }
    IEnumerator GameOverRoutine()
    {
        //활성화 된 모든 오브젝트 가져오기
        Object[] objs = FindObjectsOfType<Object>();

        //모든 오브젝트의 물리효과 제거
        foreach(var obj in objs)
        {
            obj.rb2d.simulated = false;
        }

        //모든 오브젝트에 접근해서 하나씩 지우기
        foreach(var obj in objs)
        {
            obj.GetComponent<Animator>().SetTrigger("Destroy");
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        //게임오버 씬으로 이동

    }

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
