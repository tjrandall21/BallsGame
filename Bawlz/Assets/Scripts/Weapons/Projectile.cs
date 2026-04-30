using UnityEngine;


public class Projectile : Weapon
{
    public Vector3 direction = Vector3.zero;
    public float speed = 0;
    protected override void Update()
    {
        base.Update();
        Vector3 pos = transform.position;
        pos += direction * speed * Time.deltaTime;
        transform.position = pos;
    }
    protected override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);
        Destroy(gameObject);
    }
    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        base.OnWeaponHit(otherWeapon);
        Destroy(gameObject);
    }
    protected override void OnWallHit()
    {
        base.OnWallHit();
        Destroy(gameObject);
    }
}
