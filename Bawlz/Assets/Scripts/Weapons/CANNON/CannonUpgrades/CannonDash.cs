using UnityEngine;

[CreateAssetMenu(fileName = "GuardedRecoil", menuName = "Weapon Upgrades/Cannon Upgrades/GuardedRecoil")]
public class CannonDash : CannonUpgrade
{
    [SerializeField] float damageReductionAmount;
    [SerializeField] float contactDamageAmount;
    [SerializeField] StatChangeEffect buff;
    bool queueRemoveStatus = false;

    public override void Init(BallController ball, Weapon weapon)
    {
        base.Init(ball, weapon);
        buff = Instantiate(buff);
        buff.contactDamage = contactDamageAmount;
        buff.defenseMultiplier = damageReductionAmount;
    }

    public override void Update()
    {
        base.Update();
        if (queueRemoveStatus)
        {
            queueRemoveStatus = false;
            parentBall.RemoveStatusByName("CannonRecoilBuff");
        }
    }

    public override void OnAttack()
    {
        base.OnAttack();
        parentBall.ApplyStatus(buff,parentBall);
    }

    public override void OnWallCollision()
    {
        base.OnWallCollision();
        queueRemoveStatus = true;
    }
    public override void OnBallCollision(BallController otherBall)
    {
        base.OnBallCollision(otherBall);
        queueRemoveStatus = true;
    }
}
