using UnityEngine;

public class FurGenerator : MonoBehaviour
{
    [SerializeField] private GameObject furPrefab;
    [SerializeField] private float furDensity = 10.0f;
    [SerializeField] private float furLeangth = 0.5f;

    [HideInInspector] public bool disableFur = false;

    [SerializeField] private Texture2D furTexture;
    [SerializeField] private Color furColor;
    [SerializeField] private float furCurveStrength;
    [SerializeField] private Vector2 furWindMovement;
    [SerializeField] private float furWindDensity;
    [SerializeField] private float furWindStrength;

    void Start()
    {
        GetComponent<MeshRenderer>().material.color = furColor;

        if (disableFur) return;

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3[] normals = mesh.normals;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            int index0 = triangles[i];
            int index1 = triangles[i + 1];
            int index2 = triangles[i + 2];

            Vector3 vertex0 = vertices[index0];
            Vector3 vertex1 = vertices[index1];
            Vector3 vertex2 = vertices[index2];

            Vector3 normal0 = normals[index0];
            Vector3 normal1 = normals[index1];
            Vector3 normal2 = normals[index2];

            GenerateFurOnEdge(vertex0, vertex1, normal0, normal1);
            GenerateFurOnEdge(vertex1, vertex2, normal1, normal2);
            GenerateFurOnEdge(vertex2, vertex0, normal2, normal0);
        }
    }

    void GenerateFurOnEdge(Vector3 _v0, Vector3 v_1, Vector3 _n0, Vector3 _n1)
    {
        float edgeLength = Vector3.Distance(_v0, v_1);

        int adjustedDensity = Mathf.Max(1, Mathf.RoundToInt(edgeLength * furDensity));

        for (int i = 0; i <= adjustedDensity; i++)
        {
            float t = i / (float)adjustedDensity;
            Vector3 position = Vector3.Lerp(_v0, v_1, t);
            Vector3 normal = Vector3.Lerp(_n0, _n1, t).normalized;

            Vector3 worldPosition = transform.TransformPoint(position);
            Vector3 worldNormal = transform.TransformDirection(normal);

            GameObject furPiece = Instantiate(furPrefab, worldPosition, Quaternion.LookRotation(worldNormal), transform);
            Material mat = furPiece.transform.GetChild(0).GetComponent<MeshRenderer>().material;

            mat.SetTexture("_FurTexture", furTexture);
            mat.SetColor("_FurColor", furColor);
            mat.SetFloat("_CurveStrength", furCurveStrength);
            mat.SetVector("_WindMovement",furWindMovement);
            mat.SetFloat("_WindDensity", furWindDensity);
            mat.SetFloat("_WindStrength", furWindStrength);

            Vector3 temp = furPiece.transform.localScale;
            temp.y = furLeangth;
            furPiece.transform.localScale = temp;

            System.Random rnd = new System.Random();
            float xRandom = rnd.Next(-5, 5);
            float zRandom = rnd.Next(-5, 5);
            furPiece.transform.Rotate(xRandom, 190, zRandom);
        }
    }
}