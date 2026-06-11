using System.Collections.Generic;
using UnityEngine;

public class BallArea : MonoBehaviour
{
    AreaUpgrade upgrade = null;
    TKTeleport teleportUpgrade = null;

    public virtual void Init(AreaUpgrade areaUpgrade, float radius, int layer)
    {
        upgrade = areaUpgrade;
        SetRadius(radius);
        gameObject.layer = layer;
    }

    public virtual void Init(TKTeleport tkTeleportUpgrade, float radius, int layer)
    {
        teleportUpgrade = tkTeleportUpgrade;
        SetRadius(radius);
        gameObject.layer = layer;
    }

    public void SetRadius(float radius)
    {
        Vector2 scale = transform.localScale;
        scale.x = radius;
        scale.y = radius;
        transform.localScale = scale;
    }

    

    void OnTriggerEnter2D(Collider2D collision)
    {
        BallController otherBall = collision.GetComponent<BallController>();
        if (otherBall != null)
        {
            if (upgrade != null)
                upgrade.BallEnteredArea(otherBall);
            if (teleportUpgrade != null)
                teleportUpgrade.BallEnteredArea(otherBall);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        BallController otherBall = collision.GetComponent<BallController>();
        if (otherBall != null)
        {
            if (upgrade != null)
                upgrade.BallExitedArea(otherBall);
            if (teleportUpgrade != null)
                teleportUpgrade.BallEnteredArea(otherBall);
        }
    }
}
