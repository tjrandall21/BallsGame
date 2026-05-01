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
    [SerializeField] float speed = 6;
    [SerializeField] float launchAngle = 45;
    [SerializeField] float rotationSpeed = 360;
    [SerializeField] float rotationDirection = -1;
    [SerializeField] float maxHealth = 200;
    [SerializeField] float contactDamage = 0;
    [SerializeField] float defenseMultiplier = 1;
    float health = 0;
    [SerializeField] SpriteRenderer sprite = null;
    public float RotationSpeed { get { return rotationSpeed; } }

    public int playerNum = 0;

    List<Weapon> weapons = new List<Weapon>();
    [SerializeField] List<Upgrade> upgrades = new List<Upgrade>();

    public void SetUpgrades(List<Upgrade> newUpgrades)
    {
        foreach (Upgrade upgrade in newUpgrades)
        {
            upgrades.Add(Instantiate(upgrade));
        }
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
    }

    // Update is called once per frame
    void Update()
    {
        rb.angularVelocity = rotationDirection * rotationSpeed;
        foreach (Upgrade upgrade in upgrades)
        {
            upgrade.Update();
        }
        healthText.text = math.ceil(health).ToString();
    }
    void LateUpdate()
    {
        sprite.transform.rotation = new Quaternion(0, 0, 0, 1);
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

    void SetVelocityAngle(float angle, float magnitude = 0)
    {
        if (magnitude == 0) //default to the current speed
        {
            magnitude = rb.linearVelocity.magnitude;
        }
        rb.linearVelocityX = math.sin(angle - transform.rotation.z) * magnitude;
        rb.linearVelocityY = math.cos(angle - transform.rotation.z) * magnitude;
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

    void OnBallSpawned()
    {
        foreach (Upgrade upgrade in upgrades)
        {
            upgrade.OnBallSpawned();
        }
        foreach (Weapon weapon in weapons)
        {
            weapon.OnBallSpawned();
        }
    }

    void OnDamageTaken(float amount)
    {
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

        Debug.Log($"{ballName} health: {health}");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnBallCollision(BallController otherBall)
    {
        foreach (Upgrade upgrade in upgrades)
        {
            upgrade.OnBallCollision(otherBall);
        }

        if (otherBall.contactDamage > 0)
        {
            OnDamageTaken(otherBall.contactDamage);
        }
    }

    void OnWallCollision()
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed; //normalizes the velocity to the speed stat
        foreach (Upgrade upgrade in upgrades)
        {
            upgrade.OnWallCollision();
        }
    }

}
