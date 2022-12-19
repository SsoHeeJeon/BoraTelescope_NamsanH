using PanTiltControl_v2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XRMode : MonoBehaviour
{
    public GameManager gamemanager;
    public GameObject CameraWindow;
    public Camera UICam;

    public GameObject AllMapLabels;

    public float currentMotor_x;
    public float currentMotor_y;

    public static float ValueX;
    public static float ValueY;

    private float minpan;
    private float maxpan;
    private float mintilt;
    private float maxtilt;

    string dir;
    int touchcount_int;
    float m_fOldToucDis = 0f;       // 터치 이전 거리를 저장합니다.
    float m_fFieldOfView = 0;
    public float zoommove_t;

    public static int panFreq_ARR = 0;
    public static int panFreq_Near = 0;
    public static int panFreq_Far = 0;
    public static int PanFreq = 0;

    bool alreadyminimap = false;
    public RectTransform mapX;
    public RectTransform mapCamX;
    float totalminimap;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gamemanager.UISetting();

        gamemanager.setrecord.ReadytoStart();

        mapX = gamemanager.MiniMap_Background.GetComponent<RectTransform>();
        mapCamX = gamemanager.MiniMap_Camera.GetComponent<RectTransform>();
        totalminimap = (mapX.rect.width - mapCamX.rect.width);

        minpan = XRMode_Manager.MinPan;
        maxpan = XRMode_Manager.MaxPan;
        mintilt = XRMode_Manager.MinTilt;
        maxtilt = XRMode_Manager.MaxTilt;

        if (gamemanager.WantNoLabel == false)
        {
            gamemanager.WriteLog(LogSendServer.NormalLogCode.ChangeMode, "ChangeMode : Finish(" + GameManager.PrevMode + " - " + "ARMode)", GetType().ToString());
            GameManager.PrevMode = "XRMode";
        }
        else if (gamemanager.WantNoLabel == true)
        {
            gamemanager.WriteLog(LogSendServer.NormalLogCode.ChangeMode, "ChangeMode : Finish(" + GameManager.PrevMode + " - " + "LIveMode)", GetType().ToString());
            GameManager.PrevMode = "LiveMode";
        }

        PanFreq = panFreq_ARR;
        ValueX = XRMode_Manager.TotalPan;
        ValueY = XRMode_Manager.TotalTilt;

        if (GameManager.SettingLabelPosition == true)
        {
            gamemanager.xrMode_manager.enabled = true;
            gamemanager.xrMode.enabled = false;
        }
        else if (GameManager.SettingLabelPosition == false)
        {
            gamemanager.xrMode.enabled = true;
            gamemanager.xrMode_manager.enabled = false;
        }
        gamemanager.ResetPositionTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        zoommove_t += Time.deltaTime * 3;

        currentMotor_x = PanTiltControl.NowPanPulse;
        currentMotor_y = PanTiltControl.NowTiltPulse;
        //Debug.Log("today " + currentMotor_x + " / " + currentMotor_y);

        AllMapLabels.transform.localPosition = new Vector3(AllMapLabels.transform.localPosition.x, AllMapLabels.transform.localPosition.y, CameraWindow.transform.GetChild(0).gameObject.transform.GetChild(0).transform.localPosition.z);

        UICam.orthographicSize = (385 - (-AllMapLabels.transform.localPosition.z)) / 385 * 358 + 182;

        touchcount_int = Input.touchCount;

        if (touchcount_int >= 2)
        {
            PinchZoom();
        }
        else if (touchcount_int < 2)
        {
            m_fOldToucDis = 0;
        }

        MiniMap();

        if (gamemanager.StartMiniMapDrag == true)
        {
            ButtonMinimap();
        }

        CameraWindow.transform.localPosition = new Vector3(currentMotor_x * ValueX, currentMotor_y * ValueY, CameraWindow.transform.localPosition.z);

        if (GameManager.MoveCamera == true)
        {
            MoveCamera_Arrow();
        }
    }

    public void MoveCamera_Arrow()
    {
        if (gamemanager.MiniMap_CameraGuide.activeSelf)
        {
            gamemanager.MiniMap_CameraGuide.SetActive(false);
        }
        Resetothers();
        switch (gamemanager.MoveDir)
        {
            case "Left":
                if (currentMotor_x >= XRMode_Manager.MinPan)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.LEFT);
                }
                else
                {
                    PanTiltControl.Stop();
                }
                break;
            case "Right":
                if (currentMotor_x <= XRMode_Manager.MaxPan)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.RIGHT);
                }
                else
                {
                    PanTiltControl.Stop();
                }
                break;
            case "Up":
                if (currentMotor_y < XRMode_Manager.MaxTilt)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.UP);
                }
                else
                {
                    PanTiltControl.Stop();
                }
                break;
            case "Down":
                if (currentMotor_y > XRMode_Manager.MinTilt)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.DOWN);
                }
                else
                {
                    PanTiltControl.Stop();
                }
                break;
        }
        dir = gamemanager.MoveDir;
    }

    // 네비게이션 라벨
    public void MoveCamera_Navi(float posX, float posY)
    {
        if (Mathf.Abs(currentMotor_x - posX) <= 2000 && PanFreq != panFreq_Near)
        {
            PanFreq = panFreq_Near;
            PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Slow);
            gamemanager.speed_enum = GameManager.Speed_enum.slow;
        }
        else if (Mathf.Abs(currentMotor_x - posX) > 2000 && PanFreq != panFreq_Far)
        {
            PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Fast);
            gamemanager.speed_enum = GameManager.Speed_enum.fast;
        }
        Invoke("RealMovePantilt", 0.1f);
    }

    float xpulse;
    float ypulse;

    public void RealMovePantilt()
    {
        PanTiltControl.SetPulse((uint)xpulse, (uint)ypulse);
    }

    // 미니맵
    public void MiniMap()
    {
        if (alreadyminimap == true)
        {
            gamemanager.WriteLog(LogSendServer.NormalLogCode.AR_FinishMinimap, "AR_FinishMinimap_(" + CameraWindow.transform.localPosition.x + ", " + CameraWindow.transform.localPosition.y + ")", GetType().ToString());
            alreadyminimap = false;
        }

        gamemanager.minimapCamera_x = ((currentMotor_x - XRMode_Manager.MinPan) / (XRMode_Manager.MaxPan - XRMode_Manager.MinPan)) * (totalminimap) - mapX.rect.width / 2 + mapCamX.rect.width / 2;

        gamemanager.MiniMap_Camera.transform.localPosition = new Vector3(gamemanager.minimapCamera_x, gamemanager.MiniMap_Camera.transform.localPosition.y, gamemanager.MiniMap_Camera.transform.localPosition.z);
        if (gamemanager.MiniMap_CameraGuide.gameObject.activeSelf && Mathf.Abs(gamemanager.MiniMap_Camera.transform.localPosition.x - gamemanager.MiniMap_CameraGuide.transform.localPosition.x) < 0.05f)
        {
            gamemanager.MiniMap_CameraGuide.gameObject.SetActive(false);
        }
    }

    float valueminimapx;

    public void ButtonMinimap()
    {
        if (!gamemanager.Arrow.activeSelf)
        {
            if (gamemanager.joystick.enabled)
            {
                gamemanager.joystick.enabled = false;
            }
            gamemanager.Arrow.SetActive(true);
            gamemanager.Arrow.transform.position = gamemanager.Arrowpos_extend;
            gamemanager.Arrow.transform.GetChild(0).transform.localPosition = Vector3.zero;
        }
        Resetothers();
        if (!gamemanager.MiniMap_CameraGuide.gameObject.activeSelf)
        {
            if ((Input.GetTouch(0).position.x >= 540 && Input.GetTouch(0).position.x <= 1450) || (Input.mousePosition.x >= 540 && Input.mousePosition.x <= 1450))
            {
                if (alreadyminimap == false)
                {
                    gamemanager.WriteLog(LogSendServer.NormalLogCode.AR_StartMinimap, "AR_StartMinimap_(" + CameraWindow.transform.localPosition.x + ", " + CameraWindow.transform.localPosition.y + ")", GetType().ToString());
                    alreadyminimap = true;
                }
                gamemanager.MiniMap_CameraGuide.gameObject.SetActive(true);
                valueminimapx = ((Input.GetTouch(0).position.x - 540) / 910 * totalminimap) - mapX.rect.width / 2 + mapCamX.rect.width / 2;
                //gamemanager.MiniMap_Camera.transform.localPosition = new Vector3(gamemanager.minimapCamera_x, gamemanager.MiniMap_Camera.transform.localPosition.y, gamemanager.MiniMap_Camera.transform.localPosition.z);
                gamemanager.MiniMap_CameraGuide.transform.localPosition = new Vector3(valueminimapx, gamemanager.MiniMap_Camera.transform.localPosition.y, gamemanager.MiniMap_Camera.transform.localPosition.z);

                xpulse = ((valueminimapx - mapCamX.rect.width / 2 + mapX.rect.width / 2) / totalminimap * (XRMode_Manager.MaxPan - XRMode_Manager.MinPan)) + XRMode_Manager.MinPan;
                ypulse = currentMotor_y;
                MoveCamera_Navi(xpulse, ypulse);
            }
        }
        else if (gamemanager.MiniMap_CameraGuide.gameObject.activeSelf)
        {
            valueminimapx = ((Input.GetTouch(0).position.x - 540) / 910 * totalminimap) - mapX.rect.width / 2 + mapCamX.rect.width / 2;
            if (Input.GetTouch(0).position.x >= 540 && Input.GetTouch(0).position.x <= 1450)
            {
                if (Mathf.Abs(valueminimapx - gamemanager.MiniMap_CameraGuide.transform.localPosition.x) > 0.5f)
                {
                    if (alreadyminimap == false)
                    {
                        gamemanager.WriteLog(LogSendServer.NormalLogCode.AR_StartMinimap, "AR_StartMinimap_(" + CameraWindow.transform.localPosition.x + ", " + CameraWindow.transform.localPosition.y + ")", GetType().ToString());
                        alreadyminimap = true;
                    }
                    //gamemanager.MiniMap_Camera.transform.localPosition = new Vector3(gamemanager.minimapCamera_x, gamemanager.MiniMap_Camera.transform.localPosition.y, gamemanager.MiniMap_Camera.transform.localPosition.z);
                    gamemanager.MiniMap_CameraGuide.transform.localPosition = new Vector3(valueminimapx, gamemanager.MiniMap_Camera.transform.localPosition.y, gamemanager.MiniMap_Camera.transform.localPosition.z);

                    xpulse = ((valueminimapx - mapCamX.rect.width / 2 + mapX.rect.width / 2) / totalminimap * (XRMode_Manager.MaxPan - XRMode_Manager.MinPan)) + XRMode_Manager.MinPan;
                    ypulse = currentMotor_y;
                    MoveCamera_Navi(xpulse, ypulse);
                }
            }
        }
    }
    public static bool StartMove = false;
    public void Resetothers()
    {
        if (StartMove == false)
        {
            if (gamemanager.joystick.enabled)
            {
                for (int index = 1; index < gamemanager.Arrow.transform.childCount; index++)
                {
                    gamemanager.Arrow.transform.GetChild(index).gameObject.SetActive(true);
                }
                gamemanager.joystick.enabled = false;
            }
            StartMove = true;
        }
    }

    public void PinchZoom()
    {
        if (Input.touchCount == 2 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved))
        {
            StartPinchZoom();
        }
        else if (Input.touchCount != 2)
        {
            m_fOldToucDis = 0;
        }
    }

    public void StartPinchZoom()
    {
        int nTouch = Input.touchCount;
        float m_fToucDis = 0f;
        float fDis = 0f;

        m_fToucDis = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;
        fDis = (m_fToucDis - m_fOldToucDis) * 0.001f;

        // 이전 두 터치의 거리와 지금 두 터치의 거리의 차이를 FleldOfView를 차감합니다.
        if (fDis < 100f)
        {
            m_fFieldOfView += fDis;
        }
        // 최대는 100, 최소는 20으로 더이상 증가 혹은 감소가 되지 않도록 합니다.
        m_fFieldOfView = Mathf.Clamp(m_fFieldOfView, 0.0f, 385.0f);

        // 확대 / 축소가 갑자기 되지않도록 보간합니다.
        CameraWindow.transform.GetChild(0).gameObject.transform.GetChild(0).transform.localPosition = Vector3.Lerp(CameraWindow.transform.GetChild(0).gameObject.transform.GetChild(0).transform.localPosition, new Vector3(CameraWindow.transform.GetChild(0).gameObject.transform.GetChild(0).transform.localPosition.x, CameraWindow.transform.GetChild(0).gameObject.transform.GetChild(0).transform.localPosition.y, -m_fFieldOfView), zoommove_t * 0.005f);

        m_fOldToucDis = m_fToucDis;
    }

    public void Labelactive()
    {
        for (int index = 0; index < AllMapLabels.transform.childCount; index++)
        {
            if (CameraWindow.transform.localPosition.x + 1024 - 300 >= AllMapLabels.transform.GetChild(index).gameObject.transform.localPosition.x
                && CameraWindow.transform.localPosition.x - 1024 + 300 <= AllMapLabels.transform.GetChild(index).gameObject.transform.localPosition.x
                && CameraWindow.transform.localPosition.y + 540 - 100 >= AllMapLabels.transform.GetChild(index).gameObject.transform.localPosition.y
                && CameraWindow.transform.localPosition.y - 540 + 100 <= AllMapLabels.transform.GetChild(index).gameObject.transform.localPosition.y)
            {
                /*
                if ((CameraWindow.transform.localPosition.x - AllMapLabels.transform.GetChild(index).gameObject.transform.localPosition.x) < 0)
                {
                    dir = "Right";
                }
                else if ((CameraWindow.transform.localPosition.x - AllMapLabels.transform.GetChild(index).gameObject.transform.localPosition.x) > 0)
                {
                    dir = "Left";
                }
                */
                labeleffect(index);

                AllMapLabels.transform.GetChild(index).gameObject.GetComponent<Image>().fillAmount += 2f * Time.deltaTime;
            }
            else
            {
                /*
                if ((CameraWindow.transform.localPosition.x - AllMapLabels.transform.GetChild(index).gameObject.transform.localPosition.x) < 0)
                {
                    dir = "Left";
                }
                else if ((CameraWindow.transform.localPosition.x - AllMapLabels.transform.GetChild(index).gameObject.transform.localPosition.x) > 0)
                {
                    dir = "Right";
                }*/

                labeleffect(index);

                AllMapLabels.transform.GetChild(index).gameObject.GetComponent<Image>().fillAmount -= 2f * Time.deltaTime;
            }
        }
    }

    public void labeleffect(int k)
    {
        switch (dir)
        {
            case "Left":
                AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillMethod = Image.FillMethod.Horizontal;

                if (AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillAmount == 1)
                {
                    AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Left;
                }
                else if (AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillAmount == 0)
                {
                    AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Right;
                }
                break;
            case "Right":
                AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillMethod = Image.FillMethod.Horizontal;

                if (AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillAmount == 1)
                {
                    AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Right;
                }
                else if (AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillAmount == 0)
                {
                    AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Left;
                }
                break;
            case "Up":
                AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillMethod = Image.FillMethod.Vertical;

                if (AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillAmount == 1)
                {
                    AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillOrigin = (int)Image.OriginVertical.Top;
                }
                else if (AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillAmount == 0)
                {
                    AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillOrigin = (int)Image.OriginVertical.Bottom;
                }
                break;
            case "Down":
                AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillMethod = Image.FillMethod.Vertical;

                if (AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillAmount == 1)
                {
                    AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillOrigin = (int)Image.OriginVertical.Bottom;
                }
                else if (AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillAmount == 0)
                {
                    AllMapLabels.transform.GetChild(k).gameObject.GetComponent<Image>().fillOrigin = (int)Image.OriginVertical.Top;
                }
                break;
        }
    }
}
