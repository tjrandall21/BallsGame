using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BattleResultsRow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI placeText;
    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] Image playerSprite;

    public void SetPlayer(PlayerData player, string newPlaceText = "")
    {
        if (newPlaceText != "")
        {
            placeText.text = newPlaceText;
        }
        playerSprite.sprite = player.playerSprite;
        playerText.text = $"Player {player.playerNum}";
    }
}
