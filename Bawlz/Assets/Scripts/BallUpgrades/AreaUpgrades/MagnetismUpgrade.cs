using UnityEngine;

[CreateAssetMenu(fileName = "MagnetismUpgrade", menuName = "Ball Upgrades/Area Upgrades/MagnetismUpgrade")]
public class MagnetismUpgrade : AreaUpgrade
{
    [SerializeField] float magnetismStrength = 5f;

    public override void BallUpdate(BallController otherBall)
    {
        base.BallUpdate(otherBall);

        Vector2 dirToSelf = (parentBall.transform.position - otherBall.transform.position).normalized;
        otherBall.AddVelocity(dirToSelf * magnetismStrength * Time.deltaTime);
    }
}
