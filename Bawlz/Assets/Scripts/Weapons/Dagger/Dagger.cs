using UnityEngine;

public class Dagger : Weapon
{
    [SerializeField] private DoTEffect poisonEffect;

    public float durationScaling = 0.3f;
    public float damageScaling = 0.3f;

    protected override void Start()
    {
        base.Start();
        poisonEffect = new DoTEffect{damagePerSecond = 1f,statusDuration = 2f,statusName = "Dagger Poison"};
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

        otherBall.ApplyStatus(poisonEffect, parent); 
        poisonEffect.damagePerSecond += damageScaling;
        poisonEffect.statusDuration += durationScaling;
    }
}
