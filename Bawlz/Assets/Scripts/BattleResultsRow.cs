using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BattleResultsRow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI placeText;
    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] Image playerSprite;

    public void SetPlayer(PlayerData player, string newPlaceText = "")
    {
        if (newPlaceText != "")
        {
            placeText.text = newPlaceText;
        }
        playerSprite.sprite = player.playerSprite;
        playerText.text = player.playerName;
        pointsText.text = $"+{GameManager.Instance.GetPointsByPlacement(player.placementsByRound[player.placementsByRound.Count-1])}";
    }

    public void SetPlayerForStandings(PlayerData player, string newPlaceText = "")
    {
        if (newPlaceText != "")
        {
            placeText.text = newPlaceText;
        }
        playerSprite.sprite = player.playerSprite;
        playerText.text = player.playerName;
        pointsText.text = $"{player.winPoints}";
    }
}
