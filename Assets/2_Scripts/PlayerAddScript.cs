using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAddScript : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.OnFloorChanged += AddScript;
        GameManager.instance.OnFloorChanged += DeleteScript;
    }

    void OnDestroy()
    {
        GameManager.instance.OnFloorChanged -= AddScript;
        GameManager.instance.OnFloorChanged -= DeleteScript;
    }
    
    void AddScript()
    {
        Debug.Log(RoomManager.instance.Rooms[GameManager.instance.Floor+1]);
        // if (RoomManager.instance.Rooms[GameManager.instance.Floor].name == "ShakeRoom")
        // {
        //     transform.AddComponent<HandShakeCamera>();
        // }
    }

    void DeleteScript()
    {
        Debug.Log(RoomManager.instance.Rooms[GameManager.instance.Floor]);
        // if (GameManager.instance.Floor > 0 && RoomManager.instance.Rooms[GameManager.instance.Floor-1].name == "ShakeRoom")
        // {
        //     Destroy(transform.GetComponent<HandShakeCamera>());
        // }
    }
}
