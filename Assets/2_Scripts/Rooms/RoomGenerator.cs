using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RoomGenerator : MonoBehaviour
{
    private int length;
    private GameObject prevRoom;

    public GameObject player;

    void Start() 
    {
        GameManager.instance.OnFloorChanged += InstantiateRoom;
    }

    private void OnDisable()
    {
        GameManager.instance.OnFloorChanged -= InstantiateRoom;
    }
    
    public void InstantiateRoom()
    {
        GameObject currentRoom = Instantiate(RoomManager.instance.Rooms[GameManager.instance.Floor],
            new Vector3(0, 15 * GameManager.instance.Floor, 0), 
            RoomManager.instance.Rooms[GameManager.instance.Floor].transform.rotation);
        

        if (prevRoom != null)
        {
            Destroy(prevRoom, 2.5f);
        }
        
        prevRoom = currentRoom;
    }
}
