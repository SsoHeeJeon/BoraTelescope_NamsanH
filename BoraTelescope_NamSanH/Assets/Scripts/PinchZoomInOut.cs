using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PanTiltControl_v2;

public class PinchZoomInOut : MonoBehaviour
{
    public GameManager gamemanager;
    public GameObject butzoom;
    int touchcount_int;
    float m_fOldToucDis = 0f;       // 터치 이전 거리를 저장합니다.
    public static bool ZoomMove = false;
    public static bool ZoomIN = false;

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "XRMode")
        {
            if (Mathf.Abs(gamemanager.xrMode.CameraWindow.transform.GetChild(0).transform.GetChild(0).gameObject.transform.localPosition.z - 0) <= 0.5f)
            {
                gamemanager.ZoomBar.transform.parent.gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = gamemanager.ZoomIn;
            }
            else if (Mathf.Abs(gamemanager.xrMode.CameraWindow.transform.GetChild(0).transform.GetChild(0).gameObject.transform.localPosition.z - 0) > 0.5f)
            {
                gamemanager.ZoomBar.transform.parent.gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = gamemanager.ZoomOut;
            }

            float bar_y = -gamemanager.xrMode.CameraWindow.transform.GetChild(0).transform.GetChild(0).gameObject.transform.localPosition.z / 385 * 60 - 30;

            gamemanager.ZoomBar.transform.localPosition = new Vector3(bar_y, gamemanager.ZoomBar.transform.localPosition.y, gamemanager.ZoomBar.transform.localPosition.z);
        }

        if(ZoomMove == true)
        {
            MoveCameraCanvas();
        }
    }

    public void BtnZoom()
    {
        gamemanager.xrMode.zoommove_t = 0;

        GameObject zoomimg = gamemanager.xrMode.CameraWindow.transform.GetChild(0).transform.GetChild(0).gameObject;

        gamemanager.WriteLog(LogSendServer.NormalLogCode.AR_Zoom, gamemanager.ZoomBar.transform.localPosition.x.ToString() + " / " + gamemanager.ZoomBar.transform.localPosition.y.ToString(), GetType().ToString());
        if (zoomimg.transform.localPosition.z == 0)
        {
            gamemanager.MiniMap_CameraGuide.SetActive(false);
            PanTiltControl.Stop();

            ZoomMove = true;
            ZoomIN = true;
            PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Slow);
            gamemanager.speed_enum = GameManager.Speed_enum.slow;

        }
        else if (zoomimg.transform.localPosition.z < 0)
        {

            gamemanager.MiniMap_CameraGuide.SetActive(false);
            PanTiltControl.Stop();

            ZoomMove = true;
            ZoomIN = false;
            PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Middle);
            gamemanager.speed_enum = GameManager.Speed_enum.middle;
        }

        butzoom.GetComponent<Button>().enabled = false;
    }


    public void MoveCameraCanvas()
    {
        if (SceneManager.GetActiveScene().name == "XRMode")
        {
            GameObject zoomimg = gamemanager.xrMode.CameraWindow.transform.GetChild(0).transform.GetChild(0).gameObject;
            if (ZoomIN == true)
            {
                if (Mathf.Abs(zoomimg.transform.localPosition.z - -385) > 1f)
                {
                    zoomimg.transform.localPosition = Vector3.Lerp(zoomimg.transform.localPosition, new Vector3(zoomimg.transform.localPosition.x, zoomimg.transform.localPosition.y, -385), gamemanager.xrMode.zoommove_t * 0.2f);
                    StopBtnZoom();
                }
                else if (Mathf.Abs(zoomimg.transform.localPosition.z - -385) <= 1f)
                {
                    zoomimg.transform.localPosition = new Vector3(zoomimg.transform.localPosition.x, zoomimg.transform.localPosition.y, -385);
                    ZoomIN = false;
                    ZoomMove = false;
                    gamemanager.xrMode.zoommove_t = 0;
                    butzoom.GetComponent<Button>().enabled = true;
                }
            }
            else if (ZoomIN == false)
            {
                if (Mathf.Abs(zoomimg.transform.localPosition.z - 0) > 1f)
                {
                    zoomimg.transform.localPosition = Vector3.Lerp(zoomimg.transform.localPosition, new Vector3(zoomimg.transform.localPosition.x, zoomimg.transform.localPosition.y, 0), gamemanager.xrMode.zoommove_t * 0.2f);
                    StopBtnZoom();
                }
                else if (Mathf.Abs(zoomimg.transform.localPosition.z - 0) <= 1f)
                {
                    zoomimg.transform.localPosition = new Vector3(zoomimg.transform.localPosition.x, zoomimg.transform.localPosition.y, 0);
                    ZoomIN = true;
                    ZoomMove = false;
                    gamemanager.xrMode.zoommove_t = 0;
                    butzoom.GetComponent<Button>().enabled = true;
                }
            }
        }
    }

    public void StopBtnZoom()
    {
        if (Input.touchCount >= 1)
        {
            if (SceneManager.GetActiveScene().name == "XRMode" && gamemanager.xrMode.zoommove_t > 0.5f)
            {
                GameObject zoomimg = gamemanager.xrMode.CameraWindow.transform.GetChild(0).transform.GetChild(0).gameObject;
                zoomimg.transform.localPosition = new Vector3(zoomimg.transform.localPosition.x, zoomimg.transform.localPosition.y, zoomimg.transform.localPosition.z);
                gamemanager.xrMode.zoommove_t = 0;
                butzoom.GetComponent<Button>().enabled = true;
                ZoomMove = false;
            }
        }
    }
}
