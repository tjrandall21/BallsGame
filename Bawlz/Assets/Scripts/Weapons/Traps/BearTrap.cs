using UnityEngine;

public class BearTrap : Trap
{
    [SerializeField] SlowEffect slowEffect;

    public override void OnTrapTriggered(BallController otherBall)
    {
        if (parent != null)
        {   
            SlowEffect slow = Instantiate(slowEffect);
            otherBall.ApplyStatus(slow, parent);
        }
    }
}
