using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup OptionPanel;
    [SerializeField] private CanvasGroup PlayPanel;
    [SerializeField] private CanvasGroup CharacterSelectPanel;
    private bool isFullscreen = true;
    private int playerCount;
    [SerializeField] private List<Button> playerJoinButtons;
    [SerializeField] private List<Button> playerDropoutButtons;
    [SerializeField] PlayerData emptyPlayerData;
    [SerializeField] private Button startGameButton;
    // Character selection variables 
    [SerializeField] private List<Image> playerCharacterSelectPreviews;
    [SerializeField] private List<GameObject> characterSelectGrids; // Character Grid Prefabs to be enabled/disabled based on player count

    void Start()
    {
        playerCount = 0;
        OptionPanel.alpha = 0;
        OptionPanel.interactable = false;
        OptionPanel.blocksRaycasts = false;
        PlayPanel.alpha = 0;
        PlayPanel.interactable = false;
        PlayPanel.blocksRaycasts = false;
        CharacterSelectPanel.alpha = 0;
        CharacterSelectPanel.interactable = false;
        CharacterSelectPanel.blocksRaycasts = false;
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

    public void CharacterSelectShowPanel()
    {
        PlayPanel.alpha = 0;
        PlayPanel.interactable = false;
        PlayPanel.blocksRaycasts = false;
        CharacterSelectPanel.alpha = 1;
        CharacterSelectPanel.interactable = true;
        CharacterSelectPanel.blocksRaycasts = true;
    }

    public void CharacterSelectBack()
    {
        CharacterSelectPanel.alpha = 0;
        CharacterSelectPanel.interactable = false;
        CharacterSelectPanel.blocksRaycasts = false;
        PlayPanel.alpha = 1;
        PlayPanel.interactable = true;
        PlayPanel.blocksRaycasts = true;
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
                        if (playerCount < 2)
                        {
                            startGameButton.interactable = false;
                        }
                    }
                    return;
                }
            }
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
       if(EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            var clickedObject = EventSystem.current.currentSelectedGameObject;
            var clickedButton = clickedObject.GetComponent<Button>();
            Debug.Log("Fullscreen button clicked.");
            if (clickedButton != null)
            {
                isFullscreen = !isFullscreen; // Toggle fullscreen state
                Screen.fullScreen = isFullscreen; // Apply the fullscreen setting
                if(isFullscreen)
                {
                    clickedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Fullscreen: On";
                }
                else
                {
                    clickedButton.GetComponentInChildren<TextMeshProUGUI>().text = "Fullscreen: Off";
                }
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SelectCharacterSprite()
    {
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            var clickedObject = EventSystem.current.currentSelectedGameObject;
            var clickedButton = clickedObject.GetComponent<Button>();
            if (clickedButton != null)
            {
                // Determine which grid the clicked button belongs to so we update only the corresponding player's preview
                int gridIndex = GetCharacterGridIndex(clickedObject.transform);
                var image = clickedButton.GetComponent<Image>();
                if (image != null)
                {
                    SetPlayerCharacterSprite(image, gridIndex);
                    Debug.Log("Character sprite selected: " + image.sprite.name + " for player index: " + gridIndex);
                }
            }

        }
    }

    // Traverses up from the clicked transform to find which character select grid it belongs to.
    private int GetCharacterGridIndex(Transform clickedTransform)
    {
        Transform current = clickedTransform;
        while (current != null)
        {
            if (characterSelectGrids != null)
            {
                int idx = characterSelectGrids.IndexOf(current.gameObject);
                if (idx >= 0)
                {
                    return idx;
                }
            }
            current = current.parent;
        }
        return -1;
    }

    // Updated to accept an optional playerIndex. If playerIndex is -1 or out of range, logs a warning and falls back to player 0.
    public void SetPlayerCharacterSprite(Image CharacterImage, int playerIndex = 0)
    {
        if (playerCharacterSelectPreviews == null || playerCharacterSelectPreviews.Count == 0)
        {
            Debug.LogWarning("SetPlayerCharacterSprite: No playerCharacterSelectPreviews configured.");
            return;
        }

        int indexToUse = playerIndex;
        if (playerIndex < 0 || playerIndex >= playerCharacterSelectPreviews.Count)
        {
            Debug.LogWarning("SetPlayerCharacterSprite: invalid playerIndex " + playerIndex + ". Falling back to player 0.");
            indexToUse = 0;
        }

        playerCharacterSelectPreviews[indexToUse].sprite = CharacterImage.sprite;
        playerCharacterSelectPreviews[indexToUse].SetNativeSize();
        GameManager.Instance.players[indexToUse].playerSprite = CharacterImage.sprite;
        Debug.Log("Set character sprite for player " + (indexToUse + 1) + ": " + CharacterImage.sprite.name);
    }
   
}