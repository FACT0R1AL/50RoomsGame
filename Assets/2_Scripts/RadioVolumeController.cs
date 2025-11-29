using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RadioVolumeController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource radioAudio;
    [Range(0f, 1f)]
    [SerializeField] private float maxUnityVolume = 1f;
    [Tooltip("볼륨이 바뀔 때마다 소리를 다시 재생할지 여부")]
    [SerializeField] private bool restartClipOnChange = false;

    [Header("Volume Sequence (1,2,3,4,5,0)")]
    [SerializeField] private int[] volumeSequence = new[] { 1, 2, 3, 4, 5, 0 };

    private int stepIndex;

    private void Reset()
    {
        radioAudio = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (radioAudio == null)
        {
            radioAudio = GetComponent<AudioSource>();
        }
    }

    /// <summary>
    /// F 키 입력 시 호출되어 다음 볼륨 단계를 적용한다.
    /// </summary>
    public void ApplyNextVolumeStep()
    {
        if (radioAudio == null || volumeSequence == null || volumeSequence.Length == 0)
        {
            return;
        }

        int currentValue = volumeSequence[stepIndex];
        stepIndex = (stepIndex + 1) % volumeSequence.Length;

        float normalizedVolume = currentValue <= 0
            ? 0f
            : Mathf.Clamp01(currentValue / 5f);

        radioAudio.volume = normalizedVolume * maxUnityVolume;

        if (radioAudio.volume <= 0f)
        {
            radioAudio.Stop();
        }
        else
        {
            if (restartClipOnChange)
            {
                radioAudio.Stop();
            }

            if (!radioAudio.isPlaying || restartClipOnChange)
            {
                radioAudio.Play();
            }
        }

        Debug.Log($"Radio volume set to step {currentValue} (unity volume {radioAudio.volume:0.00})");
    }
}

