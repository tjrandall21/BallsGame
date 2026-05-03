using UnityEngine;

[CreateAssetMenu(fileName = "StatusWeaponUpgrade", menuName = "Scriptable Objects/StatusWeaponUpgrade")]
public class StatusWeaponUpgrade : WeaponUpgrade
{
    [SerializeField] StatusEffect status;

    public override void OnBallHit(BallController otherBall)
    {
        otherBall.ApplyStatus(status,parentBall);
        base.OnBallHit(otherBall);
    }
}
