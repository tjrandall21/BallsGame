using UnityEngine;

[CreateAssetMenu(fileName = "ContactStatus", menuName = "Ball Upgrades/Contact Upgrades/ContactStatusUpgrade")]
public class ContactStatusUpgrade : Upgrade
{
    [SerializeField] StatusEffect status;

    public override void OnBallCollision(BallController otherBall)
    {
        otherBall.ApplyStatus(status, parentBall);
        base.OnBallCollision(otherBall);
    }
}
