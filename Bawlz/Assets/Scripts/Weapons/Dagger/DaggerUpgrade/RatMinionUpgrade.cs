using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RatMinionUpgrade", menuName = "Weapon Upgrades/Dagger Upgrades/RatMinionUpgrade")]
public class RatMinionUpgrade : DaggerUpgrade
{
    [SerializeField] GameObject ratMinionPrefab;
    [SerializeField] float ratDamage = 1f;
    [SerializeField] float ratHealth = 10f;

    [SerializeField] DoTEffect ratPoisonEffect;
    public float durationScaling = 0.3f;
    public float damageScaling = 0.3f;

    public override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);
        
        GameObject rat = Instantiate(ratMinionPrefab, otherBall.transform.position, Quaternion.identity);
        rat.layer = parentBall.gameObject.layer + 4;

        BallController ratController = rat.GetComponent<BallController>();
        ratController.Init(new List<Upgrade>(), 1, Random.Range(0.0f, 360.0f), ratController.sprite.sprite);
        ratController.contactDamage = ratDamage;
        ratController.maxHealth = ratHealth;
    }


}
