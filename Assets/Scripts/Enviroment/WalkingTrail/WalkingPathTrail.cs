using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WalkingPathTrail : MonoBehaviour
{
    [SerializeField] private Terrain terrain;
    [SerializeField] private TerrainData backUpTerrainData;
    [SerializeField] private float trailRadius = 1.0f;
    [SerializeField] private float recoveryDelay = 5.0f;
    [SerializeField] private float recoveryDuration = 3.0f;

    [SerializeField, Range(0, 255)] private int trailGrassHeight;
    [SerializeField, Range(0, 255)] public static int normalGrassHeight = 255;

    private TerrainData terrainData;
    private List<TrailRecord> trailRecords = new List<TrailRecord>();

    private void Start()
    {
        if (terrain == null)
            terrain = FindObjectsOfType<Terrain>().First();
        terrainData = terrain.terrainData;
    }

    private void Update()
    {
        Vector3 curPos = transform.position;
        FlattenGrassAtPosition(curPos);
    }

    private void FlattenGrassAtPosition(Vector3 position)
    {
        Vector3 terrainPos = terrain.transform.position;
        int mapX = (int)((position.x - terrainPos.x) / terrainData.size.x * terrainData.detailWidth);
        int mapZ = (int)((position.z - terrainPos.z) / terrainData.size.z * terrainData.detailHeight);

        int radius = Mathf.RoundToInt(trailRadius * terrainData.detailResolution / terrainData.size.x);

        int[,] details = terrainData.GetDetailLayer(mapX - radius, mapZ - radius, radius * 2, radius * 2, 0);

        trailRecords.Add(new TrailRecord(mapX - radius, mapZ - radius, radius, details.Clone() as int[,]));


        for (int x = 0; x < radius * 2; x++)
        {
            for (int z = 0; z < radius * 2; z++)
            {
                details[x, z] = trailGrassHeight;
            }
        }

        terrainData.SetDetailLayer(mapX - radius, mapZ - radius, 0, details);

        StartCoroutine(RecoverGrass(mapX, mapZ, radius, details));
    }

    private IEnumerator RecoverGrass(int mapX, int mapZ, int radius, int[,] details)
    {
        yield return new WaitForSeconds(recoveryDelay);

        float elapsedTime = 0f;
        float recoveryFraction = (normalGrassHeight - trailGrassHeight) / recoveryDuration;

        while (elapsedTime < recoveryDuration)
        {
            elapsedTime += Time.deltaTime;
            float increment = recoveryFraction * Time.deltaTime;

            for (int x = 0; x < radius * 2; x++)
            {
                for (int z = 0; z < radius * 2; z++)
                {
                    details[x, z] = Mathf.Clamp(details[x, z] + Mathf.RoundToInt(increment), trailGrassHeight, normalGrassHeight);
                }
            }

            terrainData.SetDetailLayer(mapX - radius, mapZ - radius, 0, details);

            yield return null;
        }

        trailRecords.RemoveAll(r => r.mapX == mapX && r.mapZ == mapZ);
    }

    private void OnDisable()
    {
        if (backUpTerrainData != null)
        {
            for (int i = 0; i < terrainData.detailPrototypes.Length; i++)
            {
                terrainData.SetDetailLayer(0, 0, i, backUpTerrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, i));
            }
        }
    }
}
