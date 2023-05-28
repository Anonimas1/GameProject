using UnityEngine;

public class ShotgunWeapon : Weapon
{
    [SerializeField]
    private int pelletCount = 8;

    [SerializeField]
    private float spreadDegrees = 8f;
    
    protected override void Shoot()
    {
        var initialRotation = firingPoint.rotation.eulerAngles;
        for (var i = 0; i < pelletCount; i++)
        {
            var newRotation = new Vector3(
                initialRotation.x + Random.Range(-spreadDegrees /2 , spreadDegrees /2),
                initialRotation.y + Random.Range(-spreadDegrees, spreadDegrees),
                initialRotation.z);
            Instantiate(projectile, firingPoint.position, Quaternion.Euler(newRotation));
        }
    }
}