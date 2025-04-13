using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Camera MainCamera { get; private set; }

    public Grid grid;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
