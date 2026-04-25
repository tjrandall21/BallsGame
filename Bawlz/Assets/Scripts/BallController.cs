using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class BallController : MonoBehaviour
{
    static uint ballNum = 0;
    uint ballID = 0;
    public uint BallID {get{return ballID;}}
    Rigidbody2D rb = null;
    [SerializeField] string ballName = "Ball";
    [SerializeField] float speed = 6;
    [SerializeField] float launchAngle = 45;
    [SerializeField] float acceleration = 5;
    [SerializeField] float rotationSpeed = -360;
    [SerializeField] float maxHealth = 200;
    float health = 0;
    public float RotationSpeed {get{return rotationSpeed;}}
    [SerializeField] int playerNum = 1;
    List<Weapon> weapons = new List<Weapon>();


    void Awake()
    {
        //assign each ball a unique number id
        ballNum++; //0 will never be assigned, it is left for weapons without a parent (projectiles, etc)
        ballID = ballNum; 

        health = maxHealth;

        Weapon[] weaponChildren = GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weaponChildren)
        {
            weapons.Add(weapon);
        }
        launchAngle = launchAngle*math.PI/180.0f;
        rb = GetComponent<Rigidbody2D>();
        SetVelocityAngle(launchAngle,speed);
    }

    // Update is called once per frame
    void Update()
    {
        rb.angularVelocity = rotationSpeed;
    }

    float GetCurrentAngle()
    {
        return math.atan2(rb.linearVelocityX,rb.linearVelocityY);
    }

    public void FlipRotation()
    {
        rotationSpeed *= -1;
    }

    void SetVelocityAngle(float angle, float magnitude = 0)
    {
        if (magnitude == 0) //default to the current speed
        {
            magnitude = rb.linearVelocity.magnitude;
        }
        rb.linearVelocityX = math.sin(angle-transform.rotation.z) * magnitude;
        rb.linearVelocityY = math.cos(angle-transform.rotation.z) * magnitude;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<BallController>() == null && collision.collider.GetComponent<Weapon>() == null)
        {
            rb.linearVelocity = rb.linearVelocity.normalized*speed; //normalizes the velocity
        }
    }


    public void OnHit(Weapon weapon)
    {
        health -= weapon.Damage;
        Debug.Log($"{ballName} health: {health}");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
