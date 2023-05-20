using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour, IWeapon
{
    public GameObject Projectile;
    public Transform FiringPoint;
    [Header("Delay between shorts in seconds")]
    public float DelayBetweenShots = 1;

    [Header("Magazine settings")]
    [SerializeField]
    public int BulletsInInventory = 90;
    public int MagazineCapacity = 30;
    public int BulletsInMagazine = 30;

    [Header("Audio settings")]
    public AudioSource gunshotAudio;
    

    private StarterAssetsInputs _input;
    
    private float _timePassedAfterShot;


    // Start is called before the first frame update
    private void Start()
    {
        _input = gameObject.GetComponentInParent<StarterAssetsInputs>();
    }

    private void Update()
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
        
        _timePassedAfterShot += Time.deltaTime;
    }

    private bool CanShoot()
    {
        return _timePassedAfterShot >= DelayBetweenShots && BulletsInMagazine > 0;
    }

    private void Reload()
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

    private void Shoot()
    {
        BulletsInMagazine--;
        Instantiate(Projectile, FiringPoint.position, FiringPoint.rotation);
        gunshotAudio.Play();
    }

    public void RefillAmmo(int amount)
    {
        BulletsInInventory += amount;
    }

    public bool HasAmmo()
    {
        return BulletsInInventory > 0 || BulletsInMagazine > 0;
    }
}

public interface IWeapon
{
    bool HasAmmo();
}
