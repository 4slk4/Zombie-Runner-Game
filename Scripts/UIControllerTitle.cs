using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerTitle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    public void quit()
    {
        Application.Quit();
    }
}
