using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("BGM")]
    public AudioSource bgmSource;
    public AudioClip bgmClip;

    [Header("SFX Settings")]
    public int poolSize = 10;

    [Header("SFX Clips")]
    public List<AudioClip> sfxClips = new List<AudioClip>();

    private Dictionary<string, AudioClip> sfxDictionary;
    private AudioSource[] sfxSources;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SetupDictionary();
        InitPool();

        if (bgmClip != null)
            PlayBGM();
    }

    void SetupDictionary()
    {
        sfxDictionary = new Dictionary<string, AudioClip>();

        foreach (var clip in sfxClips)
        {
            if (clip != null && !sfxDictionary.ContainsKey(clip.name))
                sfxDictionary.Add(clip.name, clip);
        }
    }

    void InitPool()
    {
        sfxSources = new AudioSource[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = new GameObject($"SFX_AudioSource_{i}");
            obj.transform.parent = transform;

            var source = obj.AddComponent<AudioSource>();

            source.spatialBlend = 1f; // 3D sound
            source.rolloffMode = AudioRolloffMode.Logarithmic;

            sfxSources[i] = source;
        }
    }

    public void PlayBGM()
    {
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
        }

        bgmSource.clip = bgmClip;
        bgmSource.Play();
    }

    public void PlaySFX(string clipName, Vector3 position, bool loop = false, Transform followTarget = null)
    {
        if (!sfxDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            Debug.LogWarning($"Audio '{clipName}' not found in dictionary!");
            return;
        }
        
        AudioSource availableSource = null;
        
        foreach (var source in sfxSources)
        {
            if (!source.isPlaying)
            {
                availableSource = source;
                break;
            }
        }
        
        if (availableSource == null)
            availableSource = sfxSources[0];
        
        availableSource.clip = clip;
        availableSource.loop = loop;
        
        if (followTarget != null)
        {
            availableSource.transform.SetParent(followTarget);
            availableSource.transform.localPosition = Vector3.zero;
        }
        else
        {
            availableSource.transform.SetParent(null); // 독립 재생
        }
        
        availableSource.spatialBlend = 1f;
        availableSource.Play();
    }
    
    public void StopSFX(string name)
    {
        foreach (var source in sfxSources)
        {
            if (source.clip != null && source.clip.name == name)
            {
                source.Stop();
                source.loop = false;
            }
        }
    }

}
