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
    private Elevator elevator1E;
    // x- EV
    public GameObject elevator2;
    private Elevator elevator2E;

    public bool isIN;
    public bool isOpen;
    
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
        
        elevator1E = elevator1.GetComponent<Elevator>();
        elevator2E = elevator2.GetComponent<Elevator>();
    }

    void Start()
    {
        ChangeNextFloor();
        GameManager.instance.OnFloorChanged += ChangeNextFloor;
    }


    public void StartUp()
    {
        StartCoroutine(elevator1E.Up());
        StartCoroutine(elevator2E.Up());
        GameManager.instance.Floor++;
    }
    
    private void ChangeNextFloor()
    {
        elevator1E.canNextFloor = GameManager.instance.Floor % 2 == 0;
        elevator2E.canNextFloor = GameManager.instance.Floor % 2 == 1;
    }
}
