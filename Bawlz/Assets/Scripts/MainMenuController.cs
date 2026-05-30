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

    [SerializeField] private GameObject MainPanel;
    [SerializeField] private CanvasGroup OptionPanel;
    [SerializeField] private CanvasGroup PlayPanel;
    [SerializeField] private CanvasGroup CharacterSelectPanel;
    [SerializeField] private CanvasGroup WeaponSelectPanel;
    private bool isFullscreen = true;
    private int playerCount;
    [SerializeField] private List<Button> playerJoinButtons;
    [SerializeField] private List<Button> playerDropoutButtons;
    [SerializeField] PlayerData emptyPlayerData;
    [SerializeField] private Button startGameButton;
    // Character selection variables 
    [SerializeField] private List<Image> playerCharacterSelectPreviews;
    [SerializeField] private List<GameObject> characterSelectGrids; // Character Grid Prefabs to be enabled/disabled based on player count
    [SerializeField] private List<GameObject> weaponSelectGrids; // Weapon Grid Prefabs to be enabled/disabled based on player count
    [SerializeField] List<Image> weaponSelectPreviews;

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
        WeaponSelectPanel.alpha = 0;
        WeaponSelectPanel.interactable = false;
        WeaponSelectPanel.blocksRaycasts = false;
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
        MainPanel.SetActive(false);
    }

    public void StartGame() // Shows the play panel where the player can choose how many players will be playing
    {
        PlayPanel.alpha = 1; // shows the play panel putting it on top of everything else
        PlayPanel.interactable = true;
        PlayPanel.blocksRaycasts = true;
        MainPanel.SetActive(false);
    }

    public void PlayBack() //Back button for the play panel
    {
        PlayPanel.alpha = 0;
        PlayPanel.interactable = false;
        PlayPanel.blocksRaycasts = false;
        MainPanel.SetActive(true);
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
        GameManager.Instance.ResetPlayers(playerCount);

        foreach (Image image in playerCharacterSelectPreviews)
        {
            image.gameObject.SetActive(false);
        }
        foreach (GameObject grid in characterSelectGrids)
        {
            grid.gameObject.SetActive(false);
        }
        for (int i = 0; i < playerCount; i++)
        {
            playerCharacterSelectPreviews[i].gameObject.SetActive(true);
            characterSelectGrids[i].gameObject.SetActive(true);
        }
        

        for (int i = 0; i < playerCount; i++)
        { //set the sprite for each player to whatever is initially selected
            GameManager.Instance.players[i].playerSprite = playerCharacterSelectPreviews[i].sprite;
            GameManager.Instance.players[i].playerName = playerCharacterSelectPreviews[i].GetComponent<SelectedCharacter>().characterName;
        }
        UpdateButtons();
    }

    public void CharacterSelectBack()
    {
        CharacterSelectPanel.alpha = 0;
        CharacterSelectPanel.interactable = false;
        CharacterSelectPanel.blocksRaycasts = false;
        PlayPanel.alpha = 1;
        PlayPanel.interactable = true;
        PlayPanel.blocksRaycasts = true;
        GameManager.Instance.players.Clear();
    }

    public void WeaponSelectShowPanel()
    {
        CharacterSelectPanel.alpha = 0;
        CharacterSelectPanel.interactable = false;
        CharacterSelectPanel.blocksRaycasts = false;
        WeaponSelectPanel.alpha = 1;
        WeaponSelectPanel.interactable = true;
        WeaponSelectPanel.blocksRaycasts = true;
        foreach (Image image in weaponSelectPreviews)
        {
            image.gameObject.SetActive(false);
        }
        foreach (GameObject grid in weaponSelectGrids)
        {
            grid.gameObject.SetActive(false);
        }
        for (int i = 0; i < playerCount; i++)
        {
            weaponSelectPreviews[i].gameObject.SetActive(true);
            weaponSelectGrids[i].gameObject.SetActive(true);
        }
    }

    public void WeaponSelectBack()
    {
        WeaponSelectPanel.alpha = 0;
        WeaponSelectPanel.interactable = false;
        WeaponSelectPanel.blocksRaycasts = false;
        CharacterSelectPanel.alpha = 1;
        CharacterSelectPanel.interactable = true;
        CharacterSelectPanel.blocksRaycasts = true;
    }

    public void Back() //Back button for the options panel
    {
        OptionPanel.alpha = 0;
        OptionPanel.interactable = false;
        OptionPanel.blocksRaycasts = false;
        MainPanel.SetActive(true);
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

    public void SelectWeapon(Button button)
    {
        WeaponSelectButton buttonData = button.GetComponent<WeaponSelectButton>();
        int playerIndex = buttonData.playerNum-1;
        GameManager.Instance.players[playerIndex].weaponPrefab = buttonData.weaponPrefab;
        weaponSelectPreviews[playerIndex].sprite = button.GetComponent<Image>().sprite;
    }

    public void UpdateButtons()
    {
        foreach (GameObject grid in characterSelectGrids)
        {
            grid.GetComponent<CharacterSelectPanel>().UpdateCells();
        }
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
                var image = clickedButton.GetComponentInChildren<Image>();
                if (image != null)
                {
                    SetPlayerCharacterSprite(image, clickedButton.name, gridIndex);
                    Debug.Log("Character sprite selected: " + clickedButton.name + " for player index: " + gridIndex);
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
    public void SetPlayerCharacterSprite(Image CharacterImage, string characterName, int playerIndex = 0)
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
        playerCharacterSelectPreviews[indexToUse].GetComponent<SelectedCharacter>().characterName = characterName;
        playerCharacterSelectPreviews[indexToUse].SetNativeSize();
        
        GameManager.Instance.players[indexToUse].playerSprite = CharacterImage.sprite;
        GameManager.Instance.players[indexToUse].playerName = characterName;
        Debug.Log("Set character sprite for player " + (indexToUse + 1) + ": " + characterName);
        UpdateButtons();
    }
   
}