using UnityEngine;

[CreateAssetMenu(fileName = "DualWieldUpgrade", menuName = "Weapon Upgrades/Dagger Upgrades/DualWieldUpgrade")]

public class DualWieldUpgrade : DaggerUpgrade
{
    [SerializeField] public GameObject secondaryDaggerPrefab;

    public override void Init(BallController ball, Weapon weapon)
    {
        base.Init(ball, weapon);

        GameObject dualWield = Instantiate(secondaryDaggerPrefab, ball.transform);
        dualWield.layer = ball.gameObject.layer;

        Dagger secondDagger = dualWield.GetComponent<Dagger>();
       foreach(WeaponUpgrade existingUpgrade in parentWeapon.WeaponUpgrades)
       {
           if (existingUpgrade is DualWieldUpgrade) continue;
           secondDagger.AddUpgrade(existingUpgrade);
       }
    }
}
