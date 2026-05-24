using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Status Effects/StatChangeEffect")]
public class StatChangeEffect : StatusEffect
{
    public float contactDamage = 0; 
    public float moveSpeed = 0;
    public float rotationSpeed = 0; //In degrees
    public float defenseMultiplier = 1;

    public override void OnStatusApplied()
    {
        base.OnStatusApplied();
        appliedBall.contactDamage += contactDamage;
        appliedBall.speed += moveSpeed;
        appliedBall.RotationSpeed += rotationSpeed;
        appliedBall.defenseMultiplier *= defenseMultiplier;
    }
    public override void OnStatusEnd()
    {
        base.OnStatusEnd();
        appliedBall.contactDamage -= contactDamage;
        appliedBall.speed -= moveSpeed;
        appliedBall.RotationSpeed -= rotationSpeed;
        appliedBall.defenseMultiplier /= defenseMultiplier;
    }

    public override void OnStatusRefresh()
    {
        base.OnStatusRefresh();
        statusTimer = statusDuration;
    }
}
