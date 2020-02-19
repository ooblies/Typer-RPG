using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GlobalWords : Singleton<GlobalWords>
{

    public string fileName = "words.txt";
    private string dataPath = "";
    private int maxLength = 0;

    public List<string> words = new List<string>();
    
    private void Awake()
    {
        dataPath = Application.dataPath;
        //WebGL
        //StartCoroutine(loadWords());

        //Windows
        loadWordsWindows();

        findMaxLength();
    }

    private void loadWordsWindows()
    {
        string filePath = System.IO.Path.Combine(Application.dataPath, fileName);
        words = File.ReadAllLines(filePath).ToList();
        //Debug.Log("words loaded locally - " + filePath);
    }

    private IEnumerator loadWords()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
        
        //WebGL
        WWW www = new WWW(filePath);
        yield return www;
        
        words = www.text.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None).ToList();
        //Debug.Log("words loaded remotely - " + filePath);
    }

    private void findMaxLength()
    {
        maxLength = words.OrderByDescending(x => x.Length).First().Length;
    }

    public string getWordByLength(int length)
    {
        string randomWord = "";

        List<string> usedWords = new List<string>();

        usedWords.AddRange(GameObject.FindGameObjectsWithTag("Projectile").Select(w => w.GetComponent<TextMesh>().text));
        usedWords.AddRange(GameObject.FindGameObjectsWithTag("SkillButton").Select(w => w.GetComponent<Text>().text));

        

        if (length <= maxLength)
        {
            List<string> potentialWords = words.Where(w => w.Length == length).ToList();
            int potentialLength = potentialWords.Count();
            int randomIndex = Random.Range(0, potentialLength);
            randomWord = potentialWords[randomIndex];
        }
        else
        {
            string firstWord = getWordByLength(Random.Range(7,13));

            string secondWord = getWordByLength(length - firstWord.Length);

            randomWord = firstWord + " " + secondWord;
        }

        while (usedWords.Contains(randomWord)) //prevent new word from containing existing word
        {
            randomWord = getWordByLength(length);
        }

        return randomWord;
    }


}
