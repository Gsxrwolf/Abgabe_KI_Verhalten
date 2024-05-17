using UnityEngine;

[CreateAssetMenu(fileName = "SaveValues", menuName = "tools/saveValues")]
public class SaveFile : ScriptableObject
{
    [SerializeField] public float volumeMaster;
    [SerializeField] public float volumeMusic;
    [SerializeField] public float volumeSFX;
    [SerializeField] public float volumeUI;
}
