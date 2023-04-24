using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour
{
    public GameObject Projectile;
    public Transform FiringPoint;
    [Header("Delay between shorts in seconds")]
    public float DelayBetweenShots = 1;
    
    [Header("Magazine settings")]
    public int BulletsInInventory = 90;
    public int MagazineCapacity = 30;
    public int BulletsInMagazine = 30;

    [Header("Audio settings")]
    public AudioSource gunshotAudio;
    

    private StarterAssetsInputs _input;
    
    private float _timePassedAfterShot;
    // Start is called before the first frame update
    void Start()
    {
        _input = gameObject.GetComponentInParent<StarterAssetsInputs>();
    }
    
    void Update()
    {
        if (_input.Fire && CanShoot())
        {
            Shoot();
            _timePassedAfterShot = 0f;
        }

        if (_input.Reload)
        {
            Reload();
            _input.Reload = false;
        }
        
        var ray = new Ray(FiringPoint.position, _input.MousePositionInWorldSpace - FiringPoint.position);
        if (Physics.Raycast(ray, out var hintData, 1000))
        {
            Debug.DrawLine(ray.origin, hintData.point, Color.red);
        }
        
        

        _timePassedAfterShot += Time.deltaTime;
        
        //Hack for now
    }

    private bool CanShoot()
    {
        return _timePassedAfterShot >= DelayBetweenShots && BulletsInMagazine > 0;
    }

    public void Reload()
    {
        if (BulletsInMagazine == MagazineCapacity || BulletsInInventory == 0)
        {
            return;
        }

        var requiredBullets = MagazineCapacity - BulletsInMagazine;
        if (BulletsInInventory >= requiredBullets)
        {
            BulletsInInventory -= requiredBullets;
            BulletsInMagazine += requiredBullets;
            return;
        }

        BulletsInMagazine += BulletsInInventory;
        BulletsInInventory = 0;
    }

    public void Shoot()
    {
        BulletsInMagazine--;
        Instantiate(Projectile, FiringPoint.position, FiringPoint.rotation);
        gunshotAudio.Play();
    }
}
