using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class BoulderProjectile : MonoBehaviour
{
    [SerializeField]
    private int damageCaused = 1;

    [SerializeField]
    private float timeUntilDestroy = 0.4f;


    [SerializeField]
    private string PlayerPlacedTag = "PlayerPlaced";

    [SerializeField]
    private string PlayerTag = "Player";


    [SerializeField]
    private GameObject audioSourceObject;

    void Start()
    {
        StartCoroutine(DestroyProjectile());
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(timeUntilDestroy);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        var shouldDamage = other.gameObject.CompareTag(PlayerTag) || other.gameObject.CompareTag(PlayerPlacedTag);
        if (shouldDamage && other.gameObject.TryGetComponent<Damageable>(out var damagable))
        {
            damagable.TakeDamage(damageCaused);
            Instantiate(audioSourceObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}