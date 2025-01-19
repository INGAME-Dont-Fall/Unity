using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class DragObj : MonoBehaviour
{
    public Size size;
    public int index = 0;
    public int DifficultyLevel = 0;
    private PlayerInput inputActions;
    private Rigidbody2D rb2D;

    public bool isClicked;
    private void Awake()
    {
        inputActions = new PlayerInput();
        inputActions.PlayerActions.Enable();
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void InputDisable()
    {
        inputActions.PlayerActions.Disable();
    }

    private void Update()
    {
        if(isClicked)
        {
            float rotateDirection = -inputActions.PlayerActions.Rotate.ReadValue<float>();
            gameObject.transform.Rotate(Vector3.forward * rotateDirection * Time.deltaTime * 100.0f);
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
    }
}
