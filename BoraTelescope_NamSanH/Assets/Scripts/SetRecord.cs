using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetRecord : MonoBehaviour
{
    public GameManager gamemanager;
    public GameObject RecordingCam;
    //public GameObject VideoRecord;
    //public AllTimeRecord alltimerecord;
    public static bool firstdontdestroy = false;

    private Vector3 recordcamPos_XR;
    private Vector3 recordcamPos_Clear = new Vector3(0,10000,0);

    // Start is called before the first frame update
    public void ReadytoStart()
    {
        if (firstdontdestroy == false)
        {
            if (SceneManager.GetActiveScene().name == "XRMode")
            {
                RecordingCam = gamemanager.xrMode.CameraWindow;
                //VideoRecord = RecordingCam.transform.GetChild(1).gameObject;
                //alltimerecord = gamemanager.arMode.CameraWindow.transform.GetChild(8).gameObject.GetComponent<AllTimeRecord>();
            }
            else if (SceneManager.GetActiveScene().name == "NamSanHMode")
            {
                RecordingCam = gamemanager.namsanMode.CameraWindow;
                //VideoRecord = RecordingCam.transform.GetChild(1).gameObject;
                //alltimerecord = gamemanager.telescopeMode.CameraWindow.transform.GetChild(1).gameObject.GetComponent<AllTimeRecord>();
            }
            recordcamPos_XR = RecordingCam.transform.position;
            DontDestroyOnLoad(RecordingCam);
            //AllTimeRecord.GetCamRecord = true;
            firstdontdestroy = true;
        } else if(firstdontdestroy == true)
        {
            if (RecordingCam == null)
            {
                if (SceneManager.GetActiveScene().name == "XRMode")
                {
                    RecordingCam = gamemanager.xrMode.CameraWindow;
                    //alltimerecord = gamemanager.arMode.CameraWindow.transform.GetChild(8).gameObject.GetComponent<AllTimeRecord>();
                }
                else if (SceneManager.GetActiveScene().name == "NamSanHMode")
                {
                    RecordingCam = gamemanager.namsanMode.CameraWindow;
                    //alltimerecord = gamemanager.telescopeMode.CameraWindow.transform.GetChild(1).gameObject.GetComponent<AllTimeRecord>();
                }
                DontDestroyOnLoad(RecordingCam);
                //AllTimeRecord.GetCamRecord = true;
            }
            else if (RecordingCam != null)
            {
                if (SceneManager.GetActiveScene().name == "XRMode")
                {
                    gamemanager.xrMode.CameraWindow.gameObject.SetActive(false);
                    gamemanager.xrMode.CameraWindow = RecordingCam;
                    gamemanager.xrMode.CameraWindow.transform.position = recordcamPos_XR;
                    //GameObject.Find("Canvas_pano (3)").GetComponent<Canvas>().worldCamera = RecordingCam.transform.GetChild(1).gameObject.GetComponent<Camera>();
                    //gamemanager.xrMode.UICam = RecordingCam.transform.GetChild(1).gameObject.GetComponent<Camera>();
                } else if (SceneManager.GetActiveScene().name == "NamSanHMode")
                {
                    gamemanager.namsanMode.CameraWindow.gameObject.SetActive(false);
                    gamemanager.namsanMode.CameraWindow = RecordingCam;
                    gamemanager.namsanMode.CameraWindow.transform.position = recordcamPos_XR;
                }
            }
        }
    }

    public void ChangeRecordCamPos()
    {
        RecordingCam.transform.position = recordcamPos_Clear;
    }
    /*
    public void ChangeRecordCamPos_Telescope()
    {
        RecordingCam.transform.position = recordcamPos_Clear;
    }*/
}
