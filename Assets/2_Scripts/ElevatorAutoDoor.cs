using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ElevatorAutoDoor : MonoBehaviour
{
    [Header("GAMEOBJECT : DOOR")]
    public GameObject elevator;
    public GameObject door1;
    public GameObject door2;
    
    [Header("BOXCAST")]
    public Vector3 boxCastSize;
    
    private Vector3 door1InitialPos;
    private Vector3 door2InitialPos;
    
    private bool prevIsOpen = false;

    void Awake()
    {
        door1InitialPos = door1.transform.position;
        door2InitialPos = door2.transform.position;
    }

    void Update()
    {
        Collider[] surround = Physics.OverlapBox(
            (door1.transform.position + door2.transform.position) / 2,
            boxCastSize,
            Quaternion.identity,
            LayerMask.GetMask("Player"));

        Collider[] inElevator = Physics.OverlapBox(
            elevator.transform.position,
            elevator.transform.localScale * 0.8f,
            Quaternion.identity,
            LayerMask.GetMask("Player"));
        
        
        bool isOpen = surround.Length > 0;
        bool isIN = inElevator.Length > 0;
        
        // 상태 변화시에만 실행
        if (isOpen != prevIsOpen)
        {
            if (isOpen && !isIN)
            {
                door1.transform.DOMoveZ(1.5f, 1);
                door2.transform.DOMoveZ(-1.5f, 1);
            }
            else
            {
                door1.transform.DOMove(door1InitialPos, 1);
                door2.transform.DOMove(door2InitialPos, 1);
            }
        }
        
        else if (isIN)
        {
            door1.transform.DOMove(door1InitialPos, 1);
            door2.transform.DOMove(door2InitialPos, 1);
        }
        
        Debug.Log($"{isOpen} / {isIN}");

        prevIsOpen = isOpen;
    }
}
