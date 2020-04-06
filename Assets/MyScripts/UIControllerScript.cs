using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Mode
{
    coin,
    exp
}
public enum Difficulty
{
    easy,
    normal,
    hard
}

public class UIControllerScript : MonoBehaviour
{
    //ボタン系
    public GameObject DangeonSelect_Panel;
    public GameObject CoinSelect_Panel;
    public GameObject ExpSelect_Panel;


    //コインか経験値か
    
    private Mode mode;

    //難易度
    
    private Difficulty difficulty;
    string CorE;

    //スタミナ系
    public Slider staminaGauge;
    private int stamina_max = 10;
    private int stamina_now = 10;
    public Text stamina_text;
    public GameObject nostamina_message;
    private float staminaRecoveryTime;
    public Text recoveryTime_text;

    //経験値系
    public Slider expGauge;
    private int exp_toNext = 10;
    private int exp_now = 0;
    public Text exp_text;
    private int nowRank = 1;
    public Text Rank_text;

    //コイン系
    public Text coin_text;
    private int coin_number = 1000;
    public GameObject nocoin_message;

    //強化系
    public GameObject powerup_Panel;

    //カットイン
    public Image cutin_image;
    public GameObject success_text;


    //unitychan
    private UnityChanScript ucs;
    public Text uc_level_text;
    private int uc_level_now = 1;

    

    

    // Start is called before the first frame update
    void Start()
    {
        //staminaGauge = GameObject.Find("Stamina_gauge").GetComponent<Slider>();
        //expGauge = GameObject.Find("Exp_gauge").GetComponent<Slider>();
        ucs = GameObject.Find("SD_unitychan_humanoid").GetComponent<UnityChanScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //スタミナ回復
        if (stamina_now  < stamina_max)
        {
            staminaRecoveryTime += Time.deltaTime;
            recoveryTime_text.text = (5.0f - staminaRecoveryTime).ToString("F0") + " s";
            if (staminaRecoveryTime >= 5.0f)
            {
                stamina_now += 1;
                staminaGauge.value = stamina_now;
                stamina_text.text = stamina_now + " / " + stamina_max;
                staminaRecoveryTime = 0.0f;
            }
        }
        else {
            recoveryTime_text.text = "";
            staminaRecoveryTime = 0.0f;
        }
        
        Debug.Log(stamina_now);
    }


    //タブを閉じる
    public void push_CloseButton()
    {
        StartCoroutine(setfalse(DangeonSelect_Panel, 0.0f));
        StartCoroutine(setfalse(CoinSelect_Panel, 0.0f));
        StartCoroutine(setfalse(ExpSelect_Panel, 0.0f));
        StartCoroutine(setfalse(powerup_Panel, 0.0f));

        //DangeonSelect_Panel.SetActive(false);
        //CoinSelect_Panel.SetActive(false);
        //ExpSelect_Panel.SetActive(false);
        //powerup_Panel.SetActive(false);
    }


    //ダンジョンボタン
    public void push_DangeonButton()
    {
        DangeonSelect_Panel.SetActive(true);
    }

    //コインか経験値を選択
    public void push_CoinOrExpButton(string which)
    {
        StartCoroutine(setfalse(DangeonSelect_Panel, 0.0f));
        CorE = which;
        if (CorE == "coin")
        {
            CoinSelect_Panel.SetActive(true);
        }
        else if (CorE == "exp")
        {
            ExpSelect_Panel.SetActive(true);
        }
    }

    
    //難易度選択
    public void push_DifficultyButton(int difficulty)
    {
       if(CorE == "coin")
        {
            Coin_reaction(difficulty);
            CoinSelect_Panel.SetActive(false);
        }else if(CorE == "exp")
        {
            Exp_reaction(difficulty);
            ExpSelect_Panel.SetActive(false);
        }
    }

    //経験値の処理
    private void Exp_reaction(int difficulty)
    {
        switch (difficulty)
        {
            case 1:
                if (check_stamina(5) == true)
                {
                    check_exp(10);
                }
                break;
            case 2:
                if (check_stamina(15) == true)
                {
                    check_exp(20);
                }
                break;
            case 3:
                if (check_stamina(25) == true)
                {
                    check_exp(30);
                }
                break;
            default:
                Debug.Log("default");
                break;
        }


       

    }

    //コインの処理
    private void Coin_reaction(int difficulty)
    {
        switch (difficulty)
        {
            case 1:
                if (check_stamina(5) == true)
                {
                    coin_number += 100;
                    StartCoroutine("increase_cutinPanel");
                    ucs.coin_coment(100);
                }
                break;
            case 2:
                if (check_stamina(15) == true)
                {
                    coin_number += 500;
                    StartCoroutine("increase_cutinPanel");
                    ucs.coin_coment(500);
                }
                break;
            case 3:
                if (check_stamina(25) == true)
                {
                    coin_number += 1000;
                    StartCoroutine("increase_cutinPanel");
                    ucs.coin_coment(1000);
                }
                break;
            default:
                Debug.Log("default");
                break;
        }

        
        coin_text.text = ": " + coin_number;
    }


    //スタミナが足りているかチェック
    private bool check_stamina(int num)
    {
        if(stamina_now - num < 0)
        {
            StartCoroutine(settrue(nostamina_message, 0.0f));
            StartCoroutine(setfalse(nostamina_message,3.0f));
            ucs.uc_sad();
            return false;
        }
        else
        {
            stamina_now -= num;
            stamina_text.text = stamina_now + " / " + stamina_max;
            staminaGauge.value = stamina_now;
            return true;
        }
    }

    

    //ランクが上がるかチェック
    private void check_exp(int num)
    {
        exp_now += num;
        if(exp_now >= exp_toNext)
        {
            Debug.Log("up");
            nowRank += 1;
            Rank_text.text = (nowRank).ToString();

            //経験値UI
            exp_now -= exp_toNext;
            exp_toNext += 10;
            expGauge.maxValue = exp_toNext;
            expGauge.value = 0;
            num = 0;

            //スタミナUI
            stamina_max += 3;
            stamina_now = stamina_max;
            staminaGauge.maxValue = stamina_max;
            staminaGauge.value = stamina_max;
            stamina_text.text = stamina_now + " / " + stamina_max;

            ucs.uc_smile();
            ucs.rank_coment(nowRank);
        }

        exp_text.text = exp_now + " / " + exp_toNext;
        
        StartCoroutine("increase_expgauge", num);
        StartCoroutine("increase_cutinPanel");
        
    }


    //expゲージを滑らかに増やす
    IEnumerator increase_expgauge(int exp_now)
    {
       

        for(int i = 0; i < exp_now; i++)
        {
            expGauge.value += 1;
            yield return null;
        }
    }


    //クリアのカットイン
    IEnumerator increase_cutinPanel()
    {
        cutin_image.fillOrigin = (int)Image.OriginHorizontal.Right;
        for (int i = 0; i <= 20; i++)
        {
            cutin_image.fillAmount += 0.05f;
            yield return null;
        }
        success_text.SetActive(true);

        yield return new WaitForSeconds(1.1f);

        success_text.SetActive(false);
        cutin_image.fillOrigin = (int)Image.OriginHorizontal.Left;
        for (int i = 0; i <= 20; i++)
        {
            cutin_image.fillAmount -= 0.05f;
            yield return null;
        }
        
    }

    //強化
    public void push_powerupButtom()
    {
        powerup_Panel.SetActive(true);
    }

    public void push_puOKButtom()
    {
        powerup_Panel.SetActive(false);

        if (coin_number < 1500)
        {
            StartCoroutine(settrue(nocoin_message, 0.0f));
            StartCoroutine(setfalse(nocoin_message,3.0f));
            ucs.uc_sad();
        }
        else
        {
            coin_number -= 1500;
            coin_text.text = ": " + coin_number;

            ucs.uc_jump();

            uc_level_now += 1;
            uc_level_text.text = "Lv. " + (uc_level_now).ToString();
        }
    }

    IEnumerator settrue(GameObject go, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        go.SetActive(true);
        Debug.Log("settrue");
    }

    IEnumerator setfalse(GameObject go,float waitTime)
    {
        //go.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        go.SetActive(false);
        Debug.Log("setfalse");
    }
}


