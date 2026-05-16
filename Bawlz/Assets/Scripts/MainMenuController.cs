using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] private Button startGameButton;

    void Start()
    {
        playerCount = 0;
        OptionPanel.alpha = 0;
        OptionPanel.interactable = false;
        OptionPanel.blocksRaycasts = false;
        PlayPanel.alpha = 0;
        PlayPanel.interactable = false;
        PlayPanel.blocksRaycasts = false;
        playerDropoutButtons.ForEach(button => button.interactable = false); // Disable all dropout buttons at the start
        playerJoinButtons[0].interactable = true; // Enable the first join button at the start
        for(int i = 1; i < playerJoinButtons.Count; i++)
        {
            playerJoinButtons[i].interactable = false; // Disable all join buttons except the first one at the start
        }
        startGameButton.interactable = false; // Disable the start game button at the start
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
        playerCount = 0;
        playerDropoutButtons.ForEach(button => button.interactable = false); // Disable all dropout buttons
        playerJoinButtons[0].interactable = true; // Enable the first join button
        for (int i = 1; i < playerJoinButtons.Count; i++)
        {
            playerJoinButtons[i].interactable = false; // Disable all join buttons except the first one
        }
    }
    public void Back() //Back button for the options panel
    {
        OptionPanel.alpha = 0;
        OptionPanel.interactable = false;
        OptionPanel.blocksRaycasts = false;
    }

    public void Join()
    {
        if(EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            var clickedObject = EventSystem.current.currentSelectedGameObject;
            var clickedButton = clickedObject.GetComponent<Button>();
            if(clickedButton != null)
            {
                int index = playerJoinButtons.IndexOf(clickedButton);
                if(index >= 0)
                {
                    Debug.Log("Player " + (index + 1) + " joined the game.");
                    playerJoinButtons[index].interactable = false; // Disable the join button for that player
                    if(index < playerDropoutButtons.Count)
                    {
                        playerDropoutButtons[index].interactable = true; // Enable the dropout button for that player
                    }
                    playerCount++; // Increment player count
                    if(playerCount >= 2)
                    {
                        startGameButton.interactable = true;
                    }
                    if(index + 1 < playerJoinButtons.Count)
                    {
                        playerJoinButtons[index + 1].interactable = true; // Enable the next join button if it exists
                    }
                    return;
                }
            }
        }
      
    }

    public void Dropout()
    {
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            var clickedObject = EventSystem.current.currentSelectedGameObject;
            var clickedButton = clickedObject.GetComponent<Button>();
            if (clickedButton != null)
            {
                int index = playerDropoutButtons.IndexOf(clickedButton);
                if (index >= 0)
                {
                    // Only proceed if that dropout button was currently interactable
                    if (playerDropoutButtons[index].interactable)
                    {
                        Debug.Log("Player " + (index + 1) + " left the game.");
                        playerDropoutButtons[index].interactable = false; 
                        if (index < playerJoinButtons.Count)
                        {
                            playerJoinButtons[index].interactable = true; 
                        }

                        // Decrement playerCount but ensure it doesn't go below zero.
                        playerCount = Mathf.Max(0, playerCount - 1);
                    }
                    return;
                }
            }
        }
        if(playerCount < 2)
        {
            startGameButton.interactable = false;
        }
   
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
