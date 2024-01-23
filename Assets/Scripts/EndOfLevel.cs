using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour
{
    public SceneField nextLevel;
    [TextArea(15,15)]
    public string nextLevelText;
    public Sprite loadingScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag ("Player"))
        {
            SceneChangeHandler.Instance.LoadLevel(nextLevel.SceneName, nextLevelText, loadingScreen);
        }
    }
}
