using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Bow : Weapon
{

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float arrowSpeed = 15;
    [SerializeField] int arrowDamage = 10;

    protected override void OnAttack()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, transform.rotation);
        BowProjectile projectile = projectileObject.GetComponent<BowProjectile>();
        float rotation = transform.eulerAngles.z * math.PI / 180.0f + math.PI / 2;
        projectile.ProjectileInit(new Vector3(math.cos(rotation), math.sin(rotation)), arrowSpeed, arrowDamage, this);
        projectileObject.layer = gameObject.layer;
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
        OnAttackEnd();
    }

    public override void OnWallCollision()
    {
        base.OnWallCollision();
        attackCooldown *= 0.98f;
    }

    public void OnArrowBallHit(BallController otherBall, BowProjectile arrow)
    {
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
