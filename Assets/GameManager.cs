using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static string token;
    public UserDB user;
    public static string AdressWallet;
    [SerializeField] private UIDocument document;
    public static Action ConnectWalletAction, TakeUserInfo;
    public void Awake()
    {
        ConnectWalletAction += Connect;
        TakeUserInfo += Authorization;
        StartCoroutine(TakeAuthorization());
    }
    public void Authorization()
    {
        StartCoroutine(TakeAuthorization());
    }
    public void Connect()
    {
        StartCoroutine(ConnectWallet());
    }
    public void GetToken(string message)
    {
        token = message;
        document.rootVisualElement.Q<Label>("Label").text = token;
    }
    public IEnumerator ConnectWallet()
    {
        string url = "https://45.93.201.74/api/user";

        WWWForm form = new WWWForm();
        form.AddField("address", AdressWallet);
        // Создание UnityWebRequest
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        www.SetRequestHeader("Authorization", $"Bearer {token}");

        // Ожидание ответа
        yield return www.SendWebRequest();

        // Проверка наличия ошибок
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("UnityWebRequest error: " + www.error);
        }
        else
        {
            Debug.Log("Okey");
        }
    }
    public IEnumerator TakeAuthorization()
    {
        string url = "https://45.93.201.74/api/user";

        // Создание UnityWebRequest
        UnityWebRequest webRequest = UnityWebRequest.Get(url);

        // Добавление HTTP заголовков
        webRequest.SetRequestHeader("Authorization", $"Bearer {token}");

        // Ожидание ответа
        yield return webRequest.SendWebRequest();

        // Обработка результата
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("UnityWebRequest error: " + webRequest.error);
        }
        else
        {
            user = JsonUtility.FromJson<UserDB>(webRequest.downloadHandler.text);
        }
    }
}
[Serializable]
public class UserDB
{
    public string id;
    public int chat_id;
    public int tgid;
    public string username;
    public string address;
    public bool Is_Electricity;
    public float money;
    public float profit;
    public int dateStart;
    public int dateEnd;
    public string machines;
    public UserDB(string id, int chat_id, int tgid, string username, string adress, bool is_Electricity, float money, float profit, int deltaTime_off, int start_dateTimeOff, string machines)
    {
        this.username = username;
        this.chat_id = chat_id;
        Is_Electricity = is_Electricity;
        this.profit = profit;
        this.money = money;
        this.dateStart = deltaTime_off;
        this.dateEnd = start_dateTimeOff;
        this.machines = machines;
        this.id = id;
        this.tgid = tgid;
        this.address = adress;
    }
}
