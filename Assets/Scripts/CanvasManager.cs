using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CanvasManager : MonoBehaviour
{
    public GameObject egyptKey;
    public GameObject egyptKeyHand;

    private static CanvasManager _instance;
    public static CanvasManager Instance { get { return _instance; } }
    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void UpdateKeys(string keyColor)
    {
        if(keyColor == "egypt")
        {
            egyptKeyHand.SetActive(true);
            egyptKey.SetActive(true);
        }
    }
    public void ClearKeys()
    {
        egyptKeyHand.SetActive(false);
        egyptKey.SetActive(false);
    }
}
