using UnityEngine;
using UnityEngine.UI;
using Telegram.Bot.Types.Enums;
using TMPro;
using System.Collections;
using System.Threading.Tasks;
using Telegram.Bot.Types;

public class BugReportDemo : MonoBehaviour
{
    [Header("Initialization")]
    public string telegramToken; // Telegram Token to initialize the bot
    public long chatID; // ChatID of selected telegram chat

    [Header("UI")]
    public TMP_InputField titleField;
    public TMP_InputField descriptionField;
    public Button submitButton;

    private UniGram uniGram = new UniGram(); // Create a new UniGram

    private void Start()
    {
        uniGram.Initialize(telegramToken, chatID); // Initialize the bot
        submitButton.onClick.AddListener(() => StartCoroutine(SubmitBug())); // Send message when 'submitButton' clicked
    }

    private void Update()
    {
        uniGram.Initialize(telegramToken, chatID);
        if (titleField.text.Length > 0 && descriptionField.text.Length > 0)
            submitButton.interactable = true;
        else
            submitButton.interactable = false;
    }

    private IEnumerator SubmitBug()
    {
        Task<Message> task = uniGram.SendMessage($"<i>Title:</i> \n{titleField.text} \n\n<i>Description:</i> \n{descriptionField.text}", ParseMode.Html); // Send message

        titleField.text = "Please wait...";
        yield return new WaitUntil(() => task.IsCompleted); // Wait to finish Message's task
        titleField.text = "Thanks for report!";
        descriptionField.text = null;
    }
}