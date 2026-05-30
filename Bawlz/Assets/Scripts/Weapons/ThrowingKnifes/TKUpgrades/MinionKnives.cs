using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Minion Knives", menuName = "Weapon Upgrades/TK Upgrades/Minion Knives")]
public class MinionKnives : TKUpgrade
{
    [SerializeField] GameObject minionPrefab;
    [SerializeField] float minionDamage = 5;
    [SerializeField] float minionHealth = 5;
    [SerializeField] float spawnChance = 0.25f;

    public override void OnProjectileTimeout(TKProdj projectile)
    {
        if (Random.value < spawnChance)
        {    
            GameObject ball = Instantiate(minionPrefab, projectile.transform.position, Quaternion.identity);
            ball.layer = parentBall.gameObject.layer+4;

            BallController ballController = ball.GetComponent<BallController>();
            ballController.maxHealth = minionHealth;
            ballController.contactDamage = minionDamage;
            ballController.Init(new List<Upgrade>(),parentBall.playerNum,Random.Range(0.0f,360.0f));
            GameManager.Instance.GetMainBallByNumber(parentBall.playerNum).OnBallSpawned(ballController);
        }

        base.OnProjectileTimeout(projectile);
    }
}
