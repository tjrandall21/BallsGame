using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShop : MonoBehaviour
{
    [SerializeField] int playerNum = 0;
    [SerializeField] List<Upgrade> upgrades = new List<Upgrade>();
    [SerializeField] List<WeaponUpgrade> weaponUpgrades = new List<WeaponUpgrade>();

    [SerializeField] List<Image> upgradeIcons = new List<Image>();
    [SerializeField] List<Image> weaponUpgradeIcons = new List<Image>();

    private PlayerData player;
    private List<Upgrade> currentShopUpgrades = new List<Upgrade>();
    private List<WeaponUpgrade> currentShopWeaponUpgrades = new List<WeaponUpgrade>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.Instance.players[playerNum];
        for (int i = 0; i < upgradeIcons.Count; i++)
        {
            if (i < upgrades.Count)
            {
                upgradeIcons[i].sprite = upgrades[i].shopIcon;
            }
            else
            {
                upgradeIcons[i].sprite = null;
            }
        }
        for (int i = 0; i < weaponUpgradeIcons.Count; i++)
        {
            if (i < weaponUpgrades.Count)
            {
                weaponUpgradeIcons[i].sprite = weaponUpgrades[i].shopIcon;
            }
            else
            {
                weaponUpgradeIcons[i].sprite = null;
            }
        }
    }

    public void BuyUpgrade(int index)
    {
        if (player.coins > 0)
        {
            if (index < upgrades.Count)
            {
                GameManager.Instance.players[playerNum].upgrades.Add(upgrades[index]);
            }
            player.coins -= 1;
        }
        else
        {
            Debug.Log("no money");
        }
    }

    public void BuyWeaponUpgrade(int index)
    {
        if (player.coins > 0)
        {
            if (index < weaponUpgrades.Count)
            {
                GameManager.Instance.players[playerNum].weaponUpgrades.Add(weaponUpgrades[index]);
            }
            player.coins -= 1;
        }
        else
        {
            Debug.Log("no money");
        }
    }

}
