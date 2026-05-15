using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionUpgrade", menuName = "Scriptable Objects/MinionUpgrade")]
public class MinionUpgrade : Upgrade
{
    [SerializeField] GameObject minionPrefab;
    [SerializeField] float minionDamage = 1;

    public override void OnWallCollision()
    {
        base.OnWallCollision();
        GameObject ball = Instantiate(minionPrefab, parentBall.transform.position, Quaternion.identity);
        ball.layer = parentBall.gameObject.layer;

        BallController ballController = ball.GetComponent<BallController>();
        ballController.Init(new List<Upgrade>(),parentBall.playerNum,Random.Range(0.0f,360.0f),ballController.sprite.sprite);
        ballController.contactDamage = minionDamage;
        parentBall.OnBallSpawned(ballController);
    }
}
