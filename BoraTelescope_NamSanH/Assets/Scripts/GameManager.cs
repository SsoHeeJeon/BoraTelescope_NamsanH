using PanTiltControl_v2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// AR��� �󺧸� �� �� ��ġ ��Ī
/// </summary>
public class XRModeLabelPosition
{
    public string LabelName;
    public float Label_X;
    public float Label_Y;

    public XRModeLabelPosition(string labelname, float label_x, float label_y)
    {
        LabelName = labelname;
        Label_X = label_x;
        Label_Y = label_y;
    }
}

public class LabelText
{
    public string LabelName;
    public string Korean;
    public string English;

    public LabelText(string labelname, string kor, string eng)
    {
        LabelName = labelname;
        Korean = kor;
        English = eng;
    }
}

/// <summary>
/// ��ƿƮ����
/// </summary>
public class PanTiltRange
{
    public float Min_Pan;
    public float Max_Pan;
    public float Min_Tilt;
    public float Max_Tilt;
    public string StartPosition;
    public float ChangeValue_x;
    public float ChangeValue_y;
    public int WaitingTime;

    public PanTiltRange(float minx, float maxx, float miny, float maxy, string startlabel, float valueX, float valueY, int waittime)
    {
        Min_Pan = minx;
        Max_Pan = maxx;
        Min_Tilt = miny;
        Max_Tilt = maxy;
        StartPosition = startlabel;
        ChangeValue_x = valueX;
        ChangeValue_y = valueY;
        WaitingTime = waittime;
    }
}

public class GameManager : ContentsInfo
{
    public enum Language_enum
    {
        Korea, English
    }
    public static Language_enum currentLang;
    public Language_enum curlang;

    public enum Speed_enum
    {
        slow,
        middle,
        fast,
    }
    public Speed_enum speed_enum;

    private GameObject GM;
    public GameManager gamemanager;

    //��ũ��Ʈ
    public Loading loading;
    public XRMode xrMode;
    public XRMode_Manager xrMode_manager;
    public NamSanHMode namsanMode;
    //public Label label;
    //public LabelDetail labeldetail;
    public WaitingMode waitingMode;
    public ChangeWaitingMode changewaiting;
    public SetDragRange setdragrange;
    public SetRecord setrecord;
    public BoraJoyStick joystick;

    //���� UI
    public GameObject UI_All;
    public GameObject MenuBar;
    public GameObject NavigationBar;
    public GameObject LanguageBar;
    public GameObject Arrow;
    public GameObject MiniMap_Background;
    public GameObject MiniMap_Camera;
    public GameObject MiniMap_CameraGuide;
    public GameObject ZoomBar;
    public GameObject PlayTime;
    public GameObject ErrorMessage;
    public GameObject CategoryContent;
    public GameObject Tip_Obj;
    public GameObject ETCBar;
    public GameObject FilterBtn;
    public GameObject CaptureBtn;
    public GameObject AutoSelectImg;
    public GameObject BackGround;
    public GameObject CaptueObject;

    public Sprite ZoomIn;
    public Sprite ZoomOut;

    // �����
    public AudioSource ButtonEffect;
    public AudioClip ButtonSound;

    // �׺���̼�â, ����â On/Off
    public string MoveDir;
    public float navi_t;
    public float langnavi_t;
    public float filternavi_t;
    public float ETCnavi_t;
    private bool touchfinish = false;
    public static float touchCount;

    public RectTransform NaviRect;
    public RectTransform ETCRect;
    public RectTransform LangRect;
    public Image LangChildImg;

    string password;

    public float minimapCamera_x;

    public bool NaviOn = false;
    public bool langNaviOn = false;
    public bool filterNaviOn = false;
    public bool ETCNaviOn = false;
    public bool moveNavi = false;
    public bool movelangNavi = false;
    public bool movefilterNavi = false;
    public bool moveETCNavi = false;
    public static bool MoveCamera = false;
    public bool WantNoLabel = false;
    public static bool AnyError = false;
    public static float ErrorMessageTime;
    public static bool Readpulse = false;

    public static bool SettingLabelPosition = false;
    public bool StartMiniMapDrag = false;

    public static string PrevMode;
    private bool alreadynaviLog = false;
    private bool alreadyarrowLog = false;
    public bool alreadywaitingLog = false;

    /// <summary>
    /// ����
    /// </summary>
    public static float waitingTime = 300;
    public float ResetPositionTime;
    string ManagerModePassword = "025697178";
    public Vector3 Arrowpos_normal = new Vector3(213.0f, 197.0f);
    //public Vector3 Arrowpos_extend = new Vector3(286.0f, 180.0f);
    public static float barOpen = 472f;
    public static float barClose = 60f;
    public static uint startlabel_x;
    public static uint startlabel_y;
    public static string MainMode = "XRMode";
    float arrowval = 40f;
    int modeNum = 6;
    Scrollbar naviscroll;
    GameObject NaviLabel;

    public float touchtime;
    int count_set;
    bool stoponce = false;
    bool CheckfilterTime = false;
    float checkfilteropen;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// ������ �����Ҷ�
    /// </summary>
    private void OnApplicationQuit()
    {
    }

    public void UISetting()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "XRMode":
                break;
            case "NamSanHMode":
                break;
        }
    }

    /// <summary>
    /// �޴��ٿ� �ִ� ��� ������ �������� ���
    /// </summary>
    /// <param name="btn"></param>
    public void Menu(GameObject btn)
    {
        switch (btn.name)
        {
            case "LiveMode":
                break;
            case "XRMode":
                break;
            case "NamSanHMode":
                break;
            case "Language":
                break;
            case "Tip":
                break;
            case "Capture":
                break;
            case "Setting":
                break;
        }
    }

    /// <summary>
    /// ȭ��ǥ��ư �������� ��� ��  UI�� �޾ƿͼ� �α� ����� (�ѹ���)
    /// </summary>
    /// <param name="btn"></param>
    public void Arrow_pointerDown(GameObject btn)
    {
        if (MiniMap_Background.transform.parent.GetChild(2).GetChild(0).GetComponent<RectTransform>().anchoredPosition.x == 30)
        {
            PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Slow);
            gamemanager.speed_enum = GameManager.Speed_enum.slow;
        }
        else
        {
            PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Middle);
            speed_enum = Speed_enum.middle;
        }
        MoveDir = btn.name;
        MoveCamera = true;
        if (alreadyarrowLog == false)
        {
            if (XRMode.PanFreq != XRMode.panFreq_ARR)
            {
                XRMode.PanFreq = XRMode.panFreq_ARR;
                PanTiltControl_v2.PanTiltControl.SetFreq(PanTiltControl_v2.PanTiltControl.Motor.Pan, PanTiltControl.Speed.Middle);
                UnityEngine.Debug.Log(XRMode.PanFreq);
            }
            WriteLog(NormalLogCode.AR_StartArrow, "AR_StartArrow(" + btn.name + ")_(" + PanTiltControl.NowPanPulse + ", " + PanTiltControl.NowTiltPulse + ")", GetType().ToString());

            alreadyarrowLog = true;
        }
    }


    /// <summary>
    /// ȭ��ǥ ��ư ������ �������� ���(ȭ��ǥ�� �����Ͽ� �������� ����) �α� �����(�ѹ���)
    /// </summary>
    public void Arrow_pointerUp()
    {
        if (alreadyarrowLog == true)
        {
            PanTiltControl.Stop();
            WriteLog(NormalLogCode.AR_FinishArrow, "AR_FinishArrow (" + PanTiltControl.NowPanPulse + ", " + PanTiltControl.NowTiltPulse + ")", GetType().ToString());

            alreadyarrowLog = false;
        }
        MoveCamera = false;
    }

    /// <summary>
    /// ��ư Ŭ���ϸ� ȿ���� ���
    /// </summary>
    public void ButtonClickSound()
    {
        ButtonEffect.clip = ButtonSound;
        ButtonEffect.Play();
    }

    /// <summary>
    /// �̴ϸʿ� ��ġ ����
    /// </summary>
    public void Minimap_TouchOn()
    {
        PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Fast);
        gamemanager.speed_enum = GameManager.Speed_enum.fast;
        StartMiniMapDrag = true;
    }

    /// <summary>
    /// �̴ϸʿ� ��ġ �Ϸ�
    /// </summary>
    public void Minimap_TouchOff()
    {
        StartMiniMapDrag = false;
    }

    public void CaptureCamera()
    {
        CaptueObject.gameObject.SetActive(true);
        CaptueObject.gameObject.GetComponent<ScreenCapture>().customMark.gameObject.SetActive(true);

        SetMark();

        ScreenCapture.startflasheffect = true;
        CaptueObject.gameObject.GetComponent<ScreenCapture>().flasheffect.SetActive(true);
        CaptueObject.gameObject.GetComponent<ScreenCapture>().flasheffect.GetComponent<Image>().color = CaptueObject.gameObject.GetComponent<ScreenCapture>().flashColor;
        BackGround.SetActive(true);
    }

    public void CaptureEndCamera()
    {
        BackGround.SetActive(false);
        CaptueObject.gameObject.SetActive(false);
    }

    public void WaitStartCap()
    {
        Invoke("waitcapture", 0.1f);
        //waitcapture();
    }

    public static Vector3 originPos;

    public void SetMark()
    {
        GameObject markcustom = CaptueObject.gameObject.GetComponent<ScreenCapture>().customMark;

        markcustom.transform.GetChild(0).gameObject.GetComponent<Text>().text = DateTime.Now.ToString("yyyy.MM.dd HH:mm");

        originPos = markcustom.transform.localPosition;

        if (SceneManager.GetActiveScene().name == "XRMode")
        {
            markcustom.transform.parent = xrMode.CameraWindow.transform.GetChild(0).gameObject.transform;
            markcustom.transform.localPosition = new Vector3(0, 0.41f);
            markcustom.transform.localScale = new Vector3(1.28f, 1.28f, 1.28f);
        }
        else if (SceneManager.GetActiveScene().name == "NamSanHMode")
        {
            markcustom.transform.parent = namsanMode.CameraWindow.transform.GetChild(7).gameObject.transform;
            markcustom.transform.localPosition = Vector3.zero;
            markcustom.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void waitcapture()
    {
        //ScreenCapture.startflasheffect = false;
        CaptueObject.gameObject.GetComponent<ScreenCapture>().ClickScreenShot();
        CaptueObject.gameObject.GetComponent<ScreenCapture>().ReadyToCapture();
        ButtonClickSound();
    }

    public void WatingtoLive()
    {
        if (ModeActive[0] == true)          // Live��尡 Ȱ��ȭ ���¶��
        {
            // �׺���̼� �� ���ֱ�
            if (NaviRect.sizeDelta.x > barClose)
            {
                navi_t = 0;
                NaviOn = true;
                moveNavi = true;
                //NavigationBar.GetComponent<RectTransform>().sizeDelta = new Vector2(80f, 1080);
                //NavigationBar.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 0;
                //NavigationBar.transform.GetChild(0).gameObject.SetActive(false);
                //NaviOn = false;
                //moveNavi = false;
            }
            else if (NaviRect.sizeDelta.x <= barClose)
            {
                NaviRect.sizeDelta = new Vector2(barClose, 1080);
                NavigationBar.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 0;
                NavigationBar.transform.GetChild(0).gameObject.SetActive(false);
                NaviOn = false;
                moveNavi = false;
            }
            if (LangRect.sizeDelta.x > barClose)        // ����â ���ֱ�
            {
                langnavi_t = 0;
                langNaviOn = true;
                movelangNavi = true;
            }
            //label.Narration.Stop();         // �󼼼��� �����̼� ����

            // �α� ������ ������(prevMode)�� Live���� �����س���
            WriteLog(NormalLogCode.ChangeMode, "ChangeMode : Start(" + PrevMode + " - " + "LiveMode)", GetType().ToString());
            WriteLog(NormalLogCode.ChangeMode, "ChangeMode : Finish(" + PrevMode + " - " + "LiveMode)", GetType().ToString());
            PrevMode = "LiveMode";
            // �������� �� AR��� ������ ��Ȱ��ȭ�ϰ� Live��� ������ Ȱ��ȭ
            MenuBar.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
            MenuBar.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            WantNoLabel = true;
        }
    }

    public void DragArrow()
    {
        Vector3 touchpo = Input.GetTouch(0).position;

        switch (Input.GetTouch(0).phase)
        {
            case TouchPhase.Stationary:
                if (touchtime > 0.15f && touchtime < 0.2f)
                {
                    Arrow.gameObject.SetActive(false);
                    Arrow.transform.position = touchpo;
                    //Arrow.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(-3.6f, 0);

                    if (MiniMap_CameraGuide.gameObject.activeSelf)
                    {
                        MiniMap_CameraGuide.gameObject.SetActive(false);
                    }
                    PanTiltControl.Stop();
                }
                else if (touchtime > 0.2f)
                {
                    if (touchpo == Arrow.transform.position)
                    {
                        Arrow.gameObject.SetActive(false);
                        Arrow.transform.position = touchpo;
                        //Arrow.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(-3.6f, 0);

                        if (MiniMap_CameraGuide.gameObject.activeSelf)
                        {
                            MiniMap_CameraGuide.gameObject.SetActive(false);
                        }

                        PanTiltControl.Stop();
                    }
                    else if (touchpo != Arrow.transform.position)
                    {
                        if (touchpo.x <= Arrow.transform.position.x + 118 && touchpo.x >= Arrow.transform.position.x - 118 && touchpo.y <= Arrow.transform.position.y + 118 && touchpo.y >= Arrow.transform.position.y - 118)
                        {
                            Arrow.transform.GetChild(0).gameObject.transform.position = touchpo;
                        }
                        else
                        {
                            float arrx;
                            float arry;
                            if (touchpo.x >= Arrow.transform.position.x + 118)
                            {
                                arrx = Arrow.transform.position.x + 100;
                            }
                            else if (touchpo.x <= Arrow.transform.position.x - 118)
                            {
                                arrx = Arrow.transform.position.x - 100;
                            }
                            else
                            {
                                arrx = touchpo.x;
                            }

                            if (touchpo.y >= Arrow.transform.position.y + 118)
                            {
                                arry = Arrow.transform.position.y + 100;
                            }
                            else if (touchpo.y <= Arrow.transform.position.y - 118)
                            {
                                arry = Arrow.transform.position.y - 100;
                            }
                            else
                            {
                                arry = touchpo.y;
                            }

                            Arrow.transform.GetChild(0).gameObject.transform.position = new Vector3(arrx, arry);
                        }

                        if ((touchpo.x - Arrow.transform.position.x) > arrowval)
                        {
                            MoveDir = "Right";
                        }
                        else if ((touchpo.x - Arrow.transform.position.x) < -arrowval)
                        {
                            MoveDir = "Left";
                        }
                        else if ((touchpo.x - Arrow.transform.position.x) <= arrowval && (touchpo.x - Arrow.transform.position.x) >= -arrowval)
                        {
                            if ((touchpo.y - Arrow.transform.position.y) <= arrowval && (touchpo.y - Arrow.transform.position.y) >= -arrowval)
                            {
                                MoveDir = null;
                                PanTiltControl.Stop();

                                if (alreadyarrowLog == false)
                                {
                                    WriteLog(NormalLogCode.AR_FinishArrow, "AR_FinishArrow (" + PanTiltControl.NowPanPulse + ", " + PanTiltControl.NowTiltPulse + ")", GetType().ToString());
                                    alreadyarrowLog = true;
                                }
                            }
                        }

                        if ((touchpo.y - Arrow.transform.position.y) > arrowval)
                        {
                            MoveDir = "Up";
                        }
                        else if ((touchpo.y - Arrow.transform.position.y) < -arrowval)
                        {
                            MoveDir = "Down";
                        }
                        else if ((touchpo.y - Arrow.transform.position.y) <= arrowval && (touchpo.y - Arrow.transform.position.y) >= -arrowval)
                        {
                            if (alreadyarrowLog == false)
                            {
                                if ((touchpo.x - Arrow.transform.position.x) <= arrowval && (touchpo.x - Arrow.transform.position.x) >= -arrowval)
                                {
                                    MoveDir = null;
                                    PanTiltControl.Stop();

                                    WriteLog(NormalLogCode.AR_FinishArrow, "AR_FinishArrow (" + PanTiltControl.NowPanPulse + ", " + PanTiltControl.NowTiltPulse + ")", GetType().ToString());
                                }
                                alreadyarrowLog = true;
                            }
                        }

                        MoveCamera = true;

                        if (alreadyarrowLog == false)
                        {
                            if (XRMode.PanFreq != XRMode.panFreq_ARR)
                            {
                                XRMode.PanFreq = XRMode.panFreq_ARR;
                                PanTiltControl_v2.PanTiltControl.SetFreq(PanTiltControl_v2.PanTiltControl.Motor.Pan, PanTiltControl.Speed.Middle);
                                Debug.Log(XRMode.PanFreq);
                            }

                            WriteLog(NormalLogCode.AR_StartArrow, "AR_StartArrow(" + MoveDir + ")_(" + PanTiltControl.NowPanPulse + ", " + PanTiltControl.NowTiltPulse + ")", GetType().ToString());
                            alreadyarrowLog = true;
                        }
                    }
                }
                break;
            case TouchPhase.Moved:
                if (touchtime > 0.15f && touchtime <= 0.17f)
                {
                    Arrow.gameObject.SetActive(false);
                    Arrow.transform.position = touchpo;
                    //Arrow.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(-3.6f, 0);

                    if (MiniMap_CameraGuide.gameObject.activeSelf)
                    {
                        MiniMap_CameraGuide.gameObject.SetActive(false);
                    }
                }
                else if (touchtime > 0.16f)
                {
                    float arrx;
                    float arry;
                    if (touchpo.x >= Arrow.transform.position.x + 118)
                    {
                        arrx = Arrow.transform.position.x + 100;
                    }
                    else if (touchpo.x <= Arrow.transform.position.x - 118)
                    {
                        arrx = Arrow.transform.position.x - 100;
                    }
                    else
                    {
                        arrx = touchpo.x;
                    }

                    if (touchpo.y >= Arrow.transform.position.y + 118)
                    {
                        arry = Arrow.transform.position.y + 100;
                    }
                    else if (touchpo.y <= Arrow.transform.position.y - 118)
                    {
                        arry = Arrow.transform.position.y - 100;
                    }
                    else
                    {
                        arry = touchpo.y;
                    }

                    Arrow.transform.GetChild(0).gameObject.transform.position = new Vector3(arrx, arry);
                    if ((touchpo.x - Arrow.transform.position.x) > arrowval)
                    {
                        MoveDir = "Right";
                    }
                    else if ((touchpo.x - Arrow.transform.position.x) < -arrowval)
                    {
                        MoveDir = "Left";
                    }
                    else if ((touchpo.x - Arrow.transform.position.x) <= arrowval && (touchpo.x - Arrow.transform.position.x) >= -arrowval)
                    {
                        if ((touchpo.y - Arrow.transform.position.y) <= arrowval && (touchpo.y - Arrow.transform.position.y) >= -arrowval)
                        {
                            MoveDir = null;
                            PanTiltControl.Stop();
                            if (alreadyarrowLog == false)
                            {
                                WriteLog(NormalLogCode.AR_FinishArrow, "AR_FinishArrow (" + PanTiltControl.NowPanPulse + ", " + PanTiltControl.NowTiltPulse + ")", GetType().ToString());
                                alreadyarrowLog = true;
                            }
                        }
                    }

                    if ((touchpo.y - Arrow.transform.position.y) > arrowval)
                    {
                        MoveDir = "Up";
                    }
                    else if ((touchpo.y - Arrow.transform.position.y) < -arrowval)
                    {
                        MoveDir = "Down";
                    }
                    else if ((touchpo.y - Arrow.transform.position.y) <= arrowval && (touchpo.y - Arrow.transform.position.y) >= -arrowval)
                    {
                        if ((touchpo.x - Arrow.transform.position.x) <= arrowval && (touchpo.x - Arrow.transform.position.x) >= -arrowval)
                        {
                            MoveDir = null;
                            PanTiltControl.Stop();

                            if (alreadyarrowLog == false)
                            {
                                WriteLog(NormalLogCode.AR_FinishArrow, "AR_FinishArrow (" + PanTiltControl.NowPanPulse + ", " + PanTiltControl.NowTiltPulse + ")", GetType().ToString());
                            }
                        }
                    }

                    MoveCamera = true;

                    if (alreadyarrowLog == false)
                    {
                        if (XRMode.PanFreq != XRMode.panFreq_ARR)
                        {
                            XRMode.PanFreq = XRMode.panFreq_ARR;
                            PanTiltControl_v2.PanTiltControl.SetFreq(PanTiltControl_v2.PanTiltControl.Motor.Pan, PanTiltControl.Speed.Middle);
                            Debug.Log(XRMode.PanFreq);
                        }

                        WriteLog(NormalLogCode.AR_StartArrow, "AR_StartArrow(" + MoveDir + ")_(" + PanTiltControl.NowPanPulse + ", " + PanTiltControl.NowTiltPulse + ")", GetType().ToString());
                        alreadyarrowLog = true;
                    }
                }
                break;
            case TouchPhase.Ended:
                touchtime = 0;
                Arrow.transform.GetChild(0).gameObject.transform.localPosition = Vector3.zero;

                Arrow_pointerUp();

                if (NaviRect.sizeDelta.x == barOpen)
                {
                    Arrow.gameObject.SetActive(false);
                }
                else if (NaviRect.sizeDelta.x == barClose)
                {
                    Arrow.gameObject.SetActive(true);
                    Arrow.transform.position = Arrowpos_normal;
                }
                break;
        }
    }
}
