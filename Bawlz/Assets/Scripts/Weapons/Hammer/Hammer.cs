using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon
{
    [SerializeField] float baseDmg = 10f;
    [SerializeField] float maxWeaponSpin = 720f;
    [SerializeField] float rotationAcceleration = 100f;
    [SerializeField] float baseRotationSpeed = 180f;
    [SerializeField] float maxSpinIncreasePerHit = 100f; // how much max spin grows each hit

    private float GetSpinDamage()
    {
        float spinRatio = Mathf.Clamp01(parent.RotationSpeed / maxWeaponSpin / 2);
        return baseDmg * (0.1f + spinRatio);
    }

    private void Update()
    {
        if (parent == null) return;
        parent.RotationSpeed = Mathf.MoveTowards(
            parent.RotationSpeed, maxWeaponSpin, rotationAcceleration * Time.deltaTime
        );
    }

    protected override void OnBallHit(BallController otherBall)
    {
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
            weaponUpgrade.OnBallHit(otherBall);
        }

        baseDmg += 10;
        maxWeaponSpin += maxSpinIncreasePerHit; // hammer can now spin faster next cycle
        parent.RotationSpeed = baseRotationSpeed; // reset to base, begins climbing again
    }
}