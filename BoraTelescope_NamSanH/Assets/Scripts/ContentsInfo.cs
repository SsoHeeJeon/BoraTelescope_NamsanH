using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// 망원경 정보 불러오기
// Contents 정보 불러오기
// 망원경 정보와 Contents 연결하여 Setting
// Json파일 불러오기



public class ContentsInfo : LogSendServer
{
    public static string ContentsName;
    public GameObject gameemanager;
    //private Label label;

    List<string> Label_Cate_1;
    List<string> Label_Cate_2;
    List<string> Label_Cate_3;

    public Sprite[] NaviLabel;
    public Sprite[] NaviLabel_K;
    public Sprite[] NaviLabel_E;
    public Sprite[] DetailImage_K;
    public Sprite[] DetailImage_E;
    public Sprite[] DetailText_K;
    public Sprite[] DetailText_E;

    public AudioClip[] Narration_K;
    public AudioClip[] Narration_E;

    public VideoClip[] WaitingVideo;
    public string[] WaitingVideo_path;

    public Sprite[] cateImage = new Sprite[12];
    public Sprite[] SelectcateImage = new Sprite[12];

    public static bool[] ModeActive;
    public static bool AwakeOnce = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (AwakeOnce == false)     // 한번만 진행
        {
            // 콘텐츠 버전 선택
            ContentsName = "NamsanH";

            //선택된 콘텐츠 버전에 따라 해당 스크립트에서 불러온 이미지 및 리소스 파일 불러오기
            LoadLabelInfo();
            WriteLog(NormalLogCode.StartContents, "StartContents", GetType().ToString());       // 콘텐츠 시작 로그 생성

            Connect_Button();       // 시스템 컨트롤러 프로그램에 접속하여 모드상태플래그 불러오기
            WriteLog(NormalLogCode.Connect_SystemControl, "Connect_SystemControl_On", GetType().ToString());        // 불러온 모드상태 플래그 로그로 표현
            gameemanager.GetComponent<GameManager>().UISetting();       // UI 셋팅
            LabelData();        // 라벨 정보 불러오기
            AwakeOnce = true;
        }
        
        //Sprite[] sprites = Resources.LoadAll<Sprite>("Sprite/bora_ver2_22_메뉴바");
        //SpriteTest.GetComponent<Image>().sprite = sprites[5];
    }

    private void Start()
    {
        //label = gameemanager.GetComponent<Label>();
        //Debug.Log(label);
        //LabelData();
    }

    /// <summary>
    /// 받아온 콘텐츠 정보에 따라 해당 스크립트를 이용해서 각종 리소스 불러오기
    /// </summary>
    public void LoadLabelInfo()
    {
        Label_Cate_1 = new List<string> {"JoR", "Saeholligi", "Crane", "Pep", "Weasel", "Gary", "Solnari", "Goose", "Otter", "Duksu",
                                         "Goshawk", "Dokminari", "Songak", "Yuni", "GunJang", "Eagle", "Wcrane", "ImjinR", "SacheonR", "Joleum"};
        Label_Cate_2 = new List<string> {"Aegibong", "TurtleShip", "Ganghaw", "Seoglyupo", "HanR", "NorthGP", "Gwansan", "Thresher", "Historic", "Center",
                                         "School", "Imhan", "House", "Manwol", "Broadcast", "Gijeong", "Panmoon", "Daeseoung", "Dora"};
        Label_Cate_3 = new List<string> {"Aegibong", "TurtleShip", "Ganghaw", "Seoglyupo", "HanR", "NorthGP", "Gwansan", "Thresher", "Historic", "Center",
                                         "School", "Imhan", "House", "Manwol", "Broadcast", "Gijeong", "Panmoon", "Daeseoung", "Dora"};

        NaviLabel_K = new Sprite[Resources.LoadAll<Sprite>("Odu/Sprite/NavigationLabel_K").Length];
        NaviLabel_E = new Sprite[Resources.LoadAll<Sprite>("Odu/Sprite/NavigationLabel_E").Length];
        DetailImage_K = new Sprite[Resources.LoadAll<Sprite>("Odu/Sprite/DetailImage_K").Length];
        Narration_K = new AudioClip[Resources.LoadAll<AudioClip>("Odu/Narration/Korea").Length];
        Narration_E = new AudioClip[Resources.LoadAll<AudioClip>("Odu/Narration/English").Length];

        NaviLabel_K = Resources.LoadAll<Sprite>("Odu/Sprite/NavigationLabel_K");
        NaviLabel_E = Resources.LoadAll<Sprite>("Odu/Sprite/NavigationLabel_E");
        DetailImage_K = Resources.LoadAll<Sprite>("Odu/Sprite/DetailImage_K");
        Narration_K = Resources.LoadAll<AudioClip>("Odu/Narration/Korea");
        Narration_E = Resources.LoadAll<AudioClip>("Odu/Narration/English");

        WaitingVideo_path = Directory.GetFiles(Application.dataPath + "/Resources/Video", "*.mp4");

        ModeActive = new bool[3];

        for (int index = 0; index < ModeActive.Length; index++)
        {
            ModeActive[index] = true;
        }
        WriteLog(LogSendServer.NormalLogCode.Load_ResourceFile, "Load_ResourceFile", GetType().ToString());
    }

    /// <summary>
    /// 불러온 리소스를 라벨 스크립트에 적용
    /// </summary>
    public void LabelData()
    {/*
        label = gameemanager.GetComponent<GameManager>().label;

        for (int index = 0; index < SetLang.transform.childCount; index++)
        {
            SetLang.transform.GetChild(index).gameObject.SetActive(true);
        }

        label.Label_Cate_1 = Label_Cate_1;
        label.Label_Cate_2 = Label_Cate_2;
        label.Label_Cate_3 = Label_Cate_3;

        gameemanager.GetComponent<ReadJsonFile>().Readfile();

        label.NaviLabel_K = NaviLabel_K;
        label.NaviLabel_E = NaviLabel_E;
        label.NaviLabel_C = NaviLabel_C;
        label.NaviLabel_J = NaviLabel_J;
        label.DetailImage = DetailImage_K;
        label.DetailImage_E = DetailImage_E;
        label.DetailImage_C = DetailImage_C;
        label.DetailImage_J = DetailImage_J;

        label.Label_Narr_K = Narration_K;
        label.Label_Narr_E = Narration_E;
        label.Label_Narr_C = Narration_C;
        label.Label_Narr_J = Narration_J;

        GameManager gm = gameemanager.GetComponent<GameManager>();

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            for (int index = 0; index < ModeActive.Length; index++)
            {
                ModeActive[index] = true;
            }
        }

        label.DetailTexts_K = ReadJsonFile.DetailText_K.ToArray();
        label.DetailTexts_E = ReadJsonFile.DetailText_E.ToArray();

        label.SelectCategoryLabel_dora("Total");*/
    }
}
