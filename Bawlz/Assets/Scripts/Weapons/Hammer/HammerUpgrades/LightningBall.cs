using UnityEngine;

[CreateAssetMenu(fileName = "LightningBall", menuName = "Weapon Upgrades/Hammer Upgrades/LightningBall")]
public class LightningBall : HammerUpgrade
{
    [SerializeField] GameObject lightningBallPrefab;
    [SerializeField] float ballSpeed = 10f;
    [SerializeField] float ballDamage = 5f;
    [SerializeField] float ballSize = 1f;
    [SerializeField] float ballLifetime = 3f;

    public override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);
        if (lightningBallPrefab == null || parentWeapon == null) return;

        // choose a random direction (uniform around the circle)
        Vector2 dir = Random.insideUnitCircle.normalized;

        GameObject ball = Instantiate(lightningBallPrefab, parentWeapon.transform.position, Quaternion.identity);
        ball.layer = parentWeapon.gameObject.layer;
        ball.transform.localScale = Vector3.one * ballSize;

        Projectile proj = ball.GetComponent<Projectile>();
        if (proj == null) { Destroy(ball); return; }

        proj.lifetime = ballLifetime;
        proj.ProjectileInit(dir, ballSpeed, ballDamage, parentWeapon);
    }
}