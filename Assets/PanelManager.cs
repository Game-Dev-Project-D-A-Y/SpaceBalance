using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] Slider audioSlider;
    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if(audioSlider != null) {
            audioSlider.minValue = 0;
            audioSlider.maxValue = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = audioSlider.value;
    }

    public void OnSettingsClicked()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }
    public void OnSettingsExit()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

}
