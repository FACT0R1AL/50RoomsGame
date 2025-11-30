using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;

public class ShrinkingWalls : MonoBehaviour
{
    public bool minus;
    public float speed = 1.3f;
    void Start()
    {
        StartCoroutine(Shrinking());
    }
    IEnumerator Shrinking()
    {
        yield return new WaitForSeconds(13.5f);
        
        transform.DOMoveZ(0f, 17.2f);
        Vector3 pos = new Vector3(transform.position.x / 3f, transform.position.y, transform.position.z);
        AudioManager.instance.PlaySFX("Shrinking", pos, followTarget:gameObject.transform);
    }
}