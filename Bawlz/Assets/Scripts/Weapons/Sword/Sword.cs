using UnityEngine;

public class Sword : Weapon
{
    //damage scaling
    [SerializeField] float damageScalingAmount = 1;

    protected override void Start()
    {
        base.Start();
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is SwordUpgrade)
            {
                SwordUpgrade swordUpgrade = (SwordUpgrade)weaponUpgrade;
                damageScalingAmount += swordUpgrade.damageScalingAmount;
                transform.localScale *= swordUpgrade.size;
                Vector2 pos = transform.localPosition;
                pos.y *= swordUpgrade.size;
                transform.localPosition = pos;
                parent.RotationSpeed += swordUpgrade.spinSpeed;
            }
        }
    }

    public override void OnWallCollision()
    {
        base.OnWallCollision();
        damage += damageScalingAmount;
    }

    protected override void OnBallHit(BallController otherBall)
    {
        FXManager.Instance.PlayWeaponHit(transform.position, this);
        base.OnBallHit(otherBall);
    }

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        base.OnWeaponHit(otherWeapon);
    }

}
