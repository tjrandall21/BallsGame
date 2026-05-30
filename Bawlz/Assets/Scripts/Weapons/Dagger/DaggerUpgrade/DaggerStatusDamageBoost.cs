using UnityEngine;

[CreateAssetMenu(fileName = "DaggerStatusDamageBoost", menuName = "Weapon Upgrades/Dagger Upgrades/DaggerStatusDamageBoost")]
public class DaggerStatusDamageBoost : DaggerUpgrade
{
    [SerializeField] float damageBoostAmount = 20;

    public override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);
        if(parentBall.HasAnyStatus())
        {
            otherBall.OnDamageTaken(damageBoostAmount);
            Debug.Log($"[Status Damage Boost] Dealt an additional {damageBoostAmount} damage to {otherBall.name} due to status effects!");
        }
    }
}
