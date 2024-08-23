using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ObstaclesGenerator : MonoBehaviour
{
    static TreeInstance[] Obstacles;  // �bernommen aus dem Originalcode
    static Terrain terrain;           // �bernommen aus dem Originalcode
    static bool isError;              // �bernommen aus dem Originalcode

    public static void GenerateTreeObstacles(Terrain _terrain, string[] _includedObstaclesNames, Vector3 _center = new Vector3(), Vector3 _size = new Vector3())
    {
        terrain = _terrain;  // Angepasst vom Originalcode
        Obstacles = terrain.terrainData.treeInstances;  // �bernommen aus dem Originalcode

        // Neue Funktionalit�t hinzugef�gt (L�schen alter Hindernisse)
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

        GameObject parent = new GameObject("Terrain_Obstacles");  // �bernommen aus dem Originalcode
        parent.transform.SetParent(terrain.transform);  // Neue Funktionalit�t hinzugef�gt

        int i = 0;  // �bernommen aus dem Originalcode
        foreach (TreeInstance obstacle in Obstacles)  // �bernommen aus dem Originalcode
        {
            GameObject obstacleTerrainObject = terrain.terrainData.treePrototypes[obstacle.prototypeIndex].prefab;  // �bernommen aus dem Originalcode
            if (_includedObstaclesNames.Contains(obstacleTerrainObject.name))
            {
                Vector3 worldPosition = Vector3.Scale(obstacle.position, terrain.terrainData.size) + terrain.transform.position;  // �bernommen aus dem Originalcode

                if (CheckPos(worldPosition, _center, _size))  // Neue Funktionalit�t hinzugef�gt
                {
                    Collider[] colliders = obstacleTerrainObject.GetComponents<Collider>();  // �bernommen aus dem Originalcode, aber erweitert auf mehrere Collider

                    if (colliders.Count() <= 0)  // �bernommen aus dem Originalcode
                    {
                        isError = true;  // �bernommen aus dem Originalcode
                        Debug.LogWarning("ERROR  There is no CapsuleCollider or BoxCollider attached to ''" + obstacleTerrainObject.name + "'' please add one of them.");  // �bernommen aus dem Originalcode
                        Destroy(parent);  // Neue Funktionalit�t hinzugef�gt
                        break;  // �bernommen aus dem Originalcode
                    }

                    foreach (Collider collider in colliders)  // Neue Funktionalit�t hinzugef�gt
                    {
                        Quaternion tempRot = Quaternion.AngleAxis(obstacle.rotation * Mathf.Rad2Deg, Vector3.up);  // �bernommen aus dem Originalcode

                        GameObject obstacleObject = new GameObject("Obstacle" + i);  // �bernommen aus dem Originalcode
                        obstacleObject.transform.SetParent(parent.transform);  // �bernommen aus dem Originalcode
                        obstacleObject.transform.position = worldPosition;  // �bernommen aus dem Originalcode
                        obstacleObject.transform.rotation = tempRot;  // �bernommen aus dem Originalcode

                        NavMeshObstacle obstacleComponent = obstacleObject.AddComponent<NavMeshObstacle>();  // �bernommen aus dem Originalcode
                        obstacleComponent.carving = true;  // �bernommen aus dem Originalcode
                        obstacleComponent.carveOnlyStationary = true;  // �bernommen aus dem Originalcode

                        if (collider.GetType() == typeof(CapsuleCollider))  // �bernommen aus dem Originalcode
                        {
                            CapsuleCollider capsuleColl = (CapsuleCollider)collider;  // �bernommen aus dem Originalcode
                            obstacleComponent.shape = NavMeshObstacleShape.Capsule;  // �bernommen aus dem Originalcode
                            obstacleComponent.center = capsuleColl.center;  // �bernommen aus dem Originalcode
                            obstacleComponent.radius = capsuleColl.radius;  // �bernommen aus dem Originalcode
                            obstacleComponent.height = capsuleColl.height;  // �bernommen aus dem Originalcode
                        }
                        else if (collider.GetType() == typeof(BoxCollider))  // �bernommen aus dem Originalcode
                        {
                            BoxCollider boxColl = (BoxCollider)collider;  // �bernommen aus dem Originalcode
                            obstacleComponent.shape = NavMeshObstacleShape.Box;  // �bernommen aus dem Originalcode
                            obstacleComponent.center = boxColl.center;  // �bernommen aus dem Originalcode
                            obstacleComponent.size = boxColl.size;  // �bernommen aus dem Originalcode
                        }
                    }

                    i++;  // �bernommen aus dem Originalcode
                }
            }
        }
        if (!isError) Debug.Log("All selected NavMeshObstacles were succesfully added to your Scene");  // �bernommen aus dem Originalcode, leicht angepasst
    }

    private static bool CheckPos(Vector3 _pos, Vector3 _center, Vector3 _size)  // Neue Funktionalit�t hinzugef�gt
    {
        if (_center == Vector3.zero && _size == Vector3.zero) return true;

        Vector3 halfSize = _size * 0.5f;
        Vector3 min = _center - halfSize;
        Vector3 max = _center + halfSize;

        return (_pos.x >= min.x && _pos.x <= max.x) && (_pos.y >= min.y && _pos.y <= max.y) && (_pos.z >= min.z && _pos.z <= max.z);
    }
}