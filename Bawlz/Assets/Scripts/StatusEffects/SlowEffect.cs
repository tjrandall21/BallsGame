using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Status Effects/SlowEffect")]
public class SlowEffect : StatusEffect
{
    [SerializeField] public float slowMultiplier = 0.5f;

    public override void OnStatusApplied()
    {
        base.OnStatusApplied();
        appliedBall.speed *= slowMultiplier;
        appliedBall.SetVelocity(appliedBall.GetComponent<Rigidbody2D>().linearVelocity.normalized * appliedBall.speed);
    }

    public override void OnStatusEnd()
    {
        base.OnStatusEnd();
        appliedBall.speed /= slowMultiplier;
    }

    public override void OnStatusRefresh()
    {
        base.OnStatusRefresh();
        statusTimer = statusDuration;
    }

}
