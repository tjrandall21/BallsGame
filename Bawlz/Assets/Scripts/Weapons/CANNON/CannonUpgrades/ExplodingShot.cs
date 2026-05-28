using UnityEngine;

[CreateAssetMenu(fileName = "ExplodingShotUpgrade", menuName = "Weapon Upgrades/Cannon Upgrades/ExplodingShotUpgrade")]
public class ExplodingShot : CannonUpgrade
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject explosionPrefab;

    [SerializeField] float explosionDamage = 10;
    [SerializeField] float explosionKnockback = 30;
    [SerializeField] float explosionSize = 1;

    public override void OnProjectileDestroyed(Projectile projectile)
    {
        GameObject explosionObject = Instantiate(explosionPrefab);
        explosionObject.layer = projectile.gameObject.layer;
        Explosion explosion = explosionObject.GetComponent<Explosion>();
        explosion.ExplosionInit(explosionDamage, projectile.transform.position, explosionKnockback, explosionSize);
        Debug.Log("Explosion created at " + projectile.transform.position);
        base.OnProjectileDestroyed(projectile);
    }

    public override void OnMinionDeath(Vector3 position)
    {
        GameObject explosionObject = Instantiate(explosionPrefab);
        explosionObject.layer = parentWeapon.gameObject.layer + 4;
        Explosion explosion = explosionObject.GetComponent<Explosion>();
        explosion.ExplosionInit(explosionDamage, position, explosionKnockback, explosionSize);
        base.OnMinionDeath(position);
    }
}
