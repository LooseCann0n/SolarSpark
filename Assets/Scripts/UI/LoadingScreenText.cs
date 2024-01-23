using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreenText : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    public  TMP_FontAsset  hieroFont;
    public float printDelay;
    public int charTranslateDelay;
    /*Loading screen text system
     * 1. print hieroglyphs quickly
     * 2. having the hieroglyphs switch font character-per-character
     */
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void LoadText(string textToPrint)
    {
        loadingText.text = "";
        StartCoroutine(PrintText(textToPrint));
    }

    public IEnumerator PrintText(string LoadingString)
    {
        string printedString = "";
        for (int i = 0; i < LoadingString.Length; i++)
        {
            printedString += LoadingString[i];
            loadingText.text = "<font=\"" + hieroFont.name + "\">" + printedString + "</font>";
            //yield return new WaitForSeconds(printDelay);
            yield return null;
        }
        StartCoroutine(ChangeText(LoadingString));
    }
    public IEnumerator ChangeText(string LoadingString)
    {
        string translated = "";
        string coded = LoadingString;
        for(int i = 0; i < LoadingString.Length; i++)
        {
            translated += LoadingString[i];
            coded = coded.Remove(0, 1);
            loadingText.text = translated+"<font=\"" + hieroFont.name + "\">" + coded + "</font>";
            yield return new WaitForSeconds(printDelay);
        }
    }

    /// <summary>
    /// Prints an encoded messae and translates it after a certain number of characters have appeared
    /// </summary>
    /// <param name="message">This is the message to print</param>
    /// <returns>Returns a wait for seconds</returns>
    public IEnumerator DisplayText(string message)
    {
        string translatedText = "";
        string encryptedText = "";
        int charNumber = 0;
        while(charNumber<message.Length+charTranslateDelay)
        {
            //printing encrypted message
            if (charNumber < message.Length)
            {
                encryptedText += message[charNumber];
            }
            //translate message
            if(charNumber >= charTranslateDelay)
            {
                encryptedText = encryptedText.Remove(0, 1);
                translatedText += message[charNumber - charTranslateDelay];
            }
            charNumber++;
            loadingText.text = "<mspace=19><cspace=0.2em>" + translatedText + "<font=\"" + hieroFont.name + "\">" + encryptedText + "</font></mspace>";
            yield return new WaitForSeconds(printDelay);
        }
    }
}
