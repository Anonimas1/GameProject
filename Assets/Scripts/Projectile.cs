using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public int DamageCaused = 1;
    public float TimeUntilDestroy = 1f;

    private Transform _objectTransform;
    private Rigidbody _rigidbody;

    void Start()
    {
        _objectTransform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(_objectTransform.forward * 0.0005f);
        StartCoroutine(DestroyProjectile());
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(TimeUntilDestroy);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Damageable>(out var damagable))
        {
            damagable.TakeDamage(DamageCaused);
        }

        Destroy(gameObject);
    }
}