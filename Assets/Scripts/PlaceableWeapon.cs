using UnityEngine;

public class PlaceableWeapon : Weapon
{
    [Header("Placeable weapon")]
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private GameObject placeholderPrefab;

    [SerializeField]
    private float range = 4f;

    [SerializeField]
    private float heightAboveGround = 0.2f;

    private GameObject InitializedPlaceholder;
    private Vector3 placePosition = new Vector3();

    protected override void Update()
    {
        base.Update();
        CalculatePlacePosition();
        if (InitializedPlaceholder == null)
        {
            InitializedPlaceholder = Instantiate(placeholderPrefab, placePosition, Quaternion.identity);
        }

        InitializedPlaceholder.transform.position = placePosition;
    }

    private void CalculatePlacePosition()
    {
        var direction = _input.MousePositionInWorldSpace - firingPoint.position;
        placePosition = Vector3.ClampMagnitude(direction, range) + firingPoint.position;
        placePosition.y = heightAboveGround;

        placePosition.x = Mathf.Round(placePosition.x);
        placePosition.z = Mathf.Round(placePosition.z);
    }

    protected override void Shoot()
    {
        Instantiate(prefab, placePosition, Quaternion.identity);
        _input.FireInput(false);
    }

    private void OnDisable()
    {
        Destroy(InitializedPlaceholder);
    }
}