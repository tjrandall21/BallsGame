using UnityEngine;

[CreateAssetMenu(fileName = "DaggerSlowUpgrade", menuName = "Weapon Upgrades/Dagger Upgrades/DaggerSlowUpgrade")]
public class DaggerSlowUpgrade : DaggerUpgrade
{
    [SerializeField] SlowEffect slowEffect;

    public override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);
        otherBall.ApplyStatus(slowEffect, parentBall);
    }
}
