using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public int index = 0;

    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] TextMeshProUGUI coinText;

    [SerializeField] ShopItemType shopItemType = ShopItemType.Upgrade;

    public enum ShopItemType
    {
        Upgrade,
        WeaponUpgrade,
        Weapon
    }

    public void Init(Sprite itemIcon,  string itemName, int newIndex, bool sellMode = false)
    {
        icon.sprite = itemIcon;
        nameText.text = itemName;
        if (sellMode)
        {
            coinText.text = "1";
            buttonText.text = "Sell";
        }
        index = newIndex;
    }

    public void Init(Upgrade upgrade, int newIndex, bool sellMode = false)
    {
        Init(upgrade.shopIcon,upgrade.upgradeName, newIndex, sellMode);
        shopItemType = ShopItemType.Upgrade;
    }

    public void Init(WeaponUpgrade upgrade, int newIndex, bool sellMode = false)
    {
        Init(upgrade.shopIcon,upgrade.upgradeName, newIndex, sellMode);
        shopItemType = ShopItemType.WeaponUpgrade;
    }

}
