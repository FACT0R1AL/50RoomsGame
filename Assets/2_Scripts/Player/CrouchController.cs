using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;           // 카메라 Transform
    public CharacterController charController; // CharacterController
    public MonoBehaviour movementScript;     // 이동 스크립트 (moveSpeed 또는 speed 공개)

    [Header("Settings")]
    public float standSpeed = 6f;
    public float crouchSpeedMultiplier = 0.6f;
    public float transitionSpeed = 10f;      // 카메라 Lerp 속도

    private bool isCrouched = false;

    // 원래 값 저장
    private float camOriginalY;
    private float targetCamY;

    private float colliderOriginalHeight;
    private Vector3 colliderOriginalCenter;
    private float targetColliderHeight;
    private Vector3 targetColliderCenter;

    [HideInInspector]
    public float currentSpeed;
    
    public bool IsCrouched => isCrouched;

    void Start()
    {
        if (!playerCamera || !charController)
        {
            Debug.LogError("PlayerCamera와 CharacterController를 연결하세요!");
            enabled = false;
            return;
        }

        camOriginalY = playerCamera.localPosition.y;
        targetCamY = camOriginalY;

        colliderOriginalHeight = charController.height;
        colliderOriginalCenter = charController.center;
        targetColliderHeight = colliderOriginalHeight;
        targetColliderCenter = colliderOriginalCenter;

        currentSpeed = standSpeed;
        ApplySpeedToMovementScript(currentSpeed);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCrouch();
        }

        // 콜라이더는 즉시 적용
        charController.height = Mathf.Lerp(charController.height, targetColliderHeight, Time.deltaTime * transitionSpeed);
        charController.center = Vector3.Lerp(charController.center, targetColliderCenter, Time.deltaTime * transitionSpeed);
    }

    void LateUpdate()
    {
        // 카메라는 LateUpdate에서 부드럽게 Lerp
        Vector3 pos = playerCamera.localPosition;
        float newY = Mathf.Lerp(pos.y, targetCamY, Time.deltaTime * transitionSpeed);
        playerCamera.localPosition = new Vector3(pos.x, newY, pos.z);
    }

    void ToggleCrouch()
    {
        isCrouched = !isCrouched;

        if (isCrouched)
        {
            targetCamY = camOriginalY * 0.5f;
            targetColliderHeight = colliderOriginalHeight * 0.5f;
            targetColliderCenter = new Vector3(colliderOriginalCenter.x, colliderOriginalCenter.y * 0.5f, colliderOriginalCenter.z);
            currentSpeed = standSpeed * crouchSpeedMultiplier;
        }
        else
        {
            targetCamY = camOriginalY;
            targetColliderHeight = colliderOriginalHeight;
            targetColliderCenter = colliderOriginalCenter;
            currentSpeed = standSpeed;
        }

        ApplySpeedToMovementScript(currentSpeed);
    }

    void ApplySpeedToMovementScript(float speed)
    {
        if (!movementScript) return;

        var t = movementScript.GetType();
        var f1 = t.GetField("moveSpeed");
        var f2 = t.GetField("speed");
        var p1 = t.GetProperty("moveSpeed");
        var p2 = t.GetProperty("speed");

        if (f1 != null && f1.FieldType == typeof(float)) f1.SetValue(movementScript, speed);
        else if (f2 != null && f2.FieldType == typeof(float)) f2.SetValue(movementScript, speed);
        else if (p1 != null && p1.PropertyType == typeof(float) && p1.CanWrite) p1.SetValue(movementScript, speed, null);
        else if (p2 != null && p2.PropertyType == typeof(float) && p2.CanWrite) p2.SetValue(movementScript, speed, null);
    }
}