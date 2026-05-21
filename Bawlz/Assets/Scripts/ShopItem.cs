using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

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

    public void Init(Sprite itemIcon, string itemName, bool sellMode = false)
    {
        icon.sprite = itemIcon;
        nameText.text = itemName;
        if (sellMode)
        {
            coinText.text = "1";
            buttonText.text = "Sell";
        }
    }

    public void Init(Upgrade upgrade, bool sellMode = false)
    {
        Init(upgrade.shopIcon,upgrade.upgradeName, sellMode);
        shopItemType = ShopItemType.Upgrade;
    }

    public void Init(WeaponUpgrade upgrade, bool sellMode = false)
    {
        Init(upgrade.shopIcon,upgrade.upgradeName,  sellMode);
        shopItemType = ShopItemType.WeaponUpgrade;
    }

}
