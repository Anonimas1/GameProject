using System;
using UnityEngine;

[RequireComponent(typeof(Damagable))]
public class Explosive : MonoBehaviour
{
    public float explosionRadius;
    public int damage;
    private Damagable _damageable;

    private void OnDrawGizmosSelected()
    {
        var transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.10f);
        Gizmos.color = transparentRed;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
    
    private void Awake()
    {
        _damageable = GetComponent<Damagable>();
    }

    private void Update()
    {
        if (_damageable.CurrentHealth <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        var hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.gameObject != gameObject && hit.TryGetComponent<Damagable>(out var damagable))
            {
                damagable.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
