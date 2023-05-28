using DefaultNamespace;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int Ammount = 20;
    public string WeaponName;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInChildren<WeaponInventory>().AddAmmo(WeaponName, Ammount);
            Destroy(gameObject);
        }
    }
}