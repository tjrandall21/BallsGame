using UnityEngine;

public class Landmine : Trap
{
    [SerializeField] GameObject explosionEffectPrefab;
    public float explosionDamage = 20f;
    public float explosionKnockback = 10f;
    public float explosionRadius = 1f;

    public override void OnTrapTriggered(BallController otherBall)
    {
        GameObject explosionObject = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        explosionObject.layer = gameObject.layer;
        Explosion explosion = explosionObject.GetComponent<Explosion>();
        explosion.ExplosionInit(explosionDamage, transform.position, explosionKnockback, explosionRadius);
    }
}
