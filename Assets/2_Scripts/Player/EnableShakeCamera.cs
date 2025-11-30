using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableShakeCamera : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.OnFloorChanged += EnableShake;
        GameManager.instance.OnFloorChanged += DisableShake;
    }

    private void OnDestroy()
    {
        GameManager.instance.OnFloorChanged -= EnableShake;
        GameManager.instance.OnFloorChanged -= DisableShake;
    }

    void EnableShake()
    {
        if (RoomManager.instance.Rooms[GameManager.instance.Floor].name == "ShakeRoom")
        {
            StartCoroutine(LateEnableShake());
        }
    }

    IEnumerator LateEnableShake()
    {
        yield return new WaitForSeconds(15.5f);
        
        transform.GetComponent<HandShakeCamera>().enabled = true;
        AudioManager.instance.PlaySFX("Noise", transform.position, loop:true, followTarget:gameObject.transform);
    }

    void DisableShake()
    {
        if (GameManager.instance.Floor > 0 && RoomManager.instance.Rooms[GameManager.instance.Floor - 1].name == "ShakeRoom")
        {
            StartCoroutine(LateDisableShake());
        }
    }

    IEnumerator LateDisableShake()
    {
        yield return new WaitForSeconds(1.5f);

        transform.GetComponent<HandShakeCamera>().enabled = false;
        AudioManager.instance.StopSFX("Noise");
    }
}
