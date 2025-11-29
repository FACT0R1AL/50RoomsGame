using UnityEngine;

public class ShowOutlineByRaycast : MonoBehaviour
{
    [Tooltip("아이템 레이어 설정해")]
    public LayerMask itemsLayer = ~0;
    public float maxDistance = 10f;
    public Camera cam;
    public Color outlineColor = Color.yellow;
    public float outlineWidth = 4f;
    public Outline.Mode outlineMode = Outline.Mode.OutlineVisible;

    private Outline currentOutline;

    void Start()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        if (cam == null) return;

        var screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        var ray = cam.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, itemsLayer))
        {
            GameObject targetGO = FindAncestorWithLayerAndRenderer(hit.collider.transform);

            if (targetGO == null)
            {
                goto RAY_END;
            }

            var target = targetGO.GetComponent<Outline>();

            if (target == null)
            {
                target = targetGO.AddComponent<Outline>();
            }

            target.OutlineMode = outlineMode;
            target.OutlineColor = outlineColor;
            target.OutlineWidth = outlineWidth;

            if (currentOutline != target)
            {
                if (currentOutline != null)
                {
                    currentOutline.enabled = false;
                }

                currentOutline = target;
                currentOutline.enabled = true;
            }
        }
        else
        {
            if (currentOutline != null)
            {
                currentOutline.enabled = false;
                currentOutline = null;
            }
        }

    RAY_END:;
    }

    bool IsInLayerMask(GameObject go, LayerMask mask)
    {
        return (mask.value & (1 << go.layer)) != 0;
    }

    GameObject FindAncestorWithLayerAndRenderer(Transform t)
    {
        Transform cur = t;

        while (cur != null)
        {
            if (IsInLayerMask(cur.gameObject, itemsLayer))
            {
                if (cur.GetComponentInChildren<Renderer>() != null)
                    return cur.gameObject;
            }

            cur = cur.parent;
        }

        return null;
    }
}