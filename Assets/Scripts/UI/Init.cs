using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init : MonoBehaviour
{
    private GameObject FatherPanel, SonPanel;
    public GameObject WelcomPanel, InitPanel, ComingSoon;
    // WelcomPanel
    private void Start()
    {
        SonPanel = WelcomPanel;
        FatherPanel = WelcomPanel;
        SwitchToPanel(WelcomPanel);
    }
    public void SinglePlayer() { SwitchToPanel(InitPanel); }
    public void MultiPlayer() { SwitchToPanel(ComingSoon); }
    public void MineCraftRealms() { SwitchToPanel(ComingSoon); }
    public void Options() { SwitchToPanel(ComingSoon); }
    public void Exit() { Application.Quit(); }
    public void Language() { SwitchToPanel(ComingSoon); }
    public void AccessAbility() { SwitchToPanel(ComingSoon); }
    // InitPanel
    public void EnterSelectedWorld()
    {
        SceneManager.LoadScene("Main");
    }
    public void CreateNewWorld() { SwitchToPanel(ComingSoon); }
    public void Edit() { SwitchToPanel(ComingSoon); }
    public void Delete() { SwitchToPanel(ComingSoon); }
    public void Rebuild() { SwitchToPanel(ComingSoon); }
    public void Cancel() { SwitchToPanel(WelcomPanel); }
    // ComingSoon
    public void Return() { SwitchToPanel(FatherPanel); }
    private void SwitchToPanel(GameObject target)
    {
        InitPanel.SetActive(false);
        ComingSoon.SetActive(false);
        WelcomPanel.SetActive(false);

        FatherPanel = SonPanel;
        target.SetActive(true);
        SonPanel = target;
    }
}
