using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class ElevatorManager : MonoBehaviour
{
    public static ElevatorManager instance;
    
    // x+ EV
    public GameObject elevator1;
    // x- EV
    public GameObject elevator2;

    public bool isIN;
    public bool isOpen;
    
    // 로비 = 0층
    private int floor;
    public int Floor
    {
        get => floor;
        set
        {
            if (floor == value) return; // 같은 값이면 이벤트 방지
            floor = value;
            ChangeNextFloor();
        }
    }

    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Floor = 0;
        ChangeNextFloor();
    }

    private void ChangeNextFloor()
    {
        elevator1.GetComponent<Elevator>().canNextFloor = Floor % 2 == 0;
        elevator2.GetComponent<Elevator>().canNextFloor = Floor % 2 == 1;
    }

    public void StartUp()
    {
        StartCoroutine(elevator1.GetComponent<Elevator>().Up());
        StartCoroutine(elevator2.GetComponent<Elevator>().Up());
    }
}
