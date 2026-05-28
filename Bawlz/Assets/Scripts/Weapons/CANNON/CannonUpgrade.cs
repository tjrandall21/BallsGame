using UnityEngine;

public class CannonUpgrade : WeaponUpgrade
{
    [SerializeField] public int u_prodCount = 0;
    [SerializeField] public float u_projSpeed = 0;
    [SerializeField] public int u_projDamage = 0;

    public override void OnAttack() { }
    public virtual void OnProjectileHit(BallController otherBall, Projectile projectile) { }
    public virtual void OnProjectileWeaponHit(Weapon otherWeapon, Projectile projectile) { }
    public virtual void OnProjectileDestroyed(Projectile projectile) { }

}