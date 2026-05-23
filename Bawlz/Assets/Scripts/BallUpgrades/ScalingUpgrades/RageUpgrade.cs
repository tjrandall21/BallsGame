using UnityEngine;

[CreateAssetMenu(fileName = "RageUpgrade", menuName = "Scriptable Objects/RageUpgrade")]
public class RageUpgrade : Upgrade
{
    [SerializeField] float damageIncrease = 0.5f;

    public override void OnWeaponCollision(Weapon weapon)
    {
        base.OnWeaponCollision(weapon);
        parentBall.contactDamage += damageIncrease;
    }



}
