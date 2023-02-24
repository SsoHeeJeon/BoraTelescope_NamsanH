using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILanguage : MonoBehaviour
{
    public Image Home_Btn;
    public Image Live_Btn;
    public Image Live_Btn_1;
    public Image XR_Btn;
    public Image XR_Btn_1;
    public Image NamSanH_Btn;
    public Image NamSanH_Btn_1;
    public Image Language_Btn;
    public Image Language_Btn_1;
    public Image Capture_Btn;
    public Image Capture_Btn_1;
    public Image Tip_Btn;
    public Image Tip_Btn_1;

    public Sprite Home_idle_K;
    public Sprite Live_idle_K;
    public Sprite Live_Select_K;
    public Sprite XR_idle_K;
    public Sprite XR_Select_K;
    public Sprite NamSanH_idle_K;
    public Sprite NamSanH_Select_K;
    public Sprite Language_idle_K;
    public Sprite Language_Select_K;
    public Sprite Capture_idle_K;
    public Sprite Capture_Select_K;
    public Sprite Tip_idle_K;
    public Sprite Tip_Select_K;

    public Sprite Home_idle_E;
    public Sprite Live_idle_E;
    public Sprite Live_Select_E;
    public Sprite XR_idle_E;
    public Sprite XR_Select_E;
    public Sprite NamSanH_idle_E;
    public Sprite NamSanH_Select_E;
    public Sprite Language_idle_E;
    public Sprite Language_Select_E;
    public Sprite Capture_idle_E;
    public Sprite Capture_Select_E;
    public Sprite Tip_idle_E;
    public Sprite Tip_Select_E;

    public Sprite Home_idle_C;
    public Sprite Live_idle_C;
    public Sprite Live_Select_C;
    public Sprite XR_idle_C;
    public Sprite XR_Select_C;
    public Sprite NamSanH_idle_C;
    public Sprite NamSanH_Select_C;
    public Sprite Language_idle_C;
    public Sprite Language_Select_C;
    public Sprite Capture_idle_C;
    public Sprite Capture_Select_C;
    public Sprite Tip_idle_C;
    public Sprite Tip_Select_C;

    public Sprite Home_idle_J;
    public Sprite Live_idle_J;
    public Sprite Live_Select_J;
    public Sprite XR_idle_J;
    public Sprite XR_Select_J;
    public Sprite NamSanH_idle_J;
    public Sprite NamSanH_Select_J;
    public Sprite Language_idle_J;
    public Sprite Language_Select_J;
    public Sprite Capture_idle_J;
    public Sprite Capture_Select_J;
    public Sprite Tip_idle_J;
    public Sprite Tip_Select_J;

    private void Start()
    {
        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            SelectKorea();
        }
        else if (GameManager.currentLang == GameManager.Language_enum.English)
        {
            SelectEnglish();
        }
        else if(GameManager.currentLang == GameManager.Language_enum.Chinese)
        {
            SelectChinese();
        }
        else if (GameManager.currentLang == GameManager.Language_enum.Japan)
        {
            SelectJapan();
        }
    }

    public void SelectKorea()
    {
        Home_Btn.sprite = Home_idle_K;
        Live_Btn.sprite = Live_idle_K;
        Live_Btn_1.sprite = Live_Select_K;
        XR_Btn.sprite = XR_idle_K;
        XR_Btn_1.sprite = XR_Select_K;
        NamSanH_Btn.sprite = NamSanH_idle_K;
        NamSanH_Btn_1.sprite = NamSanH_Select_K;
        Language_Btn.sprite = Language_idle_K;
        Language_Btn_1.sprite = Language_Select_K;
        Capture_Btn.sprite = Capture_idle_K;
        Capture_Btn_1.sprite = Capture_Select_K;
        Tip_Btn.sprite = Tip_idle_K;
        Tip_Btn_1.sprite = Tip_Select_K;

        SetSize();
    }

    public void SelectEnglish()
    {
        Home_Btn.sprite = Home_idle_E;
        Live_Btn.sprite = Live_idle_E;
        Live_Btn_1.sprite = Live_Select_E;
        XR_Btn.sprite = XR_idle_E;
        XR_Btn_1.sprite = XR_Select_E;
        NamSanH_Btn.sprite = NamSanH_idle_E;
        NamSanH_Btn_1.sprite = NamSanH_Select_E;
        Language_Btn.sprite = Language_idle_E;
        Language_Btn_1.sprite = Language_Select_E;
        Capture_Btn.sprite = Capture_idle_E;
        Capture_Btn_1.sprite = Capture_Select_E;
        Tip_Btn.sprite = Tip_idle_E;
        Tip_Btn_1.sprite = Tip_Select_E;
        SetSize();
    }

    public void SelectChinese()
    {
        Home_Btn.sprite = Home_idle_C;
        Live_Btn.sprite = Live_idle_C;
        Live_Btn_1.sprite = Live_Select_C;
        XR_Btn.sprite = XR_idle_C;
        XR_Btn_1.sprite = XR_Select_C;
        NamSanH_Btn.sprite = NamSanH_idle_C;
        NamSanH_Btn_1.sprite = NamSanH_Select_C;
        Language_Btn.sprite = Language_idle_C;
        Language_Btn_1.sprite = Language_Select_C;
        Capture_Btn.sprite = Capture_idle_C;
        Capture_Btn_1.sprite = Capture_Select_C;
        Tip_Btn.sprite = Tip_idle_C;
        Tip_Btn_1.sprite = Tip_Select_C;
        SetSize();
    }

    public void SelectJapan()
    {
        Home_Btn.sprite = Home_idle_J;
        Live_Btn.sprite = Live_idle_J;
        Live_Btn_1.sprite = Live_Select_J;
        XR_Btn.sprite = XR_idle_J;
        XR_Btn_1.sprite = XR_Select_J;
        NamSanH_Btn.sprite = NamSanH_idle_J;
        NamSanH_Btn_1.sprite = NamSanH_Select_J;
        Language_Btn.sprite = Language_idle_J;
        Language_Btn_1.sprite = Language_Select_J;
        Capture_Btn.sprite = Capture_idle_J;
        Capture_Btn_1.sprite = Capture_Select_J;
        Tip_Btn.sprite = Tip_idle_J;
        Tip_Btn_1.sprite = Tip_Select_J;
        SetSize();
    }

    public void SetSize()
    {
        Home_Btn.SetNativeSize();
        Live_Btn.SetNativeSize();
        Live_Btn_1.SetNativeSize();
        XR_Btn.SetNativeSize();
        XR_Btn_1.SetNativeSize();
        NamSanH_Btn.SetNativeSize();
        NamSanH_Btn_1.SetNativeSize();
        Language_Btn.SetNativeSize();
        Language_Btn_1.SetNativeSize();
        Capture_Btn.SetNativeSize();
        Capture_Btn_1.SetNativeSize();
        Tip_Btn.SetNativeSize();
        Tip_Btn_1.SetNativeSize();
    }

    public void SetSel(GameObject btn)
    {
        switch (btn.name)
        {
            case "LiveMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Live_Select_K;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    btn.GetComponent<Image>().sprite = Live_Select_E;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
                {
                    btn.GetComponent<Image>().sprite = Live_Select_C;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Japan)
                {
                    btn.GetComponent<Image>().sprite = Live_Select_J;
                }
                break;
            case "XRMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = XR_Select_K;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    btn.GetComponent<Image>().sprite = XR_Select_E;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
                {
                    btn.GetComponent<Image>().sprite = XR_Select_C;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Japan)
                {
                    btn.GetComponent<Image>().sprite = XR_Select_J;
                }
                break;
            case "NamSanHMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = NamSanH_Select_K;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    btn.GetComponent<Image>().sprite = NamSanH_Select_E;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
                {
                    btn.GetComponent<Image>().sprite = NamSanH_Select_C;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Japan)
                {
                    btn.GetComponent<Image>().sprite = NamSanH_Select_J;
                }
                break;
            case "Capture":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Capture_Select_K;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    btn.GetComponent<Image>().sprite = Capture_Select_E;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
                {
                    btn.GetComponent<Image>().sprite = Capture_Select_C;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Japan)
                {
                    btn.GetComponent<Image>().sprite = Capture_Select_J;
                }
                break;
            case "Language":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Language_Select_K;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    btn.GetComponent<Image>().sprite = Language_Select_E;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
                {
                    btn.GetComponent<Image>().sprite = Language_Select_C;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Japan)
                {
                    btn.GetComponent<Image>().sprite = Language_Select_J;
                }
                break;
            case "Tip":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Tip_Select_K;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    btn.GetComponent<Image>().sprite = Tip_Select_E;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
                {
                    btn.GetComponent<Image>().sprite = Tip_Select_C;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Japan)
                {
                    btn.GetComponent<Image>().sprite = Tip_Select_J;
                }
                break;
        }
    }

    public void SetOrigin(GameObject btn)
    {
        switch (btn.name)
        {
            case "LiveMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Live_idle_K;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    btn.GetComponent<Image>().sprite = Live_idle_E;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
                {
                    btn.GetComponent<Image>().sprite = Live_idle_C;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Japan)
                {
                    btn.GetComponent<Image>().sprite = Live_idle_J;
                }
                break;
            case "XRMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = XR_idle_K;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    btn.GetComponent<Image>().sprite = XR_idle_E;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
                {
                    btn.GetComponent<Image>().sprite = XR_idle_C;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Japan)
                {
                    btn.GetComponent<Image>().sprite = XR_idle_J;
                }
                break;
            case "NamSanHMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = NamSanH_idle_K;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    btn.GetComponent<Image>().sprite = NamSanH_idle_E;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
                {
                    btn.GetComponent<Image>().sprite = NamSanH_idle_C;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Japan)
                {
                    btn.GetComponent<Image>().sprite = NamSanH_idle_J;
                }
                break;
            case "Capture":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Capture_idle_K;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    btn.GetComponent<Image>().sprite = Capture_idle_E;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
                {
                    btn.GetComponent<Image>().sprite = Capture_idle_C;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Japan)
                {
                    btn.GetComponent<Image>().sprite = Capture_idle_J;
                }
                break;
            case "Language":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Language_idle_K;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    btn.GetComponent<Image>().sprite = Language_idle_E;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
                {
                    btn.GetComponent<Image>().sprite = Language_idle_C;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Japan)
                {
                    btn.GetComponent<Image>().sprite = Language_idle_J;
                }
                break;
            case "Tip":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Tip_idle_K;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    btn.GetComponent<Image>().sprite = Tip_idle_E;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
                {
                    btn.GetComponent<Image>().sprite = Tip_idle_C;
                }
                else if (GameManager.currentLang == GameManager.Language_enum.Japan)
                {
                    btn.GetComponent<Image>().sprite = Tip_idle_J;
                }
                break;
        }
    }


    public void SceneChagneSetOrigin()
    {
        Debug.LogError(GameManager.currentLang);
        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            GameManager.currentLang = GameManager.Language_enum.Korea;
            SelectKorea();
        }
        else if (GameManager.currentLang == GameManager.Language_enum.English)
        {
            GameManager.currentLang = GameManager.Language_enum.English;
            SelectEnglish();
        }
        else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
        {
            GameManager.currentLang = GameManager.Language_enum.Chinese;
            SelectChinese();
        }
        else if (GameManager.currentLang == GameManager.Language_enum.Japan)
        {
            GameManager.currentLang = GameManager.Language_enum.Japan;
            SelectJapan();
        }
    }
}
