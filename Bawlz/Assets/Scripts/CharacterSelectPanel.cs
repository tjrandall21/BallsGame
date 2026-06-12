using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPanel : MonoBehaviour
{
    [SerializeField] List<Button> cells;
    [SerializeField] List<SelectedCharacter> selectedCharacters;
    public int playerIndex = 0;

    void Awake()
    {
        GetComponent<Image>().color = GameManager.Instance.playerUIColors[playerIndex];
    }

    public void UpdateCells()
    {
        foreach (Button cell in cells)
        {
            cell.interactable = true;
            foreach (SelectedCharacter character in selectedCharacters)
            {
                if (character.characterName == cell.name && character.isActiveAndEnabled)
                {
                    cell.interactable = false;
                }
            }
        }
    }
}
