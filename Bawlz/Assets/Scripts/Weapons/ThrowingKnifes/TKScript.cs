using Unity.Mathematics;
using UnityEngine;

public class TKScript : Weapon
{
    [SerializeField, Tooltip("The throwing knife projectile prefab")] GameObject projectilePrefab;
    [SerializeField, Tooltip("Speed the knife is thrown at")] float knifeSpeed = 15;
    [SerializeField, Tooltip("Damage the knife deals on hit")] float knifeDamage = 10;
    [SerializeField] float stickDuration = 1f;
    [SerializeField] float baseFadeDuration = 0.5f;
    float fadeDuration = 0;
    [SerializeField] float meleeScaling = 0f;
    [SerializeField] float projectileScaling = 0f;
    [SerializeField] float attackSpeedScaling = 0.03f;
    [SerializeField] DoTEffect bleedEffect;
    [SerializeField] float burstSpinModifier = 0.3f;
    [SerializeField] float timeBetweenAttacks = 0.1f;
    int projectileBounces = 0;
    int projectileCount = 1;
    int attacksLeftInBurst = 0;
    bool midBurst = false;

    protected override void Start()
    {
        base.Start();
        fadeDuration = baseFadeDuration;
        foreach (WeaponUpgrade upgrade in weaponUpgrades)
        {
            if (upgrade is TKUpgrade tKUpgrade)
            {
                damage += tKUpgrade.TKDamage;
                projectileCount += tKUpgrade.extraProjectiles;
                fadeDuration *= tKUpgrade.fadeDurationMulti;
                stickDuration *= tKUpgrade.stickDurationMulti;
                attackSpeedScaling += tKUpgrade.attackSpeedScaling;
                parent.RotationSpeed += tKUpgrade.spinSpeed;
                meleeScaling += tKUpgrade.meleeScaling;
                projectileScaling += tKUpgrade.projectileScaling;
                projectileBounces += tKUpgrade.projectileBounces;
            }
        }
    }

    protected override void OnAttack()
    {
        if (attacksLeftInBurst == 0)
        {
            attacksLeftInBurst = projectileCount;
            if (!midBurst && projectileCount > 1)
            {
                parent.RotationSpeed *= burstSpinModifier;
                midBurst = true;
            }
        }
        float adjustedStickDuration = fadeDuration == 0 ? stickDuration+baseFadeDuration : stickDuration;
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, transform.rotation);
        TKProdj projectile = projectileObject.GetComponent<TKProdj>();
        float rotation = transform.eulerAngles.z * math.PI / 180.0f + math.PI / 2;
        projectile.ProjectileInit(new Vector3(math.cos(rotation), math.sin(rotation)), knifeSpeed, knifeDamage, this, adjustedStickDuration, fadeDuration);
        projectile.bounces = projectileBounces;
        projectileObject.layer = gameObject.layer + 4;
        attacksLeftInBurst--;
        if (attacksLeftInBurst > 0)
        {
            attackTimer = timeBetweenAttacks;
        }
        else
        {
            OnAttackEnd();
            if (midBurst)
            {
                parent.RotationSpeed /= burstSpinModifier;
                midBurst = false;
            }
        }
    }

    public override void OnWallCollision()
    {
        base.OnWallCollision();
    }

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        FXManager.Instance.PlayWeaponHit(otherWeapon.transform.position);
        base.OnWeaponHit(otherWeapon);
    }

    protected override void OnBallHit(BallController otherBall)
    {
        FXManager.Instance.PlayPlayerHit(otherBall.transform.position);
        attackCooldown *= 1f-attackSpeedScaling;
        knifeDamage += projectileScaling;
        base.OnBallHit(otherBall);
    }

    public virtual void OnProjectileBallHit(TKProdj projectile, BallController otherBall)
    {
        damage += meleeScaling;
        foreach (WeaponUpgrade upgrade in weaponUpgrades)
        {
            if (upgrade is TKUpgrade tKUpgrade)
            {
                tKUpgrade.OnProjectileBallHit(projectile, otherBall);
            }
        }
    }
    public virtual void OnProjectileWallHit(TKProdj projectile)
    {
        foreach (WeaponUpgrade upgrade in weaponUpgrades)
        {
            if (upgrade is TKUpgrade tKUpgrade)
            {
                tKUpgrade.OnProjectileWallHit(projectile);
            }
        }
    }
    public virtual void OnProjectileWeaponHit(TKProdj projectile, Weapon otherWeapon)
    {
        foreach (WeaponUpgrade upgrade in weaponUpgrades)
        {
            if (upgrade is TKUpgrade tKUpgrade)
            {
                tKUpgrade.OnProjectileWeaponHit(projectile, otherWeapon);
            }
        }
    }
    public virtual void OnProjectileDestroyed(TKProdj projectile)
    {
        foreach (WeaponUpgrade upgrade in weaponUpgrades)
        {
            if (upgrade is TKUpgrade tKUpgrade)
            {
                tKUpgrade.OnProjectileDestroyed(projectile);
            }
        }
    }
    public virtual void OnProjectileTimeout(TKProdj projectile)
    {
        foreach (WeaponUpgrade upgrade in weaponUpgrades)
        {
            if (upgrade is TKUpgrade tKUpgrade)
            {
                tKUpgrade.OnProjectileTimeout(projectile);
            }
        }
    }
}