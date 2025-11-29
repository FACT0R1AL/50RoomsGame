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

    void Update()
    {
        Debug.Log(transform.position);
    }
    
    IEnumerator Shrinking()
    {
        yield return new WaitForSeconds(6.5f);
        
        transform.DOMoveZ(0.1f * (minus ? -1 : 1), 16f);
    }
}