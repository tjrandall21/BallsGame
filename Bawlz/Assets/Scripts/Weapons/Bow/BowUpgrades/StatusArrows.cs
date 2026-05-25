using UnityEngine;

[CreateAssetMenu(fileName = "StatusArrowUpgrade", menuName = "Weapon Upgrades/Bow Upgrades/StatusArrowUpgrade")]
public class StatusArrows : BowUpgrade
{
    [SerializeField] StatusEffect status;
    [SerializeField] int stacks = 1;

    public override void Init(BallController ball, Weapon weapon)
    {
        base.Init(ball, weapon);
        status = Instantiate(status);
        status.statusName += ball.gameObject.layer.ToString();
    }

    public override void OnArrowBallHit(BallController otherBall, BowProjectile arrow)
    {
        for (int i = 0; i < stacks; i++)
        {
            otherBall.ApplyStatus(status,parentBall);
        }
        base.OnArrowBallHit(otherBall, arrow);
    }
}
