using System;
using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    public float Speed = 1f;
    public int TimeUntilDestroy = 1; 
    
    private Transform _objectTransform;
    void Start()
    {
        _objectTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        var position = _objectTransform.position;
        position += _objectTransform.forward * (Speed * Time.deltaTime);
        _objectTransform.position = position;

        StartCoroutine(DestroyProjectile());
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(TimeUntilDestroy);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        Destroy(gameObject);
    }
}