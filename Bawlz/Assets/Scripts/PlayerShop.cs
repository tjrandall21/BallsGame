using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShop : MonoBehaviour
{
    [SerializeField] int playerNum = 0;
    // Upgrades lists
    [SerializeField] List<Upgrade> upgrades = new List<Upgrade>();
    [SerializeField] List<WeaponUpgrade> weaponUpgrades = new List<WeaponUpgrade>();
    [SerializeField] List<GameObject> weaponPrefabs = new List<GameObject>();

    // Shop Item Slots
    [SerializeField] List<ShopItem> upgradeItems = new List<ShopItem>();
    [SerializeField] List<ShopItem> weaponUpgradeItems = new List<ShopItem>();
    [SerializeField] List<ShopItem> weaponItems = new List<ShopItem>();


    [SerializeField] Image playerSprite;
    [SerializeField] TextMeshProUGUI coinText;

    private PlayerData player;

    public void SetPlayer(int playerIndex)
    {
        playerNum = playerIndex;
        player = GameManager.Instance.players[playerNum];

        SetupShop();
    }

    void SetupShop()
    {
        //set player sprite
        playerSprite.sprite = GameManager.Instance.players[playerNum].playerSprite;
        UpdateCoinText();

        for (int i = 0; i < upgradeItems.Count; i++)
        {
            if (i < upgrades.Count)
            {
                upgradeItems[i].Init(upgrades[i]);
            }
        }

        for (int i = 0; i < weaponUpgradeItems.Count; i++)
        {
            if (i < weaponUpgrades.Count)
            {
                weaponUpgradeItems[i].Init(weaponUpgrades[i]);
            }
        }
        for (int i = 0; i < weaponItems.Count; i++)
        {
            if (i < weaponPrefabs.Count && weaponPrefabs[i] != null)
            {
                Sprite icon = null;
                var prefab = weaponPrefabs[i];

                // SpriteRenderer 
                var sr = prefab.GetComponentInChildren<SpriteRenderer>();
                if (sr != null)
                {
                    icon = sr.sprite;
                }
                else
                {
                    // UI Image
                    var img = prefab.GetComponentInChildren<Image>();
                    if (img != null)
                    {
                        icon = img.sprite;
                    }
                }

                weaponItems[i].Init(icon, weaponPrefabs[i].name);
            }
        }
    }

    public void BuyUpgrade(int index)
    {
        if (player.coins >= 3)
        {
            if (index < upgrades.Count)
            {
                GameManager.Instance.players[playerNum].AddUpgrade(upgrades[index]);
            }
            player.coins -= 3;
            UpdateCoinText();
        }
        else
        {
            Debug.Log("no money");
        }
    }

    public void BuyWeaponUpgrade(int index)
    {
        if (player.coins >= 3)
        {
            if (index < weaponUpgrades.Count)
            {
                GameManager.Instance.players[playerNum].AddWeaponUpgrade(weaponUpgrades[index]);
            }
            player.coins -= 3;
            UpdateCoinText();
        }
        else
        {
            Debug.Log("no money");
        }
    }
    // not fully working
    public void BuyWeaponPrefab(int index)
    {
        if (player.coins >= 3)
        {
            if (index < weaponPrefabs.Count && weaponPrefabs[index] != null)
            {
                GameManager.Instance.players[playerNum].weaponPrefab = weaponPrefabs[index];
            }
            player.coins -= 3;
            UpdateCoinText();
        }
        else
        {
            Debug.Log("no money");
        }
    }

    void UpdateCoinText()
    {
        coinText.text = $"{player.coins} Coins";
    }
}
