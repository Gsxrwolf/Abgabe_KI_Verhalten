using UnityEngine;
using UnityEngine.Audio;

public enum MyAudios
{

}
public enum MyAudioGroups
{
    Master,
    SFX,
    UI,
    Background
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

    [SerializeField] private AudioSource[] audioSorces;
    [SerializeField] private AudioMixer audioMixer;

    public void PlayAudio(MyAudios _audio)
    {
        audioSorces[(int)_audio].Play();
    }
    public void StopAudio(MyAudios _audio)
    {
        audioSorces[(int)_audio].Stop();
    }
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
            case "SFX":
                {
                    GameManager.Instance.saveFile.volumeSFX = _value;
                    break;
                }
            case "UI":
                {
                    GameManager.Instance.saveFile.volumeUI = _value;
                    break;
                }
        }
    }

    public AudioSource GetAudio(MyAudios _audio)
    {
        return audioSorces[(int)_audio];
    }
}
