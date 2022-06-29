using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    public class EnemyWavesManager : MonoBehaviour
    {
        public int waveStartDelayInSeconds = 1;

        [SerializeField] private int waveNumber = 1;
        [SerializeField] private GameObject objectivesParent;
        [SerializeField] private GameObject enemiesParent;
        [SerializeField] private GameObject killWaveObjective;
        [SerializeField] private GameObject hoverBotEnemy;
        [SerializeField] private GameObject turretEnemy;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(CreateNextWaveWithDelay(waveStartDelayInSeconds));
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator CreateNextWaveWithDelay(int delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);
            CreateNextWave();
        }

        void CreateNextWave()
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
            Instantiate(killWaveObjective, objectivesParent.transform);
        }
    }
}
