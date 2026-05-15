using UnityEngine;

public class Sword : Weapon
{
    //damage scaling
    float damageScalingAmount = 1;

    public override void OnWallCollision()
    {
        base.OnWallCollision();
        damage += damageScalingAmount;
    }

    protected override void OnBallHit(BallController otherBall)
    {
        FXManager.Instance.PlayPlayerHit(otherBall.transform.position);
        base.OnBallHit(otherBall);
    }

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        FXManager.Instance.PlayWeaponHit(otherWeapon.transform.position);
        base.OnWeaponHit(otherWeapon);
    }

}
