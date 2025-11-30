using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Elevator : MonoBehaviour
{
    [Header("DOOR")]
    public GameObject elevator;
    public GameObject door1;
    public GameObject door2;
    
    [Header("OPTIONS")]
    public Vector3 boxCastSize;
    public float doorMoving = 1.5f;
    
    private Vector3 door1InitialPos;
    private Vector3 door2InitialPos;

    private bool prevIsOpen;
    private bool prevIsIN;
    public bool nextFloor = false;
    public bool canNextFloor;

    private GameObject player;

    void Awake()
    {
        door1InitialPos = door1.transform.position;
        door2InitialPos = door2.transform.position;
        
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        // 엘리베이터 주변 플레이어 감지
        Collider[] surround = Physics.OverlapBox(
            (door1.transform.position + door2.transform.position) / 2,
            boxCastSize,
            Quaternion.identity,
            LayerMask.GetMask("Player"));
        
        // 엘리베이터 안 플레이어 감지
        Collider[] inElevator = Physics.OverlapBox(
            elevator.transform.position,
            elevator.transform.localScale * 0.8f,
            Quaternion.identity,
            LayerMask.GetMask("Player"));
        
        
        ElevatorManager.instance.isOpen = surround.Length > 0;
        ElevatorManager.instance.isIN = inElevator.Length > 0;
        
        // 상태 변화시에만 실행
        if (ElevatorManager.instance.isOpen != prevIsOpen)
        {
            // 플레이어가 주변에 있고 안에 없으면 문 열기
            if (ElevatorManager.instance.isOpen && !ElevatorManager.instance.isIN)
            {
                door1.transform.DOMoveZ(doorMoving, 1);
                door2.transform.DOMoveZ(-doorMoving, 1);
            }
            
            // 아니라면 문 닫기
            else
            {
                door1.transform.DOMove(door1InitialPos, 1);
                door2.transform.DOMove(door2InitialPos, 1);
            }
        }
        
        // 안에 있다면 문 닫기
        else if (ElevatorManager.instance.isIN != prevIsIN && ElevatorManager.instance.isIN && canNextFloor)
        {
            door1.transform.DOMove(door1InitialPos, 1);
            door2.transform.DOMove(door2InitialPos, 1);
            ElevatorManager.instance.StartUp();
        }
        
        // 다음 층으로 올라간 후 문 열림
        else if (nextFloor)
        {
            AudioManager.instance.PlaySFX("ElevatorAlarm", transform.position);
            door1.transform.DOMoveZ(doorMoving, 1);
            door2.transform.DOMoveZ(-doorMoving, 1);
            nextFloor = false;
            Debug.Log("UPSTAIRS COMPLETE");
        }

        prevIsOpen = ElevatorManager.instance.isOpen;
        prevIsIN = ElevatorManager.instance.isIN;
    }
    
    public IEnumerator Up()
    {
        yield return new WaitForSeconds(1.5f);
        
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 7.5f, transform.position.z);
        AudioManager.instance.PlaySFX("ElevatorUp", pos, followTarget:elevator.transform);
        
        Sequence seq = DOTween.Sequence();
        seq.Join(elevator.transform.DOMoveY(elevator.transform.position.y + 15f, 12f));
        // seq.Join(player.transform.DOMoveY(player.transform.position.y + 10f, 12f));
        seq.AppendCallback(() => door1InitialPos = door1.transform.position);
        seq.AppendCallback(() => door2InitialPos = door2.transform.position);
        seq.OnComplete(() =>
        {
            nextFloor = true;
        });
    }
}
