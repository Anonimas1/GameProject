using System;
using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public int DamageCaused = 1;
    public float Speed = 1f;
    public int TimeUntilDestroy = 1;

    private Transform _objectTransform;
    private Rigidbody _rigidbody;

    void Start()
    {
        _objectTransform = transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var position = _objectTransform.position;
        position += _objectTransform.forward * (Speed * Time.deltaTime);
        _rigidbody.MovePosition(position);

        StartCoroutine(DestroyProjectile());
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(TimeUntilDestroy);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Damagable>(out var damagable))
        {
            damagable.TakeDamage(DamageCaused);
        }

        Destroy(gameObject);
    }
}