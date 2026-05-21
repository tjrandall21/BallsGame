using UnityEngine;

[CreateAssetMenu(fileName = "StatusArrowUpgrade", menuName = "Weapon Upgrades/Bow Upgrades/StatusArrowUpgrade")]
public class StatusArrows : BowUpgrade
{
    [SerializeField] StatusEffect status;

    public override void OnArrowBallHit(BallController otherBall, BowProjectile arrow)
    {
        otherBall.ApplyStatus(status,parentBall);
        base.OnArrowBallHit(otherBall, arrow);
    }
}
