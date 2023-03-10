using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    public GameObject Projectile;
    public Transform FiringPoint;
    [FormerlySerializedAs("DelaybetweenShots")]
    [Header("Delay between shorts in seconds")]
    public float DelayBetweenShots = 1;


    private StarterAssetsInputs _input;

    private float _timePassedAfterShot;
    // Start is called before the first frame update
    void Start()
    {
        _input = gameObject.GetComponentInParent<StarterAssetsInputs>();
    }
    
    void Update()
    {
        if (_input.IsFiring && _timePassedAfterShot >= DelayBetweenShots)
        {
            Shoot();
            _timePassedAfterShot = 0f;
        }

        _timePassedAfterShot += Time.deltaTime;
    }

    public void Shoot()
    {
        Instantiate(Projectile, FiringPoint.position, FiringPoint.rotation);
    }
}
