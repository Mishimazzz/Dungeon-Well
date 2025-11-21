using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource audioSource;

    [SerializeField] private AudioClip clockUp;
    [SerializeField] private AudioClip clockDown;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayClockUp() => PlaySound(clockUp);
    public void PlayClockDown() => PlaySound(clockDown);
}
