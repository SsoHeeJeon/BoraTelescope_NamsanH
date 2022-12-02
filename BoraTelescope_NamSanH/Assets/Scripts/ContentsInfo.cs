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

    public List<string> Label_Cate_1;
    public List<string> Label_Cate_2;
    public List<string> Label_Cate_3;

    public Sprite[] NaviLabel;
    public Sprite[] NaviLabel_K;
    public Sprite[] NaviLabel_E;
    public Sprite[] DetailImage_K;
    public Sprite[] DetailImage_E;
    public Sprite[] DetailText_K;
    public Sprite[] DetailText_E;

    public Sprite[] Tip;
    public Sprite Tip_XR_K;
    public Sprite Tip_XR_E;
    public Sprite Tip_Nam_K;
    public Sprite Tip_Nam_E;

    public AudioClip[] Narration_Docent_K;
    public AudioClip[] Narration_Wisdom_K;
    public AudioClip[] Narration_Flow_K;

    public AudioClip[] Narration_Docent_E;
    public AudioClip[] Narration_Wisdom_E;
    public AudioClip[] Narration_Flow_E;

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
        Label_Cate_1 = new List<string> {"1991", "1996", "1998", "2003", "2013"};
        Label_Cate_2 = new List<string> {"House1", "House2", "House3", "House4", "House5", "House6", "House7", "House8", "House9", "House10"};
        Label_Cate_3 = new List<string> {"Aegibong", "TurtleShip", "Ganghaw"};

        NaviLabel_K = new Sprite[Resources.LoadAll<Sprite>("Sprite/NavigationLabel_K").Length];
        NaviLabel_E = new Sprite[Resources.LoadAll<Sprite>("Sprite/NavigationLabel_E").Length];
        DetailImage_K = new Sprite[Resources.LoadAll<Sprite>("Sprite/DetailImage_K").Length];
        
        Narration_Docent_K = new AudioClip[Resources.LoadAll<AudioClip>("Narration/Korea/Docent").Length];
        Narration_Wisdom_K = new AudioClip[Resources.LoadAll<AudioClip>("Narration/Korea/Wisdom").Length];
        Narration_Flow_K = new AudioClip[Resources.LoadAll<AudioClip>("Narration/Korea/Flow").Length];
        Narration_Docent_E = new AudioClip[Resources.LoadAll<AudioClip>("Narration/English/Docent").Length];
        Narration_Wisdom_E = new AudioClip[Resources.LoadAll<AudioClip>("Narration/English/Wisdom").Length];
        Narration_Flow_E = new AudioClip[Resources.LoadAll<AudioClip>("Narration/English/FLow").Length];

        NaviLabel_K = Resources.LoadAll<Sprite>("Sprite/NavigationLabel_K");
        NaviLabel_E = Resources.LoadAll<Sprite>("Sprite/NavigationLabel_E");
        DetailImage_K = Resources.LoadAll<Sprite>("Sprite/DetailImage_K");
        Narration_Docent_K = Resources.LoadAll<AudioClip>("Narration/Korea/Docent");
        Narration_Wisdom_K = Resources.LoadAll<AudioClip>("Narration/Korea/Wisdom");
        Narration_Flow_K = Resources.LoadAll<AudioClip>("Narration/Korea/Flow");
        Narration_Docent_E = Resources.LoadAll<AudioClip>("Narration/English/Docent");
        Narration_Wisdom_E = Resources.LoadAll<AudioClip>("Narration/English/Wisdom");
        Narration_Flow_E = Resources.LoadAll<AudioClip>("Narration/English/FLow");

        Tip_XR_K = Resources.Load<Sprite>("Sprite/Tip_XR_K");
        Tip_XR_E = Resources.Load<Sprite>("Sprite/Tip_XR_E");
        Tip_Nam_K = Resources.Load<Sprite>("Sprite/Tip_Nam_K");
        Tip_Nam_E = Resources.Load<Sprite>("Sprite/Tip_Nam_E");

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
    {
        /*
        label = gameemanager.GetComponent<GameManager>().label;

        for (int index = 0; index < SetLang.transform.childCount; index++)
        {
            SetLang.transform.GetChild(index).gameObject.SetActive(true);
        }

        label.Label_Cate_1 = Label_Cate_1;
        label.Label_Cate_2 = Label_Cate_2;
        label.Label_Cate_3 = Label_Cate_3;
        */
        gameemanager.GetComponent<ReadJsonFile>().Readfile();
        /*
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
