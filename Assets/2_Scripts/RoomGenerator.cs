using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject[] Rooms;
    private int length;

    void Start()
    {
        length = Rooms.Length;
        for (int i = 0; i < length; i++)
        {
            Vector3 pos = new Vector3(0, i * 15, 0);
            
            // 엘리베이터를 0, 180도로 맞추기 위해
            Quaternion rotation = Quaternion.Euler(0, 90, 0);
            Instantiate(Rooms[i], pos, rotation);
        }
    }
}
