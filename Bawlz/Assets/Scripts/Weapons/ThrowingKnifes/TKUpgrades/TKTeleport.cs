using UnityEngine;

[CreateAssetMenu(fileName = "TeleportUpgrade", menuName = "Weapon Upgrades/TK Upgrades/TeleportUpgrade")]
public class TKTeleport : TKUpgrade
{
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float teleportChance = 0.3f;
    [SerializeField] float teleportCooldown = 3f;
    [SerializeField] float explosionDamage = 5f;
    [SerializeField] float explosionKnockback = 10f;
    [SerializeField] float explosionSize = 1f;
    float teleportTimer = 0;

    public override void Init(BallController ball, Weapon weapon)
    {
        base.Init(ball, weapon);
        teleportTimer = teleportCooldown;
    }

    public override void Update()
    {
        base.Update();
        if (teleportTimer > 0)
        {
            teleportTimer -= Time.deltaTime;
        }
    }

    public override void OnProjectileTimeout(TKProdj projectile)
    {
        if (Random.value < teleportChance && teleportTimer <= 0 && parentBall != null)
        {
            GameObject explosionObject = Instantiate(explosionPrefab);
            explosionObject.layer = parentBall.gameObject.layer+4;
            Explosion explosion = explosionObject.GetComponent<Explosion>();
            explosion.ExplosionInit(explosionDamage,parentBall.transform.position,explosionKnockback,explosionSize);

            parentBall.transform.position = projectile.transform.position;
            teleportTimer = teleportCooldown;
        }

        base.OnProjectileTimeout(projectile);
    }
}