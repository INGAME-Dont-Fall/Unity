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
        Application.targetFrameRate = 60; //프레임 60으로 고정

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
        //새로운 동글을 동글 그룹에 상속하여 생성
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

        //다 활성화 되어있어서 넘겨줄 것이 없으면?
        return MakeObject();
    }
    void NextObject()
    {
        if (IsOver)
        {
            return;
        }

        lastObject = GetObject();
        //여기서 프리팹 변경해줘야함

        //오브젝트 활성화
        lastObject.gameObject.SetActive(true);

        StartCoroutine("WaitNext");
    }

    //코루틴 생성
    IEnumerator WaitNext()
    {
        while (lastObject != null)
        {
            yield return null;
        }
        //2.5초 휴식
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
        //활성화 된 모든 동글 가져오기
        Object[] Obj = FindObjectsOfType<Object>();

        //모든 동글이의 물리효과 제거
        for (int index = 0; index < Obj.Length; ++index)
        {
            Obj[index].rb2d.simulated = false;
        }

        //모든 동글에 접근해서 하나씩 지우기
        for (int index = 0; index < Obj.Length; ++index)
        {
            //절대 나올 수 없는 값을 넘긴다.
            Obj[index].Hide(Vector3.up * 100);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        // 최고 점수 갱신
        int maxScore = Mathf.Max(Score, PlayerPrefs.GetInt("MaxScore"));
        PlayerPrefs.SetInt("MaxScore", maxScore);

        //게임오버 씬으로 이동
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
