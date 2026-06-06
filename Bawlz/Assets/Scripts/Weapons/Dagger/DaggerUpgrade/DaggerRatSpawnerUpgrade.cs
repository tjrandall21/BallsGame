using UnityEngine;

[CreateAssetMenu(fileName = "DaggerRatSpawnerUpgrade", menuName = "Weapon Upgrades/Dagger Upgrades/DaggerRatSpawnerUpgrade")]

public class DaggerRatSpawnerUpgrade : DaggerUpgrade
{
    [SerializeField ] GameObject ratSpawnerPrefab;
    public int numRatsToSpawn = 5;
    public float ratDamage = 1f;
    public float ratInterval = 2f;
    float trapDuration = 20f;
    public float spawnChance = 0.5f;
    public float spawnCooldown = 2f;
    float spawnTimer = 0f;



    public override void Update()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }
        base.Update();
    }

    public override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);
        if (spawnTimer<=0 && Random.value < spawnChance)
        {
            GameObject trapObject = Instantiate(ratSpawnerPrefab, otherBall.transform.position, Quaternion.identity);
            RatSpawner ratSpawner = trapObject.GetComponent<RatSpawner>();
            ratSpawner.Damage = ratDamage;
            ratSpawner.ratCount = numRatsToSpawn;
            ratSpawner.spawnInterval = ratInterval;
            ratSpawner.duration = trapDuration;
            ratSpawner.TrapInit(0, 0, parentBall);
        }
    }
}
