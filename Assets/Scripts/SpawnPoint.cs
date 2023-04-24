using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private float radius = 2;

    [SerializeField]
    private GameObject prefabToInitiate;

    [SerializeField]
    private float timePassed;

    [SerializeField]
    private float waveMultiplication = 2f;

    [SerializeField]
    private float timeBetweenWaves = 15f;

    [SerializeField]
    private float enemiesToSpawn = 1f;

    [SerializeField]
    private Transform player;


    private void Start()
    {
        StartCoroutine(SpawnAfterStart());
    }

    IEnumerator SpawnAfterStart()
    {
        yield return new WaitForSeconds(10);
        SpawnWave();
        timePassed = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (timePassed > timeBetweenWaves)
        {
            SpawnWave();
            timePassed = 0f;
        }

        timePassed += Time.deltaTime;
    }

    private void SpawnWave()
    {
        var currPosition = gameObject.transform.position;

        for (var i = 0; i < enemiesToSpawn; i++)
        {
            var delta = Random.Range(-radius, radius);
            var enemy = Instantiate(prefabToInitiate, new Vector3(currPosition.x + delta, currPosition.y, currPosition.z), Quaternion.identity).GetComponent<EnemyController>();
            enemy.destination = player;
            enemy.gameObject.SetActive(true);
        }
        enemiesToSpawn *= waveMultiplication;
    }
}