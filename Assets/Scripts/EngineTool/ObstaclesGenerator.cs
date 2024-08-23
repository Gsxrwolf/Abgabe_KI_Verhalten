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

    public static void GenerateTreeObstacles(Terrain _terrain, string[] _includedObstaclesNames, Vector3 _center = new Vector3(), Vector3 _size = new Vector3()) // Neue Funktionalit�t hinzugef�gt (Aufrufbar aus EditorWindow)
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

                    if (colliders.Count() <= 0) 
                    {
                        isError = true;  
                        Debug.LogWarning("ERROR  There is no CapsuleCollider or BoxCollider attached to ''" + obstacleTerrainObject.name + "'' please add one of them.");  
                        Destroy(parent);  
                        break;  
                    }

                    foreach (Collider collider in colliders)  // Gesamte foreach aus Originalcode und nur leicht angepasst (foreach hinzugef�gt wegen mehreren collidern)
                    {
                        Quaternion tempRot = Quaternion.AngleAxis(obstacle.rotation * Mathf.Rad2Deg, Vector3.up);

                        GameObject obstacleObject = new GameObject("Obstacle" + i); 
                        obstacleObject.transform.SetParent(parent.transform);  
                        obstacleObject.transform.position = worldPosition;  
                        obstacleObject.transform.rotation = tempRot;  

                        NavMeshObstacle obstacleComponent = obstacleObject.AddComponent<NavMeshObstacle>();  // Leicht angepasst
                        obstacleComponent.carving = true;  
                        obstacleComponent.carveOnlyStationary = true;  

                        if (collider.GetType() == typeof(CapsuleCollider))  // If ganz aus dem Originalcode �bernommen
                        {
                            CapsuleCollider capsuleColl = (CapsuleCollider)collider; 
                            obstacleComponent.shape = NavMeshObstacleShape.Capsule;  
                            obstacleComponent.center = capsuleColl.center;  
                            obstacleComponent.radius = capsuleColl.radius;  
                            obstacleComponent.height = capsuleColl.height;  
                        }
                        else if (collider.GetType() == typeof(BoxCollider))  // If ganz aus dem Originalcode �bernommen
                        {
                            BoxCollider boxColl = (BoxCollider)collider; 
                            obstacleComponent.shape = NavMeshObstacleShape.Box; 
                            obstacleComponent.center = boxColl.center; 
                            obstacleComponent.size = boxColl.size; 
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