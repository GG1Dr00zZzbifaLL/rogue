using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public Sprite SoundOnn;
    public Sprite SoundOfff;
    public Image MusicButton;
    public bool isOn;
    public AudioSource audioSource;

    private Player player;

    private void Start()
    {
        isOn = true;
    }

    private void Update()
    {
        //включение или выключение музыки
        if (PlayerPrefs.GetInt("music") == 0)
        {
            MusicButton.GetComponent<Image>().sprite = SoundOnn;
            audioSource.enabled = true;
            isOn = true;
        }
        else if (PlayerPrefs.GetInt("music") == 1)
        {
            MusicButton.GetComponent<Image>().sprite = SoundOfff;
            audioSource.enabled = false;
            isOn = false;
        }
        PlayerPrefs.Save();
    }

    public void SoundOff() 
    {
        if (!isOn)
        {
            PlayerPrefs.SetInt("music", 0);
        }
        else if (isOn)
        {
            PlayerPrefs.SetInt("music", 1);
        }
    }
}