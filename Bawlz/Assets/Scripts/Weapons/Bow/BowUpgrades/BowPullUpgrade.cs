using UnityEngine;

[CreateAssetMenu(fileName = "BowPullUpgrade", menuName = "Weapon Upgrades/Bow Upgrades/BowPullUpgrade")]
public class BowPullUpgrade : BowUpgrade
{
    [SerializeField] float pullSpeed = 30;
    [SerializeField] float contactDamage = 5;
    [SerializeField] float contactDamageScaling = 3; //gets added to ball contact damage when your arrows hit

    public override void OnArrowBallHit(BallController otherBall, BowProjectile arrow)
    {
        Vector2 destination = parentWeapon.transform.position;
        Vector2 direction = (destination - (Vector2)arrow.transform.position).normalized;
        otherBall.SetVelocity(direction * pullSpeed);
        parentBall.contactDamage += contactDamageScaling;
        base.OnArrowBallHit(otherBall, arrow);
    }

    public override void Init(BallController ball, Weapon weapon)
    {
        base.Init(ball, weapon);
        parentBall.contactDamage += contactDamage;
    }

}
