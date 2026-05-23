using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionUpgrade", menuName = "Ball Upgrades/MinionUpgrade")]
public class MinionUpgrade : Upgrade
{
    [SerializeField] GameObject minionPrefab;
    [SerializeField] float minionDamage = 1;
    [SerializeField] float spawnChance = 0.25f;
    public override void OnWallCollision()
    {
        base.OnWallCollision();
        if (Random.value < spawnChance)
        {    
            GameObject ball = Instantiate(minionPrefab, parentBall.transform.position, Quaternion.identity);
            ball.layer = parentBall.gameObject.layer+4;

            BallController ballController = ball.GetComponent<BallController>();
            ballController.Init(new List<Upgrade>(),parentBall.playerNum,Random.Range(0.0f,360.0f));
            ballController.contactDamage = minionDamage;
            parentBall.OnBallSpawned(ballController);
        }
    }
}
