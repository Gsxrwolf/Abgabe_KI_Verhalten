using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ObstaclesGenerator : MonoBehaviour
{
    static TreeInstance[] Obstacles;  // Übernommen aus dem Originalcode
    static Terrain terrain;           // Übernommen aus dem Originalcode
    static bool isError;              // Übernommen aus dem Originalcode

    public static void GenerateTreeObstacles(Terrain _terrain, string[] _includedObstaclesNames, Vector3 _center = new Vector3(), Vector3 _size = new Vector3())
    {
        terrain = _terrain;  // Angepasst vom Originalcode
        Obstacles = terrain.terrainData.treeInstances;  // Übernommen aus dem Originalcode

        // Neue Funktionalität hinzugefügt (Löschen alter Hindernisse)
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

        GameObject parent = new GameObject("Terrain_Obstacles");  // Übernommen aus dem Originalcode
        parent.transform.SetParent(terrain.transform);  // Neue Funktionalität hinzugefügt

        int i = 0;  // Übernommen aus dem Originalcode
        foreach (TreeInstance obstacle in Obstacles)  // Übernommen aus dem Originalcode
        {
            GameObject obstacleTerrainObject = terrain.terrainData.treePrototypes[obstacle.prototypeIndex].prefab;  // Übernommen aus dem Originalcode
            if (_includedObstaclesNames.Contains(obstacleTerrainObject.name))
            {
                Vector3 worldPosition = Vector3.Scale(obstacle.position, terrain.terrainData.size) + terrain.transform.position;  // Übernommen aus dem Originalcode

                if (CheckPos(worldPosition, _center, _size))  // Neue Funktionalität hinzugefügt
                {
                    Collider[] colliders = obstacleTerrainObject.GetComponents<Collider>();  // Übernommen aus dem Originalcode, aber erweitert auf mehrere Collider

                    if (colliders.Count() <= 0)  // Übernommen aus dem Originalcode
                    {
                        isError = true;  // Übernommen aus dem Originalcode
                        Debug.LogWarning("ERROR  There is no CapsuleCollider or BoxCollider attached to ''" + obstacleTerrainObject.name + "'' please add one of them.");  // Übernommen aus dem Originalcode
                        Destroy(parent);  // Neue Funktionalität hinzugefügt
                        break;  // Übernommen aus dem Originalcode
                    }

                    foreach (Collider collider in colliders)  // Neue Funktionalität hinzugefügt
                    {
                        Quaternion tempRot = Quaternion.AngleAxis(obstacle.rotation * Mathf.Rad2Deg, Vector3.up);  // Übernommen aus dem Originalcode

                        GameObject obstacleObject = new GameObject("Obstacle" + i);  // Übernommen aus dem Originalcode
                        obstacleObject.transform.SetParent(parent.transform);  // Übernommen aus dem Originalcode
                        obstacleObject.transform.position = worldPosition;  // Übernommen aus dem Originalcode
                        obstacleObject.transform.rotation = tempRot;  // Übernommen aus dem Originalcode

                        NavMeshObstacle obstacleComponent = obstacleObject.AddComponent<NavMeshObstacle>();  // Übernommen aus dem Originalcode
                        obstacleComponent.carving = true;  // Übernommen aus dem Originalcode
                        obstacleComponent.carveOnlyStationary = true;  // Übernommen aus dem Originalcode

                        if (collider.GetType() == typeof(CapsuleCollider))  // Übernommen aus dem Originalcode
                        {
                            CapsuleCollider capsuleColl = (CapsuleCollider)collider;  // Übernommen aus dem Originalcode
                            obstacleComponent.shape = NavMeshObstacleShape.Capsule;  // Übernommen aus dem Originalcode
                            obstacleComponent.center = capsuleColl.center;  // Übernommen aus dem Originalcode
                            obstacleComponent.radius = capsuleColl.radius;  // Übernommen aus dem Originalcode
                            obstacleComponent.height = capsuleColl.height;  // Übernommen aus dem Originalcode
                        }
                        else if (collider.GetType() == typeof(BoxCollider))  // Übernommen aus dem Originalcode
                        {
                            BoxCollider boxColl = (BoxCollider)collider;  // Übernommen aus dem Originalcode
                            obstacleComponent.shape = NavMeshObstacleShape.Box;  // Übernommen aus dem Originalcode
                            obstacleComponent.center = boxColl.center;  // Übernommen aus dem Originalcode
                            obstacleComponent.size = boxColl.size;  // Übernommen aus dem Originalcode
                        }
                    }

                    i++;  // Übernommen aus dem Originalcode
                }
            }
        }
        if (!isError) Debug.Log("All selected NavMeshObstacles were succesfully added to your Scene");  // Übernommen aus dem Originalcode, leicht angepasst
    }

    private static bool CheckPos(Vector3 _pos, Vector3 _center, Vector3 _size)  // Neue Funktionalität hinzugefügt
    {
        if (_center == Vector3.zero && _size == Vector3.zero) return true;

        Vector3 halfSize = _size * 0.5f;
        Vector3 min = _center - halfSize;
        Vector3 max = _center + halfSize;

        return (_pos.x >= min.x && _pos.x <= max.x) && (_pos.y >= min.y && _pos.y <= max.y) && (_pos.z >= min.z && _pos.z <= max.z);
    }
}