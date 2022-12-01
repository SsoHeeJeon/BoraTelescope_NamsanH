using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamSanHMode : MonoBehaviour
{
    private GameManager gamemanager;
    public GameObject CameraWindow;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gamemanager.UISetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
