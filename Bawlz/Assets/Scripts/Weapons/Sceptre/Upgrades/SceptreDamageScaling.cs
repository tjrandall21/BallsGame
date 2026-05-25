using UnityEngine;

[CreateAssetMenu(fileName = "SceptreDamageUpgrade", menuName = "Weapon Upgrades/Sceptre Upgrades/SceptreDamageUpgrade")]
public class SceptreDamageScaling : SceptreUpgrade
{
    [SerializeField] float damageScaling = 0;

    public override void OnMinionDeath(BallController minion)
    {
        base.OnMinionDeath(minion);
        parentWeapon.Damage += damageScaling;
    }
}
