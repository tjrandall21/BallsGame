using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CloneUpgrade", menuName = "Ball Upgrades/HPThreshold/CloneUpgrade")]

public class CloneUpgrade : Upgrade
{
    [SerializeField] GameObject clonePrefab = null;
    [SerializeField] [Range(0f, 1f)] float hpThreshold = 0.25f;

    private bool isBelowThreshold = false;

    public override void OnRoundStart()
    {
        base.OnRoundStart();
        isBelowThreshold = false;
    }

    public override void OnDamageTaken(float amount)
    {
        base.OnDamageTaken(amount);
        bool nowBelowThreshold = parentBall.health / parentBall.maxHealth <= hpThreshold;


        if(!isBelowThreshold && nowBelowThreshold)
        {
            isBelowThreshold = true;
            SpawnClone();
        }
        else if (!nowBelowThreshold)
        {
            isBelowThreshold = false;
        }
    }

    public void SpawnClone()
    {
        GameObject ball = Instantiate(clonePrefab, parentBall.transform.position, Quaternion.identity);
        ball.layer = parentBall.gameObject.layer;

        BallController ballController = ball.GetComponent<BallController>();
        ballController.Init(GameManager.Instance.players[parentBall.playerNum-1].upgrades, parentBall.playerNum, Random.Range(0.0f, 360.0f),GameManager.Instance.players[parentBall.playerNum-1].playerSprite);
        ballController.maxHealth = parentBall.health;
        ballController.RemoveUpgradeByFamily("Clone");
        
        if(GameManager.Instance.players[parentBall.playerNum-1].weaponPrefab != null)
        {
            GameObject weaponCopy = Instantiate(GameManager.Instance.players[parentBall.playerNum-1].weaponPrefab, ball.transform);
            weaponCopy.layer = ball.layer;
            weaponCopy.GetComponent<Weapon>().SetUpgrades(GameManager.Instance.players[parentBall.playerNum-1].weaponUpgrades);
        }

        GameManager.Instance.GetMainBallByNumber(parentBall.playerNum).OnBallSpawned(ballController);
    }
}
