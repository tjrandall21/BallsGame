using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon
{
    [SerializeField] float baseDmg = 6.5f;
    [SerializeField] float maxWeaponSpin = 240f;
    [SerializeField] float rotationAcceleration = 100f;
    [SerializeField] float rotationAccelerationScaling = 20f;
    [SerializeField] float baseRotationSpeed = 180f;
    [SerializeField] float maxSpinIncreasePerHit = 45f;

    protected override void Start()
    {
        base.Start();
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is HammerUpgrade hammerUpgrade)
            {
                baseDmg += hammerUpgrade.u_baseDmg;
                maxWeaponSpin += hammerUpgrade.u_maxWeaponSpin;
                baseRotationSpeed += hammerUpgrade.u_baseRotationSpeed;
                rotationAcceleration += hammerUpgrade.u_rotationAcceleration;
                maxSpinIncreasePerHit += hammerUpgrade.u_maxSpinIncreasePerHit;
            }
        }
    }

    float GetSpinDamage()
    {
        return baseDmg * (parent.RotationSpeed / baseRotationSpeed);
    }

    protected override void Update()
    {
        if (parent == null) return;
        parent.RotationSpeed = Mathf.MoveTowards(
            parent.RotationSpeed, maxWeaponSpin, rotationAcceleration * Time.deltaTime
        );
    }

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        FXManager.Instance.PlayWeaponHit(otherWeapon.transform.position);
        base.OnWeaponHit(otherWeapon);

        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is HammerUpgrade hammerUpgrade)
                hammerUpgrade.OnWeaponHit(otherWeapon);
        }
    }

    protected override void OnBallHit(BallController otherBall)
    {
        FXManager.Instance.PlayPlayerHit(otherBall.transform.position);

        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is HammerUpgrade hammerUpgrade)
                hammerUpgrade.OnBallHit(otherBall);
        }

        if (parent != null)
            parent.FlipRotation();

        float damage = GetSpinDamage();
        otherBall.OnDamageTaken(damage);
        otherBall.OnWeaponCollision(this);

      

        rotationAcceleration += rotationAccelerationScaling;
        maxWeaponSpin += maxSpinIncreasePerHit;
        parent.RotationSpeed = baseRotationSpeed;
    }
}