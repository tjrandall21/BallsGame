using UnityEngine;

[CreateAssetMenu(fileName = "SkipSpinReset", menuName = "Weapon Upgrades/Hammer Upgrades/SkipSpinReset")]
public class SpinResetChance : HammerUpgrade
{
    [SerializeField] float skipChance = 0.125f;

    public override void OnWeaponHit(Weapon otherWeapon)
    {
        base.OnWeaponHit(otherWeapon);
        if (Random.value < skipChance)
        {
            ((Hammer)parentWeapon).skipSpinReset = true;
        }
    }
}