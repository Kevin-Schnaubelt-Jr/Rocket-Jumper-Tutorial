using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isInputEnabled { get; set;}
    public bool isCrashed { get; set; }

    void Awake()
    {
        Debug.Log("GameManager Awake");
        Debug.Log($"is crashed: {isCrashed}");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        isInputEnabled = true;
        isCrashed = false;
        Debug.Log($"is crashed: {isCrashed}");
    }

    public void Crash()
    {
        isCrashed = true;
    }

    public void InputDisabler()
    {
        isInputEnabled = false;
    }
}
