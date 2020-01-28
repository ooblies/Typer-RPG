using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GlobalWords : Singleton<GlobalWords>
{

    public string wordPath = "Assets/Words/words.txt";

    public List<string> words = new List<string>();
    
    private void Awake()
    {
        words = File.ReadAllLines(wordPath).ToList();
    }

    public string getWordByLength(int length)
    {
        List<string> potentialWords = words.Where(w => w.Length == length).ToList();
        int potentialLength = potentialWords.Count();
        int randomIndex = Random.Range(0, potentialLength);

        string randomWord = potentialWords[randomIndex];

        return randomWord;
    }


}
