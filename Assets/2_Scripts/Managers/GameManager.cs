using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public event Action OnFloorChanged;
    
    // 로비 = 0층
    private int floor;
    public int Floor
    {
        get => floor;
        set
        {
            if (floor == value) return;
            floor = value;
            OnFloorChanged?.Invoke();
            Debug.Log($"OnFloorChanged to {floor}");
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
    }
}
