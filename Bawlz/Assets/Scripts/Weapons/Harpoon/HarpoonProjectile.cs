using UnityEngine;

public class HarpoonProjectile : Projectile
{
    public float pullSpeed = 30;
    protected override void OnBallHit(BallController otherBall)
    {
        otherBall.OnWeaponCollision(this);
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            weaponUpgrade.OnBallHit(otherBall);
        }
        Vector2 destination = parentWeapon.transform.position;
        Vector2 direction = (destination - (Vector2)transform.position).normalized;
        otherBall.SetVelocity(direction * pullSpeed);
        Destroy(gameObject);
    }
}
