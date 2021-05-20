using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VSyncSettings : MonoBehaviour
{
    TMP_Dropdown vSyncDropdown;

    public void Start()
    {
        vSyncDropdown = GetComponentInChildren<TMP_Dropdown>();
        if (PlayerPrefs.HasKey("VSyncCount"))
        {
            var tmp = PlayerPrefs.GetInt("VSyncCount");
            QualitySettings.vSyncCount = tmp;
            vSyncDropdown.SetValueWithoutNotify(tmp);

        }
        else
        {
            SetVSyncCount(0);
        }
    }

    public void SetVSyncCount(int _count)
    {
        PlayerPrefs.SetInt("VSyncCount", _count);
        QualitySettings.vSyncCount = Mathf.Clamp(_count, 0, 4);
    }
}
