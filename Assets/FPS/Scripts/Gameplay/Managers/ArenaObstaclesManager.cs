using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ArenaObstaclesManager : MonoBehaviour
    {
        [Tooltip("Minimum additional radius around random tile")]
        public int minRandomTileRadius = 1;

        [Tooltip("Maximum additional radius around random tile")]
        public int maxRandomTileRadius = 3;

        [Tooltip("Maximum number of heated lava tiles in arena." +
            " Exceeding this limit will start cooling random lava tiles to match limit")]
        public int maxHeatedTiles = 192;

        [Tooltip("Maximum number of lifted lava tiles in arena." +
            " Exceeding this limit will start lowering random lava tiles to match limit")]
        public int maxLiftedTiles = 64;

        [SerializeField] private GameObject floorContainer;
        [SerializeField] private List<Lava> heatedLavaList = new List<Lava>();
        [SerializeField] private List<Lava> liftedLavaList = new List<Lava>();

        // Start is called before the first frame update
        void Start()
        {
            EventManager.AddListener<WaveCompleteEvent>(OnWaveComplete);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void HeatupLava(List<int> tilesIndexes)
        {
            foreach (int index in tilesIndexes)
            {
                GameObject floorTile = floorContainer.transform.GetChild(index).gameObject;
                GameObject lavaMesh = floorTile.transform.GetChild(0).gameObject;
                Lava lava = lavaMesh.GetComponent<Lava>();
                lava.StartHeating();
                heatedLavaList.Add(lava);
            }

            if (heatedLavaList.Count > maxHeatedTiles)
            {
                int cooloffCount = heatedLavaList.Count - maxHeatedTiles;
                for (int i = 0; i < cooloffCount; i++)
                {
                    int randomIndex = Random.Range(0, heatedLavaList.Count - 1);
                    heatedLavaList[randomIndex].StartCooling();
                    heatedLavaList.RemoveAt(randomIndex);
                }
            }
        }

        private void LiftLava(List<int> tilesIndexes)
        {
            foreach (int index in tilesIndexes)
            {
                GameObject floorTile = floorContainer.transform.GetChild(index).gameObject;
                GameObject lavaMesh = floorTile.transform.GetChild(0).gameObject;
                Lava lava = lavaMesh.GetComponent<Lava>();
                lava.StartLifting(Random.Range(-2, 3));
                liftedLavaList.Add(lava);
            }

            if (liftedLavaList.Count > maxLiftedTiles)
            {
                int lowerDownCount = liftedLavaList.Count - maxLiftedTiles;
                for (int i = 0; i < lowerDownCount; i++)
                {
                    int randomIndex = Random.Range(0, liftedLavaList.Count - 1);
                    liftedLavaList[randomIndex].StartLowering();
                    liftedLavaList.RemoveAt(randomIndex);
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
            HeatupLava(GetRandomFloorTilesIndexes(minRandomTileRadius, maxRandomTileRadius));
            LiftLava(GetRandomFloorTilesIndexes(minRandomTileRadius, maxRandomTileRadius));
        }
    }
}
