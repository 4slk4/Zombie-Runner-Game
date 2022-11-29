using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeIcon : MonoBehaviour
{
    public Sprite noSoundButton;
    public Sprite yesSoundButton;
    public Button soundButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeSoundButton()
    {
        if (soundButton.image.sprite == yesSoundButton)
        {
            soundButton.image.sprite = noSoundButton;
            return;
        }
        else soundButton.image.sprite = yesSoundButton;
    }
}
