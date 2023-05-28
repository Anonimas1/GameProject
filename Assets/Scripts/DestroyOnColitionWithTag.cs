using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DestroyOnColitionWithTag : MonoBehaviour
    {
        [SerializeField]
        private string PlayerTag;
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(PlayerTag))
            {
                Destroy(gameObject);
            }
        }
    }
}