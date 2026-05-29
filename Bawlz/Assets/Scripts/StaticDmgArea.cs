using System.Collections.Generic;
using UnityEngine;

public class StaticDmgArea : Weapon
{
    float lifetime = 0;


    public void Init(float entryDamage, Vector2 position, float duration, float size = 1)
    {
        damage = entryDamage;
        transform.position = position;
        transform.localScale = new Vector2(size,size);
        lifetime = duration;
    }

    protected override void Update()
    {
        base.Update();
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
