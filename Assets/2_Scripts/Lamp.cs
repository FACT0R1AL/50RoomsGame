using UnityEngine;
using System.Collections;

public class Lamp : MonoBehaviour
{
    [Header("Lamp Objects")]
    public GameObject lampOn;        // 불 켜진 전등 모델
    public GameObject lampOff;       // 불 꺼진 전등 모델

    [Header("Sound (선택)")]
    public AudioSource flickerSound; // 지지직 효과음
    public float minPitch = 0.9f;    // 소리 피치 최소값
    public float maxPitch = 1.2f;    // 소리 피치 최대값
    public float minVolume = 0.6f;   // 볼륨 최소값
    public float maxVolume = 1.0f;   // 볼륨 최대값

    [Header("Timing (랜덤 범위)")]
    public float minDelay = 0.05f;   // 깜빡임 최소 간격
    public float maxDelay = 1.2f;    // 깜빡임 최대 간격
    public float minOffDuration = 0.05f; // 꺼짐 최소 지속시간
    public float maxOffDuration = 0.3f;  // 꺼짐 최대 지속시간

    private bool isFlickering = false;

    void Start()
    {
        // 초기 상태: 켜진 상태로 시작
        if (lampOn != null) lampOn.SetActive(true);
        if (lampOff != null) lampOff.SetActive(false);

        // 코루틴 시작
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        // 무한 반복
        while (true)
        {
            // 다음 깜빡임까지 랜덤 대기
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            if (!isFlickering)
            {
                StartCoroutine(FlickerOnce());
            }
        }
    }

    IEnumerator FlickerOnce()
    {
        isFlickering = true;

        // 전등 꺼짐
        lampOn.SetActive(false);
        lampOff.SetActive(true);

        // 소리 랜덤 피치·볼륨 적용 후 재생
        if (flickerSound != null)
        {
            flickerSound.pitch = Random.Range(minPitch, maxPitch);
            flickerSound.volume = Random.Range(minVolume, maxVolume);

            flickerSound.Stop(); // 혹시 재생 중이면 멈춤
            flickerSound.Play();
        }

        // 랜덤한 꺼짐 지속시간
        float offTime = Random.Range(minOffDuration, maxOffDuration);
        yield return new WaitForSeconds(offTime);

        // 다시 켜짐
        lampOn.SetActive(true);
        lampOff.SetActive(false);

        isFlickering = false;
    }
}