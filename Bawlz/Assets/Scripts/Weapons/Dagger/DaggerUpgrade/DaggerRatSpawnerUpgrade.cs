using UnityEngine;

[CreateAssetMenu(fileName = "DaggerRatSpawnerUpgrade", menuName = "Weapon Upgrades/Dagger Upgrades/DaggerRatSpawnerUpgrade")]

public class DaggerRatSpawnerUpgrade : DaggerUpgrade
{
    [SerializeField ] GameObject ratSpawnerPrefab;
    public float duration = 2f;
    public int numRatsToSpawn = 5;
    public float spawnChance = 0.5f;

    public override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);

        if (Random.value > spawnChance) return;

        GameObject trapObject = Instantiate(ratSpawnerPrefab, otherBall.transform.position, Quaternion.identity);
        RatSpawner ratSpawner = trapObject.GetComponent<RatSpawner>();
        ratSpawner.trapDuration = duration;
        ratSpawner.numRatsToSpawn = numRatsToSpawn;
        ratSpawner.TrapInit(0, 0, parentBall);
    }
}
