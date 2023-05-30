using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Explosive : MonoBehaviour
{
    public float explosionRadius;
    public int damage;
    private Damageable _damageable;

    [SerializeField]
    private GameObject AudioSourceObject;

    private void OnDrawGizmosSelected()
    {
        var transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.10f);
        Gizmos.color = transparentRed;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
    
    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
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
        Instantiate(AudioSourceObject, transform.position, Quaternion.identity);
        var hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.gameObject != gameObject && hit.TryGetComponent<Damageable>(out var damagable))
            {
                damagable.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
