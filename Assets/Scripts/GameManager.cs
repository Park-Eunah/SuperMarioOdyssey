using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private float curTime = 0f;
    private float coinTime = 0.5f;                     //코인 활성화 주기

    private int life = 3;                              //생명 개수
    private int coinCount = 0;                         //획득한 코인 개수

    public GameObject[] getCoins;                      //코인을 얻었을 때 나타날 코인

    private int getCoinIndex = 0;

    private bool isStart = false;
    private bool isEnding = false;
    private bool isCoin = false;
    private int getCoinCount = 0;                      //한번에 여러개의 코인을 얻을 때 현재까지 계산된 코인의 수

    public PlayerMove playerMove;
    public GameObject startUI;
    public GameObject cappyStart;
    public AudioSource audioSource;
    public AudioClip startBGM;
    public AudioClip mainBGM;
    public AudioClip endingBGM;

    //private bool isAlive = true;                     //게임중인지 확인

    public bool ISSTART
    {
        get
        {
            return isStart;
        }
        set
        {
            isStart = value;
            if(!isStart)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public bool ISENDING
    {
        get
        {
            return isEnding;
        }
        set
        {
            isEnding = value;
            if (isEnding)
            {
                //audioSource.clip = endingBGM;

                //audioSource.Play();
                //audioSource.volume = 1;
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this);        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (!isStart)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                StartGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.instance.SetActiveMenuUI(true);
        }
    }

    public int LIFE
    {
        get { return life; }
    }

    //생명 증가 또는 감소
    //isPlus가 true이면 생명이 늘어나고, false이면 줄어듦(생명 최대 3)
    public void LifeCount(bool isPlus)
    {
        if (isPlus)
        {
            //생명의 최대 개수는 3임.
            if(life < 3)
            {
                life++;
            }
        }
        else
        {
            life--;
            playerMove.isLifedown = true;
            if(life <= 0)
            {
                playerMove.Die();
                Die();
            }
        }

        if(LIFE != 0)
            UIManager.instance.LifePlusMinus(LIFE);
    }

    private void StartGame()
    {
        startUI.SetActive(false);
        cappyStart.SetActive(true);
        audioSource.clip = mainBGM;
        audioSource.Play();
    }

    //코인 증가
    public void CoinPlus()
    {
        coinCount++;

        // 코인 UI(텍스트) 변경  
        UIManager.instance.SetCoinText(coinCount);
    }

    // 죽으면 코인 10개 감소
    public void Die()
    {
        UIManager.instance.Fade(true);

        // 코인, 생명 UI 비활성화 
        UIManager.instance.coinUI.SetActive(false);
        UIManager.instance.lifeUI.gameObject.SetActive(false);
    }

    public void Respawn()
    {
        playerMove.ReStart();

        // 코인, 생명 재정비
        coinCount = 0;
        life = 3;

        UIManager.instance.SetCoinText(coinCount);
        UIManager.instance.LifePlusMinus(LIFE);

        UIManager.instance.coinUI.SetActive(true);
        UIManager.instance.lifeUI.gameObject.SetActive(true);

    }

    public void ActiveGetCoin(Transform tr, int count)
    {
        if (!getCoins[getCoinIndex].activeSelf)
        {
            getCoinCount++;
            CoinPlus();
            getCoins[getCoinIndex].transform.position = tr.position;
            getCoins[getCoinIndex].SetActive(true);
            getCoinIndex++;
            if (getCoinIndex >= getCoins.Length)
            {
                getCoinIndex = 0;
            }
        }
    }

    public void Continue()
    {
        UIManager.instance.SetActiveMenuUI(false);
        print("continue");
    }

    public void Quit()
    {
        Application.Quit();
    }

    //public void RestartGame()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    IEnumerator GetCoins(Transform tr, int count)
    {
        if (!getCoins[getCoinIndex].activeSelf)
        {
            getCoinCount++;
            CoinPlus();
            getCoins[getCoinIndex].transform.position = tr.position;
            getCoins[getCoinIndex].SetActive(true);
            getCoinIndex++;
            if (getCoinIndex >= getCoins.Length)
            {
                getCoinIndex = 0;
            }
            if (getCoinCount == count)
            {
                StopCoroutine(GetCoins(tr, count));
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
