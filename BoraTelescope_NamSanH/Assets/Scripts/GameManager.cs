using PanTiltControl_v2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// AR모드 라벨명 및 라벨 위치 매칭
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
/// 팬틸트정보
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

    //스크립트
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

    //공통 UI
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

    // 오디오
    public AudioSource ButtonEffect;
    public AudioClip ButtonSound;

    // 네비게이션창, 언어선택창 On/Off
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
    /// 변수
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

        // 시간 초기화
        touchCount = 0;
        touchfinish = false;
        navi_t = 0;
        langnavi_t = 0;

        count_set = 0;
        NaviRect = NavigationBar.GetComponent<RectTransform>();
        LangRect = LanguageBar.GetComponent<RectTransform>();
        LangChildImg = LanguageBar.transform.GetChild(0).gameObject.GetComponent<Image>();
        // 언어선택창 닫아놓기(로딩화면에서 안보임.)
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
        // 네비게이션 창, 상세설명창 속도 조절 
        navi_t += Time.deltaTime * 0.1f;
        langnavi_t += Time.deltaTime * 0.1f;
        if (ResetPositionTime < 99)
        {
            ResetPositionTime += Time.deltaTime;
        }

        // 터치 안하는 시간을 측정하여 대기모드로 전환하기 위함
        if (Input.GetMouseButtonDown(0))        // 마우스 클릭시
        {
            touchCount = 0;
            touchfinish = true;
            ResetPositionTime = 0;
        }
        else if (Input.GetMouseButtonUp(0))     // 마우스 버튼에서 떼면
        {
            touchfinish = false;
        }

        // 터치를 안하고 있고 현재 씬이 대기모드가 아니라면
        // 터치 안한지 5분(데모기준) 이 되는 시간에 대기모드로 전환
        if (touchfinish == false && SceneManager.GetActiveScene().name != "WaitingMode")
        {
            touchCount += Time.deltaTime;
            if ((int)touchCount >= waitingTime)
            {
                Readpulse = false;
                CallWaitingMode();
            }
        }

        // 관리자모드로 들어가기
        // 10초이상 로고를 터치하고 있으면 가상키보드 활성화
        // 비밀번호 맞으면 관리자모드로 변경
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
    /// 콘텐츠 종료할때
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
                // 각 스크립트 연결
                xrMode = GameObject.Find("XRMode").GetComponent<XRMode>();
                xrMode_manager = GameObject.Find("XRMode").GetComponent<XRMode_Manager>();

                //labeldetail = GameObject.Find("ARMode").GetComponent<LabelDetail>();
                //label = GM.gameObject.GetComponent<Label>();

                // XR모드(WantNoLabel == false) 기본값 : 미니맵, 메뉴바, 네비게이션창, Tip, Capture버튼 활성화/ 화살표, 언어선택창 비활성화
                // Live모드(WantNoLabel == true) 기본값 : 미니맵, 메뉴바, 화살표, Tip 활성화/ 화살표, 네비게이션창, 언어선택창 비활성화
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
    /// 모든 UI 활성화
    /// </summary>
    public void SeeNavibar()
    {
        NavigationBar.gameObject.SetActive(true);
    }

    /// <summary>
    /// 메뉴바에 있는 모드 아이콘 선택했을 경우
    /// </summary>
    /// <param name="btn"></param>
    public void Menu(GameObject btn)
    {
        switch (btn.name)
        {
            case "LiveMode":
                if (SceneManager.GetActiveScene().name == "XRMode")
                {
                    // 메뉴바의 모드아이콘에서 Live모드 비활성화, AR모드 활성화
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
                    // 네비게이션 창 비활성화(역사모드에서는 네비게이션창 사용하지 않음)
                    if (NaviRect.sizeDelta.x > barClose)
                    {
                        navi_t = 0;
                        NaviOn = true;
                        moveNavi = true;
                    }

                    // 언어선택창 비활성화
                    if (LangRect.sizeDelta.x > barClose)
                    {
                        langnavi_t = 0;
                        langNaviOn = true;
                        movelangNavi = true;
                    }

                    // 나레이션 음성 멈춤
                    namsanMode.Narration.Stop();

                    // 메뉴바의 모드아이콘에서 Live모드 비활성화, AR모드 활성화
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
                    if (!Tip_Obj.activeSelf)        // Tip이 비활성화상태라면
                    {
                        if (ModeActive[1] == true)      // AR모드가 활성화되어있다면
                        {
                            // 언어선택창 비활성화
                            if (LangRect.sizeDelta.x > barClose)
                            {
                                langnavi_t = 0;
                                langNaviOn = true;
                                movelangNavi = true;
                            }

                            // 메뉴바의 모드아이콘에서 Live모드 비활성화, AR모드 활성화
                            MenuBar.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                            MenuBar.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);

                            // AR모드 라벨이 비활성화되어있다면 라벨 활성화
                            if (!xrMode.AllMapLabels.gameObject.activeSelf)
                            {
                                xrMode.AllMapLabels.gameObject.SetActive(true);
                            }
                            //NaviArr_set();
                            WantNoLabel = false;
                        }
                        else if (ModeActive[1] == false)        //AR모드가 비활성화 되어있다면 에러메세지 활성화하고 현재 모드는 Live 모드로하기
                        {
                            ErrorMessage.gameObject.SetActive(true);
                            if (PrevMode == "LiveMode")
                            {
                                WantNoLabel = true;
                            }
                        }
                    }
                    else if (Tip_Obj.activeSelf)       // Tip 이미지가 활성화 되어있다면 비활성화
                    {
                        Tip_Obj.SetActive(false);
                        TipClose();
                    }
                }
                else if (SceneManager.GetActiveScene().name != "XRMode")
                {
                    // 네비게이션 창 비활성화(역사모드에서는 네비게이션창 사용하지 않음)
                    if (NaviRect.sizeDelta.x > barClose)
                    {
                        navi_t = 0;
                        NaviOn = true;
                        moveNavi = true;
                    }

                    // 언어선택창 비활성화
                    if (LangRect.sizeDelta.x > barClose)
                    {
                        langnavi_t = 0;
                        langNaviOn = true;
                        movelangNavi = true;
                    }

                    // 나레이션 음성 멈춤
                    namsanMode.Narration.Stop();

                    // 메뉴바의 모드아이콘에서 Live모드 비활성화, AR모드 활성화
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
                    // 네비게이션창 활성화
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

                    // 언어선택 창 비활성화
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

                    // 메뉴바의 모드아이콘에서 Live모드 비활성화, AR모드 활성화
                    for (int index = 0; index < MenuBar.transform.GetChild(0).transform.childCount; index++)
                    {
                        MenuBar.transform.GetChild(0).gameObject.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    MenuBar.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true);

                    // 네비게이션창 활성화
                    if (NaviRect.sizeDelta.x < barOpen)
                    {
                        navi_t = 0;
                        moveNavi = true;
                        NaviOn = false;
                    }

                    // 언어선택 창 비활성화
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
                if (LangRect.sizeDelta.x > barClose)        // 언어선택 비활성화
                {
                    MenuBar.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    //langnavi_t = 0;
                    langNaviOn = true;
                    movelangNavi = true;
                }
                else if (LangRect.sizeDelta.x < barOpen)      // 언어선택 활성화
                {
                    MenuBar.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    //langnavi_t = 0;
                    movelangNavi = true;
                    langNaviOn = false;
                }
                break;
            case "Tip":
                if (!Tip_Obj.activeSelf)        // Tip 이미지가 비활성화상태면 활성화
                {
                    Tipbtn.transform.GetChild(0).gameObject.SetActive(true);
                    Tip_Obj.SetActive(true);

                    TipOpen();
                }
                else if (Tip_Obj.activeSelf)      // Tip 이미지가 활성화 상태면 비활성화
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
            case "Navi_Close":      // 네비게이션 창에 x 버튼 선택하면 네비게이션창 비활성화
                navi_t = 0;
                moveNavi = true;
                break;
            case "LangNavi_Close":      // 언어선택 창에 x 버튼 선택하면 언어선택 비활성화
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
    /// 네비게이션 창 활성화/비활성화
    /// </summary>
    public void NaviArr_set()
    {
        Image NaviBackground = NavigationBar.transform.GetChild(0).gameObject.GetComponent<Image>();

        // 네비게이션 비활성화
        if (NaviOn == true)
        {
            // 기존 기획 : 메뉴바가 길어져서 네비게이션 창이 생성
            // 현재 기획 : 네비게이션 창이 따로 있어서 크기가 커짐

            // 네비게이션창 로그
            if (alreadynaviLog == false)
            {
                WriteLog(NormalLogCode.NamSanH_Navigation, "NamSanH_NavigationOff", GetType().ToString());
                alreadynaviLog = true;
            }

            Arrow.gameObject.SetActive(false);
            NavigationBar.gameObject.SetActive(true);   // 네비게이션 창 활성화

            // 네비게이션 창이 비활성화되어 있기 때문에 네비게이션 라벨 버튼 비활성화
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
        else if (NaviOn == false)       // 네비게이션 활성화
        {
            // 네비게이션창 활성화 로그
            if (alreadynaviLog == false)
            {
                WriteLog(NormalLogCode.NamSanH_Navigation, "NamSanH_NavigationOn", GetType().ToString());
                alreadynaviLog = true;
            }

            NavigationBar.transform.GetChild(0).gameObject.SetActive(true);     // 네비게이션창 활성화
            Arrow.gameObject.SetActive(false);      // 네비게이션창이 활성화 되어있기 때문에 화살표는 비활성화

            // 네비게이션 창 천천히 활성화(열리는 효과)
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

                // 네비게이션 창이 모두 펴지면 네비게이션 라벨 버튼 활성화
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
    /// 네비게이션창에서 라벨 선택하면 모드에 따라 선택한 라벨 적용
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
    /// 화살표버튼 선택했을 경우 그  UI를 받아와서 로그 남기기 (한번만)
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
    /// 화살표 버튼 선택을 종료했을 경우(화살표를 선택하여 움직임을 멈춤) 로그 남기기(한번만)
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
    /// 버튼 클릭하면 효과음 재생
    /// </summary>
    public void ButtonClickSound()
    {
        ButtonEffect.clip = ButtonSound;
        ButtonEffect.Play();
    }

    /// <summary>
    /// 미니맵에 터치 시작
    /// </summary>
    public void Minimap_TouchOn()
    {
        PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Fast);
        gamemanager.speed_enum = GameManager.Speed_enum.fast;
        StartMiniMapDrag = true;
    }

    /// <summary>
    /// 미니맵에 터치 완료
    /// </summary>
    public void Minimap_TouchOff()
    {
        StartMiniMapDrag = false;
    }

    /// <summary>
    /// 메뉴바에서 언어선택 아이콘 선택하면 언어선택창 활성화/비활성화
    /// </summary>
    public void SelectLanguageChange()
    {
        if (langNaviOn == true)     // 언어선택 창 비활성화
        {
            if (SceneManager.GetActiveScene().name != "NamSanHMode")        // 역사모드가 아니면 네비게이션 창의 상태에 따라(활성화/비활성화) 화살표UI 활성화 비활성화
            {
                if (NaviRect.sizeDelta.x < barOpen)     // 네비게이션 창이 비활성화 상태이므로 화살표 UI 활성화
                {
                    Arrow.gameObject.SetActive(true);
                    Arrow.transform.position = Arrowpos_extend;
                }
                else if (NaviRect.sizeDelta.x > barClose)     // 네비게이션 창이 활성화상태이므로 화살표 UI 비활성화
                {
                    Arrow.gameObject.SetActive(false);
                }
            }
            else if (SceneManager.GetActiveScene().name == "NamSanHMode")      // 역사모드에서는 네비게이션 창에 상관없이 화살표 UI 비활성화
            {
                Arrow.gameObject.SetActive(false);
            }

            // 언어선택 창 비활성화 진행
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
        else if (langNaviOn == false)       // 언어선택 활성화
        {
            LanguageBar.transform.GetChild(0).gameObject.SetActive(true);       // 언어선택 창 활성화
            Arrow.gameObject.SetActive(false);      // 언어선택창이 활성화 되어있기 때문에 화살표 UI 비활성화
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
        if (ModeActive[0] == true)          // Live모드가 활성화 상태라면
        {
            // 네비게이션 바 없애기
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
            if (LangRect.sizeDelta.x > barClose)        // 언어선택창 없애기
            {
                langnavi_t = 0;
                langNaviOn = true;
                movelangNavi = true;
            }
            //label.Narration.Stop();         // 상세설명 나레이션 끄기

            // 로그 보내고 현재모드(prevMode)를 Live모드로 변경해놓기
            WriteLog(NormalLogCode.ChangeMode, "ChangeMode : Start(" + PrevMode + " - " + "LiveMode)", GetType().ToString());
            WriteLog(NormalLogCode.ChangeMode, "ChangeMode : Finish(" + PrevMode + " - " + "LiveMode)", GetType().ToString());
            PrevMode = "LiveMode";
            // 모드아이콘 중 AR모드 아이콘 비활성화하고 Live모드 아이콘 활성화
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
    /// 변경할 언어선택
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
    /// 매니저모드로 들어가기 위해 로고 10초 터치하면 키보드 활성화
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
    /// 현재 모드가 AR모드 일 경우에만 매니저모드 실행
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
    /// 매니저모드 종료
    /// </summary>
    public void CountFinishManager()
    {
        count = 0;
        startcount = false;
    }

    /// <summary>
    /// 매니저모드에 들어간 경우, 안들어간 경우에 따라 스크립트 활성화/비활성화
    /// </summary>
    public void SettingLabel()
    {
        if (SceneManager.GetActiveScene().name.Contains("ARMode") && SettingLabelPosition == false)
        {
            count = 0;
            entermode = false;
        }
        SettingLabelPosition = true;
        if (SettingLabelPosition == true)       // 매니저모드로 들어간 경우
        {
            xrMode.enabled = false;
            xrMode_manager.enabled = true;
            xrMode_manager.ManagerMode.gameObject.SetActive(true);
        }
        else if (SettingLabelPosition == false)     // 매니저모드가 아닌 경우
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
        if (alreadywaitingLog == false)     // 로그를 한번만 보내기 위해서 & 카메라 연결 끊기
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

        Tip_Obj.SetActive(false);       // Tip 비활성화
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
