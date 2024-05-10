using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using TMPro;
using System.Threading.Tasks;
using Telegram.Bot.Types;

public class ScreenshotContestPCDemo : MonoBehaviour, IPointerDownHandler
{
    [Header("Initialization")]
    public string telegramToken; // Telegram Token to initialize the bot
    public long chatID; // ChatID of selected telegram chat

    [Header("UI")]
    public TMP_InputField telegramUsername;
    public TMP_InputField titleField;
    public RawImage imageHolder;
    public Button selectImageButton;
    public Button submitButton;

    private string[] fileAddress;
    private UniGram uniGram = new UniGram(); // Create a new UniGram

    private void Start()
    {
        uniGram.Initialize(telegramToken, chatID); // Initialize the bot
        selectImageButton.onClick.AddListener(OnClick); // Open image selector
        submitButton.onClick.AddListener(() => StartCoroutine(SubmitImage())); // Send Image when 'submitButton' clicked
    }

    private void Update()
    {
        if (titleField.text.Length > 0 && fileAddress.Length > 0 && telegramUsername.text.Length > 0)
            submitButton.interactable = true;
        else
            submitButton.interactable = false;
    }

    public void OnPointerDown(PointerEventData eventData) { }

    private void OnClick() // Open image selector by using StandaloneFileBrowser
    {
        var extensions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") };

        fileAddress = StandaloneFileBrowser.OpenFilePanel("Title", "", extensions, false);

        if (fileAddress.Length > 0)
            StartCoroutine(OutputRoutine(new System.Uri(fileAddress[0]).AbsoluteUri));
    }

    private IEnumerator OutputRoutine(string url)
    {
        var loader = new WWW(url);
        yield return loader;
        imageHolder.texture = loader.texture;
    }

    private IEnumerator SubmitImage()
    {
        Task<Message> task = uniGram.SendPhoto(fileAddress[0], true, $"{titleField.text} (by @{telegramUsername.text})"); // Send Image
        titleField.text = "Please wait...";
        yield return new WaitUntil(() => task.IsCompleted); // Wait to finish Image's task
        titleField.text = "Image sent!";
    }
}