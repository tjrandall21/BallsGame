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

    public override void ProjectileInit(Vector3 moveDirection, float moveSpeed, float projectileDamage, Weapon weapon)
    {
        ProjectileInit(moveDirection, moveSpeed, projectileDamage, weapon, 1f);
    }

    public void ProjectileInit(Vector3 moveDirection, float moveSpeed, float projectileDamage, Weapon weapon, float FadeDuration)
    {
        base.ProjectileInit(moveDirection, moveSpeed, projectileDamage, weapon);
        fadeDuration += FadeDuration;
        foreach (WeaponUpgrade weaponUpgrade in weaponUpgrades)
        {
            if (weaponUpgrade is TKUpgrade tkUpgrade)
            {
                stickDuration *= tkUpgrade.fadeDurationMulti;
                damage += tkUpgrade.TKDamage;
                speed += tkUpgrade.TKSpeed;
                if (bleedEffect != null && (tkUpgrade.bleedDamageScaling != 0 || tkUpgrade.bleedDurationScaling != 0))
                {
                    DoTEffect scaledBleed = Instantiate(bleedEffect);
                    scaledBleed.damagePerSecond += tkUpgrade.bleedDamageScaling;
                    scaledBleed.statusDuration += tkUpgrade.bleedDurationScaling;
                    bleedEffect = scaledBleed;
                }
            }
        }
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
        Debug.Log($"Wall hit! stickDuration: {stickDuration}, fadeDuration: {fadeDuration}");
        StartCoroutine(StickAndFade());
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
        Destroy(gameObject);
    }
}