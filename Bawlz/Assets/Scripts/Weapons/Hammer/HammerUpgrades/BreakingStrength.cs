using UnityEngine;

[CreateAssetMenu(fileName = "BreakingStrength", menuName = "Weapon Upgrades/Hammer Upgrades/BreakingStrength")]
public class BreakingStrength : HammerUpgrade
{
    [Tooltip("Fraction of this weapon's total Damage applied when hitting another weapon (0.1 = 10%)")]
    [SerializeField] float damagePercent = 0.1f;
    [SerializeField] float flatdamage = 10f;

    public override void OnWeaponHit(Weapon otherWeapon)
    {
        if (otherWeapon == null || otherWeapon.parent == null)
        {
            Debug.LogWarning("Other weapon or its parent is null. Cannot apply damage.");
            return;
        }

        otherWeapon.parent.OnDamageTaken(flatdamage);
        Debug.Log($"Dealt {flatdamage} damage to {otherWeapon.parent.name}");
    }
}