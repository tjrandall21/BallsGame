using UnityEngine;

[CreateAssetMenu(fileName = "DaggerLandmineUpgrade", menuName = "Weapon Upgrades/Dagger Upgrades/DaggerLandmineUpgrade")]
public class DaggerLandmineUpgrade : DaggerUpgrade
{
    [SerializeField] GameObject landminePrefab;
    public float landmineDamage = 20f;
    public float landmineKnockback = 10f;
    public float landmineRadius = 1f;
    public float landmineDuration = 5f;
    public float spawnChance = 0.5f;
    public override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);

        if(Random.value > spawnChance) return;

        GameObject mineObject = Instantiate(landminePrefab, otherBall.transform.position, Quaternion.identity);
        Landmine landmine = mineObject.GetComponent<Landmine>();
        landmine.explosionDamage = landmineDamage;
        landmine.explosionKnockback = landmineKnockback;
        landmine.explosionRadius = landmineRadius;
        landmine.duration = landmineDuration;
        landmine.TrapInit(landmineDamage, landmineKnockback, parentBall);
        Debug.Log("[DaggerLandmineUpgrade] OnBallHit called!");
    }

}
