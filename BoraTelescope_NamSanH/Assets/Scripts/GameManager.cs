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
    public float LabelScale;

    public XRModeLabelPosition(string labelname, float label_x, float label_y, float labelsize)
    {
        LabelName = labelname;
        Label_X = label_x;
        Label_Y = label_y;
        LabelScale = labelsize;
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
    public float StartPosition_x;
    public float StartPosition_y;
    public float ChangeValue_x;
    public float ChangeValue_y;
    public int WaitingTime;

    public PanTiltRange(float minx, float maxx, float miny, float maxy, float startlabel_x, float startlabel_y, float valueX, float valueY, int waittime)
    {
        Min_Pan = minx;
        Max_Pan = maxx;
        Min_Tilt = miny;
        Max_Tilt = maxy;
        StartPosition_x = startlabel_x;
        StartPosition_y = startlabel_y;
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
    //public LabelDetail labeldetail;
    public WaitingMode waitingMode;
    public ChangeWaitingMode changewaiting;
    public SetDragRange setdragrange;
    public SetRecord setrecord;
    public BoraJoyStick joystick;
    public UILanguage uilang;
    public CategoryContents category;

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
    public GameObject ErrorMessage;
    public GameObject CategoryContent;
    public GameObject Tip_Obj;
    public GameObject CaptureBtn;
    public GameObject BackGround;
    public GameObject CaptueObject;
    public GameObject Homebtn;
    public GameObject Tipbtn;

    public Sprite ZoomIn;
    public Sprite ZoomOut;

    // �����
    public AudioSource ButtonEffect;
    public AudioClip ButtonSound;

    // �׺���̼�â, ����â On/Off
    public string MoveDir;
    public float navi_t;
    public float langnavi_t;
    public float ETCnavi_t;
    private bool touchfinish = false;
    public static float touchCount;

    public RectTransform NaviRect;
    public RectTransform LangRect;
    public Image LangChildImg;

    string password;

    public float minimapCamera_x;

    public bool NaviOn = false;
    public bool langNaviOn = false;
    public bool moveNavi = false;
    public bool movelangNavi = false;
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
    //public Vector3 Arrowpos_normal = new Vector3(213.0f, 197.0f);
    public Vector3 Arrowpos_extend = new Vector3(286.0f, 180.0f);
    public static float barOpen = 472f;
    public static float barClose = 60f;
    public static uint startlabel_x;
    public static uint startlabel_y;
    public static string MainMode = "NamSanHMode";
    float arrowval = 40f;
    int modeNum = 6;
    public GameObject NaviLabel;

    public float touchtime;
    int count_set;
    bool stoponce = false;
    bool CheckfilterTime = false;
    float checkfilteropen;
    bool WriteLogOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager");
        gamemanager = GM.GetComponent<GameManager>();
        DontDestroyOnLoad(GM);

        UISetting();

        // �ð� �ʱ�ȭ
        touchCount = 0;
        touchfinish = false;
        navi_t = 0;
        langnavi_t = 0;

        count_set = 0;
        NaviRect = NavigationBar.GetComponent<RectTransform>();
        LangRect = LanguageBar.GetComponent<RectTransform>();
        LangChildImg = LanguageBar.transform.GetChild(0).gameObject.GetComponent<Image>();
        // ����â �ݾƳ���(�ε�ȭ�鿡�� �Ⱥ���.)
        LangRect.sizeDelta = new Vector2(barClose, 1080);
        LangChildImg.fillAmount = 0;
        LanguageBar.transform.GetChild(0).gameObject.SetActive(false);
        langNaviOn = false;
        movelangNavi = false;

        //ChangeLanguage(LanguageBar.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        curlang = currentLang;
        // �׺���̼� â, �󼼼���â �ӵ� ���� 
        navi_t += Time.deltaTime * 0.1f;
        langnavi_t += Time.deltaTime * 0.1f;
        if (ResetPositionTime < 99)
        {
            ResetPositionTime += Time.deltaTime;
        }

        // ��ġ ���ϴ� �ð��� �����Ͽ� ������ ��ȯ�ϱ� ����
        if (Input.GetMouseButtonDown(0))        // ���콺 Ŭ����
        {
            touchCount = 0;
            touchfinish = true;
            ResetPositionTime = 0;
        }
        else if (Input.GetMouseButtonUp(0))     // ���콺 ��ư���� ����
        {
            touchfinish = false;
        }

        // ��ġ�� ���ϰ� �ְ� ���� ���� ����尡 �ƴ϶��
        // ��ġ ������ 5��(�������) �� �Ǵ� �ð��� ������ ��ȯ
        if (touchfinish == false && SceneManager.GetActiveScene().name != "WaitingMode")
        {
            touchCount += Time.deltaTime;
            if ((int)touchCount >= waitingTime)
            {
                Readpulse = false;
                CallWaitingMode();
            }
        }

        // �����ڸ��� ����
        // 10���̻� �ΰ� ��ġ�ϰ� ������ ����Ű���� Ȱ��ȭ
        // ��й�ȣ ������ �����ڸ��� ����
        if (entermode == true)
        {
            if (MenuBar.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text == ManagerModePassword)
            {
                password = "";
                MenuBar.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
                SettingLabel();
            }
            else
            {
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha0))
                    {
                        password += "0";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        password += "1";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        password += "2";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        password += "3";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        password += "4";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha5))
                    {
                        password += "5";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha6))
                    {
                        password += "6";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha7))
                    {
                        password += "7";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha8))
                    {
                        password += "8";
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha9))
                    {
                        password += "9";
                    }
                    MenuBar.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = password;
                }
            }
        }
        if (startcount == true)
        {
            if (entermode == false)
            {
                if (SettingLabelPosition == false)
                {
                    if ((int)count >= 10)
                    {
                        SetManagerPage();
                        entermode = true;
                    }
                    else if ((int)count < 10)
                    {
                        count += Time.deltaTime;
                    }
                }
            }
        }

        if (moveNavi == true)
        {
            NaviArr_set();
        }

        if (movelangNavi == true)
        {
            SelectLanguageChange();
        }

        if (SceneManager.GetActiveScene().name == "XRMode")
        {
            if (ResetPositionTime > 60 && ResetPositionTime < 61)
            {
                if (!Tip_Obj.gameObject.activeSelf)
                {
                    if (WriteLogOnce == false)
                    {
                        gamemanager.WriteLog(LogSendServer.NormalLogCode.ClickHomeBtn, "Reset All Function", GetType().ToString());
                        WriteLogOnce = true;
                    }
                    if (SceneManager.GetActiveScene().name == "XRMode")
                    {
                        StartCoroutine(Home_Btn_XR());
                    }
                    else if (SceneManager.GetActiveScene().name == "NamSanHMode")
                    {
                        Home_Btn_NamSanH();
                    }
                    ResetPositionTime = 0;
                }
            }
        }

        if (SceneManager.GetActiveScene().name == "XRMode")
        {
            if (Input.touchCount == 1)
            {
                if (joystick.enabled == false)
                {
                    if (touchtime > 0.15f)
                    {
                        if (ZoomBar.GetComponent<RectTransform>().anchoredPosition.x == 30)
                        {
                            if (speed_enum != GameManager.Speed_enum.slow)
                            {
                                print("slow");
                                PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Slow);
                                gamemanager.speed_enum = GameManager.Speed_enum.slow;
                            }
                        }
                        else
                        {
                            if (speed_enum != GameManager.Speed_enum.middle)
                            {
                                print("middle");
                                PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Middle);
                                speed_enum = Speed_enum.middle;
                            }
                        }

                        setdragrange.ALLFuncDragRange();
                    }
                }
                stoponce = true;
            }
            else if (Input.touchCount > 1 || Input.touchCount == 0)
            {
                touchtime = 0;
                //Arrow.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(-3.6f, 0);

                if (stoponce == true)
                {
                    Arrow_pointerUp();
                    stoponce = false;
                }

                Arrow.gameObject.SetActive(true);
                Arrow.transform.position = Arrowpos_extend;
            }
        }
    }

    /// <summary>
    /// ������ �����Ҷ�
    /// </summary>
    private void OnApplicationQuit()
    {
        PanTiltControl.DisConnect();
        WriteLog(NormalLogCode.Connect_Pantilt, "Connect_Pantilt:Off", GetType().ToString());

        var processList = System.Diagnostics.Process.GetProcessesByName("XRTeleSpinCam");
        if (processList.Length != 0)
        {
            processList[0].Kill();
        }
        WriteLog(NormalLogCode.Connect_Camera, "Connect_Camera:Off", GetType().ToString());
        WriteLog(NormalLogCode.EndContents, "EndContents", GetType().ToString());
        AwakeOnce = false;
        WriteLog(NormalLogCode.Connect_SystemControl, "Connect_SystemControl_Off", GetType().ToString());
        Disconnect_Button();
    }

    public void UISetting()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "XRMode":
                // �� ��ũ��Ʈ ����
                xrMode = GameObject.Find("XRMode").GetComponent<XRMode>();
                xrMode_manager = GameObject.Find("XRMode").GetComponent<XRMode_Manager>();

                //labeldetail = GameObject.Find("ARMode").GetComponent<LabelDetail>();
                //label = GM.gameObject.GetComponent<Label>();

                // XR���(WantNoLabel == false) �⺻�� : �̴ϸ�, �޴���, �׺���̼�â, Tip, Capture��ư Ȱ��ȭ/ ȭ��ǥ, ����â ��Ȱ��ȭ
                // Live���(WantNoLabel == true) �⺻�� : �̴ϸ�, �޴���, ȭ��ǥ, Tip Ȱ��ȭ/ ȭ��ǥ, �׺���̼�â, ����â ��Ȱ��ȭ
                UI_All.gameObject.SetActive(true);
                for (int index = 0; index < UI_All.transform.childCount; index++)
                {
                    UI_All.transform.GetChild(index).gameObject.SetActive(true);
                }

                Arrow.gameObject.SetActive(true);
                Arrow.gameObject.transform.position = Arrowpos_extend;
                NavigationBar.gameObject.SetActive(false);
                MiniMap_Background.transform.parent.gameObject.SetActive(true);
                MiniMap_Background.gameObject.SetActive(true);
                MenuBar.transform.GetChild(5).gameObject.SetActive(false);

                if (WantNoLabel == false)
                {
                    xrMode.AllMapLabels.gameObject.SetActive(true);

                    for (int index = 0; index < 3; index++)
                    {
                        MenuBar.transform.GetChild(0).gameObject.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    MenuBar.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    //MenuBar.GetComponent<RectTransform>().sizeDelta = new Vector2(350, 0);
                    NaviOn = true;
                }
                else if (WantNoLabel == true)
                {
                    for (int index = 0; index < 3; index++)
                    {
                        MenuBar.transform.GetChild(0).gameObject.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    MenuBar.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    xrMode.AllMapLabels.gameObject.SetActive(false);
                    
                    NaviRect.sizeDelta = new Vector2(barClose, 1080);
                    NavigationBar.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 0;
                    NavigationBar.transform.GetChild(0).gameObject.SetActive(false);
                    Invoke("SeeNavibar", 0.3f);
                }
                TipOpen();
                break;
            case "NamSanHMode":
                namsanMode = GameObject.Find("NamSanH").GetComponent<NamSanHMode>();
                UI_All.SetActive(true);
                for (int index = 0; index < UI_All.transform.childCount; index++)
                {
                    UI_All.transform.GetChild(index).gameObject.SetActive(false);
                }
                MenuBar.SetActive(true);
                for (int index = 0; index < 3; index++)
                {
                    MenuBar.transform.GetChild(0).gameObject.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                MenuBar.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                NavigationBar.SetActive(true);
                BackGround.transform.parent.gameObject.SetActive(true);
                LanguageBar.gameObject.SetActive(true);
                //NaviRect.sizeDelta = new Vector2(barOpen, 1080);
                //NavigationBar.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = 0;
                Tip_Obj.SetActive(true);
                NavigationBar.transform.GetChild(0).gameObject.SetActive(true);
                //label.SelectCategortButton(CategoryContent.transform.GetChild(0).gameObject);
                TipOpen();
                Invoke("SeeNavibar", 0.3f);
                break;
            case "Loading":
                loading = GameObject.Find("Loading_UI").GetComponent<Loading>();
                UI_All.gameObject.SetActive(false);
                break;
            case "WaitingMode":
                //waitingMode = GM.gameObject.GetComponent<WaitingMode>();

                if (AnyError == false)
                {
                    UI_All.gameObject.SetActive(false);
                    MenuBar.gameObject.transform.GetChild(5).gameObject.SetActive(false);
                }
                else if (AnyError == true)
                {
                    UI_All.gameObject.SetActive(true);
                    for (int index = 0; index < UI_All.transform.childCount; index++)
                    {
                        UI_All.transform.GetChild(index).gameObject.SetActive(false);
                    }
                    MenuBar.gameObject.SetActive(true);
                    MenuBar.gameObject.GetComponent<Image>().enabled = false;
                    for (int index = 0; index < MenuBar.gameObject.transform.childCount; index++)
                    {
                        MenuBar.transform.GetChild(index).gameObject.SetActive(false);
                    }
                    MenuBar.transform.GetChild(5).gameObject.SetActive(true);
                }
                break;
        }
    }


    /// <summary>
    /// ��� UI Ȱ��ȭ
    /// </summary>
    public void SeeNavibar()
    {
        NavigationBar.gameObject.SetActive(true);
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
                if (SceneManager.GetActiveScene().name == "XRMode")
                {
                    // �޴����� �������ܿ��� Live��� ��Ȱ��ȭ, AR��� Ȱ��ȭ
                    MenuBar.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    MenuBar.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);

                    if (xrMode.AllMapLabels.gameObject.activeSelf)
                    {
                        xrMode.AllMapLabels.gameObject.SetActive(false);
                    }

                    WantNoLabel = true;
                }
                else if (SceneManager.GetActiveScene().name != "XRMode")
                {
                    // �׺���̼� â ��Ȱ��ȭ(�����忡���� �׺���̼�â ������� ����)
                    if (NaviRect.sizeDelta.x > barClose)
                    {
                        navi_t = 0;
                        NaviOn = true;
                        moveNavi = true;
                    }

                    // ����â ��Ȱ��ȭ
                    if (LangRect.sizeDelta.x > barClose)
                    {
                        langnavi_t = 0;
                        langNaviOn = true;
                        movelangNavi = true;
                    }

                    // �����̼� ���� ����
                    namsanMode.Narration.Stop();

                    // �޴����� �������ܿ��� Live��� ��Ȱ��ȭ, AR��� Ȱ��ȭ
                    for (int index = 0; index < MenuBar.transform.GetChild(0).transform.childCount; index++)
                    {
                        MenuBar.transform.GetChild(0).gameObject.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    MenuBar.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);


                    WantNoLabel = true;

                    Loading.nextScene = "XRMode";
                    SceneManager.LoadScene("Loading");
                }
                break;
            case "XRMode":
                if (SceneManager.GetActiveScene().name == "XRMode")
                {
                    if (!Tip_Obj.activeSelf)        // Tip�� ��Ȱ��ȭ���¶��
                    {
                        if (ModeActive[1] == true)      // AR��尡 Ȱ��ȭ�Ǿ��ִٸ�
                        {
                            // ����â ��Ȱ��ȭ
                            if (LangRect.sizeDelta.x > barClose)
                            {
                                langnavi_t = 0;
                                langNaviOn = true;
                                movelangNavi = true;
                            }

                            // �޴����� �������ܿ��� Live��� ��Ȱ��ȭ, AR��� Ȱ��ȭ
                            MenuBar.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                            MenuBar.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);

                            // AR��� ���� ��Ȱ��ȭ�Ǿ��ִٸ� �� Ȱ��ȭ
                            if (!xrMode.AllMapLabels.gameObject.activeSelf)
                            {
                                xrMode.AllMapLabels.gameObject.SetActive(true);
                            }
                            //NaviArr_set();
                            WantNoLabel = false;
                        }
                        else if (ModeActive[1] == false)        //AR��尡 ��Ȱ��ȭ �Ǿ��ִٸ� �����޼��� Ȱ��ȭ�ϰ� ���� ���� Live �����ϱ�
                        {
                            ErrorMessage.gameObject.SetActive(true);
                            if (PrevMode == "LiveMode")
                            {
                                WantNoLabel = true;
                            }
                        }
                    }
                    else if (Tip_Obj.activeSelf)       // Tip �̹����� Ȱ��ȭ �Ǿ��ִٸ� ��Ȱ��ȭ
                    {
                        Tip_Obj.SetActive(false);
                        TipClose();
                    }
                }
                else if (SceneManager.GetActiveScene().name != "XRMode")
                {
                    // �׺���̼� â ��Ȱ��ȭ(�����忡���� �׺���̼�â ������� ����)
                    if (NaviRect.sizeDelta.x > barClose)
                    {
                        navi_t = 0;
                        NaviOn = true;
                        moveNavi = true;
                    }

                    // ����â ��Ȱ��ȭ
                    if (LangRect.sizeDelta.x > barClose)
                    {
                        langnavi_t = 0;
                        langNaviOn = true;
                        movelangNavi = true;
                    }

                    // �����̼� ���� ����
                    namsanMode.Narration.Stop();

                    // �޴����� �������ܿ��� Live��� ��Ȱ��ȭ, AR��� Ȱ��ȭ
                    for (int index = 0; index < MenuBar.transform.GetChild(0).transform.childCount; index++)
                    {
                        MenuBar.transform.GetChild(0).gameObject.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    MenuBar.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);

                    WantNoLabel = false;
                    MenuBar.GetComponent<UILanguage>().SceneChagneSetOrigin();

                    Loading.nextScene = "XRMode";
                    SceneManager.LoadScene("Loading");
                }
                break;
            case "NamSanHMode":
                if (SceneManager.GetActiveScene().name == "NamSanHMode")
                {
                    // �׺���̼�â Ȱ��ȭ
                    if (NaviRect.sizeDelta.x > barClose)
                    {
                        navi_t = 0;
                        NaviOn = true;
                        moveNavi = true;
                        langnavi_t = 0;
                        langNaviOn = true;
                        movelangNavi = true;
                    }
                    else if (NaviRect.sizeDelta.x < barOpen)
                    {
                        navi_t = 0;
                        moveNavi = true;
                        NaviOn = false;
                        langnavi_t = 0;
                        langNaviOn = true;
                        movelangNavi = true;
                    }

                    // ���� â ��Ȱ��ȭ
                    if (LangRect.sizeDelta.x > barClose)
                    {
                        langnavi_t = 0;
                        langNaviOn = true;
                        movelangNavi = true;
                    }
                }
                else if (SceneManager.GetActiveScene().name != "NamSanHMode")
                {
                    MenuBar.GetComponent<UILanguage>().SceneChagneSetOrigin();

                    // �޴����� �������ܿ��� Live��� ��Ȱ��ȭ, AR��� Ȱ��ȭ
                    for (int index = 0; index < MenuBar.transform.GetChild(0).transform.childCount; index++)
                    {
                        MenuBar.transform.GetChild(0).gameObject.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    MenuBar.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true);

                    // �׺���̼�â Ȱ��ȭ
                    if (NaviRect.sizeDelta.x < barOpen)
                    {
                        navi_t = 0;
                        moveNavi = true;
                        NaviOn = false;
                    }

                    // ���� â ��Ȱ��ȭ
                    if (LangRect.sizeDelta.x > barClose)
                    {
                        langnavi_t = 0;
                        langNaviOn = true;
                        movelangNavi = true;
                    }

                    Loading.nextScene = "NamSanHMode";
                    SceneManager.LoadScene("Loading");
                }
                break;
            case "Language":
                langnavi_t = 0;
                if (LangRect.sizeDelta.x > barClose)        // ���� ��Ȱ��ȭ
                {
                    MenuBar.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    //langnavi_t = 0;
                    langNaviOn = true;
                    movelangNavi = true;
                }
                else if (LangRect.sizeDelta.x < barOpen)      // ���� Ȱ��ȭ
                {
                    MenuBar.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    //langnavi_t = 0;
                    movelangNavi = true;
                    langNaviOn = false;
                }
                break;
            case "Tip":
                if (!Tip_Obj.activeSelf)        // Tip �̹����� ��Ȱ��ȭ���¸� Ȱ��ȭ
                {
                    Tipbtn.transform.GetChild(0).gameObject.SetActive(true);
                    Tip_Obj.SetActive(true);

                    TipOpen();
                }
                else if (Tip_Obj.activeSelf)      // Tip �̹����� Ȱ��ȭ ���¸� ��Ȱ��ȭ
                {
                    Tipbtn.transform.GetChild(0).gameObject.SetActive(false);
                    Tip_Obj.SetActive(false);
                    TipClose();
                }
                break;
            case "Capture":
                CaptureCamera();
                break;
            case "Setting":
                break;
            case "Navi_Close":      // �׺���̼� â�� x ��ư �����ϸ� �׺���̼�â ��Ȱ��ȭ
                navi_t = 0;
                moveNavi = true;
                break;
            case "LangNavi_Close":      // ���� â�� x ��ư �����ϸ� ���� ��Ȱ��ȭ
                langnavi_t = 0;
                movelangNavi = true;
                MenuBar.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case "Flow":
                category.SelectCategory_Flow();
                break;
            case "Docent":
                category.SelectCategory_Docent();
                break;
        }
    }

    /// <summary>
    /// �׺���̼� â Ȱ��ȭ/��Ȱ��ȭ
    /// </summary>
    public void NaviArr_set()
    {
        Image NaviBackground = NavigationBar.transform.GetChild(0).gameObject.GetComponent<Image>();

        // �׺���̼� ��Ȱ��ȭ
        if (NaviOn == true)
        {
            // ���� ��ȹ : �޴��ٰ� ������� �׺���̼� â�� ����
            // ���� ��ȹ : �׺���̼� â�� ���� �־ ũ�Ⱑ Ŀ��

            // �׺���̼�â �α�
            if (alreadynaviLog == false)
            {
                WriteLog(NormalLogCode.NamSanH_Navigation, "NamSanH_NavigationOff", GetType().ToString());
                alreadynaviLog = true;
            }

            Arrow.gameObject.SetActive(false);
            NavigationBar.gameObject.SetActive(true);   // �׺���̼� â Ȱ��ȭ

            // �׺���̼� â�� ��Ȱ��ȭ�Ǿ� �ֱ� ������ �׺���̼� �� ��ư ��Ȱ��ȭ
            //GameObject NaviLabel = NavigationBar.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            for (int index = 0; index < NaviLabel.transform.childCount; index++)
            {
                NaviLabel.transform.GetChild(index).gameObject.GetComponent<Button>().enabled = false;
            }
            if (NaviRect.sizeDelta.x > barClose)
            {
                NaviRect.sizeDelta = Vector2.Lerp(NaviRect.sizeDelta, new Vector2(barClose - 5f, 1080), navi_t);
                NaviBackground.fillAmount -= 0.5f * navi_t;
            }
            else if (NaviRect.sizeDelta.x <= barClose)
            {
                NaviRect.sizeDelta = new Vector2(barClose, 1080);
                NaviBackground.fillAmount = 0;
                NavigationBar.transform.GetChild(0).gameObject.SetActive(false);
                NaviOn = false;
                moveNavi = false;
                alreadynaviLog = false;
            }
        }
        else if (NaviOn == false)       // �׺���̼� Ȱ��ȭ
        {
            // �׺���̼�â Ȱ��ȭ �α�
            if (alreadynaviLog == false)
            {
                WriteLog(NormalLogCode.NamSanH_Navigation, "NamSanH_NavigationOn", GetType().ToString());
                alreadynaviLog = true;
            }

            NavigationBar.transform.GetChild(0).gameObject.SetActive(true);     // �׺���̼�â Ȱ��ȭ
            Arrow.gameObject.SetActive(false);      // �׺���̼�â�� Ȱ��ȭ �Ǿ��ֱ� ������ ȭ��ǥ�� ��Ȱ��ȭ

            // �׺���̼� â õõ�� Ȱ��ȭ(������ ȿ��)
            if (NaviRect.sizeDelta.x < barOpen)
            {
                NaviRect.sizeDelta = Vector2.Lerp(NaviRect.sizeDelta, new Vector2(barOpen + 5f, 1080), navi_t);
                NaviBackground.fillAmount += 0.5f * navi_t;
            }
            else if (NaviRect.sizeDelta.x >= barOpen)
            {
                NavigationBar.gameObject.SetActive(true);
                NavigationBar.transform.GetChild(0).gameObject.SetActive(true);
                NavigationBar.transform.GetChild(0).gameObject.GetComponent<ScrollRect>().enabled = true;
                NaviRect.sizeDelta = new Vector2(barOpen, 1080);
                NaviBackground.fillAmount = 1;

                // �׺���̼� â�� ��� ������ �׺���̼� �� ��ư Ȱ��ȭ
                //GameObject NaviLabel = NavigationBar.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
                for (int index = 0; index < NaviLabel.transform.childCount; index++)
                {
                    NaviLabel.transform.GetChild(index).gameObject.GetComponent<Button>().enabled = true;
                }
                NaviOn = true;
                moveNavi = false;
                alreadynaviLog = false;
            }
        }
    }

    /// <summary>
    /// �׺���̼�â���� �� �����ϸ� ��忡 ���� ������ �� ����
    /// </summary>
    /// <param name="label"></param>
    public void Navigation(GameObject label)
    {
        if (SceneManager.GetActiveScene().name == "NamSanHMode")
        {
            namsanMode.NaviLabel(label);
        }
    }

    /// <summary>
    /// ȭ��ǥ��ư �������� ��� ��  UI�� �޾ƿͼ� �α� ����� (�ѹ���)
    /// </summary>
    /// <param name="btn"></param>
    public void Arrow_pointerDown(GameObject btn)
    {
        if (ZoomBar.GetComponent<RectTransform>().anchoredPosition.x == 30)
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

    /// <summary>
    /// �޴��ٿ��� ���� ������ �����ϸ� ����â Ȱ��ȭ/��Ȱ��ȭ
    /// </summary>
    public void SelectLanguageChange()
    {
        if (langNaviOn == true)     // ���� â ��Ȱ��ȭ
        {
            if (SceneManager.GetActiveScene().name != "NamSanHMode")        // �����尡 �ƴϸ� �׺���̼� â�� ���¿� ����(Ȱ��ȭ/��Ȱ��ȭ) ȭ��ǥUI Ȱ��ȭ ��Ȱ��ȭ
            {
                if (NaviRect.sizeDelta.x < barOpen)     // �׺���̼� â�� ��Ȱ��ȭ �����̹Ƿ� ȭ��ǥ UI Ȱ��ȭ
                {
                    Arrow.gameObject.SetActive(true);
                    Arrow.transform.position = Arrowpos_extend;
                }
                else if (NaviRect.sizeDelta.x > barClose)     // �׺���̼� â�� Ȱ��ȭ�����̹Ƿ� ȭ��ǥ UI ��Ȱ��ȭ
                {
                    Arrow.gameObject.SetActive(false);
                }
            }
            else if (SceneManager.GetActiveScene().name == "NamSanHMode")      // �����忡���� �׺���̼� â�� ������� ȭ��ǥ UI ��Ȱ��ȭ
            {
                Arrow.gameObject.SetActive(false);
            }

            // ���� â ��Ȱ��ȭ ����
            if (LangRect.sizeDelta.x > barClose)
            {
                LangRect.sizeDelta = Vector2.Lerp(LangRect.sizeDelta, new Vector2(barClose - 5f, 1080), langnavi_t);
                LangChildImg.fillAmount -= 0.5f * langnavi_t;
            }
            else if (LangRect.sizeDelta.x <= barClose)
            {
                LangRect.sizeDelta = new Vector2(barClose, 1080);
                LangChildImg.fillAmount = 0;
                LanguageBar.transform.GetChild(0).gameObject.SetActive(false);
                MenuBar.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                langNaviOn = false;
                movelangNavi = false;
            }
        }
        else if (langNaviOn == false)       // ���� Ȱ��ȭ
        {
            LanguageBar.transform.GetChild(0).gameObject.SetActive(true);       // ���� â Ȱ��ȭ
            Arrow.gameObject.SetActive(false);      // ����â�� Ȱ��ȭ �Ǿ��ֱ� ������ ȭ��ǥ UI ��Ȱ��ȭ
            if (LangRect.sizeDelta.x < barOpen)
            {
                LangRect.sizeDelta = Vector2.Lerp(LangRect.sizeDelta, new Vector2(barOpen + 5f, 1080), langnavi_t);
                LangChildImg.fillAmount += 0.5f * langnavi_t;
            }
            else if (LangRect.sizeDelta.x >= barOpen)
            {
                //LanguageBar.gameObject.SetActive(true);
                LanguageBar.transform.GetChild(0).gameObject.SetActive(true);
                //LanguageBar.transform.GetChild(0).gameObject.GetComponent<ScrollRect>().enabled = true;
                LangRect.sizeDelta = new Vector2(barOpen, 1080);
                LangChildImg.fillAmount = 1;
                langNaviOn = true;
                movelangNavi = false;
            }
        }
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
            markcustom.transform.parent = namsanMode.CameraWindow.transform.GetChild(0).gameObject.transform;
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
                                    WriteLog(NormalLogCode.AR_DragFinish, "AR_DragFinish (" + PanTiltControl.NowPanPulse + ", " + PanTiltControl.NowTiltPulse + ")", GetType().ToString());
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

                            WriteLog(NormalLogCode.AR_DragStart, "AR_DragStart(" + MoveDir + ")_(" + PanTiltControl.NowPanPulse + ", " + PanTiltControl.NowTiltPulse + ")", GetType().ToString());
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
                                WriteLog(NormalLogCode.AR_DragFinish, "AR_DragFinish (" + PanTiltControl.NowPanPulse + ", " + PanTiltControl.NowTiltPulse + ")", GetType().ToString());
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
                                WriteLog(NormalLogCode.AR_DragFinish, "AR_DragFinish (" + PanTiltControl.NowPanPulse + ", " + PanTiltControl.NowTiltPulse + ")", GetType().ToString());
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

                        WriteLog(NormalLogCode.AR_DragStart, "AR_DragStart(" + MoveDir + ")_(" + PanTiltControl.NowPanPulse + ", " + PanTiltControl.NowTiltPulse + ")", GetType().ToString());
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
                    Arrow.transform.position = Arrowpos_extend;
                }
                break;
        }
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    /// <param name="btn"></param>
    public void ChangeLanguage(GameObject btn)
    {
        switch (btn.name)
        {
            case "Korea":
                currentLang = Language_enum.Korea;
                uilang.SelectKorea();
                for (int index = 0; index < NaviLabel.transform.childCount; index++)
                {
                    NaviLabel.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = NaviLabel_K[index];
                }
                break;
            case "English":
                currentLang = Language_enum.English;
                uilang.NotSelectKorea();
                for (int index = 0; index < NaviLabel.transform.childCount; index++)
                {
                    NaviLabel.transform.GetChild(index).gameObject.GetComponent<Image>().sprite = NaviLabel_E[index];
                }
                break;
        }
        if (SceneManager.GetActiveScene().name == "NamSanHMode")
        {
            category.ChangeCategory_lang(btn.name);
            if (namsanMode.timeflow.TFBackground.activeSelf)
            {
                namsanMode.timeflow.ChangeLang();
            }
            if (namsanMode.obj360.activeSelf)
            {
                namsanMode.ReadyTo360(namsanMode.SelectLabel);
            }
        }

        WriteLog(NormalLogCode.ChangeLanguage, "ChangeLanguage : " + currentLang, GetType().ToString());

        //label.SelectLanguageButton();

        langnavi_t = 0;
        langNaviOn = true;
        movelangNavi = true;
        MenuBar.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(false);

        if (Tip_Obj.activeSelf)
        {
            TipOpen();
        }
    }

    /// <summary>
    /// �Ŵ������� ���� ���� �ΰ� 10�� ��ġ�ϸ� Ű���� Ȱ��ȭ
    /// </summary>
    public void SetManagerPage()
    {
        System.Diagnostics.Process ps = new System.Diagnostics.Process();
        ps.StartInfo.FileName = "osk.exe";
        password = "";
        MenuBar.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        ps.Start();
    }

    float count;
    bool entermode = false;
    bool startcount = false;

    /// <summary>
    /// ���� ��尡 AR��� �� ��쿡�� �Ŵ������ ����
    /// </summary>
    public void CountEnterManager()
    {
        if (SceneManager.GetActiveScene().name == "XRMode")
        {
            startcount = true;
            entermode = false;
        }
    }

    /// <summary>
    /// �Ŵ������ ����
    /// </summary>
    public void CountFinishManager()
    {
        count = 0;
        startcount = false;
    }

    /// <summary>
    /// �Ŵ�����忡 �� ���, �ȵ� ��쿡 ���� ��ũ��Ʈ Ȱ��ȭ/��Ȱ��ȭ
    /// </summary>
    public void SettingLabel()
    {
        if (SceneManager.GetActiveScene().name.Contains("ARMode") && SettingLabelPosition == false)
        {
            count = 0;
            entermode = false;
        }
        SettingLabelPosition = true;
        if (SettingLabelPosition == true)       // �Ŵ������� �� ���
        {
            xrMode.enabled = false;
            xrMode_manager.enabled = true;
            xrMode_manager.ManagerMode.gameObject.SetActive(true);
        }
        else if (SettingLabelPosition == false)     // �Ŵ�����尡 �ƴ� ���
        {
            gamemanager.xrMode.enabled = true;
            xrMode_manager.ManagerMode.gameObject.SetActive(false);
            gamemanager.xrMode_manager.enabled = false;
        }
        //SceneManager.LoadScene("ARMode_" + ContentsName);
    }

    public void CallWaitingMode()
    {
        Loading.nextScene = "WaitingMode";
        if (alreadywaitingLog == false)     // �α׸� �ѹ��� ������ ���ؼ� & ī�޶� ���� ����
        {
            gamemanager.WriteLog(LogSendServer.NormalLogCode.ChangeMode, "ChangeMode : Start(" + GameManager.PrevMode + " - " + "WaitingMode)", GetType().ToString());
            alreadywaitingLog = true;
        }

        if (SceneManager.GetActiveScene().name == "NamSanHMode")
        {
            namsanMode.Narration.Stop();
        }
        changewaiting.SetPantiltOrigin();
        //Debug.Log("today CallWaitingMode");
        //CameraSpincam.EndThread = true;
        SceneManager.LoadScene("WaitingMode");
    }

    public void TipOpen()
    {
        Tipbtn.transform.GetChild(0).gameObject.SetActive(true);

        if (!Tip_Obj.activeSelf)
        {
            Tip_Obj.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name != "NamSanHMode")
        {
            if (currentLang == Language_enum.Korea)
            {
                Tip_Obj.GetComponent<Image>().sprite = Tip_XR_K;
            }
            else if (currentLang == Language_enum.English)
            {
                Tip_Obj.GetComponent<Image>().sprite = Tip_XR_E;
            }
        }
        else if (SceneManager.GetActiveScene().name == "NamSanHMode")
        {
            if (currentLang == Language_enum.Korea)
            {
                Tip_Obj.GetComponent<Image>().sprite = Tip_Nam_K;
            }
            else if (currentLang == Language_enum.English)
            {
                Tip_Obj.GetComponent<Image>().sprite = Tip_Nam_E;
            }
        }

        NavigationBar.gameObject.SetActive(false);
        Arrow.gameObject.SetActive(false);
        MiniMap_Background.transform.parent.gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name == "XRMode")
        {
            //xrMode.AllMapLabels.gameObject.SetActive(false);
        } else if(SceneManager.GetActiveScene().name == "NamSanH")
        {

        }
    }

    public void TipClose()
    {
        Tipbtn.transform.GetChild(0).gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().name == "XRMode")
        {
            NavigationBar.gameObject.SetActive(false);
            MiniMap_Background.transform.parent.gameObject.SetActive(true);
            Arrow.SetActive(true);
            Arrow.transform.position = Arrowpos_extend;
            if (WantNoLabel == false)
            {
                xrMode.AllMapLabels.gameObject.SetActive(true);
            }
            else if (WantNoLabel == true)
            {
                xrMode.AllMapLabels.gameObject.SetActive(false);
            }
        }
        else if (SceneManager.GetActiveScene().name == "NamSanHMode")
        {
            NavigationBar.gameObject.SetActive(true);
            Arrow.SetActive(false);
            MiniMap_Background.transform.parent.gameObject.SetActive(false);
            //namsanMode.AllMapLabels.gameObject.SetActive(true);
        }

        Tip_Obj.SetActive(false);       // Tip ��Ȱ��ȭ
    }

    public void ResetPosition()
    {
        gamemanager.WriteLog(LogSendServer.NormalLogCode.ClickHomeBtn, "HomeButton Click", GetType().ToString());

        if (SceneManager.GetActiveScene().name == "XRMode")
        {
            StartCoroutine(Home_Btn_XR());
        } else if(SceneManager.GetActiveScene().name == "NamSanHMode")
        {
            Home_Btn_NamSanH();
        }
    }

    IEnumerator Home_Btn_XR()
    {
        PanTiltControl.Stop();
        yield return new WaitForSeconds(0.1f);

        PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Fast);
        gamemanager.speed_enum = GameManager.Speed_enum.fast;
        PanTiltControl.SetPulse((uint)startlabel_x, (uint)startlabel_y);

        TipOpen();

        PinchZoomInOut.ZoomMove = true;
        PinchZoomInOut.ZoomIN = false;
        MiniMap_CameraGuide.gameObject.SetActive(false);

        navi_t = 0;
        NaviOn = true;
        moveNavi = true;
        langnavi_t = 0;
        langNaviOn = true;
        movelangNavi = true;

        WriteLogOnce = false;
    }

    public void Home_Btn_NamSanH()
    {
        navi_t = 0;
        NaviOn = true;
        moveNavi = false;
        langnavi_t = 0;
        langNaviOn = true;
        movelangNavi = true;

        if (namsanMode.obj360.activeSelf)
        {
            namsanMode.Close360();
        }

        if (namsanMode.timeflow.TFBackground.activeSelf)
        {
            namsanMode.timeflow.CloseTF();
        }
    }
}
