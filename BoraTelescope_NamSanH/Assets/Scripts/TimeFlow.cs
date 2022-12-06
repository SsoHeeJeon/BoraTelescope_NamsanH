using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeFlow : MonoBehaviour
{
    public NamSanHMode namsanH;

    public GameObject TFBackground;
    public GameObject TFImage;
    public Text TFText;

    public Sprite Y1991;
    public Sprite Y1996;
    public Sprite Y1998;
    public Sprite Y2003;
    public Sprite Y2013;

    Vector3 zoomIn = new Vector3(5,5,5);
    Vector3 zoomOut = new Vector3(1,1,1);

    bool ChangeZoom = false;
    bool ZoomInOut = false;

    private void Update()
    {
        if (ChangeZoom == true)
        {
            if (ZoomInOut == false)
            {
                if ((TFImage.transform.localScale.x - 1) < 0.1f)
                {
                    TFImage.transform.localScale = Vector3.Lerp(zoomIn, zoomOut, Time.deltaTime);
                }
                else if ((TFImage.transform.localScale.x - 5) < 0.1f)
                {
                    TFImage.transform.localScale = zoomIn;
                    ChangeZoom = false;
                }
            }
            else if (ZoomInOut == true)
            {
                if ((TFImage.transform.localScale.x - 5) < 0.1f)
                {
                    TFImage.transform.localScale = Vector3.Lerp(zoomOut, zoomIn, Time.deltaTime);
                }
                else if ((TFImage.transform.localScale.x - 1) < 0.1f)
                {
                    TFImage.transform.localScale = zoomOut;
                    ChangeZoom = false;
                }
            }
        }
    }

    public void ReadytoStart(GameObject Label)
    {
        TFBackground.SetActive(true);

        switch (Label.name)
        {
            case "1991":
                TFImage.GetComponent<Image>().sprite = Y1991;
                TFText.text = ReadJsonFile.DetailText_K[0];

                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    TFText.text = ReadJsonFile.DetailText_K[0];
                    namsanH.Narration.clip = namsanH.gamemanager.Narration_Flow_K[0];
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    TFText.text = ReadJsonFile.DetailText_E[0];
                    namsanH.Narration.clip = namsanH.gamemanager.Narration_Flow_E[0];
                }
                break;
            case "1996":
                TFImage.GetComponent<Image>().sprite = Y1996;

                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    TFText.text = ReadJsonFile.DetailText_K[1];
                    namsanH.Narration.clip = namsanH.gamemanager.Narration_Flow_K[1];
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    TFText.text = ReadJsonFile.DetailText_E[1];
                    namsanH.Narration.clip = namsanH.gamemanager.Narration_Flow_E[1];
                }
                break;
            case "1998":
                TFImage.GetComponent<Image>().sprite = Y1998;

                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    TFText.text = ReadJsonFile.DetailText_K[2];
                    namsanH.Narration.clip = namsanH.gamemanager.Narration_Flow_K[2];
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    TFText.text = ReadJsonFile.DetailText_E[2];
                    namsanH.Narration.clip = namsanH.gamemanager.Narration_Flow_E[2];
                }
                break;
            case "2003":
                TFImage.GetComponent<Image>().sprite = Y2003;

                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    TFText.text = ReadJsonFile.DetailText_K[3];
                    namsanH.Narration.clip = namsanH.gamemanager.Narration_Flow_K[3];
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    TFText.text = ReadJsonFile.DetailText_E[3];
                    namsanH.Narration.clip = namsanH.gamemanager.Narration_Flow_E[3];
                }
                break;
            case "2013":
                TFImage.GetComponent<Image>().sprite = Y2013;

                if (GameManager.currentLang == GameManager.Language_enum.Korea)
                {
                    TFText.text = ReadJsonFile.DetailText_K[4];
                    namsanH.Narration.clip = namsanH.gamemanager.Narration_Flow_K[4];
                }
                else if (GameManager.currentLang == GameManager.Language_enum.English)
                {
                    TFText.text = ReadJsonFile.DetailText_E[4];
                    namsanH.Narration.clip = namsanH.gamemanager.Narration_Flow_E[4];
                }
                break;
        }

        namsanH.Narration.Play();
    }

    public void TFZoom()
    {
        ChangeZoom = true;
        if(TFImage.transform.localScale.x == 1)
        {
            ZoomInOut = false; // ¡‹¿Œ(»Æ¥Îµ )
        } else if(TFImage.transform.localScale.x == 5)
        {
            ZoomInOut = true;  // ¡‹æ∆øÙ(√‡º“µ )
        }
    }

    public void CloseTF()
    {
        namsanH.Narration.Stop();
        TFBackground.SetActive(false);
    }
}
