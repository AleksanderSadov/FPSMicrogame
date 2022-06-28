using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{
    public class EnemyWavesManager : MonoBehaviour
    {
        [SerializeField] private int waveNumber = 1;
        [SerializeField] private GameObject hoverBotEnemy;
        [SerializeField] private GameObject turretEnemy;

        // Start is called before the first frame update
        void Start()
        {
            Instantiate(hoverBotEnemy, new Vector3(0, 0, 0), hoverBotEnemy.transform.rotation);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
