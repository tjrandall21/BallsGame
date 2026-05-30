using UnityEngine;

[CreateAssetMenu(fileName = "DaggerParryUpgrade", menuName = "Weapon Upgrades/Dagger Upgrades/DaggerParryUpgrade")]
public class DaggerParryUpgrade : DaggerUpgrade
{
    [SerializeField] float parryChance = 0.2f;
    [SerializeField] float parryDamageMultiplier = 0.2f;

    public override void OnWeaponCollision(Weapon weapon)
    {
        base.OnWeaponCollision(weapon);

        if(Random.value > parryChance)
        {
            return;
        }
        
        if(weapon.parent != null)
        {
            float parryDamage = weapon.Damage * parryDamageMultiplier;
            weapon.parent.OnDamageTaken(parryDamage);
            Debug.Log($"[Parry] Reflected {parryDamage} damage back to {weapon.parent.name}!");
        }
    }
}
