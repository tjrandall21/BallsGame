using System.Collections.Generic;
using UnityEngine;

public class RatSpawner : Trap
{
    [SerializeField] GameObject ratPrefab;
    public int ratCount = 5;
    public float spawnInterval = 2f;

    private int ratsSpawned = 0;
    private float spawnTimer = 0f;
    private bool isSpawning = false;

    public override void TrapInit(float trapDamage, float trapKnockback, BallController owner)
    {
        triggered = true;
        isSpawning = true;
        spawnTimer = spawnInterval; 
        base.TrapInit(trapDamage, trapKnockback, owner);
    }

    protected override void Update()
    {
        base.Update();
        if (!isSpawning)
        {
            return;
        }
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0f && ratsSpawned < ratCount)
        {
            SpawnRat();
            ratsSpawned++;
            spawnTimer = spawnInterval;

            if(ratsSpawned >= ratCount)
            {
                isSpawning = false;
                Destroy(gameObject);
            }
        }
    }

    protected override void OnBallHit(BallController otherBall)
    {
        if (triggered || !isActive)
        {
            return;
        }
        if (otherBall.playerNum == parent.playerNum)
        {
            return;
        }
    }

    void SpawnRat()
    {
        Debug.Log($"[RatSpawner] Spawning rat, parent: {parent}");
        if (parent == null)
        {
            Debug.Log("[RatSpawner] Parent is null!");
            return;
        }

        GameObject rat = Instantiate(ratPrefab, transform.position, Quaternion.identity);
        BallController ratController = rat.GetComponent<BallController>();
        ratController.Init(new List<Upgrade>(), 1, Random.Range(0.0f, 360.0f), ratController.sprite.sprite);
        ratController.contactDamage = damage;
        ratController.maxHealth = 1f;
        rat.layer = 0;
    }
}
