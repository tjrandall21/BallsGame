using UnityEngine;

[CreateAssetMenu(fileName = "ArrowBleedEffect", menuName = "Status Effects/ArrowBleedEffect")]
public class ArrowBleedEffect : DoTEffect
{
    int stacks = 1;
    [SerializeField] float baseDPS = 1.5f;
    [SerializeField] int stackThreshold = 10;
    [SerializeField] bool explodeAtThreshold = false;
    [SerializeField] float explosionDmgMultiplier = 3f;
    [SerializeField] float explosionSize = 1.8f;

    [SerializeField] GameObject bloodExplosionPrefab;
    public override void Init(BallController appliedBall, BallController sourceBall)
    {
        base.Init(appliedBall, sourceBall);
        damagePerSecond = baseDPS;
    }

    public override void OnStatusRefresh()
    {
        Debug.Log("bleed refresh");
        base.OnStatusRefresh();
        stacks++;
        damagePerSecond = baseDPS*stacks;
        statusTimer = statusDuration;

        if (stacks >= stackThreshold && explodeAtThreshold && sourceBall != null)
        {
            stacks -= stackThreshold;

            float explosionDamage = damagePerSecond * explosionDmgMultiplier;
            float explosionKnockback = explosionDamage/2f;
            

            GameObject explosionObject = Instantiate(bloodExplosionPrefab);
            explosionObject.layer = sourceBall.gameObject.layer+4;
            Explosion explosion = explosionObject.GetComponent<Explosion>();
            explosion.ExplosionInit(explosionDamage,appliedBall.transform.position,explosionKnockback,explosionSize);

            OnStatusEnd();
            appliedBall.RemoveStatus(this);
        }
    }

}
