using System.Collections;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.AI;

namespace Unity.FPS.Gameplay
{
    public class EnemyWavesManager : MonoBehaviour
    {
        [Tooltip("Delay before wave starts in seconds")]
        public int waveStartDelayInSeconds = 1;

        [Tooltip("Spawn radius in units from the spawner location")]
        public int spawnRadius = 1;

        [Tooltip("Current wave count")]
        [SerializeField] private int currentWaveCount = 0;

        [Tooltip("Objectives parent container")]
        [SerializeField] private GameObject objectivesParent;

        [Tooltip("Enemies parent container")]
        [SerializeField] private GameObject enemiesParent;

        [Tooltip("Kill wave objective prefab")]
        [SerializeField] private GameObject killWaveObjective;

        [Tooltip("Hoverbot enemy prefab")]
        [SerializeField] private GameObject hoverBotEnemy;

        [Tooltip("Turret enemy prefab")]
        [SerializeField] private GameObject turretEnemy;

        private Vector3 spawnDefaultPosition;

        // Start is called before the first frame update
        void Start()
        {
            spawnDefaultPosition = transform.position;
            EventManager.AddListener<WaveCompleteEvent>(OnWaveComplete);
            StartNextWaveWithDelay(waveStartDelayInSeconds);
        }

        void StartNextWaveWithDelay(int delaySeconds)
        {
            currentWaveCount++;
            StartCoroutine(CreateNextWaveWithDelayCoroutine(waveStartDelayInSeconds));
        }

        IEnumerator CreateNextWaveWithDelayCoroutine(int delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);
            CreateNextWave();
        }

        private void CreateNextWave()
        {
            SpawnWaveEnemies();
            CreateKillWaveObjective();
        }

        private void SpawnWaveEnemies()
        {
            GameObject enemy = Instantiate(hoverBotEnemy, spawnDefaultPosition, hoverBotEnemy.transform.rotation, enemiesParent.transform);
            MoveEnemyToRandomSpawnPosition(enemy);
        }

        private void CreateKillWaveObjective()
        {
            GameObject killWaveObject = Instantiate(killWaveObjective, objectivesParent.transform);
            killWaveObject.GetComponent<ObjectiveKillWave>().WaveCount = currentWaveCount;
        }

        private void OnWaveComplete(WaveCompleteEvent evt)
        {
            StartNextWaveWithDelay(waveStartDelayInSeconds);
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
    }
}
