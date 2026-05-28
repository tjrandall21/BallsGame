using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

//base weapon class to be extended for different weapon types
public class Weapon : MonoBehaviour
{
    public string weaponName = "Weapon";
    public string description = "This is a weapon.";
    [SerializeField] protected float damage = 10;
    public float Damage { get { return damage; } }
    [SerializeField] protected float knockbackSpeed = 0;
    public float KnockbackSpeed { get { return knockbackSpeed; } }
    [SerializeField] protected float attackCooldown = 0;
    protected float attackTimer = 0;
    protected Rigidbody2D rb = null;
    protected Collider2D collider = null;
    public BallController parent = null;
    [SerializeField] protected List<WeaponUpgrade> weaponUpgrades = new List<WeaponUpgrade>();
    public List<WeaponUpgrade> WeaponUpgrades { get { return weaponUpgrades; } }

    Dictionary<int, float> excludeDict = new Dictionary<int, float>();
    float layerExcludeDuration = 0.1f;
    



    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        parent = GetComponentInParent<BallController>();
        collider = GetComponent<Collider2D>();
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            weaponUpgrade.Init(parent, this);
            damage += weaponUpgrade.damage;
            knockbackSpeed += weaponUpgrade.knockbackSpeed;
            attackCooldown += weaponUpgrade.attackCooldown;
            attackCooldown *= weaponUpgrade.attackCooldownMultiplier;

            weaponUpgrade.OnRoundStart(); //need to move this eventually
        }
        attackTimer = attackCooldown;
    }

    public void SetUpgrades(List<WeaponUpgrade> newUpgrades)
    {
        foreach (WeaponUpgrade upgrade in newUpgrades)
        {
            weaponUpgrades.Add(Instantiate(upgrade));
        }
    }

    public void AddUpgrade(WeaponUpgrade newUpgrade)
    {
        weaponUpgrades.Add(Instantiate(newUpgrade));
    }

    protected virtual void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                OnAttack();
            }
        }
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            weaponUpgrade.Update();
        }
        List<int> includeLayerQueue = new List<int>();
        
        for (int i = 0; i < excludeDict.Count; i++)
        {
            KeyValuePair<int,float> pair = excludeDict.ElementAt(i);
            excludeDict[pair.Key] -= Time.deltaTime;
            if (excludeDict[pair.Key] <= 0)
            {
                includeLayerQueue.Add(pair.Key);
            }
        }
        foreach (int layer in includeLayerQueue)
        {
            IncludeLayer(layer);
        }
    }

    void ExcludeLayer(int layer)
    {
        if (!excludeDict.ContainsKey(layer))
        {
            excludeDict.Add(layer,layerExcludeDuration);
            collider.excludeLayers |= 1<<layer;
        }
    }
    void IncludeLayer(int layer)
    {
        excludeDict.Remove(layer);
        collider.excludeLayers &= ~(1 << layer);
    }

    protected virtual void OnBallHit(BallController otherBall)
    {
        if (parent != null)
        {
            parent.FlipRotation();
        }
        otherBall.OnWeaponCollision(this);
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            weaponUpgrade.OnBallHit(otherBall);
        }
        ExcludeLayer(otherBall.gameObject.layer);
    }
    protected virtual void OnWeaponHit(Weapon otherWeapon)
    {
        if (parent != null)
        {
            parent.FlipRotation();
        }
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            weaponUpgrade.OnWeaponHit(otherWeapon);
        }
        if (otherWeapon.gameObject.layer < 10)
        { 
            ExcludeLayer(otherWeapon.gameObject.layer);
        }
    }

    protected virtual void OnWallHit()
    {
        //called when the weapon hits a wall
    }

    protected virtual void OnAttack()
    {
        //this function can be overidden to create cooldown based attacks like projectile shots
        //when the attack is finished call OnAttackEnd to reset the cooldown
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            weaponUpgrade.OnAttack();
        }
    }

    public virtual void OnAttackEnd()
    {
        attackTimer = attackCooldown;
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            weaponUpgrade.OnAttackEnd();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        BallController ball = collision.GetComponent<BallController>();
        if (ball != null)
        {
            OnBallHit(ball);
            return;
        }
        Weapon otherWeapon = collision.GetComponent<Weapon>();
        if (otherWeapon != null)
        {
            OnWeaponHit(otherWeapon);
            return;
        }
        OnWallHit();
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        BallController ball = collision.collider.GetComponent<BallController>();
        if (ball != null)
        {
            OnBallHit(ball);
            return;
        }
        Weapon otherWeapon = collision.collider.GetComponent<Weapon>();
        if (otherWeapon != null)
        {
            OnWeaponHit(otherWeapon);
            return;
        }
        OnWallHit();
    }

    public virtual void OnRoundStart()
    {
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            weaponUpgrade.OnRoundStart();
        }
    }
    public virtual void OnBallSpawned(BallController newBall)
    {
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            weaponUpgrade.OnBallSpawned(newBall);
        }
    }
    public virtual void OnDamageTaken(float amount)
    {
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            weaponUpgrade.OnDamageTaken(amount);
        }
    }
    public virtual void OnWeaponCollision(Weapon weapon)
    { //called when the BALL is hit by another weapon
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            weaponUpgrade.OnWeaponCollision(weapon);
        }
    }
    public virtual void OnBallCollision(BallController otherBall)
    { //called when the BALL is hit by another ball
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            weaponUpgrade.OnBallCollision(otherBall);
        }
    }
    public virtual void OnWallCollision()
    { //called when the BALL hits a wall
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            weaponUpgrade.OnWallCollision();
        }
    }
    public virtual void OnMinionDeath(BallController minion)
    {
        //called when a minion or clone of this ball dies
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            weaponUpgrade.OnMinionDeath(minion);
        }
    }
}
