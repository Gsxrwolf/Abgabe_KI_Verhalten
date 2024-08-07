using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SetTerrainObstaclesStatic : MonoBehaviour
{
    static TreeInstance[] Obstacles;
    static Terrain terrain;
    static bool isError;

    public static void GenerateTreeObstacles(Terrain _terrain, string[] _includedObstaclesNames, Vector3 _center = new Vector3(), Vector3 _size = new Vector3())
    {
        terrain = _terrain;
        Obstacles = terrain.terrainData.treeInstances;

        if (terrain.transform.childCount > 0)
        {
            List<Transform> allChilds = new List<Transform>();
            for (int k = 0; k < terrain.transform.childCount; k++)
            {
                allChilds.Add(terrain.transform.GetChild(k));
            }
            foreach (Transform child in allChilds)
            {
                if (child.name == "Terrain_Obstacles")
                {
                    DestroyImmediate(child.gameObject);
                    Debug.Log("Old NavMeshObstacles were succesfully deleted");
                }
            }
        }

        GameObject parent = new GameObject("Terrain_Obstacles");
        parent.transform.SetParent(terrain.transform);

        int i = 0;
        foreach (TreeInstance obstacle in Obstacles)
        {
            GameObject obstacleTerrainObject = terrain.terrainData.treePrototypes[obstacle.prototypeIndex].prefab;
            if (_includedObstaclesNames.Contains(obstacleTerrainObject.name))
            {

                Vector3 worldPosition = Vector3.Scale(obstacle.position, terrain.terrainData.size) + terrain.transform.position;
                if (CheckPos(worldPosition, _center, _size))
                {



                    Collider[] colliders = obstacleTerrainObject.GetComponents<Collider>();

                    if (colliders.Count() <= 0)
                    {
                        isError = true;
                        Debug.LogWarning("ERROR  There is no CapsuleCollider or BoxCollider attached to ''" + obstacleTerrainObject.name + "'' please add one of them.");
                        Destroy(parent);
                        break;
                    }

                    foreach (Collider collider in colliders)
                    {

                        Quaternion tempRot = Quaternion.AngleAxis(obstacle.rotation * Mathf.Rad2Deg, Vector3.up);

                        GameObject obstacleObject = new GameObject("Obstacle" + i);
                        obstacleObject.transform.SetParent(parent.transform);
                        obstacleObject.transform.position = worldPosition;
                        obstacleObject.transform.rotation = tempRot;


                        NavMeshObstacle obstacleComponent = obstacleObject.AddComponent<NavMeshObstacle>();
                        obstacleComponent.carving = true;
                        obstacleComponent.carveOnlyStationary = true;

                        if (collider.GetType() == typeof(CapsuleCollider))
                        {
                            CapsuleCollider capsuleColl = (CapsuleCollider)collider;
                            obstacleComponent.shape = NavMeshObstacleShape.Capsule;
                            obstacleComponent.center = capsuleColl.center;
                            obstacleComponent.radius = capsuleColl.radius;
                            obstacleComponent.height = capsuleColl.height;
                        }
                        else if (collider.GetType() == typeof(BoxCollider))
                        {
                            BoxCollider boxColl = (BoxCollider)collider;
                            obstacleComponent.shape = NavMeshObstacleShape.Box;
                            obstacleComponent.center = boxColl.center;
                            obstacleComponent.size = boxColl.size;
                        }
                    }

                    i++;
                }
            }
        }
        if (!isError) Debug.Log("All selectet NavMeshObstacles were succesfully added to your Scene");
    }

    private static bool CheckPos(Vector3 _pos, Vector3 _center, Vector3 _size)
    {
        if (_center == Vector3.zero && _size == Vector3.zero) return true;

        Vector3 halfSize = _size * 0.5f;
        Vector3 min = _center - halfSize;
        Vector3 max = _center + halfSize;

        return (_pos.x >= min.x && _pos.x <= max.x) && (_pos.y >= min.y && _pos.y <= max.y) && (_pos.z >= min.z && _pos.z <= max.z);
    }
}
