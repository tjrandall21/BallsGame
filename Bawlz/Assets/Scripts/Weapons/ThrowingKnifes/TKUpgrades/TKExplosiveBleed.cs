using UnityEngine;

[CreateAssetMenu(fileName = "ExplosiveBleedUpgrade", menuName = "Weapon Upgrades/TK Upgrades/ExplosiveBleed")]
public class TKExplosiveBleed : TKUpgrade
{
    [SerializeField] StatusEffect bleedEffect;
    [SerializeField] int stacks = 1;

    public override void OnProjectileBallHit(TKProdj projectile, BallController otherBall)
    {
        for (int i = 0; i < stacks; i++)
        {
            otherBall.ApplyStatus(bleedEffect,parentBall);            
        }
        base.OnProjectileBallHit(projectile, otherBall);
    }
    public override void OnBallHit(BallController otherBall)
    {
        for (int i = 0; i < stacks; i++)
        {
            otherBall.ApplyStatus(bleedEffect,parentBall);            
        }
        base.OnBallHit(otherBall);
    }

}