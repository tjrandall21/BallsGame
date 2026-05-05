using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon
{
    [SerializeField] float maxWeaponDmg = 10f; // will be incromented by 10 
    [SerializeField] float maxWeaponSpin = 10f;
    [SerializeField] float rotationAcceleration = 100f;
    [SerializeField] float baseRotationSpeed = 180;
  

    protected virtual void Update()
    {
        // will always increase spin up to maxSpin
        parent.RotationSpeed += rotationAcceleration * Time.deltaTime;
        base.Update();
    }

    protected override void OnBallHit(BallController otherBall)
    {
        Debug.Log("Ball Collision");
        if (parent != null)
        {
            parent.FlipRotation();
        }
        otherBall.OnWeaponCollision(this);
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            weaponUpgrade.OnBallHit(otherBall);
        }

        maxWeaponDmg += 10;

        parent.RotationSpeed = 180; // resets ball rotattion
    }

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        base.OnWeaponHit(otherWeapon);
    }

    protected override void OnWallHit()
    {
       
       
        base.OnWallHit();
    }
}
