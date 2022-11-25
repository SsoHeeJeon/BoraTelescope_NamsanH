using PanTiltControl_v2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoraJoyStick : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 50;
    public VariableJoystick variableJoystick;
    public GameManager GM;
    // Update is called once per frame
    private void Start()
    {
        GM = GetComponent<GameManager>();
    }

    Vector3 direction;
    bool Vertical;
    bool Horizontal;
    public bool alreadyPinchZoom = false;

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "XRMode")
        {
            Vertical = true;
            Horizontal = true;
            if (variableJoystick.Horizontal>=0.2f)
            {
                if (GM.xrMode.currentMotor_x <= XRMode_Manager.MaxPan)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.RIGHT);
                }
            }
            else if(variableJoystick.Horizontal <= -0.2f)
            {
                if (GM.xrMode.currentMotor_x >= XRMode_Manager.MinPan)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.LEFT);
                }
            }
            else
            {
                //PanTiltControl.Stop();
                Horizontal = false; 
            }

            if(variableJoystick.Vertical>=0.2f)
            {
                if (GM.xrMode.currentMotor_y < XRMode_Manager.MaxTilt)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.UP);
                }
            }
            else if(variableJoystick.Vertical <= -0.2f)
            {
                if (GM.xrMode.currentMotor_y > XRMode_Manager.MinTilt)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.DOWN);
                }
            }
            else
            {
                //PanTiltControl.Stop();
                Vertical = false;
            }

            if (!Horizontal && !Vertical)
            {
                if (alreadyPinchZoom == false)
                {
                    GM.WriteLog(LogSendServer.NormalLogCode.AR_Joystick, "AR_Joystick : Finish", GetType().ToString());
                    alreadyPinchZoom = true;
                }
                PanTiltControl.Stop();
            }
            else
            {
                if (alreadyPinchZoom == true)
                {
                    GM.WriteLog(LogSendServer.NormalLogCode.AR_Joystick, "AR_Joystick : Start", GetType().ToString());
                    alreadyPinchZoom = false;
                }
            }
        }
    }

    public void pantiltstop()
    {
        if (alreadyPinchZoom == false)
        {
            GM.WriteLog(LogSendServer.NormalLogCode.AR_Joystick, "AR_Joystick : Finish", GetType().ToString());
            alreadyPinchZoom = true;
        }
        PanTiltControl.Stop();
    }
}
