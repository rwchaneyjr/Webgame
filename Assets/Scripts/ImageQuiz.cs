using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ImageQuiz : MonoBehaviour
{
    [Header("UI")]
    public Button[] optionButtons;       // Option1, Option2, Option3
    public TMP_Text promptText;
    public AudioSource audioSource;

    private List<Sprite> allImages = new List<Sprite>();
    private string correctName;

    void Start()
    {
        LoadImages();
        NextRound();
    }

    void LoadImages()
    {
        Sprite[] loaded = Resources.LoadAll<Sprite>("Images");
        allImages.AddRange(loaded);
    }

    void NextRound()
    {
        // pick correct image
        int correctIndex = Random.Range(0, allImages.Count);
        Sprite correctSprite = allImages[correctIndex];
        correctName = correctSprite.name;

        promptText.text = "Find: " + correctName;

        // pick 2 wrong images
        List<Sprite> roundChoices = new List<Sprite>() { correctSprite };

        while (roundChoices.Count < 3)
        {
            Sprite randomPick = allImages[Random.Range(0, allImages.Count)];
            if (!roundChoices.Contains(randomPick))
                roundChoices.Add(randomPick);
        }

        // shuffle
        for (int i = 0; i < roundChoices.Count; i++)
        {
            Sprite temp = roundChoices[i];
            int r = Random.Range(i, roundChoices.Count);
            roundChoices[i] = roundChoices[r];
            roundChoices[r] = temp;
        }

        // assign to UI buttons
        for (int i = 0; i < 3; i++)
        {
            int index = i;
            optionButtons[i].image.sprite = roundChoices[i];
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => CheckAnswer(roundChoices[index].name));
        }

        PlaySound(correctName);
    }

    void PlaySound(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/" + name);
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Missing audio for: " + name);
        }
    }

    void CheckAnswer(string chosenName)
    {
        if (chosenName == correctName)
        {
            promptText.text = "✅ Correct!";
        }
        else
        {
            promptText.text = "❌ Try Again!";
        }

        Invoke(nameof(NextRound), 1.2f);
    }
}
