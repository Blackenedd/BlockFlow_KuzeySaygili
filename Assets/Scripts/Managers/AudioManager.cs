using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HULTemplate;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public AudioSource click;
    public AudioSource coin;
    public AudioSource splash;
    public AudioSource fail;
    public List<AudioSource> whoosh;
    public AudioSource box;
    public AudioSource success;
    public AudioSource failClick;

    public static AudioManager instance;

    private int whooshCounter = 0;
    
    private void Awake()
    {
        instance = this;
    }
    public void PlayBox()
    {
        if (DataManager.instance.sound) box?.Play();
        if (DataManager.instance.vibration) Haptic.LightTaptic();
    }
    public void PlayWhoosh()
    {
        if (DataManager.instance.sound) whoosh[whooshCounter % whoosh.Count].Play(); whooshCounter++;
        if (DataManager.instance.vibration) Haptic.LightTaptic();
    }
    public void PlaySuccess()
    {
        if (DataManager.instance.sound) success?.Play();
        if (DataManager.instance.vibration) Haptic.NotificationSuccessTaptic();
    }
    public void PlayFailClick()
    {
        if (DataManager.instance.sound) failClick?.Play();
        if (DataManager.instance.vibration) Haptic.NotificationSuccessTaptic();
    }
    public void PlayFail()
    {
        if (DataManager.instance.sound) fail?.Play();
        if (DataManager.instance.vibration) Haptic.NotificationErrorTaptic();
    }
    public void PlaySplash()
    {
        if (DataManager.instance.sound) splash?.Play();
        if (DataManager.instance.vibration) Haptic.MediumTaptic();
    }
    public void PlayCoin() 
    {
        if (DataManager.instance.sound) coin?.Play();
        if (DataManager.instance.vibration) Haptic.MediumTaptic();
    }

    public void PlayClick()
    {
        if (DataManager.instance.sound) click?.Play();
        if (DataManager.instance.vibration) Haptic.MediumTaptic();
    }

}