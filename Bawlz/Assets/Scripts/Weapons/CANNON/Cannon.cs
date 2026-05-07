using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Cannon : Weapon
{
    [SerializeField, Tooltip("The projectile prefab to spawn when firing")] GameObject projectilePrefab;
    [SerializeField, Tooltip("Speed of the projectile")] float projSpeed = 15;
    [SerializeField, Tooltip("Number of projectiles fired per shot")] int projCount = 1;
    [SerializeField, Tooltip("Damage dealt by each projectile")] int projDamage = 10;
    [SerializeField, Tooltip("Force applied to the shooter on firing")] float knockbackForce = 10f;
    [SerializeField, Tooltip("Duration in seconds the knockback force is spread over")] float knockbackDuration = 0.2f;

    private Coroutine _knockbackCoroutine;

    protected override void OnAttack()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        float rotation = transform.eulerAngles.z * math.PI / 180.0f + math.PI / 2;
        Vector3 shotDirection = new Vector3(math.cos(rotation), math.sin(rotation));
        projectile.ProjectileInit(shotDirection, projSpeed, projDamage, this);
        projectileObject.layer = gameObject.layer;

        Rigidbody2D rb = parent.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (_knockbackCoroutine != null)
                StopCoroutine(_knockbackCoroutine);
            _knockbackCoroutine = StartCoroutine(SmoothKnockback(rb, -shotDirection));
        }

        OnAttackEnd();
    }

    private IEnumerator SmoothKnockback(Rigidbody2D rb, Vector3 direction)
    {
        rb.linearVelocity = Vector2.zero;
        float elapsed = 0f;
        while (elapsed < knockbackDuration)
        {
            // make it so the parent dosent take damage during the knockbaack here

            float t = 1f - (elapsed / knockbackDuration);
            rb.AddForce(direction * knockbackForce * t, ForceMode2D.Force);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _knockbackCoroutine = null;
    }
}