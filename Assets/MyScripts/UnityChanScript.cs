using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityChanScript : MonoBehaviour
{
    public GameObject SpeechBubble;
    public Text speechbubble_Text;

    private float ElapseTime;
    private float deleteBubbleTime = 9.0f;
    private float makeBubbleTime = 13.0f;


    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ElapseTime += Time.deltaTime;
        if(ElapseTime >= makeBubbleTime)
        {
            SpeechBubble.SetActive(true);

            ElapseTime = 0.0f;
        }else if(ElapseTime >= deleteBubbleTime)
        {
            SpeechBubble.SetActive(false);
        }
    }

    public void uc_jump()
    {
        _animator.SetTrigger("solute");

        SpeechBubble.SetActive(true);
        speechbubble_Text.text = "power UP!!";
    }

    public void uc_smile()
    {
        _animator.CrossFade("smile@sd_hmd", 0);
        SpeechBubble.SetActive(false);
    }

    public void uc_sad()
    {
        _animator.CrossFade("sad@sd_hmd", 0);
        SpeechBubble.SetActive(false);
    }

    public void rank_coment(int rank_now)
    {
        SpeechBubble.SetActive(true);
        speechbubble_Text.text = "Rank " + (rank_now - 1).ToString() + "⇨" + (rank_now).ToString(); 
    }

    public void coin_coment(int getcoin)
    {
        SpeechBubble.SetActive(true);
        speechbubble_Text.text = (getcoin).ToString() + "コイン\nゲット";
    }
}
