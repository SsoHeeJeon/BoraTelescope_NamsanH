using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamSanHMode : MonoBehaviour
{
    private GameManager gamemanager;
    public GameObject CameraWindow;

    public GameObject AllUI;
    public GameObject Docent_AllUI;

    public GameObject SelectLabel;

    public See360 see360;
    public GameObject obj360;
    public Material mat360;

    public AudioSource Narration;
    public Sprite[] House = new Sprite[10];

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gamemanager.UISetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NaviLabel(GameObject Label)
    {
        if (gamemanager.Label_Cate_1.Contains(Label.name))      // �ô뺰��õ
        {
            TimeFlow(Label);
        } else if (gamemanager.Label_Cate_2.Contains(Label.name))       // ����Ʈ
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

    public void TimeFlow(GameObject Time)
    {
        int num = int.Parse(Time.name.Substring(4));

        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            Narration.clip = gamemanager.Narration_Flow_K[num - 1];
        }
        else if (GameManager.currentLang == GameManager.Language_enum.English)
        {
            Narration.clip = gamemanager.Narration_Flow_E[num - 1];
        }
        Narration.Play();
    }

    public void ReadyTo360(GameObject type)
    {
        SelectLabel = type;
        obj360.gameObject.SetActive(true);
        see360.enabled = true;
        
        int num = int.Parse(type.name.Substring(5));
        mat360.SetTexture("_MainTex", House[num - 1].texture);

        Docent_AllUI.SetActive(true);

        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            Narration.clip = gamemanager.Narration_Docent_K[num - 1];
        } else if(GameManager.currentLang == GameManager.Language_enum.English)
        {
            Narration.clip = gamemanager.Narration_Docent_E[num - 1];
        }
        Narration.Play();

        if (gamemanager.NaviRect.sizeDelta.x > GameManager.barClose)
        {
            gamemanager.navi_t = 0;
            gamemanager.NaviOn = true;
            gamemanager.moveNavi = true;
        }

        AllUI.SetActive(false);

        // �󼼼��� ������
        // �ƹ�Ÿ ����
    }

    public void Close360()
    {
        see360.enabled = false;
        obj360.SetActive(false);

        Narration.Stop();

        if (gamemanager.NaviRect.sizeDelta.x < GameManager.barOpen)
        {
            gamemanager.navi_t = 0;
            gamemanager.moveNavi = true;
            gamemanager.NaviOn = false;
        }

        Docent_AllUI.SetActive(false);
        AllUI.SetActive(true);
        // �󼼼��� ����
        // �ƹ�Ÿ ��������
    }

    // ����Ʈ���� �������� ���������ϸ� ����� �Լ��Դϴ�.
    // ����Ʈ���� ������ ������ SelectLabel�� �ҷ����ø� �˴ϴ�.
    // Close360������ �������� ������ �������� Ȯ���ϰ� �Ȳ������� �� �� �ְ� ���ּ���.
    public void Wisdom()
    {
        Narration.Stop();
    }
}
