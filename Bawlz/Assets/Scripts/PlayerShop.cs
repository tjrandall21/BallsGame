using System.Collections.Generic;
using System.Linq;
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

    // Per-item spawn weights 
    [SerializeField] List<int> upgradeWeights = new List<int>();
    [SerializeField] List<int> weaponUpgradeWeights = new List<int>();
    [SerializeField] List<int> weaponPrefabWeights = new List<int>();

    // Shop Item Slots
    [SerializeField] List<ShopItem> upgradeItems = new List<ShopItem>();
    [SerializeField] List<ShopItem> weaponUpgradeItems = new List<ShopItem>();
    [SerializeField] List<ShopItem> weaponItems = new List<ShopItem>();


    [SerializeField] Image playerSprite;
    [SerializeField] TextMeshProUGUI coinText;

    [SerializeField] PlayerOverviewUI playerOverviewUI;

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

        // Character Upgrades
        var upgradeSelection = PickWeightedIndices(upgradeWeights, upgrades.Count, upgradeItems.Count);
        for (int i = 0; i < upgradeItems.Count; i++)
        {
            if (i < upgradeSelection.Count)
            {
                int chosenIndex = upgradeSelection[i];
                if (chosenIndex >= 0 && chosenIndex < upgrades.Count)
                {
                    upgradeItems[i].Init(upgrades[chosenIndex], chosenIndex);
                }
            }
        }

        weaponUpgrades = GameManager.Instance.players[playerNum].weaponPrefab.GetComponent<Weapon>().possibleWeaponUpgradesInShop;
        // Weapon Upgrades
        var weaponUpgradeSelection = PickWeightedIndices(weaponUpgradeWeights, weaponUpgrades.Count, weaponUpgradeItems.Count);
        for (int i = 0; i < weaponUpgradeItems.Count; i++)
        {
            if (i < weaponUpgradeSelection.Count)
            {
                int chosenIndex = weaponUpgradeSelection[i];
                if (chosenIndex >= 0 && chosenIndex < weaponUpgrades.Count)
                {
                    weaponUpgradeItems[i].Init(weaponUpgrades[chosenIndex], chosenIndex);
                }
            }
        }

        // Weapon Prefabs
        var weaponPrefabSelection = PickWeightedIndices(weaponPrefabWeights, weaponPrefabs.Count, weaponItems.Count);
        for (int i = 0; i < weaponItems.Count; i++)
        {
            if (i < weaponPrefabSelection.Count)
            {
                int chosenIndex = weaponPrefabSelection[i];
                if (chosenIndex >= 0 && chosenIndex < weaponPrefabs.Count && weaponPrefabs[chosenIndex] != null)
                {
                    Sprite icon = null;
                    var prefab = weaponPrefabs[chosenIndex];

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

                    weaponItems[i].Init(icon, weaponPrefabs[chosenIndex].name, chosenIndex);
                }
            }
        }

        playerOverviewUI.UpdateIcons(playerNum);
    }

    List<int> PickWeightedIndices(List<int> weightList, int availableCount, int pickCount)
    {
        List<int> selected = new List<int>();

        // stop if numbers are bad
        if (availableCount <= 0 || pickCount <= 0)
        {
            return selected;
        }

        // create weights array
        float[] weights = new float[availableCount];

        for (int i = 0; i < availableCount; i++)
        {
            if (weightList != null && i < weightList.Count)
            {
                weights[i] = Mathf.Max(0, weightList[i]);
            }
            else
            {
                weights[i] = 1;
            }
        }

        // if all weights are 0, make everything equal chance
        float totalCheck = 0;

        for (int i = 0; i < availableCount; i++)
        {
            totalCheck += weights[i];
        }

        if (totalCheck <= 0)
        {
            for (int i = 0; i < availableCount; i++)
            {
                weights[i] = 1;
            }
        }

        int maxPicks = Mathf.Min(pickCount, availableCount);

        for (int pick = 0; pick < maxPicks; pick++)
        {
            // calculate total weight
            float totalWeight = 0;

            for (int i = 0; i < availableCount; i++)
            {
                totalWeight += weights[i];
            }

            if (totalWeight <= 0)
            {
                break;
            }

            // pick random number
            float randomNum = Random.Range(0f, totalWeight);

            float current = 0;

            for (int i = 0; i < availableCount; i++)
            {
                current += weights[i];

                if (randomNum <= current)
                {
                    selected.Add(i);

                    // remove from future picks
                    weights[i] = 0;

                    break;
                }
            }
        }

        return selected;
    }

    public void BuyUpgrade(int index)
    {
        index = upgradeItems[index].index;
        if (player.coins >= 3)
        {
            if (index < upgrades.Count)
            {
                if (GameManager.Instance.players[playerNum].AddUpgrade(upgrades[index]))
                {
                    player.coins -= 3;
                    SetupShop();
                }
            }
            UpdateCoinText();
            playerOverviewUI.UpdateIcons(playerNum);
        }
        else
        {
            Debug.Log("no money");
        }
        
    }

    public void BuyWeaponUpgrade(int index)
    {
        index = weaponUpgradeItems[index].index;
        if (player.coins >= 3)
        {
            if (index < weaponUpgrades.Count)
            {
                if (GameManager.Instance.players[playerNum].AddWeaponUpgrade(weaponUpgrades[index]))
                {   
                    player.coins -= 3;
                    SetupShop();
                }
            }
            UpdateCoinText();
            playerOverviewUI.UpdateIcons(playerNum);
        }
        else
        {
            Debug.Log("no money");
        }
        
    }

    void SellAllWeaponUpgrades()
    {
        int upgradeAmount = GameManager.Instance.players[playerNum].weaponUpgrades.Count;
        GameManager.Instance.players[playerNum].weaponUpgrades.Clear();
        GameManager.Instance.players[playerNum].coins += upgradeAmount;
    }

    public void ReRoll()
    {
        if (GameManager.Instance.players[playerNum].coins > 0)
        {
            GameManager.Instance.players[playerNum].coins -= 1;
            SetupShop();
        }
    }

    public void BuyWeaponPrefab(int index)
    {
        index = weaponItems[index].index;
        if (player.coins >= 3)
        {
            if (index < weaponPrefabs.Count && weaponPrefabs[index] != null)
            {
                GameManager.Instance.players[playerNum].weaponPrefab = weaponPrefabs[index];
            }
            player.coins -= 3;
            UpdateCoinText();
            playerOverviewUI.UpdateIcons(playerNum);
            SellAllWeaponUpgrades();
            SetupShop();
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
