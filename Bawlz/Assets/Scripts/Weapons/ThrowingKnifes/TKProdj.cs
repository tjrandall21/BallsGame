using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class TKProdj : Projectile
{
    [SerializeField] float spinSpeed = 720f;
    [SerializeField] float stickDuration = 1f;
    [SerializeField] float fadeDuration = 1f;
    public float bounces = 0;
    //[SerializeField] DoTEffect bleedEffect;

    SpriteRenderer spriteRenderer;
    bool stuck = false;
    public bool Stuck => stuck;

    public void ProjectileInit(Vector3 moveDirection, float moveSpeed, float projectileDamage, Weapon weapon, float StickDuration, float FadeDuration)
    {
        base.ProjectileInit(moveDirection, moveSpeed, projectileDamage, weapon);
        fadeDuration = FadeDuration;
        stickDuration = StickDuration;
    }

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();
        if (!stuck)
        {            
            transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    protected override void OnBallHit(BallController otherBall)
    {
        FXManager.Instance.PlayProjectileFire(transform.position, this);
        ((TKScript)parentWeapon).OnProjectileBallHit(this,otherBall);
        ((TKScript)parentWeapon).OnProjectileDestroyed(this);
        //otherBall.ApplyStatus(bleedEffect, parent);
        base.OnBallHit(otherBall);
    }

    protected override void OnWeaponHit(Weapon otherWeapon)
    {
        ((TKScript)parentWeapon).OnProjectileWeaponHit(this,otherWeapon);
        ((TKScript)parentWeapon).OnProjectileDestroyed(this);
        base.OnWeaponHit(otherWeapon);
    }

    protected override void OnWallHit()
    {
        if (bounces > 0)
        {
            bounces--;
            direction *= -1;
        }
        else
        {
            stuck = true;
            travel = false;
            Debug.Log($"Wall hit! stickDuration: {stickDuration}, fadeDuration: {fadeDuration}");
            StartCoroutine(StickAndFade());
        }
        ((TKScript)parentWeapon).OnProjectileWallHit(this);
    }

    private IEnumerator StickAndFade()
    {
        yield return new WaitForSeconds(stickDuration);
        float elapsed = 0f;
        Color color = spriteRenderer.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            color.a = 1f - (t * t);
            spriteRenderer.color = color;
            yield return null;
        }

        color.a = 0f;
        spriteRenderer.color = color;
        ((TKScript)parentWeapon).OnProjectileTimeout(this);
        ((TKScript)parentWeapon).OnProjectileDestroyed(this);
        Destroy(gameObject);
    }
}