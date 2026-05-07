using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Cannon : Weapon
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projSpeed = 15;
    [SerializeField] int projCount = 1;
    [SerializeField] int projDamage = 10;
    [SerializeField] float knockbackForce = 10f;
    [SerializeField] float knockbackDuration = 0.2f; // spread force over this many seconds

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
            StartCoroutine(SmoothKnockback(rb, -shotDirection));

        OnAttackEnd();
    }

    private IEnumerator SmoothKnockback(Rigidbody2D rb, Vector3 direction)
    {
        float elapsed = 0f;
        while (elapsed < knockbackDuration)
        {
            float t = 1f - (elapsed / knockbackDuration); // starts strong, fades out
            rb.AddForce(direction * knockbackForce * t, ForceMode2D.Force);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}