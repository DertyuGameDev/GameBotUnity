using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Telegram.Bot.Types;

public class ScreenshotContestMobileDemo : MonoBehaviour
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

    private string fileAddress;
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

    private void OnClick() // Open image selector by using Native File Picker by yasirkula "https://github.com/yasirkula/UnityNativeFilePicker"
    {
        if (NativeFilePicker.IsFilePickerBusy())
            return;

        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
                Debug.Log("Operation cancelled");
            else
            {
                fileAddress = path;
                StartCoroutine(OutputRoutine(new System.Uri(fileAddress).AbsoluteUri));
            }
        }, new string[] { "image/png", "image/*" });
    }

    private IEnumerator OutputRoutine(string url)
    {
        var loader = new WWW(url);
        yield return loader;
        imageHolder.texture = loader.texture;
    }

    private IEnumerator SubmitImage()
    {
        Task<Message> task = uniGram.SendPhoto(fileAddress, true, $"{titleField.text} (by @{telegramUsername.text})"); // Send Image
        titleField.text = "Please wait...";
        yield return new WaitUntil(() => task.IsCompleted); // Wait to finish Image's task
        titleField.text = "Image sent!";
    }
}