using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using PanTiltControl_v2;
using System;

public class XRMode_Manager : MonoBehaviour
{
    public GameManager gamemanager;
    public GameObject ManagerMode;
    public GameObject CameraWindow;

    /// <summary>
    /// 3D오브젝트 위치정보 XRLabelPosition.json
    /// </summary>
    public GameObject AllLabel;     // 3D 오브젝트 상위오브젝트(학, 잉어, 다람쥐, 참새)
    public GameObject Crane;
    public GameObject Fish;
    public GameObject Bird;
    public GameObject Squirrel;

    string LabelName;
    float Label_x;
    float Label_y;
    float LabelScale;

    string Allstr;
    string RangePT;
    string[] allstr_json;

    public List<string> AllLabelPosition;
    public string[] AllLabelPosition_arr;

    public GameObject CurrentlabelPosition_x;
    public GameObject CurrentlabelPosition_y;
    public Slider ChangeValuePos;
    public GameObject ChangeValuePos_t;
    public GameObject SelectLabel;
    public GameObject SelectLabel_btn;

    /// <summary>
    /// 팬틸트 정보
    /// - 최대최소 범위
    /// - 초기지점
    /// </summary>
    float Current_x;
    float Current_y;

    public static float MinPan;
    public static float MaxPan;
    public static float MinTilt;
    public static float MaxTilt;
    public static float StartPosition_x;
    public static float StartPosition_y;
    public static float TotalPan;
    public static float TotalTilt;

    public bool AllchangeLabel = false;
    public bool setlabelcamera = false;

    public InputField SetValueX;
    public InputField SetValueY;
    public GameObject Totalpan_Text;
    public GameObject Totaltilt_Text;

    float SetValueX_min;
    float SetValueX_max;
    float SetValueY_min;
    float SetValueY_max;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //ManagerMode = GameObject.Find("ManagerMode");
        gamemanager.UISetting();

        AllchangeLabel = false;
        ReadFile();
        XRMode.ValueX = TotalPan;
        XRMode.ValueY = TotalTilt;
        
        if (GameManager.SettingLabelPosition == true)
        {
            gamemanager.xrMode.enabled = false;
            gamemanager.xrMode_manager.enabled = true;
            ManagerMode.gameObject.SetActive(true);
            for (int index = 0; index < AllLabel.transform.childCount; index++)
            {
                AllLabel.transform.GetChild(index).gameObject.GetComponent<Image>().fillAmount = 1;
            }
        }
        else if (GameManager.SettingLabelPosition == false)
        {
            gamemanager.xrMode.enabled = true;
            ManagerMode.gameObject.SetActive(false);
            gamemanager.xrMode_manager.enabled = false;
        }
        //SettingLabel();
    }

    // Update is called once per frame
    void Update()
    {
        Current_x = PanTiltControl.NowPanPulse;
        Current_y = PanTiltControl.NowTiltPulse;
        CameraWindow.transform.localPosition = new Vector3(Current_x * TotalPan, Current_y * TotalTilt, CameraWindow.transform.localPosition.z);

        ChangeValuePos_t.GetComponent<Text>().text = ((int)(ChangeValuePos.value * 200)).ToString();

        if (SelectLabel == null)
        {
            CurrentlabelPosition_x.GetComponent<Text>().text = "(" + PanTiltControl.NowPanPulse + ",";
            CurrentlabelPosition_y.GetComponent<Text>().text = PanTiltControl.NowTiltPulse + ")";
        } else if(SelectLabel != null)
        {
            if (setlabelcamera == true)
            {
                CurrentlabelPosition_x.GetComponent<Text>().text = "(" + SelectLabel.transform.localPosition.x/TotalPan + ",";
                CurrentlabelPosition_y.GetComponent<Text>().text = SelectLabel.transform.localPosition.y/TotalTilt + ")";
            }else if(setlabelcamera == false)
            {
                CurrentlabelPosition_x.GetComponent<Text>().text = "(" + PanTiltControl.NowPanPulse + ",";
                CurrentlabelPosition_y.GetComponent<Text>().text = PanTiltControl.NowTiltPulse + ")";
                if(Mathf.Abs(SelectLabel.transform.localPosition.x/TotalPan - Current_x)<1 && Mathf.Abs(SelectLabel.transform.localPosition.y / TotalTilt - Current_y)<1)
                {
                    setlabelcamera = true;
                }
            }
        }

        if (GameManager.MoveCamera == true)
        {
            MoveCamera_Arrow();
            if (setlabelcamera == true && SelectLabel != null)
            {
                SelectLabel.transform.localPosition = new Vector3(CameraWindow.transform.localPosition.x, CameraWindow.transform.localPosition.y, SelectLabel.transform.localPosition.z);
            }
        }
    }

    public void SettingLabel()
    {
        Fish.transform.GetChild(0).gameObject.SetActive(false);
        for (int index = 0; index < AllLabel.transform.childCount; index++)
        {
            LabelName = AllLabel.transform.GetChild(index).transform.gameObject.name;
            Label_x = AllLabel.transform.GetChild(index).transform.localPosition.x / TotalPan;
            Label_y = AllLabel.transform.GetChild(index).transform.localPosition.y / TotalTilt;
            LabelScale = AllLabel.transform.GetChild(0).transform.GetChild(index).transform.localScale.x;
            
            SaveLabelPosition(LabelName, Label_x, Label_y, LabelScale);
        }
    }

    public void SaveLabelPosition(string labelname, float label_x, float label_y, float labelsize)
    {
        XRModeLabelPosition labelposition = new XRModeLabelPosition(labelname, label_x, label_y, labelsize);
        //StartPosition = gamemanager.StartLabel;

        PanTiltRange pantilt = new PanTiltRange(MinPan, MaxPan, MinTilt, MaxTilt, GameManager.startlabel_x, GameManager.startlabel_y, TotalPan, TotalTilt, (int)GameManager.waitingTime);

        string str = JsonUtility.ToJson(labelposition);
        string str_1 = JsonUtility.ToJson(pantilt);

        AllLabelPosition_arr = new string[AllLabel.transform.childCount];

        AllLabelPosition.Add(str);
        AllLabelPosition_arr = AllLabelPosition.ToArray();

        Allstr = "";

        if (AllLabelPosition.Count == AllLabel.transform.childCount)
        {
            for (int index = 0; index < AllLabel.transform.childCount; index++)
            {
                Allstr += AllLabelPosition_arr[index].ToString();
            }

            File.WriteAllText(Application.dataPath + ("/XRModeLabelPosition.json"), Allstr + str_1);

            GameObject.Find("GameManager").GetComponent<ReadJsonFile>().Readfile();
        }
    }

    public void ReadFile()
    {
        /*
        Allstr = File.ReadAllText(Application.dataPath + ("/XRModeLabelPosition.json"));

        if (Allstr.Contains("}"))
        {
            allstr_json = Allstr.Split('}');

            for(int index = -1; index<allstr_json.Length-1; index++)
            {
                if (index < allstr_json.Length - 2)
                {
                    allstr_json[index + 1] = allstr_json[index + 1] + "}";
                } else if(index == allstr_json.Length - 2)
                {
                    RangePT = allstr_json[index];
                }
            }
        }
        */
        PanTiltRange pantilt = JsonUtility.FromJson<PanTiltRange>(ReadJsonFile.RangePT);
        MinPan = pantilt.Min_Pan;
        MaxPan = pantilt.Max_Pan;
        MinTilt = pantilt.Min_Tilt;
        MaxTilt = pantilt.Max_Tilt;
        TotalPan = pantilt.ChangeValue_x;
        TotalTilt = pantilt.ChangeValue_y;
        for (int index = 0; index< ReadJsonFile.allstr_json.Length - 2; index++)
        {
            XRModeLabelPosition labelPosition = JsonUtility.FromJson<XRModeLabelPosition>(ReadJsonFile.allstr_json[index]);

            if(AllLabel.transform.GetChild(index).gameObject.name == labelPosition.LabelName)
            {
                AllLabel.transform.GetChild(index).gameObject.transform.localPosition = new Vector3(labelPosition.Label_X * TotalPan, labelPosition.Label_Y * TotalTilt, 0);
            }
        }
    }

    // 라벨 버튼이나 네비게이션의 라벨을 눌렀을때 라벨의 위치로 팬틸트이동
    public void PanTiltMove()
    {
        if (SelectLabel.transform.localPosition.x != PanTiltControl.NowPanPulse / TotalPan || SelectLabel.transform.localPosition.y != PanTiltControl.NowTiltPulse / TotalTilt)
        {
            float xpulse = SelectLabel.transform.localPosition.x / TotalPan;
            float ypulse = SelectLabel.transform.localPosition.y / TotalTilt;

            PanTiltControl.SetPulse((uint)xpulse, (uint)ypulse);
        }
    }

    // 화살표 선택하면 해당 방향으로 팬틸트와 카메라 움직이기
    // 카메라 중앙에 라벨이 있다면 라벨도 동시에 움직이기
    public void MoveCamera_Arrow()
    {
        switch (gamemanager.MoveDir)
        {
            case "Left":
                if (Current_x >= MinPan)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.LEFT);
                }
                break;
            case "Right":
                if (Current_x <= MaxPan)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.RIGHT);
                }
                break;
            case "Up":
                //PanTiltControl.SetFreq(PanTiltControl.Motor.Tilt, 10);
                if (Current_y < MaxTilt)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.UP);
                }
                else
                {
                    PanTiltControl.Stop();
                }
                break;
            case "Down":
                if (Current_y > MinTilt)
                {
                    PanTiltControl.ButtonAction(PanTiltControl.ButtonDIR.DOWN);
                } else
                {
                    PanTiltControl.Stop();
                }
                break;
        }
    }

    public void AllLabelChange(GameObject btn)
    {
        for (int index = 0; index < btn.transform.parent.gameObject.transform.childCount; index++)
        {
            btn.transform.parent.gameObject.transform.GetChild(index).gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
        }
        btn.GetComponent<Image>().color = new Color(0.8632076f, 0.9884475f, 1f);
        switch (btn.name)
        {
            case "AllLabel":
                SelectLabel = null;
                AllchangeLabel = true;
                break;
            case "SelectCrane":
                SelectLabel = Crane;
                AllchangeLabel = false;
                PanTiltMove();
                break;
            case "SelectBird":
                SelectLabel = Bird;
                AllchangeLabel = false;
                PanTiltMove();
                break;
            case "SelectFish":
                SelectLabel = Fish;
                AllchangeLabel = false;
                Fish.transform.GetChild(0).gameObject.SetActive(true);
                PanTiltMove();
                break;
            case "SelectSquirrel":
                SelectLabel = Squirrel;
                AllchangeLabel = false;
                PanTiltMove();
                break;
        }
    }

    public void ChangePositionScale(GameObject btn)
    {
        float changepos =  float.Parse(ChangeValuePos_t.GetComponent<Text>().text);
        switch (btn.name)
        {
            case "XPlus":
                if (AllchangeLabel == true)
                {
                    for (int index = 0; index < AllLabel.transform.childCount; index++)
                    {
                        float lab_x = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.x + changepos;
                        float lab_y = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.y;
                        float lab_z = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.z;
                        AllLabel.transform.GetChild(index).gameObject.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                }
                else if (AllchangeLabel == false)
                {
                    if (SelectLabel != null)
                    {
                        float lab_x = SelectLabel.transform.localPosition.x + changepos;
                        float lab_y = SelectLabel.transform.localPosition.y;
                        float lab_z = SelectLabel.transform.localPosition.z;
                        SelectLabel.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                }
                break;
            case "XMinus":
                if (AllchangeLabel == true)
                {
                    for (int index = 0; index < AllLabel.transform.childCount; index++)
                    {
                        float lab_x = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.x - changepos;
                        float lab_y = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.y;
                        float lab_z = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.z;
                        AllLabel.transform.GetChild(index).gameObject.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                }
                else if (AllchangeLabel == false)
                {
                    if (SelectLabel != null)
                    {
                        float lab_x = SelectLabel.transform.localPosition.x - changepos;
                        float lab_y = SelectLabel.transform.localPosition.y;
                        float lab_z = SelectLabel.transform.localPosition.z;
                        SelectLabel.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                }
                break;
            case "YPlus":
                if (AllchangeLabel == true)
                {
                    for (int index = 0; index < AllLabel.transform.childCount; index++)
                    {
                        float lab_x = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.x;
                        float lab_y = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.y + changepos;
                        float lab_z = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.z;
                        AllLabel.transform.GetChild(index).gameObject.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                }
                else if (AllchangeLabel == false)
                {
                    if (SelectLabel != null)
                    {
                        float lab_x = SelectLabel.transform.localPosition.x;
                        float lab_y = SelectLabel.transform.localPosition.y + changepos;
                        float lab_z = SelectLabel.transform.localPosition.z;
                        SelectLabel.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                }
                break;
            case "YMinus":
                if (AllchangeLabel == true)
                {
                    for (int index = 0; index < AllLabel.transform.childCount; index++)
                    {
                        float lab_x = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.x;
                        float lab_y = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.y - changepos;
                        float lab_z = AllLabel.transform.GetChild(index).gameObject.transform.localPosition.z;
                        AllLabel.transform.GetChild(index).gameObject.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                }
                else if (AllchangeLabel == false)
                {
                    if (SelectLabel != null)
                    {
                        float lab_x = SelectLabel.transform.localPosition.x;
                        float lab_y = SelectLabel.transform.localPosition.y - changepos;
                        float lab_z = SelectLabel.transform.localPosition.z;
                        SelectLabel.transform.localPosition = new Vector3(lab_x, lab_y, lab_z);
                    }
                }
                break;
            case "PlusScale":
                if (SelectLabel != Fish)
                {
                    float changeplus = SelectLabel.transform.localScale.x + changepos / 100;
                    SelectLabel.transform.localScale = new Vector3(changeplus, changeplus, changeplus);
                } else if(SelectLabel == Fish)
                {
                    float changeplus = SelectLabel.transform.GetChild(0).transform.localScale.x + changepos / 100;
                    SelectLabel.transform.GetChild(0).transform.localScale = new Vector3(changeplus, changeplus, changeplus);
                }
                break;
            case "MinusScale":
                if (SelectLabel != Fish)
                {
                    float changeminus = SelectLabel.transform.localScale.x - changepos / 100;
                    SelectLabel.transform.localScale = new Vector3(changeminus, changeminus, changeminus);
                } else if(SelectLabel == Fish)
                {
                    float changeminus = SelectLabel.transform.GetChild(0).transform.localScale.x - changepos / 100;
                    SelectLabel.transform.GetChild(0).transform.localScale = new Vector3(changeminus, changeminus, changeminus);
                }
                break;
        }
    }

    public void ConfirmPantiltRange(GameObject btn)
    {
        switch (btn.name)
        {
            case "PantiltSetting":
                MinPan = 0;
                MaxPan = 23600;
                MinTilt = 0;
                MaxTilt = 1500;
                for (int index = 0; index < 6; index++)
                {
                    btn.transform.parent.gameObject.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = btn.name;
                }
                break;
            case "MinPan":
                MinPan = Current_x;
                btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = Current_x.ToString();
                Debug.Log("MinPan" + MinPan);
                break;
            case "MaxPan":
                MaxPan = Current_x;
                btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = Current_x.ToString();
                Debug.Log("MaxPan" + MaxPan);
                break;
            case "MinTilt":
                MinTilt = Current_y;
                btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = Current_y.ToString();
                Debug.Log("MinTilt" + MinTilt);
                break;
            case "MaxTilt":
                MaxTilt = Current_y;
                btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = Current_y.ToString();
                Debug.Log("MaxTilt" + MaxTilt);
                break;
            case "Start_X":
                StartPosition_x = Current_x;
                btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = Current_x.ToString();
                break;
            case "Start_y":
                StartPosition_y = Current_y;
                btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = Current_y.ToString();
                break;
        }
        //PTRange.GetComponent<Text>().text = "(" + MinPan + "~" + MaxPan + ", " + MinTilt + "~" + MaxTilt + ")";
    }

    public void SetFinish()
    {
        if (AllchangeLabel == false)
        {
            if (SelectLabel != null)
            {
                for(int index = 0; index<AllLabel.transform.childCount; index++)
                {
                    if(AllLabel.transform.GetChild(index).gameObject.name == SelectLabel.name)
                    {
                        AllLabel.transform.GetChild(index).gameObject.transform.localPosition = SelectLabel.transform.localPosition;
                    }
                }
                SelectLabel_btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = "LabelName";
                SelectLabel = null;
            }
        } else if(AllchangeLabel == true)
        {
            SelectLabel = null;
            SelectLabel_btn.transform.GetChild(0).gameObject.GetComponent<Text>().text = "LabelName";
            AllchangeLabel = false;
        }
    }

    public void SetValue(GameObject btn)
    {
        switch (btn.name)
        {
            case "ValueSetting":
                TotalPan = 1;
                TotalTilt = 1;
                System.Diagnostics.Process ps = new System.Diagnostics.Process();
                ps.StartInfo.FileName = "osk.exe";
                ps.Start();
                break;
            case "Move":
                float pulseX = int.Parse(SetValueX.text);
                float pulseY = int.Parse(SetValueY.text);

                PanTiltControl.SetPulse((uint)pulseX, (uint)pulseY);
                break;
            case "MinPan":
                SetValueX_min = float.Parse(PanTiltControl.NowPanPulse.ToString());
                btn.transform.GetChild(0).GetComponent<Text>().text = PanTiltControl.NowPanPulse.ToString();

                if(btn.transform.parent.gameObject.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text != "MaxPan")
                {
                    Totalpan_Text.GetComponent<Text>().text = Math.Round((1920 / Mathf.Abs(SetValueX_max - SetValueX_min)),3).ToString();
                }
                break;
            case "MaxPan":
                SetValueX_max = float.Parse(PanTiltControl.NowPanPulse.ToString());
                btn.transform.GetChild(0).GetComponent<Text>().text = PanTiltControl.NowPanPulse.ToString();

                if (btn.transform.parent.gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text != "MinPan")
                {
                    Totalpan_Text.GetComponent<Text>().text = Math.Round((1920 / Mathf.Abs(SetValueX_max - SetValueX_min)),3).ToString();
                }
                break;
            case "MinTilt":
                SetValueY_min = float.Parse(PanTiltControl.NowTiltPulse.ToString());
                btn.transform.GetChild(0).GetComponent<Text>().text = PanTiltControl.NowTiltPulse.ToString();

                if (btn.transform.parent.gameObject.transform.GetChild(7).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text != "MaxTilt")
                {
                    Totaltilt_Text.GetComponent<Text>().text = Math.Round((1080 / Mathf.Abs(SetValueY_max - SetValueY_min)), 3).ToString();
                }
                break;
            case "MaxTilt":
                SetValueY_max = float.Parse(PanTiltControl.NowTiltPulse.ToString());
                btn.transform.GetChild(0).GetComponent<Text>().text = PanTiltControl.NowTiltPulse.ToString();

                if (btn.transform.parent.gameObject.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text != "MinTilt")
                {
                    Totaltilt_Text.GetComponent<Text>().text = Math.Round((1080 / Mathf.Abs(SetValueY_max - SetValueY_min)),3).ToString();
                }
                break;
            case "Finish":
                TotalPan = (float)Math.Round((1920 / Mathf.Abs(SetValueX_max - SetValueX_min)), 3);
                TotalTilt = (float)Math.Round((1080 / Mathf.Abs(SetValueY_max - SetValueY_min)), 3);

                btn.transform.parent.gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "MinPan";
                btn.transform.parent.gameObject.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "MaxPan";
                btn.transform.parent.gameObject.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "MinTilt";
                btn.transform.parent.gameObject.transform.GetChild(7).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "MaxTilt";
                break;
        }
    }
}
