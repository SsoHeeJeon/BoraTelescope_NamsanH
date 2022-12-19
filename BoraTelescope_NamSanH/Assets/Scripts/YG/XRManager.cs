using Amazon.Rekognition.Model;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class XRManager : MonoBehaviour
{
    public class XR
    {
        public string LabelName;
        public int Label_X;
        public int Label_Y;
        public int LabelScale;
    }

    public class XRPan
    {
        public float Max_Pan;
        public float Min_Pan;
        public float Max_Tilt;
        public float Min_Tilt;
    }


    public string[] XRarr;
    public GameObject[] XRobject;
    // Start is called before the first frame update

    void Start()
    {
        string XRjson = File.ReadAllText(Application.dataPath + "/XRModeLabelPosition.json");
        if (XRjson.Contains("}"))
        {
            XRarr = XRjson.Split('}');

            for (int index = -1; index < XRarr.Length - 1; index++)
            {
                if (index < XRarr.Length - 2)
                {
                    XRarr[index + 1] = XRarr[index + 1] + "}";
                }
            }
        }

        for(int i=0; i<XRarr.Length; i++)
        {
            try
            {
                XR myxr = JsonUtility.FromJson<XR>(XRarr[i].ToString());

                XRobject[i].transform.position = new Vector3(myxr.Label_X * XRMode.ValueX, myxr.Label_Y * XRMode.ValueY, 500);



                //XRobject[i].transform.position = new Vector3(myxr.Label_X, myxr.Label_Y, 500);
                XRobject[i].transform.localScale = new Vector3(myxr.LabelScale, myxr.LabelScale, myxr.LabelScale);


                if(i==3)
                {
                    XRobject[i].GetComponent<Crane>().speed = myxr.LabelScale * 0.5f;
                    XRobject[i].GetComponent<Crane>().StartPos = XRobject[i].transform.position;
                }
                else if(i==0)
                {
                    XRobject[0].GetComponent<Sparrow>().Pos1 = XRobject[0].transform.position + new Vector3(5 * XRobject[0].transform.localScale.x, 5 * XRobject[0].transform.localScale.y, 5 * XRobject[0].transform.localScale.z);
                    XRobject[0].GetComponent<Sparrow>().Pos2 = XRobject[0].transform.position;
                }
            }
            catch
            {
                try
                {
                    XRPan myxr = JsonUtility.FromJson<XRPan>(XRarr[i].ToString());
                    XRobject[3].GetComponent<Crane>().Max_Pan = myxr.Max_Pan;
                    XRobject[3].GetComponent<Crane>().Min_Pan = myxr.Min_Pan;
                }
                catch
                { }

            }
        }

        XR koixr = JsonUtility.FromJson<XR>(XRarr[2].ToString());
        for (int i=0; i < XRobject[2].transform.GetChild(0).GetComponentInChildren<KoiManager>().Koiarr.Length; i++)
        {
            XRobject[2].transform.GetChild(0).GetComponentInChildren<KoiManager>().Koiarr[i].GetComponent<Koi>().speed = koixr.LabelScale * 0.5f;
            XRobject[2].transform.GetChild(0).GetComponentInChildren<KoiManager>().Koiarr[i].GetComponent<Koi>().defaultspeed = koixr.LabelScale * 0.5f;
            XRobject[2].transform.GetChild(0).GetComponentInChildren<KoiManager>().Koiarr[i].GetComponent<Koi>().StartPos = XRobject[2].transform.GetChild(0).GetComponentInChildren<KoiManager>().Koiarr[i].transform.position;
        }
    }
}
