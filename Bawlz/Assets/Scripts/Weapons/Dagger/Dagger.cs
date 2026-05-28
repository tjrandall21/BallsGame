using UnityEngine;

public class Dagger : Weapon
{
    [SerializeField] private DoTEffect poisonEffect;
    public float durationScaling = 0.3f;
    public float damageScaling = 0.3f;


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
        FXManager.Instance.PlayPlayerHit(otherBall.transform.position);
        Debug.Log("Ball Collision");

        if (parent != null)
            parent.FlipRotation();

        otherBall.OnWeaponCollision(this);

        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
            weaponUpgrade.OnBallHit(otherBall);

        DoTEffect scaledPoison = Instantiate(poisonEffect);
        scaledPoison.damagePerSecond += damageScaling;
        scaledPoison.statusDuration += durationScaling;

        otherBall.ApplyStatus(scaledPoison, parent);
    }

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        FXManager.Instance.PlayWeaponHit(otherWeapon.transform.position);
        base.OnWeaponHit(otherWeapon);
    }
}