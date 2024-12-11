using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BGM : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioSource SirenSource;

    private bool copEnabled;

    public bool CopEnabled
    {
        get { return copEnabled; }
        set { 
            
                copEnabled = value;
                if(value)
                {
                    
                    audioSource.Stop();
                    SirenSource = GameObject.Find("Cop Car(Clone)").GetComponent<AudioSource>();
                    var _audioClipArray = SirenSource.GetComponent<CopController>().audioClipArray;
                    SirenSource.PlayOneShot(_audioClipArray[UnityEngine.Random.Range(0, _audioClipArray.Length - 1)]);
                }
                else
                {
                    try
                    {
                        SirenSource.Stop();
                    }
                    catch(Exception e)
                    {
                        Debug.Log(e);
                    }
                    audioSource.Play();
                }
            
            }
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();    
    }

}
