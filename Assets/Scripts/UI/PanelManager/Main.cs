using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private GameObject FatherPanel, SonPanel;
    public GameObject LoadingPanel, PlayingPanel;
    private void SwitchToPanel(GameObject target)
    {
        LoadingPanel.SetActive(false);
        PlayingPanel.SetActive(false);

        FatherPanel = SonPanel;
        target.SetActive(true);
        SonPanel = target;
    }
}
