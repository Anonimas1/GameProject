using System.Collections;
using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    [SerializeField]
    private float explosionRadius = 2f;

    [SerializeField]
    private int damage = 10;

    [SerializeField]
    private float timeUntilDestroy = 2f;

    [SerializeField]
    private GameObject audioSourceGameObject;

    private void OnDrawGizmosSelected()
    {
        var transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.10f);
        Gizmos.color = transparentRed;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }

    [SerializeField]
    private float force = 50f;

    private Transform _objectTransform;
    private Rigidbody _rigidbody;

    void Start()
    {
        _objectTransform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(_objectTransform.forward * force);
        StartCoroutine(DestroyProjectile());
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(timeUntilDestroy);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(audioSourceGameObject, transform.position, Quaternion.identity);
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