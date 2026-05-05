using UnityEngine;

public class Sceptre : Weapon
{
    [SerializeField] GameObject minionPrefab;
    [SerializeField] float minionDamageScaling = 2f;
    [SerializeField] float minionDamage = 2;
    [SerializeField] float minionHealth = 1;
    [SerializeField] float minionHealthScaling = 1.5f;
    [SerializeField] int minionsPerHit = 1;
    protected override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);
        for (int i = 0; i < minionsPerHit; i++)
        {       
            GameObject ball = Instantiate(minionPrefab, parent.transform.position, Quaternion.identity);
            ball.layer = gameObject.layer;

            BallController ballController = ball.GetComponent<BallController>();
            ballController.launchAngle = Random.Range(0.0f,360.0f);
            ballController.contactDamage = minionDamage;
            ballController.maxHealth = minionHealth;
            parent.OnBallSpawned(ballController);
        }

        minionDamage += minionDamageScaling;
        minionHealth += minionHealthScaling;
    }

    protected override void Start()
    {
        base.Start();
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is SceptreUpgrade)
            {
                SceptreUpgrade sceptreUpgrade = (SceptreUpgrade)weaponUpgrade;
                minionDamageScaling += sceptreUpgrade.minionDamageScaling;
                minionDamage += sceptreUpgrade.minionDamage;
                minionHealth += sceptreUpgrade.minionHealth;
                minionHealthScaling += sceptreUpgrade.minionHealthScaling;
                minionsPerHit += sceptreUpgrade.minionsPerHit;
            }
        }
    }
}
