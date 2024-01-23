using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using InputIcons;
using System.Collections.Generic;
using static InputIcons.InputIconsUtility;

public class II_UITextDisplayAllActions : MonoBehaviour
{

    private InputDevice inputDevice;
    private TextMeshProUGUI textMesh;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        InputIconsManagerSO.onControlsChanged += OnControlsChanged;
        InputIconsManagerSO.onBindingsChanged += UpdateText;
        UpdateText();
    }

    private void OnDestroy()
    {
        InputIconsManagerSO.onControlsChanged -= OnControlsChanged;
        InputIconsManagerSO.onBindingsChanged -= UpdateText;
    }

    public void OnControlsChanged(InputDevice device)
    {
        this.inputDevice = device;
        UpdateText();
    }

    private void UpdateText()
    {
 

        InputIconSetBasicSO iconSet = InputIconSetConfiguratorSO.GetIconSet(inputDevice);

        List<InputStyleData> myList;
        if (iconSet.controlSchemeName == InputIconsManagerSO.Instance.controlSchemeName_Keyboard)
            myList = InputIconsManagerSO.Instance.inputStyleKeyboardDataList;
        else
            myList = InputIconsManagerSO.Instance.inputStyleGamepadDataList;

        bool showAll = InputIconsManagerSO.Instance.showAllInputOptionsInStyles;
        string s = "";
        for(int i=0; i<myList.Count; i++)
        {
            if(showAll)
                s += myList[i].bindingName + ": " + myList[i].tmproReferenceText + " <size=80%>[" + myList[i].humanReadableString + "]</size>\n";
            else
            {
                s += myList[i].bindingName + ": " + myList[i].tmproReferenceText + " <size=80%>[" + myList[i].humanReadableString_singleInput + "]</size>\n";
                //s += myList[i].bindingName + ": " + myList[i].tmproReferenceText + " <size=80%>[" + myList[i].humanReadableString_singleInput + "]</size>" + myList[i].binding.effectivePath + "\n";
            }
              
        }

        textMesh.SetText(s);
    }
}
