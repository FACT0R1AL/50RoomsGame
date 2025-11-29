using UnityEngine;

public class HandShakeCamera : MonoBehaviour
{
    [Header("Shake Settings")]
    public float idleIntensity = 0.015f;   // 가만히 있을 때 흔들림
    public float moveIntensity = 0.2f;    // 움직일 때 흔들림
    public float shakeFrequency = 70f;    // 초당 흔들림 횟수

    [Header("Player Reference")]
    public PlayerMovement playerMovement;  // 이동 감지용
    public PlayerCrouch playerCrouch;      // 앉기 상태 확인용

    private Vector3 shakeOffset;
    private float shakeTimer;
    private Vector3 lastOffset;

    void LateUpdate()
    {
        if (playerMovement == null) return;

        // 움직임 감지
        bool isMoving = Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f;

        // 강도 결정
        float intensity = isMoving ? moveIntensity : idleIntensity;

        // 앉았으면 흔들림 감소      // 앉았을때 화면 튀는거 안고침
        if (playerCrouch != null && playerCrouch.IsCrouched)
        {
            intensity *= 0.3f; // 앉으면 흔들림 30%로
        }
        
        shakeTimer += Time.deltaTime;
        if (shakeTimer > 1f / shakeFrequency)
        {
            shakeTimer = 0f;
            lastOffset = shakeOffset;
            shakeOffset.x = Random.Range(-intensity, intensity);
            shakeOffset.y = Random.Range(-intensity, intensity);
            shakeOffset.z = 0f;
        }

        // 강제로 화면 흔들림 적용
        transform.localPosition += shakeOffset - lastOffset;
    }
}