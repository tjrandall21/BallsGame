using UnityEngine;

public class Dagger : Weapon
{
    [SerializeField] private DoTEffect poisonEffect;
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

        otherBall.ApplyStatus(poisonEffect, parent); 

    }

}
