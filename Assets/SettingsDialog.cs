using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsDialog : MonoBehaviour
{

    public Toggle PointerOffsetToggle;

    // Start is called before the first frame update
    void Start()
    {
        PointerOffsetToggle.SetIsOnWithoutNotify(SettingsHelper.PointerDragOffsetEnabled);
        PointerOffsetToggle.onValueChanged.AddListener(OnPointerDragOffsetEnableChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDragOffsetEnableChanged(bool value)
    {
        SettingsHelper.PointerDragOffsetEnabled = value;
    }
}
