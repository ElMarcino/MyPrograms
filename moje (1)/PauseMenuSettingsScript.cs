using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuSettingsScript : MonoBehaviour
{
    public List<MenuWindow> menuWindows;

    public enum PauseSettingsState
    {
        Game,
        Video,
        Audio,
        Hud,
        Controls
    }
    PauseSettingsState currentPauseSettingState;

    public void HideAllWindows()
    {
        foreach (MenuWindow w in menuWindows)
        {
            w.HideWindow();
        }
    }

    public void ShowMenuWindow(PauseSettingsState menuWindow)
    {
        HideAllWindows();
        currentPauseSettingState = menuWindow;
        switch (menuWindow)
        {
            case PauseSettingsState.Game:
                menuWindows[0].ShowWindow();
                break;
            case PauseSettingsState.Video:
                menuWindows[1].ShowWindow();
                break;
            case PauseSettingsState.Audio:
                menuWindows[2].ShowWindow();
                break;
            case PauseSettingsState.Hud:
                menuWindows[3].ShowWindow();
                break;
            case PauseSettingsState.Controls:
                menuWindows[4].ShowWindow();
                break;
        }
    }

    public void GameSettingButton()
    {
        ShowMenuWindow(PauseSettingsState.Game);
    }

    public void VideoSettingButton()
    {
        ShowMenuWindow(PauseSettingsState.Video);
    }

    public void AudioSettingButton()
    {
        ShowMenuWindow(PauseSettingsState.Audio);
    }

    public void HudSettingButton()
    {
        ShowMenuWindow(PauseSettingsState.Hud);
    }

    public void ControlsSettingButton()
    {
        ShowMenuWindow(PauseSettingsState.Controls);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
