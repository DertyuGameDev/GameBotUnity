using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class Message1
{
    public int chat_id;
    public string username;
}
[System.Serializable]
public class User
{
    public List<Message1> users;
}