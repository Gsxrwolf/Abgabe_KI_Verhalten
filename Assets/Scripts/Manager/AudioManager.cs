using UnityEngine;
using UnityEngine.Audio;

public enum MyAudios
{

}
public enum MyAudioGroups
{
    Master,
    Music,
}
public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioMixer audioMixer;

    public void SetVolume(string _name, float _value)
    {
        audioMixer.SetFloat(_name, _value);
        switch (_name)
        {
            case "Master":
                {
                    GameManager.Instance.saveFile.volumeMaster = _value;
                    break;
                }
            case "Music":
                {
                    GameManager.Instance.saveFile.volumeMusic = _value;
                    break;
                }
        }
    }

    public AudioClip GetAudio(MyAudios _audio)
    {
        return audioClips[(int)_audio];
    }
}
