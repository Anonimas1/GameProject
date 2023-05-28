using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private float radius = 2;

    public GameObject Spawn(GameObject prefabToSpawn)
    {
        var currPosition = gameObject.transform.position;
        var delta = Random.Range(-radius, radius);
        return Instantiate(prefabToSpawn, new Vector3(currPosition.x + delta, currPosition.y, currPosition.z + delta), Quaternion.identity);
    }
}