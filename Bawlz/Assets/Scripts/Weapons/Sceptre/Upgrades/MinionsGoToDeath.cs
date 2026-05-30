using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinionsToPos", menuName = "Weapon Upgrades/Sceptre Upgrades/MinionsToPos")]
public class CoordinatedMinions: SceptreDamageScaling
{
    [SerializeField] float attackInterval = 3;
    [SerializeField] float attackSpeed = 20;
    float attackTimer = 0;
    [SerializeField] bool defend = false;
    [SerializeField] bool attackDeathPos = false;

    public override void Init(BallController ball, Weapon weapon)
    {
        base.Init(ball, weapon);
        attackTimer = attackInterval;
    }

    public override void Update()
    {
        base.Update();
        attackTimer-=Time.deltaTime;
        if (attackTimer <= 0 && GameManager.Instance.GetMainBallCount()>1)
        {
            List<BallController> balls = new List<BallController>();
            foreach (BallController ball in GameManager.Instance.GetMainBalls())
            {
                if (ball.playerNum != parentBall.playerNum)
                {
                    balls.Add(ball);
                }
            }
            if (balls.Count != 0)
            {
                parentBall.SendMinionsTo(balls[Random.Range(0,balls.Count)].transform.position, attackSpeed);
            }
            attackTimer = attackInterval;
        }
    }

    public override void OnMinionDeath(BallController minion)
    {
        if (attackDeathPos)
        {
            parentBall.SendMinionsTo(minion.transform.position);
        }
        base.OnMinionDeath(minion);
    }
    public override void OnWeaponCollision(Weapon otherWeapon)
    {
        base.OnWeaponCollision(otherWeapon);
        parentBall.SendMinionsTo(parentBall.transform.position);
    }
    
}
