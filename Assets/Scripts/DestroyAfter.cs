using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField]
    private float Seconds = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this, Seconds);
    }
}
