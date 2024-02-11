using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public TMP_Text coin_Text;                         //코인 개수 UI(텍스트)
    public TMP_Text life_Text;                         //생명 개수 UI(텍스트)

    public Image lifeImage;

    private float fadeScale = 0f;
    private float curTime = 0f;
    private float coinSetTime = 0.1f;
    private readonly float fadeSpeed = 3f;
    private readonly float fadeScaleMax = 10f;
    private readonly float fadeScaleMiddle = 2f;
    private readonly float fadeScaleMin = 0.01f;
    private readonly float  lifeFillSpeed = 0.3f;
    private readonly float lifeColorChangeSpeed = 0.005f;
    private readonly float fadeMiddleTime = 2f;

    private int life = 3;
    private int coin = 0;
    private bool isLifeBarFilling = false;
    private bool isLifeColorChange = false;
    private bool isFading = false;
    private bool isCoinReseting = false;

    // 페이드할 때 사용할 변수들
    private bool isFadeInOut = false;
    private int fadeInOutLevel = -1;

    private Animator lifeUIAnim;

    public GameObject coinUI;
    public GameObject endingUI;
    public GameObject menuUI;
    public GameObject coinResetUI;
    public GameObject blackScreen;
    public GameObject endingCreditUI;
    public RectTransform fadeUI;
    public RectTransform lifeUI;
    public TMP_Text coinResetText;



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lifeUIAnim = lifeUI.GetComponent<Animator>();
        SetCoinText(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLifeBarFilling)
        {
            LifeImageFill();
        }

        if (isLifeColorChange)
        {
            LifeImageColorChange();
        }

        if (isFading)
        {
            FadeInOut();
        }

        //if (isCoinReseting)
        //{
        //    CoinReset();
        //}

        //LifeUIMove();
    }

    // 코인 개수를 나타내는 텍스트 바꿔주기
    public void SetCoinText(int coinCount)
    {
        coin_Text.text = coinCount.ToString();
    }

    // 생명을 늘리거나 줄임
    public void LifePlusMinus(int lifeChange)
    {
        life_Text.text = GameManager.instance.LIFE.ToString();
        if(life > GameManager.instance.LIFE)
        {
            lifeUIAnim.SetBool("isMinus", true);
        }

        isLifeBarFilling = true;
        isLifeColorChange = true;
    }

    // 생명 이미지 색 변경
    public void LifeImageColorChange()
    {
        // 생명 UI(이미지, 텍스트) 변경
        switch (GameManager.instance.LIFE)
        {
            case 3:
                lifeImage.color = Color.Lerp(lifeImage.color, Color.green, lifeColorChangeSpeed);
                if (lifeImage.color == Color.green)
                {
                    isLifeColorChange = false;
                }
                break;
            case 2:
                lifeImage.color = Color.Lerp(lifeImage.color, Color.yellow, lifeColorChangeSpeed);
                if (lifeImage.color == Color.yellow)
                {
                    isLifeColorChange = false;
                }
                break;
            case 1:
                lifeImage.color = Color.Lerp(lifeImage.color, Color.red, lifeColorChangeSpeed);
                if (lifeImage.color == Color.red)
                {
                    isLifeColorChange = false;
                }
                break;
        }
    }

    //생명 줄어들기 늘어나기
    public void LifeImageFill()
    {
        switch (GameManager.instance.LIFE)
        {
            case 3:
                if(lifeImage.fillAmount < 1)
                {
                    lifeImage.fillAmount += lifeFillSpeed * Time.deltaTime;
                }
                else
                {
                    isLifeBarFilling = false;
                }
                break;

            case 2:
                if(life < 2)
                {
                    if(lifeImage.fillAmount < 0.66f)
                    {
                        lifeImage.fillAmount += lifeFillSpeed * Time.deltaTime;
                    }
                    else
                    {
                        isLifeBarFilling = false;
                    }
                }
                else if(life > 2)
                {
                    if(lifeImage.fillAmount > 0.66f)
                    {
                        lifeImage.fillAmount -= lifeFillSpeed * Time.deltaTime;
                    }
                    else
                    {
                        isLifeBarFilling = false;
                    }
                }
                break;

            case 1:
                if(lifeImage.fillAmount > 0.33f)
                {
                    lifeImage.fillAmount -= lifeFillSpeed * Time.deltaTime;
                }
                else
                {
                    isLifeBarFilling = false;
                }
                break;
        }
    }

    public void EndingUI()
    {
        coinUI.SetActive(false);
        lifeUI.gameObject.SetActive(false);
        endingUI.SetActive(true);
    }

    public void Fade(bool isFadeOut )
    {
        isFadeInOut = isFadeOut;
        fadeScale = fadeUI.localScale.x;
        curTime = 0f;
        isFading = true;
    }
     public void EndingCredit()
    {
        GameManager.instance.ISENDING = true;
        //Time.timeScale = 1;
        //endingUI.SetActive(false);
        endingCreditUI.SetActive(true);
    }

    //public void CoinReset(int coinCount)
    //{
    //    if (!isCoinReseting)
    //    {
    //        coin = coinCount;
    //    }

    //    isCoinReseting = true;
    //    coinResetText.text = coin.ToString();
    //    curTime += Time.deltaTime;
    //    if (curTime >= coinSetTime)
    //    {
    //        coin--;
    //        if(coin < 0)
    //        {
    //            isCoinReseting = false;
    //            return;
    //        }
    //        coinResetText.text = coin.ToString();
    //        curTime = 0f;
    //    }
    //}

    private void FadeInOut()
    {
        if (isFadeInOut)
        {
            if (fadeScale <= fadeScaleMiddle)
            {
                curTime += Time.deltaTime;
                if (curTime >= fadeMiddleTime)
                {
                    fadeScale -= Time.deltaTime * fadeSpeed;
                }
            }
            else
            {
                fadeScale -= Time.deltaTime * fadeSpeed;
            }

            if (fadeScale <= fadeScaleMin)
            {
                curTime = 0f;
                blackScreen.SetActive(true);
                GameManager.instance.Respawn();
                isFadeInOut = false;
            }
        }
        else
        {
            curTime += Time.deltaTime;
            if (curTime >= fadeMiddleTime)
            {
                blackScreen.SetActive(false);
                fadeScale += Time.deltaTime * fadeSpeed;
                if (fadeScale >= fadeScaleMax)
                {
                    isFading = false;
                }
            }
        }

        fadeUI.transform.localScale = new Vector3(fadeScale, fadeScale, fadeScale);
    }

    public void SetActiveMenuUI(bool isActive)
    {
        if (isActive)
        {
            Time.timeScale = 0;
            menuUI.SetActive(true);
            lifeUI.gameObject.SetActive(false);
            coinUI.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            menuUI.SetActive(false);
            lifeUI.gameObject.SetActive(true);
            coinUI.SetActive(true);
        }
    }
}
