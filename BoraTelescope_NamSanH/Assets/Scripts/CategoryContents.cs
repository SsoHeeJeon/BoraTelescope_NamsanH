using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryContents : MonoBehaviour
{
    private GameManager gamemanager;

    public Image Flow_btn;
    public Image Flow_btn_Sel;
    public Image Docent_btn;
    public Image Docent_btn_Sel;
    public Scrollbar naviscroll;

    public Sprite Flow_K;
    public Sprite Flow_K_sel;
    public Sprite Flow_E;
    public Sprite Flow_E_sel;
    public Sprite Docent_K;
    public Sprite Docent_K_sel;
    public Sprite Docent_E;
    public Sprite Docent_E_sel;

    float Flow_val = 1;
    float Docent_val = 0.02f;

    bool clickbtn = false;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            ChangeCategory_lang("Korea");
        } else if(GameManager.currentLang == GameManager.Language_enum.English)
        {
            ChangeCategory_lang("English");
        }
        SelectCategory_Flow();
    }

    // Update is called once per frame
    void Update()
    {
        // 네비게이션 창 라벨선택부 스크롤바 사이즈
        naviscroll.value = Mathf.Clamp(naviscroll.value, 0, 1);
        naviscroll.size = 0.0f;

        if (clickbtn == false)
        {
            if (Mathf.Abs(naviscroll.value - Docent_val) > 0.1f)
            {
                Flow_btn_Sel.gameObject.SetActive(true);
                Docent_btn_Sel.gameObject.SetActive(false);
            }
            else if (Mathf.Abs(naviscroll.value - Docent_val) <= 0.1f)
            {
                Flow_btn_Sel.gameObject.SetActive(false);
                Docent_btn_Sel.gameObject.SetActive(true);
            }
        }
    }

    public void SelectCategory_Flow()
    {
        clickbtn = true;
        naviscroll.value = Flow_val;
        Flow_btn_Sel.gameObject.SetActive(true);
        Docent_btn_Sel.gameObject.SetActive(false);
        Invoke("Returnbool", 0.2f);
    }
    
    public void SelectCategory_Docent()
    {
        clickbtn = true;
        naviscroll.value = Docent_val;
        Flow_btn_Sel.gameObject.SetActive(false);
        Docent_btn_Sel.gameObject.SetActive(true);
        Invoke("Returnbool", 0.2f);
    }

    public void Returnbool()
    {
        clickbtn = false;
    }

    public void ChangeCategory_lang(string lang)
    {
        if(lang == "Korea")
        {
            Flow_btn.sprite = Flow_K;
            Flow_btn_Sel.sprite = Flow_K_sel;
            Docent_btn.sprite = Docent_K;
            Docent_btn_Sel.sprite = Docent_K_sel;
        } else if(lang == "English")
        {
            Flow_btn.sprite = Flow_E;
            Flow_btn_Sel.sprite = Flow_E_sel;
            Docent_btn.sprite = Docent_E;
            Docent_btn_Sel.sprite = Docent_E_sel;
        }

        Flow_btn.SetNativeSize();
        Flow_btn_Sel.SetNativeSize();
        Docent_btn.SetNativeSize();
        Docent_btn_Sel.SetNativeSize();

        // 카테고리별 위치 수정
        float firstcate_x = -177;
        float secondcate_x = firstcate_x + Flow_btn.gameObject.GetComponent<RectTransform>().rect.width + 10;
        //float total_Width = Flow_btn.gameObject.GetComponent<RectTransform>().rect.width + Docent_btn.gameObject.GetComponent<RectTransform>().rect.width;

        Flow_btn.gameObject.transform.localPosition = new Vector3(firstcate_x, Flow_btn.gameObject.transform.localPosition.y, 0);
        Docent_btn.gameObject.transform.localPosition = new Vector3(secondcate_x, Flow_btn.gameObject.transform.localPosition.y, 0);
        //CategoryContent.GetComponent<RectTransform>().sizeDelta = new Vector2(total_Width, CategoryContent.GetComponent<RectTransform>().sizeDelta.y);  // 카테고리 스크롤뷰 크기 수정
    }
}
