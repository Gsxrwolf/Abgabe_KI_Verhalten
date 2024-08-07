using UnityEngine;

public class SelectionVolumeGizmos : MonoBehaviour
{
    public Vector3 center;
    public Vector3 size;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, size);
    }
}
