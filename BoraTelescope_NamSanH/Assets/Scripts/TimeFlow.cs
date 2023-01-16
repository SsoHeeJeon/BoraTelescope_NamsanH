using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeFlow : MonoBehaviour
{
    public NamSanHMode namsanH;

    public GameObject TFBackground;
    public GameObject TFImage;
    public GameObject TFText_k;
    public GameObject TFText_e;
    public GameObject TFText_C;
    public GameObject TFText_J;
    [SerializeField] Text YearText;

    public Sprite Y1991;
    public Sprite Y1996;
    public Sprite Y1998;
    public Sprite Y2003;
    public Sprite Y2013;

    Vector3 zoomIn = new Vector3(3f, 3f, 3f);
    Vector3 zoomOut = new Vector3(2f, 2f, 2f);
    Vector3 Pos = new Vector3(-2,39);
    Vector3 Pos_1 = new Vector3(0,0);

    bool ChangeZoom = false;
    bool ZoomInOut = false;

    private void Update()
    {
        if (ChangeZoom == true)
        {
            if (ZoomInOut == false)
            {
                if (Mathf.Abs(TFImage.transform.localScale.x - zoomOut.x) > 0.005f)
                {
                    TFImage.transform.localScale = Vector3.Lerp(TFImage.transform.localScale, zoomOut, Time.deltaTime * 1.8f);
                    if (Pos_1 != Vector3.zero)
                    {
                        if (Mathf.Abs(TFImage.transform.localPosition.x - Pos.x) > 0.005f)
                        {
                            TFImage.transform.localPosition = Vector3.Lerp(TFImage.transform.localPosition, Pos, Time.deltaTime * 1.8f);
                        }
                    }
                }
                else if (Mathf.Abs(TFImage.transform.localScale.x - zoomOut.x) <= 0.005f)
                {
                    TFImage.transform.localScale = zoomOut;

                    if (Pos_1 != Vector3.zero)
                    {
                        if (Mathf.Abs(TFImage.transform.localPosition.x - Pos.x) <= 0.005f)
                        {
                            TFImage.transform.localPosition = Pos;
                            ChangeZoom = false;
                        }
                    }
                    else if (Pos_1 == Vector3.zero)
                    {
                        ChangeZoom = false;
                    }
                }
            }
            else if (ZoomInOut == true)
            {
                if (Mathf.Abs(TFImage.transform.localScale.x - zoomIn.x) > 0.005f)
                {
                    TFImage.transform.localScale = Vector3.Lerp(TFImage.transform.localScale, zoomIn, Time.deltaTime * 1.8f);
                    if (Pos_1 != Vector3.zero)
                    {
                        if (Mathf.Abs(TFImage.transform.localPosition.x - Pos_1.x) > 0.005f)
                        {
                            TFImage.transform.localPosition = Vector3.Lerp(TFImage.transform.localPosition, Pos_1, Time.deltaTime * 1.8f);
                        }
                    }
                }
                else if (Mathf.Abs(TFImage.transform.localScale.x - zoomIn.x) <= 0.005f)
                {
                    TFImage.transform.localScale = zoomIn;

                    if (Pos_1 != Vector3.zero)
                    {
                        if (Mathf.Abs(TFImage.transform.localPosition.x - Pos_1.x) <= 0.005f)
                        {
                            TFImage.transform.localPosition = Pos_1;
                            ChangeZoom = false;
                        }
                    }
                    else if (Pos_1 == Vector3.zero)
                    {
                        ChangeZoom = false;
                    }
                }
            }
        }
    }

    public void ChangeLang()
    {
        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            if (!YearText.text.Contains("≥‚"))
            {
                YearText.text = namsanH.SelectLabel.name + "≥‚";
            }
            TFText_k.SetActive(true);
            TFText_e.SetActive(false);
            TFText_C.SetActive(false);
            TFText_J.SetActive(false);
        }
        else if (GameManager.currentLang == GameManager.Language_enum.English)
        {
            if (YearText.text.Contains("≥‚"))
            {
                YearText.text = namsanH.SelectLabel.name;
            }
            TFText_k.SetActive(false);
            TFText_e.SetActive(true);
            TFText_C.SetActive(false);
            TFText_J.SetActive(false);

        }
        else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
        {
            if (YearText.text.Contains("≥‚"))
            {
                YearText.text = namsanH.SelectLabel.name;
            }
            TFText_k.SetActive(false);
            TFText_C.SetActive(true);
            TFText_e.SetActive(false);
            TFText_J.SetActive(false);
        }
        else if (GameManager.currentLang == GameManager.Language_enum.Japan)
        {
            if (YearText.text.Contains("≥‚"))
            {
                YearText.text = namsanH.SelectLabel.name;
            }
            TFText_k.SetActive(false);
            TFText_J.SetActive(true);
            TFText_C.SetActive(false);
            TFText_e.SetActive(false);
        }
    }

    public void ReadytoStart(GameObject Label)
    {

        namsanH.SelectLabel = Label;
        TFBackground.SetActive(true);
        if (GameManager.currentLang == GameManager.Language_enum.Korea)
        {
            YearText.text = Label.name + "≥‚";
            TFText_k.SetActive(true);
            TFText_e.SetActive(false);
            TFText_C.SetActive(false);
            TFText_J.SetActive(false);
        }
        else if(GameManager.currentLang == GameManager.Language_enum.English)
        {
            YearText.text = Label.name;
            TFText_k.SetActive(false);
            TFText_e.SetActive(true);
            TFText_C.SetActive(false);
            TFText_J.SetActive(false);
        }
        else if (GameManager.currentLang == GameManager.Language_enum.Chinese)
        {
            YearText.text = Label.name;
            TFText_k.SetActive(false);
            TFText_C.SetActive(true);
            TFText_e.SetActive(false);
            TFText_J.SetActive(false);
        }
        else if (GameManager.currentLang == GameManager.Language_enum.Japan)
        {
            YearText.text = Label.name;
            TFText_k.SetActive(false);
            TFText_J.SetActive(true);
            TFText_C.SetActive(false);
            TFText_e.SetActive(false);
        }

        switch (Label.name)
        {
            case "1991":
                TFImage.GetComponent<Image>().sprite = Y1991;
                zoomIn = new Vector3(4.2f, 4.2f, 4.2f);
                zoomOut = new Vector3(2.18f, 2.18f, 2.18f);
                Pos = new Vector3(-2, 65);
                Pos_1 = Vector3.zero;
                break;
            case "1996":
                TFImage.GetComponent<Image>().sprite = Y1996;
                zoomIn = new Vector3(3.8f, 3.8f, 3.8f);
                zoomOut = new Vector3(2f, 2f, 2f);
                Pos = new Vector3(-2, 23);
                Pos_1 = Vector3.zero;
                break;
            case "1998":
                TFImage.GetComponent<Image>().sprite = Y1998;
                zoomIn = new Vector3(3.8f, 3.8f, 3.8f);
                zoomOut = new Vector3(2f, 2f, 2f);
                Pos = new Vector3(-2, -163);
                Pos_1 = Vector3.zero;
                break;
            case "2003":
                TFImage.GetComponent<Image>().sprite = Y2003;
                zoomIn = new Vector3(4.3f, 4.3f, 4.3f);
                zoomOut = new Vector3(2.23f, 2.23f, 2.23f);
                Pos = new Vector3(-2, -26);
                Pos_1 = new Vector3(86, -26);
                break;
            case "2013":
                TFImage.GetComponent<Image>().sprite = Y2013;
                zoomIn = new Vector3(3.18f, 3.18f, 3.18f);
                zoomOut = new Vector3(1.84f, 1.84f, 1.84f);
                Pos = new Vector3(-2, -103);
                Pos_1 = new Vector3(58, -103);
                break;
        }

        TFImage.transform.localPosition = Pos;
        TFImage.transform.localScale = zoomOut;
        TFImage.GetComponent<Image>().SetNativeSize();
        namsanH.Narration.Play();
    }

    public void TFZoom()
    {
        ChangeZoom = true;
        if(TFImage.transform.localScale.x == zoomIn.x)
        {
            ZoomInOut = false; // ¡‹¿Œ(»Æ¥Îµ )
        } else if(TFImage.transform.localScale.x == zoomOut.x)
        {
            ZoomInOut = true;  // ¡‹æ∆øÙ(√‡º“µ )
        }
    }

    public void CloseTF()
    {
        namsanH.SelectLabel = null;
        namsanH.Narration.clip = null;
        namsanH.Narration.Stop();
        TFBackground.SetActive(false);
    }
}
