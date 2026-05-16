using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class TKProdj : Projectile
{
    [SerializeField] float spinSpeed = 720f;
    [SerializeField] float stickDuration = 1f;
    [SerializeField] float fadeDuration = 1f;

    [SerializeField] DoTEffect bleedEffect;

    SpriteRenderer spriteRenderer;
    bool stuck = false;

    public virtual void ProjectileInit(Vector3 moveDirection, float moveSpeed, float projectileDamage, Weapon weapon)
    {
        direction = moveDirection;
        speed = moveSpeed;
        damage = projectileDamage;
        parentWeapon = weapon;
    }

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    protected override void Update()
    {
        if (stuck) return;
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
        transform.position += direction * speed * Time.deltaTime;
        base.Update();
    }

    protected override void OnBallHit(BallController otherBall)
    {
        FXManager.Instance.PlayPlayerHit(otherBall.transform.position);
        base.OnBallHit(otherBall);
        otherBall.ApplyStatus(bleedEffect, parent);
    }

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        FXManager.Instance.PlayWeaponHit(otherWeapon.transform.position);
        base.OnWeaponHit(otherWeapon);
        Destroy(gameObject);
    }

    protected override void OnWallHit()
    {
        stuck = true;
        StartCoroutine(StickAndFade());
    }

    private IEnumerator StickAndFade()
    {
        // Stick to wall
        yield return new WaitForSeconds(stickDuration);

        // Fade out
        float elapsed = 0f;
        Color color = spriteRenderer.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - (elapsed / fadeDuration);
            spriteRenderer.color = color;
            yield return null;
        }

        Destroy(gameObject);
    }

    
}