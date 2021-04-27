using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{

    public List<MenuWindow> menuWindows = new List<MenuWindow>();
    public MainMenuSettingsScript set;
    public Animator anim;
    public TMP_Text whereText;



    public enum MainMenuState
    {
        Main,
        NewGame,
        LoadGame,
        Settings,
        Extras,
        Quit
    };
    MainMenuState currentMenuState;

    private void Start()
    {
        ShowMenuWindow(MainMenuState.Main);
    }
    public void HideAllWindows()
    {
        foreach (MenuWindow w in menuWindows)
        {
            w.HideWindow();
        }
        if(set.menuWindows != null)
        {
            foreach (MenuWindow a in set.menuWindows)
            {
                a.HideWindow();
            }
        }
    }
    public void ShowMenuWindow(MainMenuState menuWindow)
    {
        HideAllWindows();
        currentMenuState = menuWindow;
        switch (menuWindow)
        {
            case MainMenuState.Main:
                menuWindows[0].ShowWindow();
                whereText.text = "";

                break;
            case MainMenuState.NewGame:
                menuWindows[1].ShowWindow();
                whereText.text = "New Game";
                break;
            case MainMenuState.LoadGame:
                menuWindows[2].ShowWindow();
                whereText.text = "Load Game";
                break;
            case MainMenuState.Settings:
                menuWindows[3].ShowWindow();
                whereText.text = "Settings";
                break;
            case MainMenuState.Extras:
                whereText.text = "Extras";
                menuWindows[4].ShowWindow();
                break;
            case MainMenuState.Quit:
                menuWindows[5].ShowWindow();
                whereText.text = "Quit";
                break;
        }
    }
    public void NewGameButton()
    {
        LoadingManager.instance.LoadScene((int)SceneIndexes.Garage);
        //GameLoader.instance.LoadGame();
    }
    public void NewGameWindow()
    {
        ShowMenuWindow(MainMenuState.NewGame);
    }
    public void LoadGameButton()
    {
        ShowMenuWindow(MainMenuState.LoadGame);
    }
    public void SettingsButton()
    {
        ShowMenuWindow(MainMenuState.Settings);
    }
    public void ExtrasButton()
    {
        ShowMenuWindow(MainMenuState.Extras);
    }
    public void QuitButton()
    {
        ShowMenuWindow(MainMenuState.Quit);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowMenuWindow(MainMenuState.Main);
            anim.SetInteger("IntTransition", 10);
        }
    }
    public void dcButton()
    {
        Application.OpenURL("https://discord.gg/w35bkGHu");
    }
    public void backButton()
    {
        ShowMenuWindow(MainMenuState.Main);
        anim.SetInteger("IntTransition", 10);

    }
}
