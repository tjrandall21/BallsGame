using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShop : MonoBehaviour
{
   [SerializeField] public int playerNum = 0;

    [SerializeField] Color freezeColor;
    [SerializeField] Color unfrozenColor;

    [SerializeField] Button sellButton;

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

    [SerializeField] GameObject buyPanel;
    [SerializeField] GameObject sellPanel;

    [SerializeField] List<ShopItem> sellUpgradeItems = new List<ShopItem>();
    [SerializeField] List<ShopItem> sellWeaponUpgradeItems = new List<ShopItem>();
    [SerializeField] ShopItem sellWeaponItem;

    [SerializeField] Image playerSprite;
    [SerializeField] TextMeshProUGUI coinText;

    [SerializeField] TextMeshProUGUI sellButtonText;
    [SerializeField] TextMeshProUGUI freezeButtonText;

    [SerializeField] PlayerOverviewUI playerOverviewUI;

    private PlayerData player;

    bool sellmode = false;
    bool detailsMode = false;
    bool freezeMode = false;



    public void SetPlayer(int playerIndex)
    {
        playerNum = playerIndex;
        player = GameManager.Instance.players[playerNum];

        SetupShop();
    }

    void SetupShop()
    {
        sellPanel.SetActive(false);
        //set player sprite
        playerSprite.sprite = player.playerSprite;
        UpdateCoinText();

        // Character Upgrades
        var upgradeSelection = PickWeightedIndices(upgradeWeights, upgrades.Count, upgradeItems.Count);
        upgradeSelection = MergeFrozenIntoSelection( upgradeSelection, player.frozenUpgradeIndices, upgradeItems.Count, upgrades.Count);
        for (int i = 0; i < upgradeItems.Count; i++)
        {
            if (i < upgradeSelection.Count)
            {
                int chosenIndex = upgradeSelection[i];
                if (chosenIndex >= 0 && chosenIndex < upgrades.Count)
                {
                    upgradeItems[i].Init(upgrades[chosenIndex], chosenIndex);
                    if (i < player.frozenUpgradeIndices.Count)
                    {
                        upgradeItems[i].makeFrozen(true,freezeColor);
                    }
                    else
                    {
                        upgradeItems[i].makeFrozen(false,unfrozenColor);
                    }
                }
            }
        }

        if (player.weaponPrefab != null)
        {
            weaponUpgrades = player.weaponPrefab.GetComponent<Weapon>().possibleWeaponUpgradesInShop;
            // Weapon Upgrades
            var weaponUpgradeSelection = PickWeightedIndices(weaponUpgradeWeights, weaponUpgrades.Count, weaponUpgradeItems.Count);
            weaponUpgradeSelection = MergeFrozenIntoSelection( weaponUpgradeSelection, player.frozenWeaponUpgradeIndices, weaponUpgradeItems.Count, weaponUpgrades.Count);
            for (int i = 0; i < weaponUpgradeItems.Count; i++)
            {
                if (i < weaponUpgradeSelection.Count)
                {
                    int chosenIndex = weaponUpgradeSelection[i];
                    if (chosenIndex >= 0 && chosenIndex < weaponUpgrades.Count)
                    {
                        weaponUpgradeItems[i].gameObject.SetActive(true);
                        weaponUpgradeItems[i].Init(weaponUpgrades[chosenIndex], chosenIndex);

                        if (i < player.frozenWeaponUpgradeIndices.Count)
                        {
                            weaponUpgradeItems[i].makeFrozen(true,freezeColor);
                        }
                        else
                        {
                            weaponUpgradeItems[i].makeFrozen(false,unfrozenColor);
                        }
                    }
                }
            }
        }
        else
        {
            foreach (ShopItem shopItem in weaponUpgradeItems)
            {
                shopItem.gameObject.SetActive(false);
            }
        }

        // Weapon Prefabs
        var weaponPrefabSelection = PickWeightedIndices(weaponPrefabWeights, weaponPrefabs.Count, weaponItems.Count);
        weaponPrefabSelection = MergeFrozenIntoSelection( weaponPrefabSelection, player.frozenWeaponPrefabIndices, weaponItems.Count, weaponPrefabs.Count);
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
                    Weapon weapon = weaponPrefabs[chosenIndex].GetComponent<Weapon>();
                    weaponItems[i].Init(icon, weapon.weaponName, chosenIndex, weapon.description);
                    if (i < player.frozenWeaponPrefabIndices.Count)
                    {
                        weaponItems[i].makeFrozen(true,freezeColor);
                    }
                    else
                    {
                        weaponItems[i].makeFrozen(false,unfrozenColor);
                    }
                }
            }
        }

        playerOverviewUI.UpdateIcons(playerNum);
        UpdateDetails();
    }

    void SetupSellMenu()
    {
        foreach (ShopItem shopItem in sellUpgradeItems)
        {
            shopItem.gameObject.SetActive(false);
        }
        foreach (ShopItem shopItem in sellWeaponUpgradeItems)
        {
            shopItem.gameObject.SetActive(false);
        }
        sellWeaponItem.gameObject.SetActive(false);

        for (int i = 0; i < player.upgrades.Count; i++)
        {
            sellUpgradeItems[i].gameObject.SetActive(true);
            sellUpgradeItems[i].Init(player.upgrades[i], i, true);
        }
        for (int i = 0; i < GameManager.Instance.players[playerNum].weaponUpgrades.Count; i++)
        {
            sellWeaponUpgradeItems[i].gameObject.SetActive(true);
            sellWeaponUpgradeItems[i].Init(player.weaponUpgrades[i], i, true);
        }
        if (player.weaponPrefab != null)
        {
            Weapon weapon = player.weaponPrefab.GetComponent<Weapon>(); ;
            sellWeaponItem.gameObject.SetActive(true);

            sellWeaponItem.Init(weapon.GetComponentInChildren<SpriteRenderer>().sprite, weapon.weaponName, 0, weapon.description, true);
        }
        UpdateDetails();
    }

    public void ToggleBuySell()
    {
        sellmode = !sellmode;
        if (sellmode)
        {
            SetupSellMenu();
            sellButtonText.text = "Back to Shop";
            buyPanel.SetActive(false);
            sellPanel.SetActive(true);
        }
        else
        {
            sellButtonText.text = "See Current Upgrades";
            buyPanel.SetActive(true);
            sellPanel.SetActive(false);
        }
    }

    public void ToggleDetails()
    {
        detailsMode = !detailsMode;
        UpdateDetails();
    }

    void UpdateDetails()
    {
        foreach (ShopItem shopItem in upgradeItems)
        {
            shopItem.ShowDetails(detailsMode);
        }
        foreach (ShopItem shopItem in weaponUpgradeItems)
        {
            shopItem.ShowDetails(detailsMode);
        }
        foreach (ShopItem shopItem in weaponItems)
        {
            shopItem.ShowDetails(detailsMode);
        }
        foreach (ShopItem shopItem in sellUpgradeItems)
        {
            shopItem.ShowDetails(detailsMode);
        }
        foreach (ShopItem shopItem in sellWeaponUpgradeItems)
        {
            shopItem.ShowDetails(detailsMode);
        }
        sellWeaponItem.ShowDetails(detailsMode);
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

    public void ToggleFreezeMode()
    {
        freezeMode = !freezeMode;

        foreach (ShopItem item in upgradeItems)
        {
            item.SetFreezeMode(freezeMode);
        }
        foreach (ShopItem item in weaponItems)
        {
            item.SetFreezeMode(freezeMode);
        }
        foreach (ShopItem item in weaponUpgradeItems)
        {
            item.SetFreezeMode(freezeMode);
        }


        sellButton.interactable = !freezeMode;
        if (freezeMode)
        {
            freezeButtonText.text = "Cancel";
        }
        else
        {
            freezeButtonText.text = "Freeze";
        }
    }

    // merge frozen indices into the selection so frozen items occupy slots first (unique, validated).
    List<int> MergeFrozenIntoSelection(List<int> selection, List<int> frozenIndices, int pickCount, int availableCount)
    {
        List<int> result = new List<int>();

        if (pickCount <= 0) return result;

        // add valid frozen indices first (preserve order they were frozen in)
        if (frozenIndices != null)
        {
            foreach (int f in frozenIndices)
            {
                if (result.Count >= pickCount)
                {
                    break;
                }
                if (f >= 0 && f < availableCount && !result.Contains(f))
                {
                    result.Add(f);
                }
            }
        }

        // fill remaining slots from the random selection, skipping duplicates and invalid indices
        if (selection != null)
        {
            foreach (int s in selection)
            {
                if (result.Count >= pickCount)
                {
                    break;
                }
                if (s >= 0 && s < availableCount && !result.Contains(s))
                {
                    result.Add(s);
                }
            }
        }

        return result;
    }

    public void BuyUpgrade(int index)
    {
        int itemIndex = index;
        index = upgradeItems[index].index;
        // if in freeze mode add item to frozen list and return
        if (freezeMode)
        {
            Debug.Log("Freezing upgrade index: " + index);
            if (player.ToggleFreezeUpgrade(index))
            {
                upgradeItems[itemIndex].makeFrozen(true,freezeColor);
            }
            else
            {
                upgradeItems[itemIndex].makeFrozen(false,unfrozenColor);
            }
            return;
        }
        if (player.coins >= 3)
        {
            if (index < upgrades.Count)
            {
                if (GameManager.Instance.players[playerNum].AddUpgrade(upgrades[index]))
                {
                    player.coins -= 3;
                    // remove freeze 
                    player.RemoveFrozenUpgrade(index);
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
    
    public void SellUpgrade(int index)
    {
        Upgrade upgrade = player.upgrades[index];
        player.coins += 1 + upgrade.upgradeLevel;
        player.upgrades.Remove(upgrade);
        UpdateCoinText();
        playerOverviewUI.UpdateIcons(playerNum);
        SetupSellMenu();
    }
    public void SellWeaponUpgrade(int index)
    {
        WeaponUpgrade upgrade = player.weaponUpgrades[index];
        player.coins += 1 + upgrade.upgradeLevel;
        player.weaponUpgrades.Remove(upgrade);
        UpdateCoinText();
        playerOverviewUI.UpdateIcons(playerNum);
        SetupSellMenu();
    }

    public void BuyWeaponUpgrade(int index)
    {
        int itemIndex = index;
        index = weaponUpgradeItems[index].index;

        if (freezeMode)
        {
            if (player.ToggleFreezeWeaponUpgrade(index))
            {
                weaponUpgradeItems[itemIndex].makeFrozen(true,freezeColor);
            }
            else
            {
                weaponUpgradeItems[itemIndex].makeFrozen(false,unfrozenColor);
            }
            return;
        }
        if (player.coins >= 3)
        {
            if (index < weaponUpgrades.Count)
            {
                if (GameManager.Instance.players[playerNum].AddWeaponUpgrade(weaponUpgrades[index]))
                {
                    player.coins -= 3;
                    // remove freeze 
                    player.RemoveFrozenWeaponUpgrade(index);
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

    public void SellWeapon()
    {
        player.weaponPrefab = null;
        SellAllWeaponUpgrades();
        UpdateCoinText();
        playerOverviewUI.UpdateIcons(playerNum);
        SetupSellMenu();
        foreach (ShopItem shopItem in weaponUpgradeItems)
        {
            shopItem.gameObject.SetActive(false);
        }
    }

    void SellAllWeaponUpgrades()
    {
        for (int i = 0; i < player.weaponUpgrades.Count; i++)
        {
            SellWeaponUpgrade(0);
        }
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
        int itemIndex = index;
        index = weaponItems[index].index;

        if (freezeMode)
        {
            if (player.ToggleFreezeWeaponPrefab(index))
            {
                weaponItems[itemIndex].makeFrozen(true,freezeColor);
            }
            else
            {
                weaponItems[itemIndex].makeFrozen(false,unfrozenColor);
            }
            return;
        }
        if (player.coins >= 3)
        {
            if (index < weaponPrefabs.Count && weaponPrefabs[index] != null)
            {
                SellWeapon();
                player.weaponPrefab = weaponPrefabs[index];
                player.coins -= 3;
                // remove freeze 
                player.RemoveFrozenWeaponPrefab(index);
                UpdateCoinText();
                playerOverviewUI.UpdateIcons(playerNum);
                SetupShop();
            }
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
