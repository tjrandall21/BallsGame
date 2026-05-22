using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOverviewUI : MonoBehaviour
{
    [SerializeField] List<Image> playerUpgradeIcons;
    [SerializeField] List<Image> weaponUpgradeIcons;
    [SerializeField] List<Image> playerUpgradeBars;
    [SerializeField] List<Image> weaponUpgradeBars;
    [SerializeField] Image weaponIcon;

    [SerializeField] Color upgradeBarColor;

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
        for (int i = 0; i < player.upgrades.Count; i++)
        {
            playerUpgradeIcons[i].sprite = player.upgrades[i].shopIcon;
            playerUpgradeIcons[i].color = Color.white;
            if (!player.weaponUpgrades[i].isUpgradeMaxLevel())
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
            if (!player.weaponUpgrades[i].isUpgradeMaxLevel())
            {  
                weaponUpgradeBars[i].color = upgradeBarColor;
                weaponUpgradeBars[i].fillAmount = (float)player.weaponUpgrades[i].upgradeExp / 
                        GameManager.Instance.GetLevelUpThreshold(player.weaponUpgrades[i].upgradeLevel);
            }
        }
        weaponIcon.sprite = player.weaponPrefab.GetComponentInChildren<SpriteRenderer>().sprite;
    }
}
