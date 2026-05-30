using System.Collections.Generic;
using UnityEngine;

public class RatSpawner : Trap
{
    [SerializeField] GameObject ratPrefab;
    public int numRatsToSpawn = 5;
    public float trapDuration = 2f;

    public override void OnTrapTriggered(BallController otherBall)
    {
        for(int i = 0; i < numRatsToSpawn; i++)
        {
            
            GameObject rat = Instantiate(ratPrefab, transform.position, Quaternion.identity);
            BallController ratController = rat.GetComponent<BallController>();
            ratController.Init(new List<Upgrade>(), 1, Random.Range(0.0f, 360.0f), ratController.sprite.sprite);
            ratController.contactDamage = damage;
            ratController.maxHealth = 1f;
            parent.OnBallSpawned(ratController);
        }
    }
}
