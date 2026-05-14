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
    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        FXManager.Instance.PlayWeaponHit(otherWeapon.transform.position);
        base.OnWeaponHit(otherWeapon);
    }

    protected override void OnBallHit(BallController otherBall)
    {
        FXManager.Instance.PlayPlayerHit(otherBall.transform.position);
    }

    private IEnumerator SmoothKnockback(Rigidbody2D rb, Vector3 direction)
    {
        rb.linearVelocity = Vector2.zero;
        // Apply the full knockback force instantly as an impulse
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        // Optionally, you can still wait for knockbackDuration if you need to disable damage or other effects
        yield return new WaitForSeconds(knockbackDuration);

        _knockbackCoroutine = null;
    }
}