using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ArenaObstaclesManager : MonoBehaviour
    {
        [SerializeField] private GameObject floorContainer;

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
            StartHeatingLava(GetRandomFloorTilesIndexes(1, 3));
        }
    }
}
