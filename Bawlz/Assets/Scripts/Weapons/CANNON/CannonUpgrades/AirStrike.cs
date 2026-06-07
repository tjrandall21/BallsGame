using UnityEngine;

[CreateAssetMenu(fileName = "AirStrike", menuName = "Weapon Upgrades/Cannon Upgrades/AirStrike")]
public class AirStrike : CannonUpgrade
{
    [SerializeField] GameObject airStrikePrefab;
    [SerializeField] float airStrikeDamage = 20f;
    [SerializeField] float airStrikeKnockback = 30f;
    [SerializeField] float airStrikeExplosionSize = 1f;
    [SerializeField] float airStrikeDelay = 1f;
    [SerializeField] float airStrikeSize = 1.2f;
    float airStrikeCooldown = 2f;

    public override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);

        Vector3 pos = otherBall.transform.position;

        GameObject airStrikeObject = Instantiate(airStrikePrefab);
        airStrikeObject.layer = parentBall.gameObject.layer + 4;
        CannonAirStrike airStrike = airStrikeObject.GetComponent<CannonAirStrike>();
        airStrike.AirStrikeInit(airStrikeDamage, airStrikeKnockback, airStrikeExplosionSize, airStrikeDelay, otherBall.transform.position);
        airStrike.Init(0.0f, otherBall.transform.position, airStrikeDelay, airStrikeSize);
    }
}