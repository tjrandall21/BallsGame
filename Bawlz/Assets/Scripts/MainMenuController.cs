using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup OptionPanel;
    [SerializeField] private CanvasGroup PlayPanel;
    private bool isFullscreen = true;
    private int playerCount;
    [SerializeField] private List<Button> playerJoinButtons;
    [SerializeField] private List<Button> playerDropoutButtons;
    [SerializeField] PlayerData emptyPlayerData;

    void Start()
    {
        playerCount = 0;
        OptionPanel.alpha = 0;
        OptionPanel.interactable = false;
        OptionPanel.blocksRaycasts = false;
        PlayPanel.alpha = 0;
        PlayPanel.interactable = false;
        PlayPanel.blocksRaycasts = false;
    }

    public void OpenOptions()
    {
        OptionPanel.alpha = 1;
        OptionPanel.interactable = true;
        OptionPanel.blocksRaycasts = true;
    }

    public void StartGame() // Shows the play panel where the player can choose how many players will be playing
    {
        PlayPanel.alpha = 1; // shows the play panel putting it on top of everything else
        PlayPanel.interactable = true;
        PlayPanel.blocksRaycasts = true;
    }

    public void PlayBack() //Back button for the play panel
    {
        PlayPanel.alpha = 0;
        PlayPanel.interactable = false;
        PlayPanel.blocksRaycasts = false;
    }
    public void Back() //Back button for the options panel
    {
        OptionPanel.alpha = 0;
        OptionPanel.interactable = false;
        OptionPanel.blocksRaycasts = false;
    }

    public void Join()
    {
        playerCount++;
        Debug.Log("Player " + playerCount + " joined the game.");
        playerJoinButtons[playerCount - 1].interactable = false; // Disable the join button for the current player
        playerDropoutButtons[playerCount - 1].interactable = true; // Enable the dropout button for the current player
    }

    public void Dropout()
    {
        Debug.Log("Player " + playerCount + " left the game.");
        playerCount--;
        playerJoinButtons[playerCount].interactable = true; // Enable the join button for the current player
        playerDropoutButtons[playerCount].interactable = false; // Disable the dropout button for the current player

    }

    public void StartGameplayWithPlayers()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.players.Clear(); //reset playerdata
            for (int i = 0; i < playerCount; i++)
            { //generate empty playerdata
                PlayerData player = Instantiate(emptyPlayerData);
                player.playerNum = i+1;
                GameManager.Instance.players.Add(player);
                
            }
            GameManager.Instance.StartShopWithPlayerCount(playerCount);
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    public void FullScreen()
    {
        isFullscreen = Screen.fullScreen;

        Screen.fullScreen = !isFullscreen;

        Debug.Log(isFullscreen ? "Switched to Windowed" : "Switched to Fullscreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
