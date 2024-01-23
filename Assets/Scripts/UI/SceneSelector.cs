using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    public GameObject mainMenu;

    public Sprite[] loadingScreenSprite;
    [TextArea(15, 15)]
    public string[] loadingText;
    public void ChangeScene(string priorityScene)
    {
        switch (priorityScene)
        {
            case "TutoriaLevelOrSomething":
                SceneChangeHandler.Instance.LoadLevel(priorityScene, loadingText[0], loadingScreenSprite[0]);
                break;
            case "CityAboveGround":
                SceneChangeHandler.Instance.LoadLevel(priorityScene, loadingText[1], loadingScreenSprite[1]);
                break;
            case "Temple Blockout":
                SceneChangeHandler.Instance.LoadLevel(priorityScene, loadingText[2], loadingScreenSprite[2]);
                break;
            case "TempleInterior Concept Scene":
                SceneChangeHandler.Instance.LoadLevel(priorityScene, loadingText[3], loadingScreenSprite[3]);
                break;
            default:
                SceneChangeHandler.Instance.LoadLevel(priorityScene, loadingText[0], loadingScreenSprite[0]);
                break;
        }
    }

    public void BackButton()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
