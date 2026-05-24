using UnityEngine;

[CreateAssetMenu(fileName = "SwordLifeSteal", menuName = "Weapon Upgrades/Sword Upgrades/SwordLifeSteal")]
public class SwordLifeSteal : SwordUpgrade
{
    [SerializeField] float multipleOfDamageStolen = 0.01f;
    [SerializeField] float damagePerSecond = 2;
    [SerializeField] float tickRate = 0.5f;
    float dmgTimer = 0;

    public override void OnBallHit(BallController otherBall)
    {
        base.OnBallHit(otherBall);
        float hitDamage = ((Sword)parentWeapon).Damage;
        parentBall.health += hitDamage*multipleOfDamageStolen;
    }

    public override void Update()
    {
        base.Update();
        dmgTimer += Time.deltaTime;
        if (dmgTimer >= tickRate)
        {
            dmgTimer = 0;
            parentBall.OnDamageTaken(damagePerSecond*tickRate);
        }
    }
}
