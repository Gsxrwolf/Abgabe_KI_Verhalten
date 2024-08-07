using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObstaclesGeneratorWindow : EditorWindow
{
    private Terrain terrain;
    private Terrain lastTerrain;
    private List<string> obstacleNames;
    private List<string> selectedObstacles;

    private bool useVolume = false;

    private GameObject selectionObject;
    private SelectionVolumeGizmos gizmo;
    private Vector3 center = Vector3.zero;
    private Vector3 size = Vector3.zero;

    [MenuItem("Window/Terrain Obstacles Generator")]
    public static void OpenWindow()
    {
        ObstaclesGeneratorWindow window = (ObstaclesGeneratorWindow)EditorWindow.GetWindow<ObstaclesGeneratorWindow>("Terrain Obstacles Generator");
        window.minSize = new Vector2(250, 250);
        window.Show();
    }
    private void OnBecameInvisible()
    {
        if (useVolume)
        {
            DestroyImmediate(selectionObject);
            selectionObject = null;
            center = Vector3.zero;
            size = Vector3.zero;
        }
    }
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.Space();

            // Terrain field
            EditorGUILayout.LabelField("Terrain", EditorStyles.boldLabel);
            terrain = (Terrain)EditorGUILayout.ObjectField(terrain, typeof(Terrain), true);
            if (terrain != lastTerrain)
            {
                obstacleNames = null;
                selectedObstacles = null;
            }
            if (terrain == null)
            {
                obstacleNames = null;
                selectedObstacles = null;
            }
            else
            {
                EditorGUILayout.Space();

                // Get obstacle names from the terrain
                if (obstacleNames == null)
                {
                    obstacleNames = new List<string>();
                    foreach (var treePrototype in terrain.terrainData.treePrototypes)
                    {
                        obstacleNames.Add(treePrototype.prefab.name);
                    }
                    selectedObstacles = new List<string>();
                }

                // Dropdown for selecting obstacles
                EditorGUILayout.LabelField("Obstacles to take into account", EditorStyles.boldLabel);

                if (EditorGUILayout.DropdownButton(new GUIContent("Select Obstacles"), FocusType.Keyboard))
                {
                    GenericMenu menu = new GenericMenu();

                    // Option to select all
                    menu.AddItem(new GUIContent("Everything"), false, () =>
                    {
                        selectedObstacles.Clear();
                        selectedObstacles.AddRange(obstacleNames);
                    });

                    // Option to deselect all
                    menu.AddItem(new GUIContent("Nothing"), false, () =>
                    {
                        selectedObstacles.Clear();
                    });

                    menu.AddSeparator("");

                    // Options to select individual obstacles
                    foreach (var obstacleName in obstacleNames)
                    {
                        bool isSelected = selectedObstacles.Contains(obstacleName);
                        menu.AddItem(new GUIContent(obstacleName), isSelected, () =>
                        {
                            if (isSelected)
                            {
                                selectedObstacles.Remove(obstacleName);
                            }
                            else
                            {
                                selectedObstacles.Add(obstacleName);
                            }
                        });
                    }

                    menu.ShowAsContext();
                }

                EditorGUILayout.Space();

                // Display selected obstacles
                if (selectedObstacles.Count > 0)
                {
                    EditorGUILayout.LabelField("Selected Obstacles:", EditorStyles.boldLabel);
                    foreach (var obstacle in selectedObstacles)
                    {
                        EditorGUILayout.LabelField(obstacle);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("No obstacles selected.");
                }

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Use selection volume", EditorStyles.boldLabel);
                    useVolume = EditorGUILayout.Toggle(useVolume);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                if (useVolume)
                {
                    if (selectionObject == null)
                    {
                        selectionObject = new GameObject("SelectionVolumeGizmo");
                        selectionObject.transform.SetParent(terrain.transform);
                        gizmo = selectionObject.AddComponent<SelectionVolumeGizmos>();
                    }

                    center = EditorGUILayout.Vector3Field("Center", center);
                    size = EditorGUILayout.Vector3Field("Size", size);

                    if (gizmo != null)
                    {
                        gizmo.center = center;
                        gizmo.size = size;
                    }
                }
                else
                {
                    if (selectionObject != null)
                    {
                        DestroyImmediate(selectionObject);
                        selectionObject = null;
                        center = Vector3.zero;
                        size = Vector3.zero;
                    }
                }
            }
            lastTerrain = terrain;

            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.Space(20);
                if (GUILayout.Button("Generate Obstacles"))
                {
                    if (terrain == null)
                    {
                        Debug.LogError("Please assign a terrain.");
                        return;
                    }
                    else if (selectedObstacles.Count <= 0)
                    {
                        Debug.LogError("Please select Obstacles.");
                        return;
                    }

                    SetTerrainObstaclesStatic.GenerateTreeObstacles(terrain, selectedObstacles.ToArray(), center, size);
                }
                EditorGUILayout.Space(20);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }


}
