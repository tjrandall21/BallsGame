using UnityEngine;

[CreateAssetMenu(fileName = "ExplosiveUpgrade", menuName = "Weapon Upgrades/Hammer Upgrades/ExplosiveUpgrade")]
public class BigBoomUpgrade : HammerUpgrade
{
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float explosionDamage = 1000;
    [SerializeField] float explosionKnockback = 10;
    [SerializeField] float explosionSize = 10;
     
    public override void OnBallHit(BallController otherBall)
    {
        GameObject explosionObject = Instantiate(explosionPrefab);
        explosionObject.layer = parentBall.gameObject.layer;
        Explosion explosion = explosionObject.GetComponent<Explosion>();
        explosion.ExplosionInit(explosionDamage,otherBall.transform.position,explosionKnockback,explosionSize);
        base.OnBallHit(otherBall);
    }
}
