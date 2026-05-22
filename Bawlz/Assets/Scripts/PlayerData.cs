using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public int playerNum = 0;
    public GameObject weaponPrefab = null;
    public GameObject ballPrefab = null;
    public List<Upgrade> upgrades = new List<Upgrade>();
    public List<WeaponUpgrade> weaponUpgrades = new List<WeaponUpgrade>();

    public int roundsWon = 0;
    public int coins = 4;

    public List<int> placementsByRound = new List<int>();

    public Sprite playerSprite;


    public bool AddUpgrade(Upgrade newUpgrade)
    {
        for (int i = 0; i < upgrades.Count; i++)
        {
            if (upgrades[i].upgradeFamily == newUpgrade.upgradeFamily)
            {
                if (upgrades[i].isUpgradeMaxLevel())
                {
                    Debug.Log($"Player {playerNum} is trying to buy an upgrade they already have the max level of.");
                    return false;
                }
                upgrades[i].upgradeExp++;
                if (upgrades[i].canLevelUp())
                {
                    upgrades[i] = Instantiate(upgrades[i].nextLevelUpgrade);
                }
                return true;
            }
        } 
        if (upgrades.Count >= 3)
        {
            Debug.Log("Player already has the max number of upgrades");
            return false;
        }
        upgrades.Add(Instantiate(newUpgrade));
        return true;
    }

    public bool AddWeaponUpgrade(WeaponUpgrade newUpgrade)
    {
        for (int i = 0; i < weaponUpgrades.Count; i++)
        {
            if (weaponUpgrades[i].upgradeFamily == newUpgrade.upgradeFamily)
            {
                if (weaponUpgrades[i].isUpgradeMaxLevel())
                {
                    Debug.Log($"Player {playerNum} is trying to buy a weapon upgrade they already have the max level of.");
                    return false;
                }
                weaponUpgrades[i].upgradeExp++;

                if (weaponUpgrades[i].canLevelUp())
                {
                    weaponUpgrades[i] = Instantiate(weaponUpgrades[i].nextLevelUpgrade);
                }
                return true;
            }
        }
        if (weaponUpgrades.Count >= 3)
        {
            Debug.Log("Player already has the max number of weapon upgrades");
            return false;
        }
        weaponUpgrades.Add(Instantiate(newUpgrade));
        return true;
    }



}
