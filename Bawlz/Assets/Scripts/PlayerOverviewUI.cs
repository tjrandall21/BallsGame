using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOverviewUI : MonoBehaviour
{
    [SerializeField] List<Image> playerUpgradeIcons;
    [SerializeField] List<Image> weaponUpgradeIcons;
    [SerializeField] List<Image> playerUpgradeBars;
    [SerializeField] List<Image> weaponUpgradeBars;
    [SerializeField] List<Image> playerUpgradeRankBorders;
    [SerializeField] List<Image> weaponUpgradeRankBorders;
    [SerializeField] Image weaponIcon;

    [SerializeField] Color upgradeBarColor;
    [SerializeField] List<Color> rankColors;

    public void UpdateIcons(int playerIndex)
    {
        PlayerData player = GameManager.Instance.players[playerIndex];
        foreach (Image icon in playerUpgradeIcons)
        {
            icon.color = new Color(0,0,0,0); //make transparent until a sprite is assigned
        }
        foreach (Image icon in weaponUpgradeIcons)
        {
            icon.color = new Color(0,0,0,0); //make transparent until a sprite is assigned
        }
        foreach (Image bar in playerUpgradeBars)
        {
            bar.color = new Color(0,0,0,0);
        }
        foreach (Image bar in weaponUpgradeBars)
        {
            bar.color = new Color(0,0,0,0);
        }
        foreach (Image border in playerUpgradeRankBorders)
        {
            border.color = new Color(0,0,0,0);
        }
        foreach (Image border in weaponUpgradeRankBorders)
        {
            border.color = new Color(0,0,0,0);
        }
        weaponIcon.color = new Color(0,0,0,0);
        for (int i = 0; i < player.upgrades.Count; i++)
        {
            playerUpgradeIcons[i].sprite = player.upgrades[i].shopIcon;
            playerUpgradeIcons[i].color = Color.white;
            playerUpgradeRankBorders[i].color = rankColors[player.upgrades[i].upgradeLevel];
            if (!player.upgrades[i].isUpgradeMaxLevel())
            {              
                playerUpgradeBars[i].color = upgradeBarColor;
                playerUpgradeBars[i].fillAmount = (float)player.upgrades[i].upgradeExp / 
                        GameManager.Instance.GetLevelUpThreshold(player.upgrades[i].upgradeLevel);
            }
        }
        for (int i = 0; i < player.weaponUpgrades.Count; i++)
        {
            weaponUpgradeIcons[i].sprite = player.weaponUpgrades[i].shopIcon;
            weaponUpgradeIcons[i].color = Color.white;
            weaponUpgradeRankBorders[i].color = rankColors[player.weaponUpgrades[i].upgradeLevel];
            if (!player.weaponUpgrades[i].isUpgradeMaxLevel())
            {  
                weaponUpgradeBars[i].color = upgradeBarColor;
                weaponUpgradeBars[i].fillAmount = (float)player.weaponUpgrades[i].upgradeExp / 
                        GameManager.Instance.GetLevelUpThreshold(player.weaponUpgrades[i].upgradeLevel);
            }
        }
        if (player.weaponPrefab != null)
        {
            weaponIcon.sprite = player.weaponPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
            weaponIcon.color = Color.white;
        }
    }
}
