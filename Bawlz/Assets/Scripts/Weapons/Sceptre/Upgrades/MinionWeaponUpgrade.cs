using UnityEngine;

[CreateAssetMenu(fileName = "MinionWeaponUpgrade", menuName = "Weapon Upgrades/Sceptre Upgrades/MinionWeaponUpgrade")]
public class MinionWeaponUpgrade : SceptreUpgrade
{
    [SerializeField] GameObject weaponPrefab;

    public override void OnBallSpawned(BallController newBall)
    {
        base.OnBallSpawned(newBall);
        if (newBall.GetComponentInChildren<Weapon>() == null)
        {
            GameObject weapon = Instantiate(weaponPrefab, newBall.transform);
            weapon.layer = newBall.gameObject.layer;
            Vector3 weaponScale = weapon.transform.localScale;
            weaponScale *= 0.5f;
            weapon.transform.localScale = weaponScale;
            Vector3 weaponPosition = weapon.transform.localPosition;
            weaponPosition.y *= 0.5f;
            weapon.transform.localPosition = weaponPosition;
        }
    }
}
