using UnityEngine;

[CreateAssetMenu(fileName = "DoTEffect", menuName = "Status Effects/DoTEffect")]
public class DoTEffect : StatusEffect
{
    public float damagePerSecond = 5;
    public float tickRate = 0.2f;
    float tickTimer = 0;

    public override void OnStatusApplied()
    {
        base.OnStatusApplied();
        tickTimer = tickRate;
        FXManager.Instance.StartStatusLoop(SFX);
    }

    public override void OnStatusEnd()
    {
        base.OnStatusEnd();
        FXManager.Instance.StopStatusLoop();
    }

    public override void Update()
    {
        tickTimer -= Time.deltaTime;
        if (tickTimer <= 0)
        {
            appliedBall.OnDamageTaken(damagePerSecond * tickRate);
            PlayTickFX();
            tickTimer = tickRate;
        }
        base.Update();
    }
}