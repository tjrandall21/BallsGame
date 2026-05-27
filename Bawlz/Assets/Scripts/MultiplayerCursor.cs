using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    private Transform playerRoot;

    void Awake()
    {
        playerRoot = transform.root;

        DontDestroyOnLoad(playerRoot.gameObject);

        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        clickAction = playerInput.actions["Click"];

        SceneManager.sceneLoaded += OnSceneLoaded;

        MoveToPersistentCursorCanvas();
        ReconnectToSceneUI();
        SetCursorColor();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnEnable()
    {
        if (moveAction != null)
            moveAction.Enable();

        if (clickAction != null)
            clickAction.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null)
            moveAction.Disable();

        if (clickAction != null)
            clickAction.Disable();
    }

    void Start()
    {
        if (cursor != null)
            cursorPos = cursor.position;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Cursor carried into scene: " + scene.name);

        MoveToPersistentCursorCanvas();
        ReconnectToSceneUI();

        if (cursor != null)
        {
            cursorPos.x = Mathf.Clamp(cursorPos.x, 0, Screen.width);
            cursorPos.y = Mathf.Clamp(cursorPos.y, 0, Screen.height);
            cursor.position = cursorPos;
        }
    }

    void MoveToPersistentCursorCanvas()
    {
        GameObject cursorCanvasObject = GameObject.Find("Persistent Cursor Canvas");

        if (cursorCanvasObject == null)
        {
            cursorCanvasObject = new GameObject("Persistent Cursor Canvas");

            Canvas canvas = cursorCanvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999;

            cursorCanvasObject.AddComponent<CanvasScaler>();

            DontDestroyOnLoad(cursorCanvasObject);
        }

        playerRoot.SetParent(cursorCanvasObject.transform, false);
    }

    void ReconnectToSceneUI()
    {
        eventSystem = FindAnyObjectByType<EventSystem>();

        GraphicRaycaster[] raycasters = FindObjectsByType<GraphicRaycaster>(FindObjectsInactive.Exclude);
        raycaster = null;

        foreach (GraphicRaycaster currentRaycaster in raycasters)
        {
            if (currentRaycaster.gameObject.name != "Persistent Cursor Canvas")
            {
                raycaster = currentRaycaster;
                break;
            }
        }

        if (cursor == null)
        {
            Image image = GetComponentInChildren<Image>();

            if (image != null)
                cursor = image.rectTransform;
        }

        if (cursor != null)
        {
            cursorImage = cursor.GetComponent<Image>();
        }

        if (cursorImage != null)
        {
            cursorImage.raycastTarget = false;
        }
    }

    void SetCursorColor()
    {
        if (cursorImage == null || playerInput == null)
            return;

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

    void Update()
    {
        if (cursor == null || moveAction == null || clickAction == null)
            return;

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
        if (eventSystem == null || raycaster == null)
            return;

        PointerEventData data = new PointerEventData(eventSystem);
        data.position = cursorPos;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(data, results);

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