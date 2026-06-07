using UnityEngine;

public class BowProjectile : Projectile
{
    protected override void OnBallHit(BallController otherBall)
    {
        FXManager.Instance.PlayProjectileFire(transform.position, this);
        ((Bow)parentWeapon).OnArrowBallHit(otherBall, this);
        base.OnBallHit(otherBall);

    }

    protected override void OnWallHit()
    {
        ((Bow)parentWeapon).OnArrowWallHit(this);
        base.OnWallHit();
    }

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        ((Bow)parentWeapon).OnArrowWeaponHit(otherWeapon,this);
        base.OnWeaponHit(otherWeapon);
    }
}
