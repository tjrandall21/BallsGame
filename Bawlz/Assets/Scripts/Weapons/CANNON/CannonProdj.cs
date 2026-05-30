using UnityEngine;

public class CannonProdj : Projectile
{
    protected override void OnBallHit(BallController otherBall)
    {
        ((Cannon)parentWeapon).OnProjectileHit(otherBall, this);
        ((Cannon)parentWeapon).OnProjectileDestroyed(this);
        base.OnBallHit(otherBall);
    }

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        ((Cannon)parentWeapon).OnProjectileWeaponHit(otherWeapon, this);
        ((Cannon)parentWeapon).OnProjectileDestroyed(this);
        base.OnWeaponHit(otherWeapon);
    }

    protected override void OnWallHit()
    {
        ((Cannon)parentWeapon).OnProjectileDestroyed(this);
        base.OnWallHit();
    }
}