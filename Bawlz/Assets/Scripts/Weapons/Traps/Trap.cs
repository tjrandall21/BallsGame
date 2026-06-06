using UnityEngine;

public class Trap : Weapon
{
    protected bool triggered = false;
    public float duration = 10f;
    public float activationDelay = 0.5f;
    private float timer;
    private float activationTimer;
    protected bool isActive = false;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }

    public virtual void TrapInit(float trapDamage, float trapKnockback, BallController owner)
    {
        Debug.Log("[TrapInit] Called!");
        damage = trapDamage;
        knockbackSpeed = trapKnockback;
        parent = owner;
        gameObject.layer = owner.gameObject.layer;
        timer = duration;
        activationTimer = activationDelay;
    }

    protected override void Update()
    {
        if (!isActive)
        {
            activationTimer -= Time.deltaTime;
            if (activationTimer <= 0)
            {
                isActive = true;
                collider.enabled = true;
                Debug.Log("[Trap] Activated!");
            }
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            OnTrapTriggered(null);
            Destroy(gameObject);
        }
    }

    protected override void OnBallHit(BallController otherBall)
    {
        if (triggered || !isActive) return;
        if (otherBall.playerNum == parent.playerNum) return;

        triggered = true;
        OnTrapTriggered(otherBall);
        Destroy(gameObject);
    }

    public virtual void OnTrapTriggered(BallController otherBall)
    {
        base.OnBallHit(otherBall);
    }
}