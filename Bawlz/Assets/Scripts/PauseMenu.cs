using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenuUI;
    private bool isPaused = false;

    private void Awake()
    {
        if (pauseMenuUI == null)
        {
            pauseMenuUI = GameObject.Find("PauseMenuUI");
        }
    }

    
   

    public void Start()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.transform.SetAsLastSibling();
            pauseMenuUI.SetActive(true);
        }
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        // When loading the main menu, make sure game is unpaused and timeScale restored.
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("TestMenuScene");
    }

    void Update()
    {
        // uses new Input System, so it checks for the "Pause" action being triggered using the newly made Pause Input action
        if (Keyboard.current.escapeKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("Escape key pressed");
        }
    }
}
