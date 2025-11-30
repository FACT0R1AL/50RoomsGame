using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;
    public float gravity = -9.81f;

    private CharacterController controller;
    private float verticalRotation = 0f;
    private float verticalVelocity = 0f;

    private float headBobTimer = 0f;
    private Vector3 defaultCameraPos;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 머ㅓㅁ언머아ㅓㅏ 마우스 돌라ㅣ기
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        // cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation,
        //     Quaternion.Euler(verticalRotation, 0f, 0f), 10f * Time.deltaTime);

        // 수정 코드
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // 무브무브
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        move *= moveSpeed;

        // 중력
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);

        // 머리 흔들기
        if (controller.isGrounded && (h != 0 || v != 0))
        {
            headBobTimer += Time.deltaTime * 10f;
            float bobAmount = 0.02f;
            float bobSpeed = 2f;
            cameraTransform.localPosition = defaultCameraPos + new Vector3(
                0f,
                Mathf.Sin(headBobTimer * bobSpeed) * bobAmount + 0.5f,
                0f
            );
        }
        else
        {
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition,
                defaultCameraPos + Vector3.up * 0.5f, Time.deltaTime * 5f);
        }
        
    }

}
