using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int playerNum = 0;
    public GameObject weaponPrefab = null;
    public GameObject ballPrefab = null;
    public List<Upgrade> upgrades = new List<Upgrade>();
    public List<WeaponUpgrade> weaponUpgrades = new List<WeaponUpgrade>();

    public int lives = 0;
    public int roundsWon = 0;
    public int coins = 4;

    public Sprite playerSprite;

}
