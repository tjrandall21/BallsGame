using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EndBattlePanel : MonoBehaviour
{
    [SerializeField] List<BattleResultsRow> rows = new List<BattleResultsRow>();


    void Awake()
    {
        GameManager.Instance.endBattlePanel = this;
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
            int placeIndex = player.placementsByRound[index]-1; // <- error here
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
                rows[placeIndex].SetPlayer(player,"1st");
            }
            else
            {
                rows[placeIndex].SetPlayer(player);
            }
        }
    }

    public void EndBattle()
    {
        GameManager.Instance.EndBattleScene();
    }
}
