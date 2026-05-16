using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class BallController : MonoBehaviour
{
    static uint ballNum = 0;
    uint ballID = 0;
    public uint BallID { get { return ballID; } }
    Rigidbody2D rb = null;
    TextMeshPro healthText = null;
    [SerializeField] string ballName = "Ball";
    [SerializeField] public float speed = 6;
    [SerializeField] public float launchAngle = 45;
    [SerializeField] float rotationSpeed = 360;
    public float RotationSpeed {get{return rotationSpeed;}set{rotationSpeed = value;}}
    [SerializeField] float rotationDirection = -1;
    [SerializeField] public float maxHealth = 200;
    [SerializeField] public float contactDamage = 0;
    [SerializeField] float defenseMultiplier = 1;
    public float health = 0;
    [SerializeField] public SpriteRenderer sprite = null;

    public int playerNum = 0;

    public List<Weapon> weapons = new List<Weapon>();
    [SerializeField] List<Upgrade> upgrades = new List<Upgrade>();
    [SerializeField] List<StatusEffect> statusEffects;

    public void Init(List<Upgrade> newUpgrades, int playerNumber, float startingAngle,Sprite playerSprite)
    {
        foreach (Upgrade upgrade in newUpgrades)
        {
            upgrades.Add(Instantiate(upgrade));
        }
        playerNum = playerNumber;

        if (playerSprite != null)
        {
            sprite.sprite = playerSprite;
        }
           
        launchAngle = startingAngle;
    }
    

    void Start()
    {
        //assign each ball a unique number id
        ballNum++; //0 will never be assigned, it is left for weapons without a parent (projectiles, etc)
        ballID = ballNum;

        //load weapons
        Weapon[] weaponChildren = GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weaponChildren)
        {
            weapons.Add(weapon);
        }

        //load upgrades
        foreach (Upgrade upgrade in upgrades)
        {
            upgrade.Init(this);
            //add stats
            maxHealth += upgrade.health;
            speed += upgrade.moveSpeed;
            rotationSpeed += upgrade.rotationSpeed;
            contactDamage += upgrade.contactDamage;
            defenseMultiplier *= upgrade.defenseMultiplier;

            upgrade.OnRoundStart(); //Need to move this once a start round countdown is added
        }

        health = maxHealth;

        launchAngle = launchAngle * math.PI / 180.0f;
        rb = GetComponent<Rigidbody2D>();
        healthText = GetComponentInChildren<TextMeshPro>();
        SetVelocityAngle(launchAngle, speed);

        FXManager.Instance.RegisterPlayer(GetComponent<AudioSource>());
    }

    // Update is called once per frame
    void Update()
    {
        rb.angularVelocity = rotationDirection * rotationSpeed;
        foreach (Upgrade upgrade in upgrades.ToArray())
        {
            upgrade.Update();
        }
        foreach (StatusEffect statusEffect in statusEffects.ToArray())
        {
            statusEffect.Update();
        }
        healthText.text = math.ceil(health).ToString();
    }
    void LateUpdate()
    {
        sprite.transform.rotation = new Quaternion(0, 0, 0, 1);
    }


    public bool HasStatus(string statusName) // confirms if the ball has a status matching the name
    {
        foreach (StatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.name == statusName)
            {
                return true;
            }
        }
        return false;
    }

    public StatusEffect GetStatus(string statusName) // returns if the player has a status matching the name
    {
        foreach (StatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.name == statusName)
            {
                return statusEffect;
            }
        }
        return null;
    }

    public void ApplyStatus(StatusEffect status, BallController sourceBall)
    {
        if (!status.stackable)
        { //check for any active status with a matching name
            foreach (StatusEffect statusEffect in statusEffects)
            {
                if (statusEffect.name == status.name)
                {
                    statusEffect.OnStatusRefresh();
                    return;
                }
            }
        }
        StatusEffect newStatus = Instantiate(status);
        newStatus.Init(this,sourceBall);
        statusEffects.Add(newStatus);
    }

    public void RemoveStatus(StatusEffect status)
    {
        statusEffects.Remove(status);
    }

    public void AddVelocity(Vector2 velocity)
    {
        rb.linearVelocity += velocity;
    }


    float GetCurrentAngle()
    {
        return math.atan2(rb.linearVelocityX, rb.linearVelocityY);
    }

    public void FlipRotation()
    {
        rotationDirection *= -1;
    }

    public void SetVelocityAngle(float angle, float magnitude = -1)
    {
        if (magnitude == -1) //default to the current speed
        {
            magnitude = rb.linearVelocity.magnitude;
        }
        rb.linearVelocityX = math.sin(angle - transform.rotation.eulerAngles.z) * magnitude;
        rb.linearVelocityY = math.cos(angle - transform.rotation.eulerAngles.z) * magnitude;
    }
    
    public void SetVelocity(Vector2 velocity)
    {
        rb.linearVelocity = velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        BallController otherBall = collision.collider.GetComponent<BallController>();
        if (otherBall != null)
        {
            OnBallCollision(otherBall);
        }
        else if (collision.collider.GetComponent<Weapon>() == null)
        {
            OnWallCollision();
        }
    }


    public void OnWeaponCollision(Weapon otherWeapon) //this is called by the other weapon
    {
        foreach (Upgrade upgrade in upgrades)
        {
            upgrade.OnWeaponCollision(otherWeapon);
        }
        foreach (Weapon weapon in weapons)
        {
            weapon.OnWeaponCollision(otherWeapon);
        }
        foreach (StatusEffect statusEffect in statusEffects)
        {
            statusEffect.OnWeaponCollision(otherWeapon);
        }
        if (otherWeapon.Damage > 0)
        {
            OnDamageTaken(otherWeapon.Damage);
        }

        if (otherWeapon.KnockbackSpeed > 0)
        {
            float currentSpeed = rb.linearVelocity.magnitude;
            rb.linearVelocity = rb.linearVelocity.normalized * (currentSpeed + otherWeapon.KnockbackSpeed);
        }



        Debug.Log($"{ballName} health: {health}");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnBallSpawned(BallController newBall)
    {
        foreach (Upgrade upgrade in upgrades)
        {
            upgrade.OnBallSpawned(newBall);
        }
        foreach (Weapon weapon in weapons)
        {
            weapon.OnBallSpawned(newBall);
        }
        foreach (StatusEffect statusEffect in statusEffects)
        {
            statusEffect.OnBallSpawned(newBall);
        }
    }

    public void OnDamageTaken(float amount)
    {
        //FXManager.Instance.PlayPlayerHit(transform.position);

        amount *= defenseMultiplier;
        health -= amount;

        foreach (Upgrade upgrade in upgrades)
        {
            upgrade.OnDamageTaken(amount);
        }
        foreach (Weapon weapon in weapons)
        {
            weapon.OnDamageTaken(amount);
        }
        foreach (StatusEffect statusEffect in statusEffects)
        {
            statusEffect.OnDamageTaken(amount);
        }

        Debug.Log($"{ballName} health: {health}");
        if (health <= 0)
        {
            FXManager.Instance.PlayDeath(gameObject);
            Destroy(gameObject);
        }
    }

    void OnBallCollision(BallController otherBall)
    {
        FXManager.Instance.PlayPlayerHit(otherBall.transform.position);
        foreach (Upgrade upgrade in upgrades)
        {
            upgrade.OnBallCollision(otherBall);
        }
        foreach (Weapon weapon in weapons)
        {
            weapon.OnBallCollision(otherBall);
        }

        if (otherBall.contactDamage > 0)
        {
            OnDamageTaken(otherBall.contactDamage);
        }
        foreach (StatusEffect statusEffect in statusEffects)
        {
            statusEffect.OnBallCollision(otherBall);
        }
    }

    void OnWallCollision()
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed; //normalizes the velocity to the speed stat
        foreach (Upgrade upgrade in upgrades)
        {
            upgrade.OnWallCollision();
        }
        foreach (Weapon weapon in weapons)
        {
            weapon.OnWallCollision();
        }
        foreach (StatusEffect statusEffect in statusEffects)
        {
            statusEffect.OnWallCollision();
        }
    }

}
