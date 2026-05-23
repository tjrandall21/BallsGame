using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MinionArrowUpgrade", menuName = "Weapon Upgrades/Bow Upgrades/MinionArrowUpgrade")]
public class MinionArrow : BowUpgrade
{
    [SerializeField] GameObject minionPrefab;
    [SerializeField] float minionDamage = 1;
    [SerializeField] float minionHealth = 1;
    [SerializeField] float spawnChance = 0.25f;

    public override void OnArrowBallHit(BallController otherBall, BowProjectile arrow)
    {
        if (Random.value < spawnChance)
        {    
            GameObject ball = Instantiate(minionPrefab, arrow.transform.position, Quaternion.identity);
            ball.layer = parentBall.gameObject.layer+4;

            BallController ballController = ball.GetComponent<BallController>();
            ballController.maxHealth = minionHealth;
            ballController.Init(new List<Upgrade>(),parentBall.playerNum,Random.Range(0.0f,360.0f));
            ballController.contactDamage = minionDamage;
            parentBall.OnBallSpawned(ballController);
        }
        base.OnArrowBallHit(otherBall, arrow);
    }
}
