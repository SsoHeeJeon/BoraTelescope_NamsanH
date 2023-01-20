using PanTiltControl_v2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LabelDetail : MonoBehaviour
{
    public NamSanHMode namsanH;
    public GameObject Detail_Background; // detail
    public GameObject Detail_ScrollView;        // scrollview
    public GameObject Detail_Viewport;          // viewport
    public GameObject InfoHeight;        // contentheight
    public GameObject Detail_Scrollbar;      // scrollbar vertical
    public GameObject DetailMore_but;
    public Button DetailClose_but;

    float timescale;
    float timescale_window;
    public float Detail_Open_x = 725.0f;
    public float Detail_Close_x = 1218.0f;
    float Detail_y = 393.5f;
    float Change_y;
    public float InfoImageHeight;
    Scrollbar detailscroll;
    public Image detailsoundbut;
    RectTransform detailbackground_rect;

    public bool SeeLabelDetail = false;
    public bool SeeDetail_Open = false;
    public bool moredetail = false;
    public bool moredetail_scroll = false;
    public bool CheckDetailTime = false;
    float checkdetail;
    float scrollview_y = 198;
    //float scrollview_y = 179;

    // Start is called before the first frame update
    void Start()
    {
        //PanTiltControl.SetFreq(PanTiltControl.Motor.Pan, PanTiltControl.Speed.Middle);
        //gamemanager.speed_enum = GameManager.Speed_enum.middle;

        SeeLabelDetail = false;
        SeeDetail_Open = false;

        moredetail = false;
        moredetail_scroll = false;
        timescale = 0;
        timescale_window = 0;
        detailscroll = Detail_Scrollbar.GetComponent<Scrollbar>();
        //detailscroll.size = 0;
        Detail_Viewport.GetComponent<RectTransform>().anchorMax = new Vector2(1,1);
        detailsoundbut.sprite = namsanH.Narr_Off;
        detailbackground_rect = Detail_Background.GetComponent<RectTransform>();

        //namsanH.Narration.clip = null;
        namsanH.PlayNarr = true;
        //namsanH.Narration.Stop();

        Detail_Background.transform.localPosition = new Vector3(Detail_Close_x, Detail_y, 0);
        DetailOpen();
    }


    // Update is called once per frame
    void Update()
    {
        timescale += Time.deltaTime * 2.5f;
        timescale_window += Time.deltaTime;
        detailscroll.value = Mathf.Clamp(detailscroll.value, 0,1);
        
        if(SeeLabelDetail == true)
        {
            if(SeeDetail_Open == true)
            {
                DetailWindow_Open();
            } 
            else if(SeeDetail_Open == false)
            {
                DetailWindow_Close();
            }
        }

        if(CheckDetailTime == true)
        {
            checkdetail += Time.deltaTime;

            if((int)checkdetail >= 60)
            {
                if (SceneManager.GetActiveScene().name.Contains("NamSanHMode"))
                {
                    //namsanH.SelectLabel = null;
                    //namsanH.gamemanager.Home_Btn_NamSanH();
                    //CloseDetailWindow();
                }
                CheckDetailTime = false;
            }
        }

        if (moredetail == true)
        {
            if (moredetail_scroll == false)
            {
                Detail_ScrollView.GetComponent<ScrollRect>().enabled = false;
                Detail_Scrollbar.gameObject.SetActive(false);
            }

            ClickMoreDetail();
        }
        else if (moredetail == false)
        {
            Detail_ScrollView.GetComponent<ScrollRect>().enabled = false;
            Detail_Scrollbar.gameObject.SetActive(false);
            Detail_Viewport.GetComponent<RectTransform>().offsetMax = new Vector2(17, 0);

            OriginMoreDetail();
        }
    }

    public void ChangeDetailLanguage()
    {

        Invoke("ClickLabel", 0.05f);
    }

    // 라벨 선택하면 상세설명 사이즈 확인
    public void ClickLabel()
    {
        timescale = 0;
        timescale_window = 0;

        InfoImageHeight = InfoHeight.GetComponent<RectTransform>().rect.height;
        InfoHeight.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(InfoHeight.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.x, InfoImageHeight);
        InfoHeight.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        if (moredetail == false)
        {
            if (InfoHeight.GetComponent<UIText>())
            {
                if (InfoImageHeight <= 197)
                {
                    DetailMore_but.gameObject.SetActive(false);
                }
                else
                {
                    DetailMore_but.gameObject.SetActive(true);
                }
            }
        } else if (moredetail == true)
        {
            InfoImageHeight = InfoHeight.GetComponent<RectTransform>().rect.height;

            if (InfoImageHeight > 366)
            {
                Detail_ScrollView.GetComponent<ScrollRect>().enabled = true;
                Detail_Scrollbar.gameObject.SetActive(true);
                moredetail_scroll = true;

                if (!DetailMore_but.activeSelf)
                {
                    SeeMore();
                }
            } else if(InfoImageHeight <= 197)
            {
                moredetail = false;
            } else if(InfoImageHeight > 197 && InfoImageHeight <= 366)
            {
                if (!DetailMore_but.activeSelf)
                {
                    SeeMore();
                }
            }
        }
    }

    // 상세설명창 열기
    public void DetailOpen()
    {
        if (SceneManager.GetActiveScene().name == "NamSanHMode") {
            timescale_window = 0;
            SeeLabelDetail = true;
            SeeDetail_Open = true;
            DetailClose_but.enabled = false;
            namsanH.gamemanager.WriteLog(LogSendServer.NormalLogCode.NamSanH_Detail, "NamSanH_Detail:On", GetType().ToString());
        }
    }


    // 상세설명창 열기
    public void DetailWindow_Open()
    {
        if (Mathf.Abs(Detail_Background.transform.localPosition.x - Detail_Open_x) > 0.5f)
        {
            if (Detail_Background.transform.localPosition.x > Detail_Open_x + 1)
            {
                Detail_Background.transform.localPosition = new Vector3(Mathf.Lerp(Detail_Close_x, Detail_Open_x, timescale_window), Detail_y, 0);
            }
            else if (Detail_Background.transform.localPosition.x <= Detail_Open_x + 1)
            {
                Detail_Background.transform.localPosition = new Vector3(Detail_Open_x, Detail_y, 0);
                SeeDetail_Open = true;
                SeeLabelDetail = false;

                DetailClose_but.enabled = true;

                //if (namsanH.PlayNarr == true)
                //{
                //    namsanH.Narration.Play();
                //}
            }
        } else if (Mathf.Abs(Detail_Background.transform.localPosition.x - Detail_Open_x) <= 0.5f)
        {
            Detail_Background.transform.localPosition = new Vector3(Detail_Open_x, Detail_y, 0);
            SeeDetail_Open = true;
            SeeLabelDetail = false;
            checkdetail = 0;
            CheckDetailTime = true;

            DetailClose_but.enabled = true;

            //if (namsanH.PlayNarr == true)
            //{
            //    namsanH.Narration.Play();
            //}
        }
    }
    
    // 상세설명창 닫기
    public void DetailWindow_Close()
    {
        if (Mathf.Abs(Detail_Background.transform.localPosition.x - Detail_Close_x)>1f)
        {
            if (Detail_Background.transform.localPosition.x < Detail_Close_x - 2)
            {
                Detail_Background.transform.localPosition = new Vector3(Mathf.Lerp(Detail_Open_x, Detail_Close_x+10, timescale_window), Detail_y, 0);
            }
            else if (Detail_Background.transform.localPosition.x >= Detail_Close_x - 2)
            {
                Detail_Background.transform.localPosition = new Vector3(Detail_Close_x, Detail_y, 0);
                SeeDetail_Open = false;
                SeeLabelDetail = false;
            }
        } else if (Mathf.Abs(Detail_Background.transform.localPosition.x - Detail_Close_x) <= 1f)
        {
            SeeDetail_Open = false;
            SeeLabelDetail = false;

            Invoke("WaitOpenButton", 0.5f);
        }
    }

    public void WaitOpenButton()
    {
        //for (int index = 0; index < namsanH.Docent_AllUI.transform.childCount; index++)
        //{
        //    namsanH.Docent_AllUI.transform.GetChild(index).gameObject.GetComponent<Button>().enabled = true;
        //}

        GameObject NaviLabel = namsanH.gamemanager.NavigationBar.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        for (int index = 0; index < NaviLabel.transform.childCount; index++)
        {
            NaviLabel.transform.GetChild(index).gameObject.GetComponent<Button>().enabled = true;
        }
    }

    // 상세설명 더보기
    public void SeeMore()
    {
        namsanH.gamemanager.WriteLog(LogSendServer.NormalLogCode.NamSanH_DetailMore, "NamSanH_Detail:More", GetType().ToString());

        timescale = 0;
        if (moredetail == false)
        {
            moredetail = true;

            //ClickLabel();
            if (InfoImageHeight > 366)
            {
                Change_y = 787;
                moredetail_scroll = true;
            } else if(InfoImageHeight <= 366 && InfoImageHeight > 192)
            {
                Change_y = (InfoImageHeight - scrollview_y) + 618;
                moredetail_scroll = false;
            }
        }
        else if (moredetail == true)
        {
            //moredetail = false;
            if (InfoImageHeight > 366 && Change_y != 787)
            {
                Change_y = 787;
                moredetail_scroll = true;
            }
            else if ((InfoImageHeight <= 366 && InfoImageHeight > 192) && Change_y != ((InfoImageHeight - scrollview_y) + 618))
            {
                Change_y = (InfoImageHeight - scrollview_y) + 618;
                moredetail_scroll = false;
            }
        }
        DetailMore_but.gameObject.SetActive(false);
    }

    public void ClickMoreDetail()
    {
        if (detailbackground_rect.rect.height != Change_y)
        {
            if (detailbackground_rect.rect.height < Change_y - 1)
            {
                detailbackground_rect.sizeDelta = new Vector2(detailbackground_rect.rect.width, Mathf.Lerp(detailbackground_rect.rect.height, Change_y, timescale));
                float maskinfoY = 178 + (detailbackground_rect.sizeDelta.y - 618) / 169 * 178;
                //Detail_Viewport.GetComponent<RectTransform>().offsetMin = new Vector2(Detail_Viewport.GetComponent<RectTransform>().rect.xMin, maskinfoY);
                Detail_ScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(Detail_ScrollView.GetComponent<RectTransform>().rect.width, maskinfoY);
                detailscroll.size = 0;
            }
            else if (detailbackground_rect.rect.height >= Change_y - 1)
            {
                detailbackground_rect.sizeDelta = new Vector2(detailbackground_rect.rect.width, Change_y);
                //Detail_Viewport.GetComponent<RectTransform>().offsetMin = new Vector2(Detail_Viewport.GetComponent<RectTransform>().rect.xMin, 0.0f);
                Detail_ScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(Detail_ScrollView.GetComponent<RectTransform>().rect.width, 356.0f);

                if (moredetail_scroll == true)
                {
                    Detail_ScrollView.GetComponent<ScrollRect>().enabled = true;
                    Detail_Scrollbar.gameObject.SetActive(true);
                    detailscroll.size = 0;
                }
            }
        }
        else if (detailbackground_rect.rect.height == Change_y)
        {
            //Detail_Viewport.GetComponent<RectTransform>().offsetMin = new Vector2(Detail_Viewport.GetComponent<RectTransform>().rect.xMin, 0.0f);
            Detail_ScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(Detail_ScrollView.GetComponent<RectTransform>().rect.width, 356.0f);

            if (moredetail_scroll == true)
            {
                Detail_ScrollView.GetComponent<ScrollRect>().enabled = true;
                Detail_Scrollbar.gameObject.SetActive(true);
                detailscroll.size = 0;
            }
        }
    }

    // 상세설명 원래상태로 복원
    public void OriginMoreDetail()
    {
        if (detailbackground_rect.rect.height != 618.0f)
        {
            if (detailbackground_rect.rect.height > 619.0f)
            {
                detailbackground_rect.sizeDelta = new Vector2(detailbackground_rect.rect.width, Mathf.Lerp(Change_y, 618.0f, timescale));
                float maskinfoY = scrollview_y + (detailbackground_rect.sizeDelta.y - 618) / 169 * scrollview_y;
                //Detail_Viewport.GetComponent<RectTransform>().offsetMin = new Vector2(Detail_Viewport.GetComponent<RectTransform>().rect.xMin, maskinfoY);
                Detail_ScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(Detail_ScrollView.GetComponent<RectTransform>().rect.width, maskinfoY);
            }
            else if (detailbackground_rect.rect.height <= 619.0f)
            {
                detailbackground_rect.sizeDelta = new Vector2(detailbackground_rect.rect.width, 618.0f);
                detailscroll.size = 0;
            }
        }
        else if (detailbackground_rect.rect.height == 618.0f)
        {
            detailbackground_rect.sizeDelta = new Vector2(detailbackground_rect.rect.width, 618);
            if (Detail_ScrollView.GetComponent<RectTransform>().sizeDelta.y != scrollview_y)
            {
                if (Detail_ScrollView.GetComponent<RectTransform>().sizeDelta.y > 202.0f)
                {
                    Detail_ScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(Detail_ScrollView.GetComponent<RectTransform>().rect.width, Mathf.Lerp(356.0f, scrollview_y, timescale));
                }
                else if (Detail_ScrollView.GetComponent<RectTransform>().sizeDelta.y <= 202.0f)
                {
                    Detail_ScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(Detail_ScrollView.GetComponent<RectTransform>().rect.width, scrollview_y);
                }
            }
            else if (Detail_ScrollView.GetComponent<RectTransform>().sizeDelta.y == scrollview_y)
            {
                Detail_ScrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(Detail_ScrollView.GetComponent<RectTransform>().rect.width, scrollview_y);
            }
            detailscroll.size = 0;
        }
    }

    // 상세설명창 닫기
    public void CloseDetailWindow()
    {
        timescale = 0;
        timescale_window = 0;
        SeeLabelDetail = true;
        moredetail = false;
        moredetail_scroll = false;
        SeeDetail_Open = false;
        checkdetail = 0;
        CheckDetailTime = false;
        DetailMore_but.gameObject.SetActive(true);
        namsanH.Narration.Stop();
        
        //for (int index = 0; index < namsanH.Docent_AllUI.transform.childCount; index++)
        //{
        //    namsanH.Docent_AllUI.transform.GetChild(index).gameObject.GetComponent<Button>().enabled = true;
        //}

        namsanH.gamemanager.WriteLog(LogSendServer.NormalLogCode.NamSanH_DetailClose, "NamSanH_Detail:Close", GetType().ToString());
    }

    public void SelectCloseButton()
    {
        namsanH.gamemanager.WriteLog(LogSendServer.NormalLogCode.NamSanH_DetailClose, "NamSanH_Detail:Close", GetType().ToString());
    }

    public void ChangeSprite_CursorOn()
    {
        if(namsanH.PlayNarr == false)
        {
            detailsoundbut.sprite = namsanH.Narr_Off;
        }
        else if(namsanH.PlayNarr == true)
        {
            detailsoundbut.sprite = namsanH.Narr_On;
        }
    }
    public void ChangeSprite_CursorOff()
    {
        if (namsanH.PlayNarr == false)
        {
            detailsoundbut.sprite = namsanH.Narr_On;
        }
        else if (namsanH.PlayNarr == true)
        {
            detailsoundbut.sprite = namsanH.Narr_Off;
        }
    }

    private Vector3 firstTouchPosition;
    private Vector3 secondTouchPosition;
    //public GameObject Detail3D;
    private bool resettouch = false;

    //public void ObjectDetail()
    //{
    //    if (Input.touchCount == 1)
    //    {
    //        if (Input.GetTouch(0).phase == TouchPhase.Began)
    //        {
    //            if (Input.GetTouch(0).position.x >= 1450 && Input.GetTouch(0).position.x <= 1780 && Input.GetTouch(0).position.y >= 650 && Input.GetTouch(0).position.y <= 850)
    //            {
    //                firstTouchPosition = Input.GetTouch(0).position;
    //                resettouch = false;
    //            } else
    //            {
    //                firstTouchPosition = Input.GetTouch(0).position;
    //            }
    //        }
    //        else if (Input.GetTouch(0).phase == TouchPhase.Stationary)
    //        {
    //            if (Input.GetTouch(0).position.x >= 1450 && Input.GetTouch(0).position.x <= 1780 && Input.GetTouch(0).position.y >= 650 && Input.GetTouch(0).position.y <= 850)
    //            {
    //                firstTouchPosition = Input.GetTouch(0).position;
    //                resettouch = false;
    //            } else
    //            {
    //                firstTouchPosition = Input.GetTouch(0).position;
    //            }
    //        }
    //        else if (Input.GetTouch(0).phase == TouchPhase.Moved)
    //        {
    //            if (resettouch == false)
    //            {
    //                if (firstTouchPosition.x >= 1450 && firstTouchPosition.x <= 1780 && firstTouchPosition.y >= 650 && firstTouchPosition.y <= 850)
    //                {
    //                    secondTouchPosition = Input.GetTouch(0).position;

    //                    float rotationValue = Detail3D.transform.GetChild(0).gameObject.transform.rotation.eulerAngles.y;

    //                    if ((secondTouchPosition.x - firstTouchPosition.x) > 1f)
    //                    {
    //                        gamemanager.clearMode.TouchStop = true;
    //                        //float rotationValue = Detail3D.transform.GetChild(0).gameObject.transform.rotation.eulerAngles.y;
    //                        rotationValue += 4;
    //                        Detail3D.transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(0, rotationValue, 0);
    //                    }
    //                    else if ((secondTouchPosition.x - firstTouchPosition.x) < 1f)
    //                    {
    //                        gamemanager.clearMode.TouchStop = true;
    //                        //float rotationValue = Detail3D.transform.GetChild(0).gameObject.transform.rotation.eulerAngles.y;
    //                        rotationValue -= 4;
    //                        Detail3D.transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(0, rotationValue, 0);
    //                    }

    //                    firstTouchPosition = Input.GetTouch(0).position;
    //                    resettouch = true;
    //                }
    //            } else if (resettouch == true)
    //            {
    //                secondTouchPosition = Input.GetTouch(0).position;

    //                float rotationValue = Detail3D.transform.GetChild(0).gameObject.transform.rotation.eulerAngles.y;

    //                if ((secondTouchPosition.x - firstTouchPosition.x) > 1f)
    //                {
    //                    gamemanager.clearMode.TouchStop = true;
    //                    //float rotationValue = Detail3D.transform.GetChild(0).gameObject.transform.eulerAngles.y;
    //                    rotationValue += 4;
    //                    Detail3D.transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(0, rotationValue, 0);
    //                }
    //                else if ((secondTouchPosition.x - firstTouchPosition.x) < 1f)
    //                {
    //                    gamemanager.clearMode.TouchStop = true;
    //                    //float rotationValue = Detail3D.transform.GetChild(0).gameObject.transform.eulerAngles.y;
    //                    rotationValue -= 4;
    //                    Detail3D.transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(0, rotationValue, 0);
    //                }

    //                firstTouchPosition = Input.GetTouch(0).position;
    //            }
    //        }
    //    } else if (Input.touchCount != 1)
    //    {
    //        if (SceneManager.GetActiveScene().name.Contains("ClearMode"))
    //        {
    //            gamemanager.clearMode.TouchStop = false;
    //        }
    //        resettouch = false;
    //    }
    //}

    public void PlayButtonClick()
    {
        namsanH.gamemanager.ButtonClickSound();
    }

    public void Detail_etc()
    {
        // 버튼, 드래그 하면 사진 변경
    }
}
