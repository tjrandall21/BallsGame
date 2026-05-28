using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public int index = 0;

    [SerializeField] Image icon;
    [SerializeField] Image detailsIcon;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI detailsNameText;
    [SerializeField] TextMeshProUGUI description;


    [SerializeField] ShopItemType shopItemType = ShopItemType.Upgrade;

    [SerializeField] GameObject defaultPanel;
    [SerializeField] GameObject detailsPanel;
    int sellCost = 1;

    public void ShowDetails(bool show)
    {
        detailsPanel.SetActive(show);
        defaultPanel.SetActive(!show);
    }

    public enum ShopItemType
    {
        Upgrade,
        WeaponUpgrade,
        Weapon
    }

    public void Init(Sprite itemIcon,  string itemName, int newIndex, string desc, bool sellMode = false)
    {
        icon.sprite = itemIcon;
        detailsIcon.sprite = itemIcon;
        nameText.text = itemName;
        detailsNameText.text = itemName;
        description.text = desc;
        if (sellMode)
        {
            coinText.text = sellCost.ToString();
            buttonText.text = "Sell";
        }
        index = newIndex;
    }

    public void Init(Upgrade upgrade, int newIndex, bool sellMode = false)
    {
        sellCost = 1 + upgrade.upgradeLevel;
        Init(upgrade.shopIcon,upgrade.upgradeName, newIndex, upgrade.description, sellMode);
        shopItemType = ShopItemType.Upgrade;
    }

    public void Init(WeaponUpgrade upgrade, int newIndex, bool sellMode = false)
    {
        sellCost = 1 + upgrade.upgradeLevel;
        Init(upgrade.shopIcon,upgrade.upgradeName, newIndex, upgrade.description, sellMode);
        shopItemType = ShopItemType.WeaponUpgrade;
    }
    
    

}
