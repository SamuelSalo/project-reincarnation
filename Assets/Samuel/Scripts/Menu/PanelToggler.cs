using UnityEngine;

public class PanelToggler : MonoBehaviour
{
    public GameObject panel1, panel2;

    public void TogglePanels()
    {
        panel1.SetActive(!panel1.activeSelf);
        panel2.SetActive(!panel2.activeSelf);
    }
}
