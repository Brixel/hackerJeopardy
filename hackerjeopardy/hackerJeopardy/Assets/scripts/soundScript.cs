using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundScript : MonoBehaviour
{
    public AudioClip musicSound;
    public AudioClip whooshSound;
    public AudioClip dingSound;
    public AudioClip beepSound;
    public AudioClip tadaSound;
    public AudioClip downSlideSound;
    public AudioClip loadedSound;
    public AudioClip answerButtonSound;
    public AudioClip answerWrongSound;
    public AudioClip answerRightSound;
    public AudioClip boardLoadSound;
    public AudioClip timeoutSound;






    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void playSound(AudioClip theSound)
    {
        transform.GetComponent<AudioSource>().clip = theSound;
        transform.GetComponent<AudioSource>().Play();
    }

    public void playWhoosh()
    {
        playSound(whooshSound);
    }
    public void playDing()
    {
        playSound(dingSound);
    }
    public void playBeep()
    {
        playSound(beepSound);
    }
    public void playTada()
    {
        playSound(tadaSound);
    }     
    public void playDownSlide()
    {
        playSound(downSlideSound); 
    }
    public void playLoaded()
    {
        playSound(loadedSound);
    }
    public void playMusic()
    {
        GameObject.Find("musicPlayer").GetComponent<AudioSource>().clip = musicSound;
        GameObject.Find("musicPlayer").GetComponent<AudioSource>().Play();
    }
    public void stopMusic()
    {
        GameObject.Find("musicPlayer").GetComponent<AudioSource>().Stop();
    }
    public void playAnswerButton()
    {
        playSound(answerButtonSound);
    }
    public void playAnswerWrong()
    {
        playSound(answerWrongSound);
    }
    public void playAnswerRight()
    {
        playSound(answerRightSound);
    }
    public void playBoardLoad()
    {
        playSound(boardLoadSound);
    }
    public void playTimeOut()
    {
        playSound(timeoutSound);
    }
}
