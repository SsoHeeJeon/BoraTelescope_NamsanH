using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamSanHMode : MonoBehaviour
{
    public GameManager gamemanager;
    public GameObject CameraWindow;
    public LabelDetail labeldetail;
    public Docent_Avatar avatar;
    public TimeFlow timeflow;

    public GameObject Docent_avartar;
    public GameObject AllUI;
    public GameObject Docent_AllUI;
    public GameObject Docent_Detail;

    public GameObject SelectLabel;

    public See360 see360;
    public GameObject obj360;
    public Material mat360;
    public Sprite Narr_Off;
    public Sprite Narr_On;

    public AudioSource Narration;
    public Sprite[] House = new Sprite[10];

    public bool PlayNarr = false;

    [SerializeField]
    scroll scroll;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gamemanager.UISetting();
        if (timeflow.TFBackground.activeSelf)
        {
            timeflow.TFBackground.SetActive(false);
        }
        Close360();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NaviLabel(GameObject Label)
    {
        if (gamemanager.Label_Cate_1.Contains(Label.name))      // 시대별변천
        {
            timeflow.ReadytoStart(Label);
        } else if (gamemanager.Label_Cate_2.Contains(Label.name))       // 도슨트
        {
            if (obj360.activeSelf)
            {
                Close360();
            } else if (!obj360.activeSelf)
            {
                ReadyTo360(Label);
            }
        }
    }

    public void ReadyTo360(GameObject type)
    {
        scroll.GetComponent<scroll>().ScrollHome();
        SelectLabel = type;
        obj360.gameObject.SetActive(true);
        see360.enabled = true;

        Docent_Start();

        int num = int.Parse(type.name.Substring(5));
        mat360.SetTexture("_MainTex", House[num - 1].texture);

        Docent_AllUI.SetActive(true);
        print(1);
        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            print(2);
            labeldetail.InfoHeight.GetComponent<UIText>().text = ReadJsonFile.DetailText_K[num + 4];
            Narration.clip = gamemanager.Narration_Docent_K[num - 1];
            Docent_avartar.transform.GetChild(0).GetComponent<DocentAni>().NarrtionLen = Narration.clip.length;
        } else if(GameManager.currentLang == GameManager.Language_enum.English)
        {
            labeldetail.InfoHeight.GetComponent<UIText>().text = ReadJsonFile.DetailText_E[num + 4];
            Narration.clip = gamemanager.Narration_Docent_E[num - 1];
            Docent_avartar.transform.GetChild(0).GetComponent<DocentAni>().NarrtionLen = Narration.clip.length;
        }
        //Narration.Play();
        Narration.Pause();

        if (gamemanager.NaviRect.sizeDelta.x > GameManager.barClose)
        {
            gamemanager.navi_t = 0;
            gamemanager.NaviOn = true;
            gamemanager.moveNavi = true;
        }

        AllUI.SetActive(false);

        labeldetail.ChangeDetailLanguage();
        Docent_avartar.transform.GetChild(0).GetComponent<DocentAni>().AniHi();
        // 아바타 생성
    }

    public void Close360()
    {
        SelectLabel = null;

        see360.enabled = false;
        obj360.SetActive(false);

        Narration.Stop();
        if(scroll.ad!=null)
        {
            scroll.ad.Stop();
        }
        if (gamemanager.NaviRect.sizeDelta.x < GameManager.barOpen)
        {
            gamemanager.navi_t = 0;
            gamemanager.moveNavi = true;
            gamemanager.NaviOn = false;
        }

        //Docent_AllUI.SetActive(false);
        AllUI.SetActive(true);
        scroll.CancelInvoke();
        Docent_Finish();        // 상세설명, 아바타 없어지기
    }

    public void Docent_OnOff()
    {
        if (!Docent_avartar.activeSelf)
        {
            Docent_Start();
        }
        else if (Docent_avartar.activeSelf)
        {
            Docent_Finish();
        }
    }

    public void Docent_Start()
    {
        Docent_Detail.SetActive(true);
        labeldetail.enabled = true;

        Docent_avartar.SetActive(true);
        avatar.enabled = true;

        labeldetail.DetailOpen();
    }

    public void Docent_Finish()     // 상세설명 없어지기
    {
        labeldetail.CloseDetailWindow();

        avatar.enabled = false;
        Docent_avartar.SetActive(false);

        //labeldetail.enabled = false;
        Docent_Detail.SetActive(false);

        Narration.Stop();
    }

    // 도슨트에서 선조들의 지혜선택하면 실행될 함수입니다.
    // 도슨트에서 선택한 가옥은 SelectLabel로 불러오시면 됩니다.
    // Close360에서도 선조들의 지혜가 꺼졌는지 확인하고 안꺼졌으면 끌 수 있게 해주세요.
    public void Wisdom()
    {
        string name = SelectLabel.name;
        name = name.Replace("House", "");
        float num = int.Parse(name);
        switch(num)
        {
            case 1:
                scroll.ContentName = "메인광장";
                break;
            case 2:
                scroll.ContentName = "천우각";
                break;
            case 3:
                scroll.ContentName = "청학지";
                break;
            case 4:
                scroll.ContentName = "남산국악당";
                break;
            case 5:
                scroll.ContentName = "이승업가옥";
                break;
            case 6:
                scroll.ContentName = "윤씨가옥";
                break;
            case 7:
                scroll.ContentName = "김춘영가옥";
                break;
            case 8:
                scroll.ContentName = "민씨가옥";
                break;
            case 9:
                scroll.ContentName = "윤택영재실";
                break;
            case 10:
                scroll.ContentName = "전망대";
                break;
        }

        scroll.TitleText.transform.parent.parent.gameObject.SetActive(true);
        scroll.Intelligence();

        Narration.Stop();

    }
}
