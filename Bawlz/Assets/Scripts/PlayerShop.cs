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
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        if (index < upgrades.Count)
        {
            GameManager.Instance.players[playerNum].upgrades.Add(upgrades[index]);
        }
    }

    public void BuyWeaponUpgrade(int index)
    {
        if (index < weaponUpgrades.Count)
        {
            GameManager.Instance.players[playerNum].weaponUpgrades.Add(weaponUpgrades[index]);
        }
    }

}
