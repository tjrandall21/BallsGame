using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
// To use this you must have a canvas in the scene an event system with the InputSystemUIInputModule component
// that is set to use my custom made Input Actions asset with the move and click actions or IT WILL NOT WORK
// it should automatically find the canvas and event system in the scene
// The prefab will be made for each player when they hit A or X or whatever
// Make sure the playerInputManager has a max of 4
// Make sure to use the one named Player Controls Input Actions in the player input manager and not the default
public class MultiplayerCursor : MonoBehaviour
{
    public RectTransform cursor;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    public Image cursorImage;

    public float speed = 1000f;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction clickAction;

    private Vector2 cursorPos;
    
    void Awake()
    {
        transform.SetParent(FindAnyObjectByType<Canvas>().transform, false);

        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        clickAction = playerInput.actions["Click"];

        eventSystem = FindAnyObjectByType<EventSystem>();
        raycaster = FindAnyObjectByType<GraphicRaycaster>();

        if (cursor == null)
            cursor = GetComponentInChildren<Image>().rectTransform;
        cursorImage = cursor.GetComponent<Image>();

        switch (playerInput.playerIndex)
        {
            case 0:
                cursorImage.color = Color.blue;
                break;

            case 1:
                cursorImage.color = Color.red;
                break;

            case 2:
                cursorImage.color = Color.green;
                break;

            case 3:
                cursorImage.color = Color.yellow;
                break;
        }

    }

    void OnEnable()
    {
        if (moveAction != null)
            moveAction.Enable();

        if (clickAction != null)
            clickAction.Enable();
    }

    void Start()
    {
        cursorPos = cursor.position;
    }

    void Update()
    {
        MoveCursor();
        HandleUI();
    }

    void MoveCursor()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();

        cursorPos += move * speed * Time.deltaTime;

        cursorPos.x = Mathf.Clamp(cursorPos.x, 0, Screen.width);
        cursorPos.y = Mathf.Clamp(cursorPos.y, 0, Screen.height);

        cursor.position = cursorPos;
    }

    void HandleUI()
    {
        PointerEventData data = new PointerEventData(eventSystem);
        data.position = cursorPos;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(data, results);
        if (results.Count > 0)
        {
            Debug.Log("Hovering over: " + results[0].gameObject.name);
        }

        if (results.Count == 0)
            return;

        GameObject target = results[0].gameObject;

        Button button = target.GetComponentInParent<Button>();

        if (button == null)
            return;

        target = button.gameObject;

        if (clickAction.WasPressedThisFrame())
        {
            ExecuteEvents.Execute(target, data, ExecuteEvents.pointerDownHandler);
        }

        if (clickAction.WasReleasedThisFrame())
        {
            ExecuteEvents.Execute(target, data, ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(target, data, ExecuteEvents.pointerClickHandler);
        }
    }
}