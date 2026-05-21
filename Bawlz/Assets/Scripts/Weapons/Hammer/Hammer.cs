using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon
{
    [SerializeField] float baseDmg = 6.5f;
    [SerializeField] float maxWeaponSpin = 240f;
    [SerializeField] float rotationAcceleration = 100f;
    [SerializeField] float rotationAccelerationScaling = 20f;
    [SerializeField] float baseRotationSpeed = 180f;
    [SerializeField] float maxSpinIncreasePerHit = 45f; // how much max spin grows each hit

    protected override void Start()
    {
        base.Start();
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is HammerUpgrades)
            {
                baseDmg += ((HammerUpgrades)weaponUpgrade).damage;
                maxWeaponSpin += ((HammerUpgrades)weaponUpgrade).attackCooldown;
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
    }

    protected override void OnBallHit(BallController otherBall)
    {
        FXManager.Instance.PlayPlayerHit(otherBall.transform.position);

        Debug.Log("Ball Collision");
        if (parent != null)
        {
            parent.FlipRotation();
        }

        float damage = GetSpinDamage();
        Debug.Log($"Hammer hit for {damage} (baseDmg={baseDmg}, spin={parent.RotationSpeed})");
        otherBall.OnDamageTaken(damage);

        otherBall.OnWeaponCollision(this);

        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is HammerUpgrades)
            {
                weaponUpgrade.OnBallHit(otherBall);
            } 
        }

        rotationAcceleration += rotationAccelerationScaling; // hammer accelerates faster next cycle
        maxWeaponSpin += maxSpinIncreasePerHit; // hammer can now spin faster next cycle
        parent.RotationSpeed = baseRotationSpeed; // reset to base, begins climbing again
    }
}