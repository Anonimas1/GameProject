using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    [System.Serializable]
    public struct WaveDescription
    {
        public WaveDescription(int numberToSpawn, GameObject gameobject, LootTable lootTable)
        {
            NumberToSpawn = numberToSpawn;
            GameObjectToSpawn = gameobject;
            LootTable = lootTable;
        }

        public int NumberToSpawn;
        public GameObject GameObjectToSpawn;
        public LootTable LootTable;
    }

    public struct AmmoAmount
    {
        public int Amount;
        public string WeaponName;

        public AmmoAmount(int amount, string weaponName)
        {
            Amount = amount;
            WeaponName = weaponName;
        }
    }
    
    public class LootTable
    {
        private List<AmmoAmount> AmmoTable = new List<AmmoAmount>();

        public void Add(AmmoAmount ammo, int chance)
        {
            for (int i = 0; i < chance; i++)
            {
                AmmoTable.Add(ammo);
            }
        }

        public AmmoAmount GetRandom()
        {
            return AmmoTable[Random.Range(0, AmmoTable.Count)];
        }
    }

    public class WaveController : MonoBehaviour
    {
        [SerializeField]
        private SpawnPoint[] spawnPoints;

        [SerializeField]
        private GameObject EnemyOne;

        [SerializeField]
        private GameObject EnemyTwo;

        [SerializeField]
        private Transform player;

        [FormerlySerializedAs("TimeBetweenWaves")]
        [SerializeField]
        private float timeBetweenWaves = 500f;

        private List<List<WaveDescription>> _waves;
        private int _currentWave = 0;

        private List<GameObject> _spawnedEnemies;

        [SerializeField]
        private float _timeSinceLastSpawn = 0f;

        private void Update()
        {
            if (_timeSinceLastSpawn < timeBetweenWaves)
            {
                _timeSinceLastSpawn += Time.deltaTime;
                return;
            }

            _spawnedEnemies = new List<GameObject>();

            foreach (var waveDescription in _waves[_currentWave])
            {
                for (var i = 0; i < waveDescription.NumberToSpawn; i++)
                {
                    var enemy = GetSpawnPoint()
                        .Spawn(waveDescription.GameObjectToSpawn);
                    enemy.SetActive(true);
                    var damagable = enemy.GetComponent<Damageable>();
                    damagable.LootTable = waveDescription.LootTable;
                    _spawnedEnemies.Add(enemy);
                }
            }

            _timeSinceLastSpawn = 0f;
        }

        private SpawnPoint GetSpawnPoint()
        {
            return spawnPoints[Random.Range(0, spawnPoints.Length)];
        }
        
        

        private void Start()
        {
            var lootTable1 = new LootTable();
            lootTable1.Add(new AmmoAmount(10, Constants.AK47), 1);


            _waves = new List<List<WaveDescription>>()
            {
                new()
                {
                    new WaveDescription(5, EnemyOne, lootTable1)
                },
                new()
                {
                    new WaveDescription(10, EnemyOne, lootTable1)
                },
                new()
                {
                    new WaveDescription(15, EnemyOne, lootTable1)
                },
                new()
                {
                    new WaveDescription(20, EnemyOne, lootTable1)
                },
                new()
                {
                    new WaveDescription(25, EnemyOne, lootTable1)
                },
                new()
                {
                    new WaveDescription(30, EnemyOne, lootTable1)
                }
            };
        }
    }
}