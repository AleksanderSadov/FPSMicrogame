using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ArenaObstaclesManager : MonoBehaviour
    {
        [Tooltip("Minimum additional radius around random lava tile")]
        public int minLavaPuddleSize = 1;

        [Tooltip("Maximum additional radius around random lava tile")]
        public int maxLavaPuddleSize = 3;

        [Tooltip("Maximum number of lava tiles in arena." +
            " Exceeding this limit will start cooling random lava tiles to match limit")]
        public int maxLavaTiles = 300;

        [SerializeField] private GameObject floorContainer;
        [SerializeField] private List<Lava> heatedLavaList = new List<Lava>();

        // Start is called before the first frame update
        void Start()
        {
            EventManager.AddListener<WaveCompleteEvent>(OnWaveComplete);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void StartHeatingLava(List<int> tilesIndexes)
        {
            foreach (int index in tilesIndexes)
            {
                GameObject floorTile = floorContainer.transform.GetChild(index).gameObject;
                GameObject lavaMesh = floorTile.transform.GetChild(0).gameObject;
                Lava lava = lavaMesh.GetComponent<Lava>();
                lava.StartHeating();
                heatedLavaList.Add(lava);
            }

            if (heatedLavaList.Count > maxLavaTiles)
            {
                int cooloffCount = heatedLavaList.Count - maxLavaTiles;
                for (int i = 0; i < cooloffCount; i++)
                {
                    int randomIndex = Random.Range(0, heatedLavaList.Count - 1);
                    heatedLavaList[randomIndex].StartCooling();
                    heatedLavaList.RemoveAt(randomIndex);
                }
            }
        }

        private List<int> GetRandomFloorTilesIndexes(int minNeighbourRange, int maxNeighbourRange)
        {
            List<int> randomTilesIndexes = new List<int>();

            int floorChildsCount = floorContainer.transform.childCount;
            int randomTileIndex = Random.Range(0, floorChildsCount - 1);
            int randomNeighbourRange = Random.Range(minNeighbourRange, maxNeighbourRange);

            for (int i = -randomNeighbourRange; i <= randomNeighbourRange; i++)
            {
                int neighbourSizeTileIndex = randomTileIndex + i;
                if (neighbourSizeTileIndex > 0 && neighbourSizeTileIndex < floorChildsCount)
                {
                    randomTilesIndexes.Add(neighbourSizeTileIndex);
                }
            }

            return randomTilesIndexes;
        }

        private void OnWaveComplete(WaveCompleteEvent evt)
        {
            StartHeatingLava(GetRandomFloorTilesIndexes(minLavaPuddleSize, maxLavaPuddleSize));
        }
    }
}
