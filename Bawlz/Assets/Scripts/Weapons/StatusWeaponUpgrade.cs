using UnityEngine;

[CreateAssetMenu(fileName = "StatusWeaponUpgrade", menuName = "Weapon Upgrades/StatusWeaponUpgrade")]
public class StatusWeaponUpgrade : WeaponUpgrade
{
    [SerializeField] StatusEffect status;

    public override void OnBallHit(BallController otherBall)
    {
        otherBall.ApplyStatus(status,parentBall);
        base.OnBallHit(otherBall);
    }
}
