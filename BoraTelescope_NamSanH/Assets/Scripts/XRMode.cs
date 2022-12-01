using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRMode : MonoBehaviour
{
    public GameObject CameraWindow;
    public Camera UICam;

    public GameObject AllMapLabels;

    public float currentMotor_x;
    public float currentMotor_y;

    public static float ValueX;
    public static float ValueY;

    public float zoommove_t;

    public static int panFreq_ARR = 0;
    public static int panFreq_Near = 0;
    public static int panFreq_Far = 0;
    public static int PanFreq = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
