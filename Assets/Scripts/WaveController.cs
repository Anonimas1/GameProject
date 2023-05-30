using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

        public LootTable Add(AmmoAmount ammo, int chance)
        {
            for (int i = 0; i < chance; i++)
            {
                AmmoTable.Add(ammo);
            }

            return this;
        }

        public AmmoAmount GetRandom()
        {
            if (AmmoTable.Count == 0)
            {
                return new AmmoAmount(0, "");
            }

            var index = Random.Range(0, AmmoTable.Count);
            return AmmoTable[index];
        }
    }

    public class WaveController : MonoBehaviour
    {
        [SerializeField]
        private SpawnPoint[] spawnPoints;

        [SerializeField]
        private GameObject MeleeEnemy;

        [SerializeField]
        private GameObject RangeEnemy;

        [SerializeField]
        private Transform player;

        [FormerlySerializedAs("TimeBetweenWaves")]
        [SerializeField]
        private float timeBetweenWaves = 500f;

        private List<List<WaveDescription>> _waves;
        private int _currentWave = 0;

        private List<GameObject> _spawnedEnemies = new List<GameObject>();

        [SerializeField]
        private float _timeSinceLastSpawn = 0f;

        private void Update()
        {
            if (_timeSinceLastSpawn < timeBetweenWaves)
            {
                _timeSinceLastSpawn += Time.deltaTime;
                return;
            }

            if (_spawnedEnemies.Any(x => x != null))
            {
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

            _currentWave += 1;
            _timeSinceLastSpawn = 0f;
        }

        private SpawnPoint GetSpawnPoint()
        {
            return spawnPoints[Random.Range(0, spawnPoints.Length)];
        }


        private void Start()
        {
            var lootTable1 = new LootTable();
            var lootTable2 = new LootTable()
                .Add(new AmmoAmount(10, Constants.AK47), 10)
                .Add(new AmmoAmount(0, ""), 40);
            var lootTable3 = new LootTable()
                .Add(new AmmoAmount(15, Constants.AK47), 10)
                .Add(new AmmoAmount(10, Constants.Shotgun), 10)
                .Add(new AmmoAmount(0, ""), 40);
            var lootTable4 = new LootTable()
                .Add(new AmmoAmount(20, Constants.AK47), 10)
                .Add(new AmmoAmount(15, Constants.Shotgun), 10)
                .Add(new AmmoAmount(10, Constants.Barrel), 5)
                .Add(new AmmoAmount(10, Constants.Box), 5)
                .Add(new AmmoAmount(0, ""), 40);
            var lootTable5 = new LootTable()
                .Add(new AmmoAmount(30, Constants.AK47), 10)
                .Add(new AmmoAmount(25, Constants.Shotgun), 10)
                .Add(new AmmoAmount(15, Constants.Barrel), 10)
                .Add(new AmmoAmount(15, Constants.Box), 10)
                .Add(new AmmoAmount(0, ""), 40);

            var lootTable6 = new LootTable()
                .Add(new AmmoAmount(40, Constants.AK47), 10)
                .Add(new AmmoAmount(35, Constants.Shotgun), 10)
                .Add(new AmmoAmount(20, Constants.Barrel), 10)
                .Add(new AmmoAmount(20, Constants.Box), 10)
                .Add(new AmmoAmount(2, Constants.RPG), 5)
                .Add(new AmmoAmount(0, ""), 40);


            var specialLootTable = new LootTable()
                .Add(new AmmoAmount(10, Constants.RPG), 1);

            _waves = new List<List<WaveDescription>>()
            {
                new()
                {
                    new WaveDescription(10, MeleeEnemy, lootTable1)
                },
                new()
                {
                    new WaveDescription(15, MeleeEnemy, lootTable2)
                },
                new()
                {
                    new WaveDescription(15, MeleeEnemy, lootTable3)
                },
                new()
                {
                    new WaveDescription(25, MeleeEnemy, lootTable4)
                },
                new()
                {
                    new WaveDescription(15, MeleeEnemy, lootTable5),
                    new WaveDescription(1, RangeEnemy, specialLootTable)
                },
                new()
                {
                    new WaveDescription(20, MeleeEnemy, lootTable6),
                    new WaveDescription(2, RangeEnemy, specialLootTable)
                }
            };
        }
    }
}