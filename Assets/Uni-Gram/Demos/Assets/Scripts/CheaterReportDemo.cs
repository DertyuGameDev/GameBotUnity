using UnityEngine;
using UnityEngine.UI;
using Telegram.Bot.Types.Enums;
using TMPro;
using System.Collections;
using System.Threading.Tasks;
using Telegram.Bot.Types;

public class CheaterReportDemo : MonoBehaviour
{
    [Header("Initialization")]
    public string telegramToken; // Telegram Token to initialize the bot
    public long chatID; // ChatID of selected telegram chat

    [Header("UI")]
    public TMP_InputField cheatorUsernameField;
    public TMP_InputField detailsField;
    public TMP_Dropdown location;
    public Button reportButton;

    private UniGram uniGram = new UniGram(); // Create a new UniGram

    private void Start()
    {
        uniGram.Initialize(telegramToken, chatID); // Initialize the bot
        reportButton.onClick.AddListener(() => StartCoroutine(ReportCheater())); // Send message when 'reportButton' clicked
    }

    private void Update()
    {
        if (cheatorUsernameField.text.Length > 0 && detailsField.text.Length > 0)
            reportButton.interactable = true;
        else
            reportButton.interactable = false;
    }

    private IEnumerator ReportCheater()
    {
        Task<Message> task = uniGram.SendMessage($"<i>Cheater Username:</i> \n{cheatorUsernameField.text} \n\n<i>Location:</i>\n{location.options[location.value].text} \n\n<i>Additional Comments:</i> \n{detailsField.text}", ParseMode.Html); // Send message

        detailsField.text = "Please wait...";
        yield return new WaitUntil(() => task.IsCompleted); // Wait to finish Message's task
        cheatorUsernameField.text = null;
        detailsField.text = "Thanks for the report!";
    }
}