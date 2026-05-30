using UnityEngine;

[CreateAssetMenu(fileName = "ThrowingKnifeUpgrade", menuName = "Weapon Upgrades/TK Upgrades/TKUpgrade")]
public class TKUpgrade : WeaponUpgrade
{
    [SerializeField] public float TKSpeed = 0;
    [SerializeField] public float TKDamage = 0;
    [SerializeField] public float meleeScaling = 0f;
    [SerializeField] public float projectileScaling = 0f;
    [SerializeField] public int extraProjectiles = 0;
    [SerializeField] public float attackSpeedScaling = 0;
    [SerializeField] public float fadeDurationMulti = 1f;
    [SerializeField] public float stickDurationMulti = 1f;
    [SerializeField] public float spinSpeed = 0;
    [SerializeField] public int projectileBounces = 0;
    

    public virtual void OnProjectileBallHit(TKProdj projectile, BallController otherBall) {}
    public virtual void OnProjectileWallHit(TKProdj projectile) {}
    public virtual void OnProjectileWeaponHit(TKProdj projectile, Weapon otherWeapon) {}
    public virtual void OnProjectileDestroyed(TKProdj projectile) {}
    public virtual void OnProjectileTimeout(TKProdj projectile) {}
}