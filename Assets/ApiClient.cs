//using UnityEngine;
//using UnityEngine.Networking;
//using System.Collections;

//public class ApiClient : MonoBehaviour
//{
//    public void AddNote(string noteText)
//    {
//        StartCoroutine(AddNoteCoroutine(noteText));
//    }

//    IEnumerator AddNoteCoroutine(string noteText)
//    {
//        string json = JsonUtility.ToJson(new NoteData(noteText));
//        UnityWebRequest request = new UnityWebRequest("http://localhost:3000/api/add-note", "POST");
//        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
//        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//        request.downloadHandler = new DownloadHandlerBuffer();
//        request.SetRequestHeader("Content-Type", "application/json");

//        yield return request.SendWebRequest();

//        if (request.result != UnityWebRequest.Result.Success)
//        {
//            Debug.LogError("Add note error: " + request.error);
//        }
//        else
//        {
//            Debug.Log("Note added successfully!");
//            yield return new WaitForSeconds(0.2f);
//            VRContentLoader.Instance.LoadDataFromAPI();
//        }
//    }

//    [System.Serializable]
//    public class NoteData
//    {
//        public string text;
//        public NoteData(string t) { text = t; }
//    }
//}




// WSClient.cs
// WSClient.cs
using UnityEngine;
using NativeWebSocket;

public class WSClient : MonoBehaviour
{
    private WebSocket websocket;
    public VRContentLoader loader; 

    async void Start()
    {
        websocket = new WebSocket("ws://localhost:3000");

        websocket.OnOpen += () =>
        {
            Debug.Log("✅ WebSocket connected!");
        };

        websocket.OnMessage += (bytes) =>
        {
            string json = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("📩 Received: " + json);
            ProcessMessage(json);
        };

        websocket.OnError += (e) =>
        {
            Debug.LogError("⚠ WebSocket error: " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("❌ WebSocket closed!");
        };

        await websocket.Connect();
    }

    //void Update()
    //{


    //    websocket?.DispatchMessageQueue();
    //}

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket?.DispatchMessageQueue();
#endif
    }
    private async void OnDestroy()
    {
        await websocket.Close();
    }

    void ProcessMessage(string json)
    {
        MessageData msg = JsonUtility.FromJson<MessageData>(json);

        if (msg.type == "note" && msg.action == "add")
        {
            loader.AddNote(msg.data.id, msg.data.text);
        }
        else if (msg.type == "image" && msg.action == "add")
        {
            loader.AddImage(msg.data.id, msg.data.url);
        }
        else if (msg.type == "model" && msg.action == "add")
        {
            loader.AddModel(msg.data.id, msg.data.url);
        }
        else
        {
            Debug.LogWarning("🚫 Unsupported message type or action.");
        }
    }

    [System.Serializable]
    public class MessageData
    {
        public string type;   // "note", "image", "model"
        public string action; // "add", "delete", ...
        public Data data;

        [System.Serializable]
        public class Data
        {
            public string id;
            public string text; 
            public string url; 
        }
    }
}


//using UnityEngine;
//using NativeWebSocket;
//using System.Threading.Tasks;

//public class WSClient : MonoBehaviour
//{
//    WebSocket websocket;
//    public VRContentLoader loader;

//    async void Start()
//    {
//        websocket = new WebSocket("ws://localhost:3000");

//        websocket.OnOpen += () =>
//        {
//            Debug.Log("WebSocket connected!");
//        };

//        websocket.OnMessage += (bytes) =>
//        {
//            var message = System.Text.Encoding.UTF8.GetString(bytes);
//            Debug.Log("Received: " + message);

//            ProcessMessage(message);
//        };

//        websocket.OnClose += (e) =>
//        {
//            Debug.Log("WebSocket closed!");
//        };

//        websocket.OnError += (e) =>
//        {
//            Debug.LogError("WebSocket error: " + e);
//        };

//        await websocket.Connect();
//    }

//    void ProcessMessage(string json)
//    {
//        var msg = JsonUtility.FromJson<MessageData>(json);

//        if (msg.type == "note" && msg.action == "add")
//        {
//            loader.AddNote(msg.data.id, msg.data.text);
//        }
//        else if (msg.type == "image" && msg.action == "add")
//        {
//            loader.AddImage(msg.data.id, msg.data.url);
//        }
//        // ممكن تضيف حالات حذف أو تعديل هنا بعدين
//    }

//    void Update()
//    {
//        websocket?.DispatchMessageQueue();
//    }

//    private async void OnDestroy()
//    {
//        await websocket.Close();
//    }

//    [System.Serializable]
//    public class MessageData
//    {
//        public string type;
//        public string action;
//        public Data data;

//        [System.Serializable]
//        public class Data
//        {
//            public string id;
//            public string text;
//            public string url;
//        }
//    }
//}

//using UnityEngine;
//using NativeWebSocket;

//public class WSClient : MonoBehaviour
//{
//    private WebSocket websocket;
//    public VRContentLoader loader; // اسحبه في الـ Inspector

//    async void Start()
//    {
//        websocket = new WebSocket("ws://localhost:3000");

//        websocket.OnOpen += () =>
//        {
//            Debug.Log("✅ WebSocket connected!");
//        };

//        websocket.OnMessage += (bytes) =>
//        {
//            var json = System.Text.Encoding.UTF8.GetString(bytes);
//            Debug.Log("📩 Received: " + json);
//            ProcessMessage(json);
//        };

//        websocket.OnClose += (e) =>
//        {
//            Debug.Log("❌ WebSocket closed!");
//        };

//        websocket.OnError += (e) =>
//        {
//            Debug.LogError("⚠ WebSocket error: " + e);
//        };

//        await websocket.Connect();
//    }

//    void ProcessMessage(string json)
//    {
//        var msg = JsonUtility.FromJson<MessageData>(json);

//        if (msg.type == "note" && msg.action == "add")
//        {
//            loader.AddNote(msg.data.id, msg.data.text);
//        }
//        else if (msg.type == "image" && msg.action == "add")
//        {
//            loader.AddImage(msg.data.id, msg.data.url);
//        }
//        else if (msg.type == "model" && msg.action == "add")
//        {
//            loader.AddModel(msg.data.id, msg.data.url);
//        }

//    }

//    void Update()
//    {
//        websocket?.DispatchMessageQueue();
//    }

//    private async void OnDestroy()
//    {
//        await websocket.Close();
//    }

//    [System.Serializable]
//    public class MessageData
//    {
//        public string type;
//        public string action;
//        public Data data;

//        [System.Serializable]
//        public class Data
//        {
//            public string id;
//            public string text;
//            public string url;
//        }
//    }
//}

// WSClient.cs
//using UnityEngine;
//using NativeWebSocket;

//public class WSClient : MonoBehaviour
//{
//    private WebSocket websocket;
//    public VRContentLoader loader; // اربطه من الـ Inspector

//    async void Start()
//    {
//        websocket = new WebSocket("ws://localhost:3000");

//        websocket.OnOpen += () =>
//        {
//            Debug.Log("✅ WebSocket connected!");
//        };

//        websocket.OnMessage += (bytes) =>
//        {
//            var message = System.Text.Encoding.UTF8.GetString(bytes);
//            Debug.Log("📩 Received: " + message);
//            loader.ProcessWebSocketMessage(message);
//        };

//        websocket.OnError += (e) =>
//        {
//            Debug.LogError("❌ WebSocket error: " + e);
//        };

//        websocket.OnClose += (e) =>
//        {
//            Debug.Log("🔌 WebSocket closed");
//        };

//        await websocket.Connect();
//    }

//    void Update()
//    {
//        websocket?.DispatchMessageQueue();
//    }

//    private async void OnDestroy()
//    {
//        await websocket.Close();
//    }
//}
