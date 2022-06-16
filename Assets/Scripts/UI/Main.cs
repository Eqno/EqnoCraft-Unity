using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private GameObject FatherPanel, SonPanel;
    public GameObject LoadingPanel, PlayingPanel, SettingPanel;
    private void SwitchToPanel(GameObject target)
    {
        LoadingPanel.SetActive(false);
        PlayingPanel.SetActive(false);
        SettingPanel.SetActive(false);

        FatherPanel = SonPanel;
        target.SetActive(true);
        SonPanel = target;
    }
    public void Return()
    {
        LoadingPanel.SetActive(false);
        PlayingPanel.SetActive(true);
        SettingPanel.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Save()
    {
        GameDataManager.SaveGameData();
    }
}
