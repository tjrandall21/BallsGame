using UnityEngine;

[CreateAssetMenu(fileName = "DamageOvertimeUpgrade", menuName = "Ball Upgrades/Area Upgrades/DamageOvertimeUpgrade")]
public class DamageOvertimeUpgrade : AreaUpgrade
{
    [SerializeField] float damagePerSecond = 0.5f;

    public override void BallUpdate(BallController otherBall)
    {
        base.BallUpdate(otherBall);
        otherBall.OnDamageTaken(damagePerSecond * Time.deltaTime);
    }
}
