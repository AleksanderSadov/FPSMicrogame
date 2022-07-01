using System.Collections;
using System.Collections.Generic;
using Unity.FPS.AI;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.AI;

namespace Unity.FPS.Gameplay
{
    public class EnemyWavesManager : MonoBehaviour
    {
        [Tooltip("Spawn radius in units from the spawner location")]
        public int spawnRadius = 1;

        [Tooltip("Current hover bots enemies in wave. It will increase automaticaly as wave count increases")]
        public int waveHoverbotsCount = 1;

        [Tooltip("Current turret enemies in wave. It will increase automaticaly as wave count increases")]
        public int waveTurretsCount = 0;

        [Tooltip("Loot health pack chance from hoverbots")]
        public float lootHealthPackChance = 0.25f;

        [Tooltip("Current wave count")]
        [SerializeField] private int waveCurrentCount = 0;

        [Tooltip("Objectives parent container")]
        [SerializeField] private GameObject objectivesParent;

        [Tooltip("Enemies parent container")]
        [SerializeField] private GameObject enemiesParent;

        [Tooltip("Kill wave objective prefab")]
        [SerializeField] private GameObject objectiveKillWave;

        [Tooltip("Hoverbot without loot enemy prefab")]
        [SerializeField] private GameObject enemyHoverbot;

        [Tooltip("Hoverbot with loot enemy prefab")]
        [SerializeField] private GameObject enemyHoverbotWithLoot;

        [Tooltip("Turret enemy prefab")]
        [SerializeField] private GameObject enemyTurret;

        [Tooltip("Loot jetpack prefab")]
        [SerializeField] private GameObject lootJetpack;

        [Tooltip("Loot health pack prefab")]
        [SerializeField] private GameObject lootHealthPack;

        [Tooltip("Loot shotgun prefab")]
        [SerializeField] private GameObject lootShotgun;

        [Tooltip("Loot launcher prefab")]
        [SerializeField] private GameObject lootLauncher;

        [Tooltip("Loot sniper rifle prefab")]
        [SerializeField] private GameObject lootSniperRifle;

        private Vector3 spawnDefaultPosition;
        private List<EnemySpawnQueueItem> enemySpawnQueue = new List<EnemySpawnQueueItem>();

        [System.Serializable]
        private class EnemySpawnQueueItem
        {
            public EnemySpawnQueueItem(GameObject enemyPrefab, int spawnCount, GameObject lootPrefab, float lootChance)
            {
                this.enemyPrefab = enemyPrefab;
                this.spawnCount = spawnCount;
                this.lootPrefab = lootPrefab;
                this.lootChance = lootChance;
            }

            public EnemySpawnQueueItem(GameObject enemyPrefab, int spawnCount) : this(enemyPrefab, spawnCount, null, 0)
            {

            }

            public GameObject enemyPrefab;
            public int spawnCount;
            public GameObject lootPrefab;
            public float lootChance;
        }

        // Start is called before the first frame update
        void Start()
        {
            spawnDefaultPosition = transform.position;
            EventManager.AddListener<NavMeshReadyEvent>(OnWaveCompleteAndNavMeshReady);
        }

        private void CreateNextWave()
        {
            waveCurrentCount++;
            HandleDifficulty();
            SpawnWaveEnemies();
            CreateKillWaveObjective();
        }

        private void HandleDifficulty()
        {
            switch (waveCurrentCount)
            {
                case 1:
                    waveHoverbotsCount = 1;
                    break;
                case 3:
                    waveHoverbotsCount = 2;
                    break;
                case 5:
                    enemySpawnQueue.Add(new EnemySpawnQueueItem(enemyHoverbotWithLoot, 1, lootShotgun, 1));
                    break;
                case 6:
                    waveHoverbotsCount = 3;
                    break;
                case 7:
                    enemySpawnQueue.Add(new EnemySpawnQueueItem(enemyHoverbotWithLoot, 1, lootJetpack, 1));
                    break;
                case 8:
                    waveHoverbotsCount = 4;
                    break;
                case 9:
                    enemySpawnQueue.Add(new EnemySpawnQueueItem(enemyHoverbotWithLoot, 1, lootSniperRifle, 1));
                    break;
                case 10:
                    waveHoverbotsCount = 5;
                    waveTurretsCount = 1;
                    break;
                case 12:
                    enemySpawnQueue.Add(new EnemySpawnQueueItem(enemyHoverbotWithLoot, 1, lootLauncher, 1));
                    break;
                case 14:
                    waveTurretsCount = 2;
                    break;
                case 16:
                    waveTurretsCount = 3;
                    break;
                case int n when n >= 20 && n % 5 == 0 && n % 10 == 5:
                    waveHoverbotsCount++;
                    break;
                case int n when n >= 20 && n % 10 == 0:
                    waveTurretsCount++;
                    break;
                default:
                    break;
            }

            enemySpawnQueue.Add(new EnemySpawnQueueItem(enemyHoverbot, waveHoverbotsCount, lootHealthPack, lootHealthPackChance));
            enemySpawnQueue.Add(new EnemySpawnQueueItem(enemyTurret, waveTurretsCount));
        }

        private void SpawnWaveEnemies()
        {
            for (int i = enemySpawnQueue.Count - 1; i >= 0; i--)
            {
                EnemySpawnQueueItem enemyQueueItem = enemySpawnQueue[i];
                SpawnEnemy(enemyQueueItem.enemyPrefab, enemyQueueItem.spawnCount, enemyQueueItem.lootPrefab, enemyQueueItem.lootChance);
                enemySpawnQueue.RemoveAt(i);
            }
        }

        private void SpawnEnemy(GameObject enemyPrefab, int spawnCount, GameObject lootPrefab, float lootChance)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnDefaultPosition, enemyPrefab.transform.rotation, enemiesParent.transform);
                MoveEnemyToRandomSpawnPosition(enemy);

                if (lootPrefab != null)
                {
                    EnemyController enemyController = enemy.GetComponent<EnemyController>();
                    enemyController.LootPrefab = lootPrefab;
                    enemyController.DropRate = lootChance;
                }
            }
        }

        private void CreateKillWaveObjective()
        {
            GameObject killWaveObject = Instantiate(objectiveKillWave, objectivesParent.transform);
            killWaveObject.GetComponent<ObjectiveKillWave>().WaveCount = waveCurrentCount;
            killWaveObject.GetComponent<ObjectiveKillWave>().EnemiesInWave = enemiesParent.transform.childCount;
        }

        private void OnWaveCompleteAndNavMeshReady(NavMeshReadyEvent evt)
        {
            CreateNextWave();
        }

        private void MoveEnemyToRandomSpawnPosition(GameObject enemy)
        {
            Vector3 randomSpawnPosition;
            if (FindRandomSpawnPosition(spawnDefaultPosition, spawnRadius, out randomSpawnPosition))
            {
                enemy.GetComponent<NavMeshAgent>().Warp(randomSpawnPosition);
            }
        }

        private bool FindRandomSpawnPosition(Vector3 center, float range, out Vector3 result)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = center + new Vector3(
                    Random.Range(-range, range),
                    1,
                    Random.Range(-range, range)
                );
                NavMeshHit hit;
                int maxDistance = 1;
                int walkableAreaMask = 1 << NavMesh.GetAreaFromName("Walkable"); // Need to bit shift area according to docs example
                if (NavMesh.SamplePosition(randomPoint, out hit, maxDistance, walkableAreaMask))
                {
                    result = hit.position;
                    return true;
                }
            }
            result = Vector3.zero;
            return false;
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<NavMeshReadyEvent>(OnWaveCompleteAndNavMeshReady);
        }
    }
}
