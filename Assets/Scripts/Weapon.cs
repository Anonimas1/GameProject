using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public interface IWeapon
{
    bool HasAmmo();
    void RefillAmmo(int ammount);
    string WeaponName { get; }
}

public static class Constants
{
    public const int InfiniteAmmo = -1;
    public const string AK47 = nameof(AK47);
    public const string M1911Pistol = nameof(M1911Pistol);
    public const string Shotgun = nameof(Shotgun);
    public const string RPG = nameof(RPG);
    public const string Barrel = nameof(Barrel);
    public const string Box = nameof(Box);
}

public class Weapon : MonoBehaviour, IWeapon
{
    public string WeaponName => _weaponName;
    
    [SerializeField]
    private string _weaponName;

    [SerializeField]
    protected GameObject projectile;
    
    [SerializeField]
    protected Transform firingPoint;
    
    [Header("Delay between shorts in seconds")]
    [SerializeField]
    private float delayBetweenShots = 1;
    
    [SerializeField]
    private bool hasInfiniteAmmo = false;

    public int Bullets = 90;
    
    [Header("Audio settings")]
    public AudioSource gunshotAudio;

    public AudioClip AudioClip;
    public float volume = 1f;
    protected StarterAssetsInputs _input;

    [SerializeField]
    private float _timePassedAfterShot;


    private void Start()
    {
        if (hasInfiniteAmmo)
        {
            Bullets = Constants.InfiniteAmmo;
        }

        _input = gameObject.GetComponentInParent<StarterAssetsInputs>();
    }

    protected virtual void Update()
    {
        if (_input.Fire && CanShoot())
        {
            Shoot();
            
            if (!hasInfiniteAmmo)
            {
                Bullets--;
            }
            
            if (gunshotAudio != null)
                gunshotAudio.Play();

            if (AudioClip != null)
            {
                var soundpos = firingPoint.position;
                soundpos.y += 13;
                AudioSource.PlayClipAtPoint(AudioClip, soundpos,volume );
            }
                
            _timePassedAfterShot = 0f;
        }

        _timePassedAfterShot += Time.deltaTime;
    }

    private bool CanShoot()
    {
        return _timePassedAfterShot >= delayBetweenShots && HasAmmo();
    }

    protected virtual void Shoot()
    {
        Instantiate(projectile, firingPoint.position, firingPoint.rotation);
    }

    public void RefillAmmo(int amount)
    {
        if (!hasInfiniteAmmo)
        {
            Bullets += amount;
        }
    }

    public bool HasAmmo()
    {
        return hasInfiniteAmmo || Bullets > 0;
    }
}