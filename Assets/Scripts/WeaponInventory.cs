using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class WeaponInventory : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] weapons;
    }
}