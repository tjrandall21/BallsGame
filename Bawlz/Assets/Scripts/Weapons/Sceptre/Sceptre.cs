using System.Collections.Generic;
using UnityEngine;

public class Sceptre : Weapon
{
    [SerializeField] GameObject minionPrefab;
    [SerializeField] float minionDamageScaling = 2f;
    [SerializeField] float minionDamage = 2;
    [SerializeField] float minionHealth = 1;
    [SerializeField] float minionHealthScaling = 1.5f;
    [SerializeField] int minionsPerHit = 1;
    [SerializeField] float extraMinionChance = 0;

    protected override void OnBallHit(BallController otherBall)
    {
        FXManager.Instance.PlayPlayerHit(otherBall.transform.position);


        int minionAmount = minionsPerHit;
        if (Random.value < extraMinionChance)
        {
            minionAmount++;
        }
        base.OnBallHit(otherBall);
        for (int i = 0; i < minionAmount; i++)
        {       
            GameObject ball = Instantiate(minionPrefab, parent.transform.position, Quaternion.identity);
            ball.layer = gameObject.layer+4;

            BallController ballController = ball.GetComponent<BallController>();
            ballController.Init(new List<Upgrade>(),parent.playerNum,Random.Range(0.0f,360.0f));
            ballController.contactDamage = minionDamage;
            ballController.maxHealth = minionHealth;
            parent.OnBallSpawned(ballController);
        }

        minionDamage += minionDamageScaling;
        minionHealth += minionHealthScaling;
    }

    

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        FXManager.Instance.PlayWeaponHit(otherWeapon.transform.position);
        base.OnWeaponHit(otherWeapon);
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
                extraMinionChance += sceptreUpgrade.extraMinionChance;
            }
        }
    }
}
