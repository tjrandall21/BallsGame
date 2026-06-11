using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bow : Weapon
{

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float arrowSpeed = 15;
    [SerializeField] public float arrowDamage = 5;
    [SerializeField] float arrowDamageScaling = 1;
    [SerializeField] int extraProjectiles = 0;
    [SerializeField] float spreadIncreasePerArrow = 20;
    [SerializeField] float attackSpeedScaling = 0.02f;
    

    protected override void Start()
    {
        base.Start();
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is BowUpgrade)
            {
                arrowSpeed += ((BowUpgrade)weaponUpgrade).arrowSpeed;
                arrowDamage += ((BowUpgrade)weaponUpgrade).arrowDamage;
                arrowDamageScaling += ((BowUpgrade)weaponUpgrade).arrowDamageScaling;
                extraProjectiles += ((BowUpgrade)weaponUpgrade).extraProjectiles;
                attackSpeedScaling += ((BowUpgrade)weaponUpgrade).attackSpeedScaling;
            }
        }
    }

    protected override void OnAttack()
    {
        float halfSpreadRadians = extraProjectiles * spreadIncreasePerArrow * (math.PI / 180.0f)  * 0.5f;

        for (int i = 0; i < 1 + extraProjectiles; i++)
        {
            float rotation = transform.eulerAngles.z * math.PI / 180.0f + math.PI * 0.5f;
            if (extraProjectiles > 0)
            {
                float spreadRatio = (i / (float)extraProjectiles - 0.5f) * 2;
                rotation += spreadRatio * halfSpreadRadians;
            }
            quaternion arrowRotation = quaternion.Euler(0,0,rotation-math.PI/2);
            GameObject projectileObject = Instantiate(projectilePrefab, transform.position, arrowRotation);
            BowProjectile projectile = projectileObject.GetComponent<BowProjectile>();

            FXManager.Instance.PlayWeaponHit(transform.position, this);

            projectile.ProjectileInit(new Vector3(math.cos(rotation), math.sin(rotation)), arrowSpeed, arrowDamage, this);
            projectileObject.layer = gameObject.layer < 10 ? gameObject.layer+4 : gameObject.layer;

            foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
            {
                if (weaponUpgrade is BowUpgrade)
                {
                    if (((BowUpgrade)weaponUpgrade).passToArrows)
                    {
                        projectile.AddUpgrade(weaponUpgrade);
                    }
                } 
            }
        }

        OnAttackEnd();
    }

    public override void OnWallCollision()
    {
        base.OnWallCollision();
        attackCooldown *= 1f - attackSpeedScaling;
    }

    public void OnArrowBallHit(BallController otherBall, BowProjectile arrow)
    {
        arrowDamage += arrowDamageScaling;
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
           if (weaponUpgrade is BowUpgrade)
            {
                ((BowUpgrade)weaponUpgrade).OnArrowBallHit(otherBall,arrow);
            } 
        }
        OnArrowDestroyed(arrow);
    }

    public void OnArrowWallHit(BowProjectile arrow)
    {
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
           if (weaponUpgrade is BowUpgrade)
            {
                ((BowUpgrade)weaponUpgrade).OnArrowWallHit(arrow);
            } 
        }
        OnArrowDestroyed(arrow);
    }

    public void OnArrowWeaponHit(Weapon otherWeapon, BowProjectile arrow)
    {
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
           if (weaponUpgrade is BowUpgrade)
            {
                ((BowUpgrade)weaponUpgrade).OnArrowWeaponHit(otherWeapon,arrow);
            } 
        }
        OnArrowDestroyed(arrow);
    }

    public void OnArrowDestroyed(BowProjectile arrow)
    {
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
           if (weaponUpgrade is BowUpgrade)
            {
                ((BowUpgrade)weaponUpgrade).OnArrowDestroyed(arrow);
            } 
        }
    }
}
