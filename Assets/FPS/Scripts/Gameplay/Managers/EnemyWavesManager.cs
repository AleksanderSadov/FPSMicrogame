using System.Collections;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class EnemyWavesManager : MonoBehaviour
    {
        [Tooltip("Delay before wave starts in seconds")]
        public int waveStartDelayInSeconds = 1;

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

        // Start is called before the first frame update
        void Start()
        {
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
            Instantiate(hoverBotEnemy, Vector3.zero, hoverBotEnemy.transform.rotation, enemiesParent.transform);
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
    }
}
