using UnityEngine;

public class Dagger : Weapon
{
    [SerializeField] private DoTEffect poisonEffect;
    public float durationScaling = 0.3f;
    public float damageScaling = 0.3f;
    public float damagePerSecond = 1;
    public float duration = 5f;


    protected override void Start()
    {
        base.Start();
        foreach(WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if(weaponUpgrade is DaggerUpgrade)
            {
                DaggerUpgrade daggerUpgrade = (DaggerUpgrade)weaponUpgrade;
                damageScaling += daggerUpgrade.damageScalingAmount;
                parent.RotationSpeed += daggerUpgrade.spinSpeed;
            }
        }
    }

    protected override void OnBallHit(BallController otherBall)
    {
        Debug.Log("Ball Collision");

        if (parent != null)
            parent.FlipRotation();

        FXManager.Instance.PlayWeaponHit(transform.position, this);
        otherBall.OnWeaponCollision(this);

        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
            weaponUpgrade.OnBallHit(otherBall);

        DoTEffect scaledPoison = Instantiate(poisonEffect);
        damagePerSecond += damageScaling;
        scaledPoison.damagePerSecond = damagePerSecond;
        duration += durationScaling;
        scaledPoison.statusDuration = duration;

        otherBall.ApplyStatus(scaledPoison, parent);
    }

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        base.OnWeaponHit(otherWeapon);
        Debug.Log("HIT");
    }
}