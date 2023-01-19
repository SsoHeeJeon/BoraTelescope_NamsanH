using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Language : MonoBehaviour
{
    public GameObject thisObject;
    private Image objImg;
    public Sprite[] Lang_s;

    // Start is called before the first frame update
    void Start()
    {
        objImg = thisObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeLanguage();
    }

    public void ChangeLanguage()
    {
        switch (GameManager.currentLang)
        {
            case GameManager.Language_enum.Korea:
                objImg.sprite = Lang_s[0];
                break;
            case GameManager.Language_enum.English:
                objImg.sprite = Lang_s[1];
                break;
            case GameManager.Language_enum.Chinese:
                objImg.sprite = Lang_s[2];
                break;
            case GameManager.Language_enum.Japan:
                objImg.sprite = Lang_s[3];
                break;
        }

        if(!objImg.gameObject.name.Contains("(none)"))
        {
            objImg.SetNativeSize();
        }
    }
}
