using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Cannon : Weapon
{
    [SerializeField, Tooltip("The projectile prefab to spawn when firing")] GameObject projectilePrefab;
    [SerializeField, Tooltip("Speed of the projectile")] float projSpeed = 15;
    [SerializeField, Tooltip("Number of projectiles fired per shot")] int projCount = 1;
    [SerializeField, Tooltip("Damage dealt by each projectile")] int projDamage = 10;
    [SerializeField, Tooltip("Force applied to the shooter on firing")] float knockbackForce = 10f;
    [SerializeField, Tooltip("Duration in seconds the knockback force is spread over")] float knockbackDuration = 0.2f;
    [HideInInspector] public bool suppressBaseShot = false; // <- for grapeshot and volly upgrades to prevent base shot from firing

    public GameObject ProjectilePrefab => projectilePrefab;
    public float ProjSpeed => projSpeed;
    public int ProjDamage => projDamage;

    private Coroutine _knockbackCoroutine;

    protected override void Start()
    {
        base.Start();
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is CannonUpgrade cannonUpgrade)
            {
                projSpeed += cannonUpgrade.u_projSpeed;
                projDamage += cannonUpgrade.u_projDamage;
                projCount += cannonUpgrade.u_prodCount;
            }
        }
    }

    protected override void OnAttack()
    {
        float rotation = transform.eulerAngles.z * math.PI / 180.0f + math.PI / 2;
        Vector3 shotDirection = new Vector3(math.cos(rotation), math.sin(rotation));

        // Let upgrades override attack behaviour (e.g. GrapeShotUpgrade)
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is CannonUpgrade cannonUpgrade)
            {
                cannonUpgrade.OnAttack();
            }
        }

        // Fire base projectiles only if no upgrade suppressed it
        if (!suppressBaseShot)
        {
            for (int i = 0; i < projCount; i++)
            {
                GameObject projectileObject = Instantiate(projectilePrefab, transform.position, transform.rotation);
                CannonProdj projectile = projectileObject.GetComponent<CannonProdj>();
                projectile.ProjectileInit(shotDirection, projSpeed, projDamage, this);
                projectileObject.layer = gameObject.layer + 4;
            }
        }
        suppressBaseShot = false;

        Rigidbody2D rb = parent.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (_knockbackCoroutine != null)
                StopCoroutine(_knockbackCoroutine);
            _knockbackCoroutine = StartCoroutine(SmoothKnockback(rb, -shotDirection));
        }

        OnAttackEnd();
    }
    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        FXManager.Instance.PlayWeaponHit(otherWeapon.transform.position);
        base.OnWeaponHit(otherWeapon);

        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is CannonUpgrade cannonUpgrade)
            {
                cannonUpgrade.OnWeaponHit(otherWeapon);
            }
        }
    }

    protected override void OnBallHit(BallController otherBall)
    {
        FXManager.Instance.PlayPlayerHit(otherBall.transform.position);

        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is CannonUpgrade cannonUpgrade)
            {
                cannonUpgrade.OnBallHit(otherBall);
            }
        }
    }

    public void OnProjectileHit(BallController otherBall, Projectile projectile)
    {
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is CannonUpgrade cannonUpgrade)
            {
                cannonUpgrade.OnProjectileHit(otherBall, projectile);
            }
        }
    }

    public void OnProjectileWeaponHit(Weapon otherWeapon, Projectile projectile)
    {
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is CannonUpgrade cannonUpgrade)
            {
                cannonUpgrade.OnProjectileWeaponHit(otherWeapon, projectile);
            }
        }
    }

    public void OnProjectileDestroyed(Projectile projectile)
    {
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is CannonUpgrade cannonUpgrade)
            {
                cannonUpgrade.OnProjectileDestroyed(projectile);
            }
        }
    }

    private IEnumerator SmoothKnockback(Rigidbody2D rb, Vector3 direction)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackDuration);
        _knockbackCoroutine = null;
    }

    public void FireVolley()
    {
        float rotation = transform.eulerAngles.z * math.PI / 180.0f + math.PI / 2;
        Vector3 shotDirection = new Vector3(math.cos(rotation), math.sin(rotation));

        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is CannonUpgrade cannonUpgrade && !(cannonUpgrade is BurstUpgrade))
            {
                cannonUpgrade.OnAttack();
            }
        }

        if (!suppressBaseShot)
        {
            for (int i = 0; i < projCount; i++)
            {
                GameObject projectileObject = Instantiate(projectilePrefab, transform.position, transform.rotation);
                CannonProdj projectile = projectileObject.GetComponent<CannonProdj>();
                projectile.ProjectileInit(shotDirection, projSpeed, projDamage, this);
                projectileObject.layer = gameObject.layer + 4;
            }
        }
        suppressBaseShot = false;
    }
}