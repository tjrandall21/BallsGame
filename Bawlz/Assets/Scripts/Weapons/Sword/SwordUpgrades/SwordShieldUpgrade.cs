using UnityEngine;

[CreateAssetMenu(fileName = "SwordShieldUpgrade", menuName = "Weapon Upgrades/Sword Upgrades/ShieldUpgrade")]
public class SwordShieldUpgrade : SwordUpgrade
{
    [SerializeField] float duration = 1;
    [SerializeField] float dmgMult = 1;
    StatChangeEffect buff;

    void OnEnable()
    {
        buff = new StatChangeEffect();
    }

    public override void OnRoundStart()
    {
        base.OnRoundStart();
        buff.statusDuration = duration;
        buff.defenseMultiplier = dmgMult;
        buff.name = "Sword Defense Buff";
        buff.stackable = false;
    }

    public override void OnBallHit(BallController otherBall)
    {
        parentBall.ApplyStatus(buff,parentBall);
        base.OnBallHit(otherBall);
    }
    
}
