using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.InputSystem;

public class Object : MonoBehaviour
{
    public ParticleSystem effect;
    public Rigidbody2D rb2d;

    public bool isDrag = false;

    int score;
    float deadTime;
    GameObject gameObj;
    SpriteRenderer spriteRenderer;
    public GameObject Effect;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        gameObj = GetComponent<GameObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Effect.GetComponent<Animator>().SetTrigger("Bomb");
    }

    void Update()
    {
        if (isDrag)
        {
            Vector2 mousePos2D = Mouse.current.position.ReadValue();

            //��ũ�� ��ǥ�踦 ���� ��ǥ��� ��ȯ
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos2D.x, mousePos2D.y, 0.0f));

            float radius = transform.localScale.x / 2f;
            float leftBorder = -9.0f + radius;
            float rightBorder = 9.0f - radius;

            //������ �����
            if (mousePos.x < leftBorder)
            {
                //x ����
                mousePos.x = leftBorder;
            }
            //���� ���� �����
            else if (mousePos.x > rightBorder)
            {
                mousePos.x = rightBorder;
            }

            mousePos.z = 0f;
            transform.position = Vector3.Lerp(transform.position, mousePos, 0.5f);
        }
    }

    public void Drag()
    {
        isDrag = true;
    }
    public void Drop()
    {
        isDrag = false;
        rb2d.simulated = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Finish")
        {
            deadTime += Time.deltaTime;

            if (deadTime > 0)
            {
                spriteRenderer.color = new Color(0.9f, 0.2f, 0.2f);
            }

            if (deadTime > 2)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}
