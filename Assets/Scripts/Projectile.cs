using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int damageCaused = 1;

    [SerializeField]
    private float timeUntilDestroy = 0.4f;

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
        if (!other.gameObject.CompareTag("Player") && other.gameObject.TryGetComponent<Damageable>(out var damagable))
        {
            damagable.TakeDamage(damageCaused);
            Destroy(gameObject);
        }
    }
}