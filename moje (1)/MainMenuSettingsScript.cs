using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MainMenuSettingsScript : MonoBehaviour
{
    public List<MenuWindow> menuWindows;
    Resolution[] resolutions;
    public Text FPScounter;

    //Graphic Settings
    public RightLeftPanelScript preset;
    public RightLeftPanelScript antyAliasingRL;
    public RightLeftPanelScript textureQuality;
    public RightLeftPanelScript shadowResolution;
    public RightLeftPanelScript vSync;
    public RightLeftPanelScript shadowQuality;
    public RightLeftPanelScript resolution;
    public RightLeftPanelScript FPSToggle;

    //Audio Settings
    public RightLeftPanelScript masterVolume;
    public RightLeftPanelScript musicVolume;
    public RightLeftPanelScript sfxVolume;
    public AudioManager audioManager;

    public enum SettingsState
    {
        Game,
        Video,
        Audio,
        Hud,
        Controls,
        
    }
    SettingsState currentSettingState;


    private void Start()
    {

        resolutions = Screen.resolutions;
        resolution.data.Clear();
        //Get avaible resolutions and put them into list
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolution.data.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
                
            }
        }
        preset.value = "Ultra";
    }
    private void Update()
    {
        SetAntiAliasing();
        TextureQuality();
        ShadowResolutionn();
        VerticalSync();
        //FPSCounterTogle();
        SetQuality();
        Volume();
    }

    public void SetResolution(int resolutionIndex)
    {
        //Get current resolution
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }



    public void HideAllWindows()
    {
        foreach (MenuWindow w in menuWindows)
        {
            w.HideWindow();
        }
    }

    public void ShowMenuWindow(SettingsState menuWindow)
    {
        HideAllWindows();
        currentSettingState = menuWindow;
        switch (menuWindow)
        {
            case SettingsState.Game:
                menuWindows[0].ShowWindow();
                break;
            case SettingsState.Video:
                menuWindows[1].ShowWindow();
                break;
            case SettingsState.Audio:
                menuWindows[2].ShowWindow();
                break;
            case SettingsState.Hud:
                menuWindows[3].ShowWindow();
                break;
            case SettingsState.Controls:
                menuWindows[4].ShowWindow();
                break;
        }
    }
  
    public void SettingsGameButton()
    {
        ShowMenuWindow(SettingsState.Game);
    }
    public void SettingsVideosButton()
    {
        ShowMenuWindow(SettingsState.Video);
    }
    public void SettingsAudioButton()
    {
        ShowMenuWindow(SettingsState.Audio);
    }
    public void SettingsControlsButton()
    {
        ShowMenuWindow(SettingsState.Controls);
    }
    public void SettingsHudButton()
    {
        ShowMenuWindow(SettingsState.Hud);
    }
  


    public void SetQuality()
    {
        //Set settings based on preset
        switch (preset.value)
        {
            case "Very Low":
                antyAliasingRL.value = "0";
                textureQuality.value = "Low";
                shadowResolution.value = "Low";
                shadowQuality.value = "Disable";
                break;
            case "Low":
                antyAliasingRL.value = "2";
                textureQuality.value = "Medium";
                shadowResolution.value = "Low";
                shadowQuality.value = "Low";
                break;
            case "Medium":
                antyAliasingRL.value = "4";
                textureQuality.value = "Low";
                shadowResolution.value = "Medium";
                shadowQuality.value = "Low";
                break;
            case "High":
                antyAliasingRL.value = "4";
                textureQuality.value = "High";
                shadowResolution.value = "High";
                shadowQuality.value = "High";
                break;
            case "Very High":
                antyAliasingRL.value = "4";
                textureQuality.value = "High";
                shadowResolution.value = "Very High";
                shadowQuality.value = "High";
                break;
            case "Ultra":
                antyAliasingRL.value = "8";
                textureQuality.value = "Ultra";
                shadowResolution.value = "Very High";
                shadowQuality.value = "High";
                break;
        }
    }
    public void SetAntiAliasing()
    {
        switch (antyAliasingRL.value)
        {
            case "0":
                QualitySettings.antiAliasing = 0;
                break;
            case "2":
                QualitySettings.antiAliasing = 2;
                break;
            case "4":
                QualitySettings.antiAliasing = 4;
                break;
            case "8":
                QualitySettings.antiAliasing = 8;
                break;
        }
      
    }
    public void VerticalSync()
    {
        if (vSync.value == "On")
        {
            QualitySettings.vSyncCount = 1;
        }
        else QualitySettings.vSyncCount = 0;
    }
    public void TextureQuality()
    {
        switch (textureQuality.value)
        {
            case "Ultra":
                QualitySettings.masterTextureLimit = 0;
                break;
            case "High":
                QualitySettings.masterTextureLimit = 1;
                break;
            case "Medium":
                QualitySettings.masterTextureLimit = 2;
                break;
            case "Low":
                QualitySettings.masterTextureLimit = 3;
                break;
        }

    }
    public void ShadowResolutionn()
    {
        switch (shadowResolution.value)
        {
            case "Low":
                QualitySettings.shadowResolution = ShadowResolution.Low;
                break;
            case "Medium":
                QualitySettings.shadowResolution = ShadowResolution.Medium;
                break;
            case "High":
                QualitySettings.shadowResolution = ShadowResolution.High;
                break;
            case "Very High":
                QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
                break;
        }       
    }
    public void ShadowQuality()
    {
        switch (shadowQuality.value)
        {
            case "Disable":
                QualitySettings.shadows = UnityEngine.ShadowQuality.Disable;
                break;
            case "Low":
                QualitySettings.shadows = UnityEngine.ShadowQuality.HardOnly;
                break;
            case "High":
                QualitySettings.shadows = UnityEngine.ShadowQuality.All;
                break;
        }
    }
    public void FPSCounterTogle()
    {
        if (FPSToggle.value == "On")
        {
            FPScounter.gameObject.SetActive(true);
        }
        else FPScounter.gameObject.SetActive(false);
    }

    public void Volume()
    {
        string value = musicVolume.value;
        Debug.Log(value);
        float floatValue = float.Parse(value) / 10;
        //audioManager.SetVolume(floatValue);
    }
}
