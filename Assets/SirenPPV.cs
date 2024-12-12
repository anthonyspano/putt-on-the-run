using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SirenPPV : MonoBehaviour
{
   PostProcessVolume m_Volume;
   ColorGrading m_ColorGrading;
   Vector4 redColor = new Vector4(0.79f, -0.21f, -0.15f, -1);
   Vector4 blueColor = new Vector4(-0.12f, -0.05f, 0.88f, -1);

   bool blue;

   private BGM _BGM;

    void Start()
    {
        m_Volume = GetComponent<PostProcessVolume>();

        // Create or use an existing profile
        if (m_Volume.sharedProfile == null)
        {
            m_Volume.sharedProfile = ScriptableObject.CreateInstance<PostProcessProfile>();
        }

        // Add ColorGrading if it's not already in the profile
        if (!m_Volume.sharedProfile.TryGetSettings(out m_ColorGrading))
        {
            m_ColorGrading = m_Volume.sharedProfile.AddSettings<ColorGrading>();
        }

        m_ColorGrading.enabled.Override(true);
        m_ColorGrading.lift.Override(redColor);
        //m_Volume.weight = .4f;

        // to see if cop car is in the scene
        _BGM = GameObject.FindWithTag("Background").GetComponent<BGM>();
        Debug.Log(_BGM);

        StartCoroutine(Siren());
    }

    void Update()
    {
        if(_BGM.CopEnabled)
            m_Volume.weight = .3f;
        else
            m_Volume.weight = 0f;
    }

    IEnumerator Siren()
    {
        while(true)
        {
            if(blue)
                m_ColorGrading.lift.Override(blueColor);
            else
                m_ColorGrading.lift.Override(redColor);
            //m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_ColorGrading);
            blue = !blue;  
            yield return new WaitForSeconds(0.8f);

            //Debug.Log("Current Lift: " + m_ColorGrading.lift.value);

        }
    }
}
