using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightningBall", menuName = "Weapon Upgrades/Hammer Upgrades/LightningBall")]
public class LightningBall : HammerUpgrade
{
    [SerializeField] GameObject lightningBallPrefab; // should be a Ball prefab with BallController
    [SerializeField] float minionSpeed = 6f;
    [SerializeField] float minionContactDamage = 5f;
    [SerializeField] float minionHealth = 1f;
    [SerializeField] float minionSize = 1f;

    public override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);
        GameObject ball = Instantiate(lightningBallPrefab, otherBall.transform.position, Quaternion.identity);
        ball.layer = parentBall.gameObject.layer + 4;

        BallController bc = ball.GetComponent<BallController>();
 
        bc.contactDamage = 5; // change ltr
        bc.maxHealth = minionHealth;
        bc.Init(new List<Upgrade>(), parentBall.playerNum, Random.Range(0.0f, 360.0f));
        GameManager.Instance.GetMainBallByNumber(parentBall.playerNum).OnBallSpawned(bc);

    }
    
}