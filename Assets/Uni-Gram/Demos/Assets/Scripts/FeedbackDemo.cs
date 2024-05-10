using UnityEngine;
using UnityEngine.UI;
using Telegram.Bot.Types.Enums;
using TMPro;
using System.Collections;
using System.Threading.Tasks;
using Telegram.Bot.Types;

public class FeedbackDemo : MonoBehaviour
{
    [Header("Initialization")]
    public string telegramToken; // Telegram Token to initialize the bot
    public long chatID; // ChatID of selected telegram chat

    [Header("UI")]
    public TMP_InputField descriptionField;
    public Button submitButton;

    private UniGram uniGram = new UniGram(); // Create a new UniGram

    private void Start()
    {
        uniGram.Initialize(telegramToken, chatID); // Initialize the bot
        submitButton.onClick.AddListener(() => StartCoroutine(SubmitFeedback())); // Send message when 'submitButton' clicked
    }

    private void Update()
    {
        if (descriptionField.text.Length > 0)
            submitButton.interactable = true;
        else
            submitButton.interactable = false;
    }

    private IEnumerator SubmitFeedback()
    {
        Task<Message> task = uniGram.SendMessage($"<i>Feedback:</i> \n{descriptionField.text}", ParseMode.Html); // Send message

        descriptionField.text = "Please wait...";
        yield return new WaitUntil(() => task.IsCompleted); // Wait to finish Message's task
        descriptionField.text = "Thanks for your feedback!";
    }
}