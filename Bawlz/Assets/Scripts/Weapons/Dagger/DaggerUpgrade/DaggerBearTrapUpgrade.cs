using UnityEngine;

[CreateAssetMenu(fileName = "DaggerBearTrapUpgrade", menuName = "Weapon Upgrades/Dagger Upgrades/DaggerBearTrapUpgrade")]
public class DaggerBearTrapUpgrade : DaggerUpgrade
{
    [SerializeField] GameObject bearTrapPrefab;
    public float duration = 10f;
    public float spawnChance = 0.5f;

    public override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);

        if (Random.value > spawnChance) return;

        GameObject trapObject = Instantiate(bearTrapPrefab, otherBall.transform.position, Quaternion.identity);
        BearTrap bearTrap = trapObject.GetComponent<BearTrap>();
        bearTrap.duration = duration;
        bearTrap.TrapInit(0, 0, parentBall);
    }
}
