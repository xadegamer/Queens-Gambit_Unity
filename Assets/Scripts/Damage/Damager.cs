using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] bool hasParent;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] Vector2 offset;
    [SerializeField] private LayerMask damageLayer;

    public void Damage()
    {
        Collider2D[] enemiesToDamage = null;

        Vector2 currentOffset;

        if (hasParent) currentOffset = new Vector2(offset.x * transform.parent.localScale.x, offset.y);
        else currentOffset = new Vector2(offset.x * transform.localScale.x, offset.y);

        enemiesToDamage = Physics2D.OverlapCircleAll((Vector2)transform.position + currentOffset, range, damageLayer);

        for (int i = 0; i < enemiesToDamage.Length; i++) enemiesToDamage[i].GetComponent<Damageable>().TakeDamage(damage);
    }

    private void OnDrawGizmos()
    {
        Vector2 currentOffset;

        if (hasParent) currentOffset = new Vector2(offset.x * transform.parent.localScale.x, offset.y);
        else  currentOffset = new Vector2(offset.x * transform.localScale.x, offset.y);

        if (hasParent) Gizmos.DrawWireSphere((Vector2)transform.position + currentOffset, range);
        else Gizmos.DrawWireSphere((Vector2)transform.position + currentOffset, range);
    }
}
