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
        if (_input.Fire && _timePassedAfterShot >= DelayBetweenShots)
        {
            Shoot();
            _timePassedAfterShot = 0f;
        }

        _timePassedAfterShot += Time.deltaTime;
        
        
        
        var targetRotationVector = GetTargetRotationVector();
        Debug.Log(targetRotationVector.ToString());
        var _targetRotation = Mathf.Atan2(targetRotationVector.z, targetRotationVector.y) * Mathf.Rad2Deg;
        /*float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
            RotationSmoothTime);*/
        transform.LookAt(_input.MousePositionInWorldSpace);
        // rotate to face mouse cursor
        
        
    }
    
    private Vector3 GetTargetRotationVector()
    {
        return (_input.MousePositionInWorldSpace -  transform.position).normalized;
    }


    public void Shoot()
    {
        Instantiate(Projectile, FiringPoint.position, FiringPoint.rotation);
    }
}
