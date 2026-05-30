using UnityEngine;

[CreateAssetMenu(fileName = "DualWieldUpgrade", menuName = "Weapon Upgrades/Dagger Upgrades/DualWieldUpgrade")]

public class DualWieldUpgrade : DaggerUpgrade
{
    [SerializeField] public GameObject secondaryDaggerPrefab;
    private Dagger secondDagger;

    public override void Init(BallController ball, Weapon weapon)
    {
       base.Init(ball, weapon);
       BoxCollider2D boxCollider = parentWeapon.GetComponent<BoxCollider2D>();
       boxCollider.size = new Vector2(boxCollider.size.x,3.69f);
       boxCollider.offset = new Vector2(0,-1.15f);
       GameObject dualWield = Instantiate(secondaryDaggerPrefab, ball.transform);
       dualWield.layer = ball.gameObject.layer;
       secondDagger = dualWield.GetComponent<Dagger>();
       foreach(WeaponUpgrade existingUpgrade in parentWeapon.WeaponUpgrades)
       {
           if (existingUpgrade is DualWieldUpgrade) continue;
           secondDagger.AddUpgrade(existingUpgrade);
       }
    }
    public override void OnUpgradeAdded(WeaponUpgrade newUpgrade)
    {
        base.OnUpgradeAdded(newUpgrade);
        if (newUpgrade is DualWieldUpgrade)
        {
            return;
        }
        secondDagger.AddUpgrade(newUpgrade);
    }
}
