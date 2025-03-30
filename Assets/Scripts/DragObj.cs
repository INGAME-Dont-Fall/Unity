using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;


public class DragObj : MonoBehaviour
{
    public bool isRock;
    public Size size;
    public int index = 0;
    public float mass = 0;
    public int DifficultyLevel = 0;
    public ObjectID[] interactables;
    private PlayerInput inputActions;
    private Rigidbody2D rb2D;

    [Serializable]
    public struct ObjectID
    {
        public Size size;
        public int index;
    }

    public bool isClicked;
    private void Awake()
    {
        isRock = false;
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
        if (isClicked)
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


    private bool IsInteracting(GameObject other)
    {
        if (isRock) return false;
        var otherObj = other.GetComponent<DragObj>();
        if (otherObj == null)
            return false;

        return Array.Exists(interactables, (ObjectID i) => i.size == otherObj.size && (i.index < 0 || i.index == otherObj.index));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsInteracting(other.gameObject))
        {
            var light = GetComponent<Light2D>();
            if (light != null)
            {
                light.enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsInteracting(other.gameObject))
        {
            var light = GetComponent<Light2D>();
            if (light != null)
            {
                light.enabled = false;
            }
        }
    }
}
