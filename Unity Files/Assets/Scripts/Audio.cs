using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [Header("SFX")]
    public AudioSource sfxSource;

    [Header("Music")]
    public AudioSource musicSource;

    //  Jump sound cooldown system
    private float jumpSoundCooldown = 0f;
    public float jumpSoundDelay = 0.2f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // safety check: prevents missing assignment bugs
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        sfxSource.playOnAwake = false;
        musicSource.playOnAwake = false;

        sfxSource.loop = false;
        musicSource.loop = false;

        sfxSource.spatialBlend = 0f;   // 2D sound
        musicSource.spatialBlend = 0f;
    }

    void Update()
    {
        // reduce jump cooldown over time
        if (jumpSoundCooldown > 0f)
        {
            jumpSoundCooldown -= Time.deltaTime;
        }
    }


    // SFX (generic)

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip, volume);
    }


    // JUMP SFX (anti-spam)

    public void PlayJumpSFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        // block spam
        if (jumpSoundCooldown > 0f) return;

        sfxSource.PlayOneShot(clip, volume);
        jumpSoundCooldown = jumpSoundDelay;
    }


    // MUSIC

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}