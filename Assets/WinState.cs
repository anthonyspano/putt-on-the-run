using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinState : MonoBehaviour
{
    public Text winStateText;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("cup"))
        {
            Debug.Log("gimme");
            if(other.gameObject.transform.parent.parent.name == "Hole6")
            {
                winStateText.gameObject.SetActive(true);
                Debug.Log("restarting");
                StartCoroutine(RestartGame());
            }
        }
    }

    IEnumerator RestartGame()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
