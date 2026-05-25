using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EndBattlePanel : MonoBehaviour
{
    [SerializeField] List<BattleResultsRow> rows = new List<BattleResultsRow>();
    [SerializeField] GameObject goToStandingsButton;
    [SerializeField] GameObject nextRoundButton;
    [SerializeField] TextMeshProUGUI Title;

    void Awake()
    {
        GameManager.Instance.endBattlePanel = this;
        nextRoundButton.SetActive(false);
        foreach (BattleResultsRow row in rows)
        {
            row.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public void SetUpPanel()
    {
        
        int index = GameManager.Instance.roundNumber-1;
        foreach (PlayerData player in GameManager.Instance.players)
        {
            int placeIndex = player.placementsByRound[index]-1;
            rows[placeIndex].gameObject.SetActive(true);
            rows[placeIndex].SetPlayer(player);
        }
    }

    public void SetUpPanelForDraw()
    {
        int index = GameManager.Instance.roundNumber-1;
        bool firstOccupied = false;
        foreach (PlayerData player in GameManager.Instance.players)
        {
            int placeIndex = player.placementsByRound[index]-1;
            if (placeIndex == 0)
            {
                if (firstOccupied)
                {
                    placeIndex++;
                }
                else
                {
                    firstOccupied = true;
                }
            }
            rows[placeIndex].gameObject.SetActive(true);
            if (placeIndex == 1)
            {
                rows[placeIndex].SetPlayer(player,"1st:");
            }
            else
            {
                rows[placeIndex].SetPlayer(player);
            }
        }
    }

    public void SetUpPanelForStandings()
    {
        if (GameManager.Instance.roundNumber == 1)
        {
            EndBattle();
            return;
        }
        goToStandingsButton.SetActive(false);
        nextRoundButton.SetActive(true);
        Title.text = "Standings:";
        List<PlayerData> playersByPoints = new List<PlayerData>();
        for (int i = 0; i < GameManager.Instance.PlayerCount; i++)
        {
            foreach (PlayerData player in GameManager.Instance.players)
            {
                if (!playersByPoints.Contains(player))
                {
                    if (playersByPoints.Count == i)
                    {
                        playersByPoints.Add(player);
                    }
                    else if (player.winPoints > playersByPoints[i].winPoints)
                    {
                        playersByPoints[i] = player;
                    }
                }
            }
            rows[i].SetPlayerForStandings(playersByPoints[i]);
        }
    }

    public void EndBattle()
    {
        GameManager.Instance.EndBattleScene();
    }
}
