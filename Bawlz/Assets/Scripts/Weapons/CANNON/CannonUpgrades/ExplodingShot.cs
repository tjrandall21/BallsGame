using UnityEngine;

[CreateAssetMenu(fileName = "ExplodingShotUpgrade", menuName = "Weapon Upgrades/Cannon Upgrades/ExplodingShotUpgrade")]
public class ExplodingShot : CannonUpgrade
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float explosionDamage = 10;
    [SerializeField] float explosionKnockback = 30;
    [SerializeField] float explosionSize = 1;
    [SerializeField] float grapeShotExplosionDamage = 5;
    [SerializeField] float grapeShotExplosionKnockback = 30;
    [SerializeField] float grapeShotExplosionSize = 0.5f;
    

    public override void OnProjectileDestroyed(Projectile projectile)
    {
     

        if (projectile.tag == "Grape Shot")
        {
            GameObject explosionObject = Instantiate(explosionPrefab);
            explosionObject.layer = projectile.parentWeapon.gameObject.layer + 4;
            Explosion explosion = explosionObject.GetComponent<Explosion>();
            explosion.ExplosionInit(grapeShotExplosionDamage, projectile.transform.position, grapeShotExplosionKnockback, grapeShotExplosionSize);
        }
        else
        {
          
            GameObject explosionObject = Instantiate(explosionPrefab);
            explosionObject.layer = projectile.parentWeapon.gameObject.layer + 4;
            Explosion explosion = explosionObject.GetComponent<Explosion>();
            explosion.ExplosionInit(explosionDamage, projectile.transform.position, explosionKnockback, explosionSize);
        }
        base.OnProjectileDestroyed(projectile);
    }

    public override void OnMinionDeath(BallController minion)
    {
        if (minion.tag == "Cannon Minion")
        {
            GameObject explosionObject = Instantiate(explosionPrefab);
            explosionObject.layer = parentWeapon.gameObject.layer + 4;
            Explosion explosion = explosionObject.GetComponent<Explosion>();
            explosion.ExplosionInit(explosionDamage, minion.transform.position, explosionKnockback, explosionSize);
        }
        else if (minion.tag == "Grape Minion")
        {
            GameObject explosionObject = Instantiate(explosionPrefab);
            explosionObject.layer = parentWeapon.gameObject.layer + 4;
            Explosion explosion = explosionObject.GetComponent<Explosion>();
            explosion.ExplosionInit(grapeShotExplosionDamage, minion.transform.position, grapeShotExplosionKnockback, grapeShotExplosionSize);
        }
        base.OnMinionDeath(minion);
    }
}
