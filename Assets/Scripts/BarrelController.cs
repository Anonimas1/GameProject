using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(StarterAssetsInputs))]
public class BarrelController : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPlace;

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private float range = 4f;

    private StarterAssetsInputs inputs;

    // Start is called before the first frame update
    void Start()
    {
        inputs = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputs.PlaceBarrel)
        {
            var direction = inputs.MousePositionInWorldSpace - transform.position;
            var placePosition = Vector3.ClampMagnitude(direction, range);

            Instantiate(prefab, transform.position + placePosition, Quaternion.identity);
            inputs.PlaceBarrel = false;
        }
    }
}