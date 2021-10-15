using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JelleVer.CityGenerator
{
    public class CityGenerator : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField]
        private GameObject buildingPrefab;
        [SerializeField]
        private GameObject borderPrefab;
        [SerializeField]
        private Material[] materials;

        [Header("Parameters")]
        [SerializeField]
        private bool updateGrid;
        [SerializeField]
        private bool spawnFloor, spawnLines, SpawnBorder, spawnPavement;
        [SerializeField]
        private Vector3 noiseStartPos = Vector3.zero;
        [SerializeField]
        private Vector3 noiseScale = Vector3.one;
        [SerializeField]
        private Vector2 minBuildingArea;
        [SerializeField]
        private Vector2 maxBuildingArea;
        [SerializeField]
        private float roadLineOffset = 0.1f;
        [SerializeField]
        [Range(0, 5)]
        private float fallofSteepness = 1;
        [SerializeField]
        private Vector2 gridSpacing = new Vector2(2, 2);
        [SerializeField]
        private Vector2Int gridSize = Vector2Int.one;
        [SerializeField]
        [Tooltip("x = borderwidth, y = BorderHeight, z = OuterDownOffset")]
        private Vector3 BorderDimensions = Vector3.one;


        private List<BuildingGenerator> buildings = new List<BuildingGenerator>();
        private List<GameObject> tiles = new List<GameObject>();
        private List<LineRenderer> roadLines = new List<LineRenderer>();
        private Transform cityFloor;
        private BorderGenerator border;
        private Vector2 citySize;

        // Start is called before the first frame update
        void Start()
        {
            CreateCity();
        }

        // Update is called once per frame
        void Update()
        {

            if (updateGrid) UpdateGrid();
        }

        public void UpdateGrid()
        {
            citySize = new Vector2((gridSize.x - 1) * gridSpacing.x, (gridSize.y - 1) * gridSpacing.y);

            if (tiles.Count > gridSize.x * gridSize.y)
            {
                for (int i = Mathf.RoundToInt(gridSize.x * gridSize.y); i < tiles.Count; i++)
                {
                    GameObject oldObj = tiles[i];
                    tiles.RemoveAt(i);
                    Destroy(oldObj);

                    GameObject oldBuilding = buildings[i].gameObject;
                    buildings.RemoveAt(i);
                    Destroy(oldBuilding);
                }
            }
            else if (tiles.Count < gridSize.x * gridSize.y)
            {
                for (int i = tiles.Count; i < gridSize.x * gridSize.y; i++)
                {
                    Debug.Log("Adding building nr: " + tiles.Count);

                    PlaceBlock(Vector3.zero, new Vector3(Random.Range(minBuildingArea.x, maxBuildingArea.x), 1, Random.Range(minBuildingArea.y, maxBuildingArea.y)));

                }
            }

            if (cityFloor) cityFloor.localScale = new Vector3((gridSize.x) * gridSpacing.x, 1, (gridSize.y) * gridSpacing.y) / 10f;
            if (border) border.UpdateSize(new Vector3((gridSize.x) * gridSpacing.x, BorderDimensions.y, (gridSize.y) * gridSpacing.y), new Vector2(BorderDimensions.x, BorderDimensions.z));

            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    int curNr = i * (int)gridSize.y + j;

                    if (tiles.Count < curNr)
                    {
                        PlaceBlock(Vector3.zero, Vector3.one);
                    }
                    if (!buildings[curNr]) continue;

                    Vector3 position = new Vector3(gridSpacing.x * i - citySize.x / 2f, transform.position.y, gridSpacing.y * j - citySize.y / 2f);
                    float distanceFactor = Mathf.Pow(1 - position.magnitude / (citySize.magnitude / 2f), fallofSteepness);
                    float noiseValue = Mathf.PerlinNoise(noiseStartPos.x + (position.x * noiseScale.x), noiseStartPos.z + (position.z * noiseScale.z)) * noiseScale.y * distanceFactor + noiseStartPos.y;
                    Vector3 size = new Vector3(buildings[curNr].buildingSize.x, noiseValue, buildings[curNr].buildingSize.z);

                    tiles[curNr].transform.position = position;
                    buildings[curNr].transform.position = position;
                    buildings[curNr].UpdateSize(size);
                }
            }

            if (spawnLines) SetRoadLinePosition();


        }

        void CreateCity()
        {
            citySize = new Vector2((gridSize.x - 1) * gridSpacing.x, (gridSize.y - 1) * gridSpacing.y);

            if (spawnFloor)
            {
                cityFloor = GameObject.CreatePrimitive(PrimitiveType.Plane).transform;
                cityFloor.position = transform.position;
                cityFloor.localScale = new Vector3((gridSize.x) * gridSpacing.x, 1, (gridSize.y) * gridSpacing.y) / 10f;
            }
            if (SpawnBorder)
            {
                border = Instantiate(borderPrefab, transform).GetComponent<BorderGenerator>();
                border.UpdateSize(new Vector3((gridSize.x) * gridSpacing.x, BorderDimensions.y, (gridSize.y) * gridSpacing.y), new Vector2(BorderDimensions.x, BorderDimensions.z));
            }

            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    int curNr = i * (int)gridSize.y + j;

                    Vector3 position = new Vector3(gridSpacing.x * i - citySize.x / 2f, transform.position.y, gridSpacing.y * j - citySize.y / 2f);
                    float distanceFactor = Mathf.Pow(1 - position.magnitude / (citySize.magnitude / 2f), fallofSteepness);
                    float noiseValue = Mathf.PerlinNoise(noiseStartPos.x + (position.x * noiseScale.x), noiseStartPos.z + (position.z * noiseScale.z)) * noiseScale.y * distanceFactor + noiseStartPos.y;
                    Vector3 size = new Vector3(Random.Range(minBuildingArea.x, maxBuildingArea.x), noiseValue, Random.Range(minBuildingArea.y, maxBuildingArea.y));

                    PlaceBlock(position, size);

                    Debug.Log("Buildingnr: " + curNr + ", has height: " + noiseValue);
                }
            }

            if (spawnLines) SetRoadLinePosition();
        }

        void PlaceBlock(Vector3 pos, Vector3 size)
        {
            GameObject newTile = GameObject.CreatePrimitive(PrimitiveType.Plane);
            newTile.transform.position = pos;
            newTile.transform.localScale = size / 10f;
            tiles.Add(newTile);
            buildings.Add(Instantiate(buildingPrefab, pos, transform.rotation, tiles[tiles.Count - 1].transform).GetComponent<BuildingGenerator>());

            buildings[buildings.Count - 1].buildingSize = size;
            buildings[buildings.Count - 1].SetMaterial(materials[Random.Range(0, materials.Length)]);

        }

        void SetRoadLinePosition()
        {
            /*
            Vector2 lineGridSize = new Vector2((gridSize.x - 2) * gridSpacing.x, (gridSize.y - 2) * gridSpacing.y);
            citySize = new Vector2((gridSize.x) * gridSpacing.x, (gridSize.y) * gridSpacing.y);

            if (roadLines.Count < (gridSize.x - 1) * (gridSize.y - 1))
            {
                for (int i = roadLines.Count; i < (gridSize.x - 1) * (gridSize.y - 1); i++)
                {
                    roadLines.Add(Instantiate(roadLinePrefab, transform).GetComponent<LineRenderer>());
                }
            }
            else if (roadLines.Count > (gridSize.x - 1) * (gridSize.y - 1))
            {
                for (int i = (gridSize.x - 1) * (gridSize.y - 1); i < roadLines.Count; i++)
                {
                    GameObject oldLine = roadLines[i].gameObject;
                    roadLines.RemoveAt(i);
                    Destroy(oldLine);
                }
            }

            for (int i = 0; i < (gridSize.x - 1); i++)
            {
                Vector3 pos = new Vector3(gridSpacing.x * i - lineGridSize.x / 2f, 0, -roadLineOffset); //gridSpacing.x * i - citySize.x / 2f, transform.position.y, gridSpacing.y * j - citySize.y / 2f

                roadLines[i].positionCount = 2;
                roadLines[i].SetPosition(0, pos + Vector3.up * citySize.y / 2f); //set in local space, x = x, y = z
                roadLines[i].SetPosition(1, pos + Vector3.down * citySize.y / 2f);
            }

            for (int i = 0; i < (gridSize.y - 1); i++)
            {
                Vector3 pos = new Vector3(0, gridSpacing.y * i - lineGridSize.y / 2f, -roadLineOffset); //gridSpacing.x * i - citySize.x / 2f, transform.position.y, gridSpacing.y * j - citySize.y / 2f

                roadLines[i + gridSize.x - 1].positionCount = 2;
                roadLines[i + gridSize.x - 1].SetPosition(0, pos + Vector3.right * citySize.x / 2f); //set in local space, x = x, y = z
                roadLines[i + gridSize.x - 1].SetPosition(1, pos + Vector3.left * citySize.x / 2f);
            }
            */


        }

    }
}