using UnityEngine;
using Unity.Mathematics;

public class Harpoon : Weapon
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 30;
    [SerializeField] int projectileDamage = 4;
    [SerializeField] float contactDamageScaling = 1;
    float extraProjectileChance = 0;
    [SerializeField ]float extraProjectileChanceScaling = 0.3f;
    
    [SerializeField] float spreadIncreasePerBullet = 10;
    float maxSpread = 100;


    protected override void OnAttack()
    {
        float extraProjectiles = math.floor(extraProjectileChance);
        if (UnityEngine.Random.value <= extraProjectileChance - extraProjectiles)
            extraProjectiles++;
        float halfSpreadRadians = extraProjectiles * spreadIncreasePerBullet * (math.PI / 180.0f)  * 0.5f;
        halfSpreadRadians = math.min(halfSpreadRadians, maxSpread * 0.5f);
        for (int i = 0; i < 1 + extraProjectiles; i++)
        {
            float rotation = transform.eulerAngles.z * math.PI / 180.0f + math.PI * 0.5f;
            if (extraProjectiles > 0)
            {
                float spreadRatio = (i / extraProjectiles - 0.5f) * 2;
                rotation += spreadRatio * halfSpreadRadians;
            }
            GameObject projectileObject = Instantiate(projectilePrefab, transform.position, transform.rotation);
            HarpoonProjectile projectile = projectileObject.GetComponent<HarpoonProjectile>();
            projectile.ProjectileInit(new Vector3(math.cos(rotation), math.sin(rotation)), projectileSpeed, projectileDamage, this);
            projectileObject.layer = gameObject.layer;
        }

        OnAttackEnd();
    }

    public override void OnBallCollision(BallController otherBall)
    {   //increase the ball's contact damage on each ball collision
        base.OnBallCollision(otherBall);
        parent.contactDamage += contactDamageScaling;
        extraProjectileChance += extraProjectileChanceScaling;
    }

    public override void OnWallCollision()
    {
        base.OnWallCollision();
    }
}
