using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "BigArrowUpgrade", menuName = "Weapon Upgrades/Bow Upgrades/BigArrowUpgrade")]
public class BigArrowUpgrade : BowUpgrade
{
    [SerializeField] bool explodingBigArrows = false;
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float explosionDamage = 10;
    [SerializeField] float explosionKnockback = 30;
    [SerializeField] float explosionSize = 1;

    [SerializeField] float bigArrowDamageMultiple = 3; //how many times the base arrow damage the big arrow will do
    [SerializeField] float bigArrowSpeed = 8;
    [SerializeField] float bigArrowCooldown = 2.2f;
    [SerializeField] float bigArrowSize = 3;
    [SerializeField] GameObject arrowPrefab;

    

    float bigArrowTimer = 0;

    public override void Init(BallController ball, Weapon weapon)
    {
        base.Init(ball, weapon);
        bigArrowTimer = bigArrowCooldown;
    }

    public override void Update()
    {
        base.Update();
        if (bigArrowTimer <= 0)
        {
            ShootBigArrow();
            bigArrowTimer = bigArrowCooldown;
        }
        else
        {
            bigArrowTimer -= Time.deltaTime;
        }

    }

    void ShootBigArrow()
    {
        float rotation = parentBall.transform.eulerAngles.z * math.PI / 180.0f + math.PI * 0.5f;
        quaternion arrowRotation = quaternion.Euler(0,0,rotation-math.PI/2);
        GameObject projectileObject = Instantiate(arrowPrefab, parentBall.transform.position, arrowRotation);
        BowProjectile projectile = projectileObject.GetComponent<BowProjectile>();
        projectile.ProjectileInit(new Vector3(math.cos(rotation), math.sin(rotation)), bigArrowSpeed, ((Bow)parentWeapon).arrowDamage, parentWeapon);
        projectileObject.layer = parentBall.gameObject.layer;
        projectileObject.tag = "Big Arrow";
        projectile.transform.localScale = new Vector3(bigArrowSize,bigArrowSize,1);
    }

    public override void OnArrowDestroyed(BowProjectile arrow)
    {
        if (explodingBigArrows && arrow.tag == "Big Arrow")
        {
            GameObject explosionObject = Instantiate(explosionPrefab);
            explosionObject.layer = arrow.gameObject.layer;
            Explosion explosion = explosionObject.GetComponent<Explosion>();
            explosion.ExplosionInit(explosionDamage,arrow.transform.position,explosionKnockback,explosionSize);
        }
        base.OnArrowDestroyed(arrow);
    }
}
