using UnityEngine;

public class CannonAirStrike : StaticDmgArea
{
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float explosionDamage = 20f;
    [SerializeField] float explosionKnockback = 30f;
    [SerializeField] float explosionSize = 1f;
    [SerializeField] float callInTime = 1.0f;
    Vector3 targetPOS;

    bool calledIn = false;

    public void AirStrikeInit(float dmg, float knockback, float size, float delay, Vector3 position)
    {
        explosionDamage = dmg;
        explosionKnockback = knockback;
        explosionSize = size;
        callInTime = delay;
        targetPOS = position;
    }

    protected override void Update()
    {
        base.Update();
        if (!calledIn)
        {
            callInTime -= Time.deltaTime;
            if (callInTime <= 0)
            {
                calledIn = true;
                CallInStrike();
            }
        }
    }

    public void CallInStrike()
    {
        if (explosionPrefab != null)
        {
            GameObject explosionObject = Instantiate(explosionPrefab);
            explosionObject.layer = gameObject.layer;
            Explosion explosion = explosionObject.GetComponent<Explosion>();
            explosion.ExplosionInit(explosionDamage, targetPOS, explosionKnockback, explosionSize);
        }

        Destroy(gameObject);
    }
}