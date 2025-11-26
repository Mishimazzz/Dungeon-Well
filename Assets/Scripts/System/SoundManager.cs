using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource audioSource;

    [SerializeField] private AudioClip clockUp;
    [SerializeField] private AudioClip clockDown;
    [SerializeField] private AudioClip planted;
    [SerializeField] private AudioClip harvest;
    [SerializeField] private AudioClip bagButton;
    [SerializeField] private AudioClip StartTime;
    [SerializeField] private AudioClip ToDoList;
    [SerializeField] private AudioClip SwitchScene;

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
    public void PlayPlanted() => PlaySound(planted);
    public void PlayHarvest() => PlaySound(harvest);
    public void PlayBagButton() => PlaySound(bagButton);
    public void PlayStartTime() => PlaySound(StartTime);
    public void PlayToDoList() => PlaySound(ToDoList);
    public void PlaySwitchScene() => PlaySound(SwitchScene);
}
