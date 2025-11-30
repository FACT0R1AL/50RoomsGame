using UnityEngine;

public class RaycastCheck : MonoBehaviour
{
    private Outline currentOutline;
    private RadioVolumeController currentRadio;

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f, LayerMask.GetMask("Items")))
        {
            Outline outline = hit.collider.GetComponent<Outline>();

            if (outline != null)
            {
                //딴 옵젝 아웃라인 끄기
                if (currentOutline != null && currentOutline != outline)
                {
                    currentOutline.enabled = false;
                }

                //켜기
                outline.enabled = true;
                currentOutline = outline;
            }

            // 라디오를 보고 있을 때 F 키로 상호작용
            RadioVolumeController radio = hit.collider.GetComponentInParent<RadioVolumeController>();
            if (radio != null)
            {
                currentRadio = radio;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    radio.ApplyNextVolumeStep();
                }
            }
            else
            {
                currentRadio = null;
            }
        }
        else
        {
            // 레이캐스트가 아무 것도 안 맞으면 기존 외곽선 끄기 땡쓰 지피티
            if (currentOutline != null)
            {
                currentOutline.enabled = false;
                currentOutline = null;
            }

            currentRadio = null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }
}
