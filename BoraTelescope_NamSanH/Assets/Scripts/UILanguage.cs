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

    private void Start()
    {
        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            SelectKorea();
        }
        else if (GameManager.currentLang != GameManager.Language_enum.Korea)
        {
            NotSelectKorea();
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

    public void NotSelectKorea()
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
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Live_Select_E;
                }
                break;
            case "XRMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = XR_Select_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = XR_Select_E;
                }
                break;
            case "NamSanHMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = NamSanH_Select_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = NamSanH_Select_E;
                }
                break;
            case "Capture":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Capture_Select_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Capture_Select_E;
                }
                break;
            case "Language":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Language_Select_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Language_Select_E;
                }
                break;
            case "Tip":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Tip_Select_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Tip_Select_E;
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
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Live_idle_E;
                }
                break;
            case "XRMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = XR_idle_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = XR_idle_E;
                }
                break;
            case "NamSanHMode":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = NamSanH_idle_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = NamSanH_idle_E;
                }
                break;
            case "Capture":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Capture_idle_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Capture_idle_E;
                }
                break;
            case "Language":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Language_idle_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Language_idle_E;
                }
                break;
            case "Tip":
                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Tip_idle_K;
                }
                else if (GameManager.currentLang != GameManager.Language_enum.Korea)
                {
                    btn.GetComponent<Image>().sprite = Tip_idle_E;
                }
                break;
        }
    }


    public void SceneChagneSetOrigin()
    {
        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            SelectKorea();
        }
        else if (GameManager.currentLang != GameManager.Language_enum.Korea)
        {
            NotSelectKorea();
        }
    }
}
