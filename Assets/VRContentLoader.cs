//////using UnityEngine;
//////using UnityEngine.Networking;
//////using System.Collections;
//////using System.Collections.Generic;

//////public class VRContentLoader : MonoBehaviour
//////{
//////    public static VRContentLoader Instance;

//////    public GameObject prefab;
//////    public Transform container;

//////    [System.Serializable]
//////    public class Note
//////    {
//////        public string _id;
//////        public string text;
//////    }

//////    [System.Serializable]
//////    public class DataResponse
//////    {
//////        public List<Note> notes;
//////        public List<string> images;
//////    }

//////    private Dictionary<string, NoteOrImageItem> noteItems = new Dictionary<string, NoteOrImageItem>();
//////    private Dictionary<string, NoteOrImageItem> imageItems = new Dictionary<string, NoteOrImageItem>();


//////    void Awake()
//////    {
//////        Instance = this;
//////    }

//////    void Start()
//////    {
//////        LoadDataFromAPI();
//////        InvokeRepeating(nameof(LoadDataFromAPI), 0, 5f);
//////    }

//////    public void LoadDataFromAPI()
//////    {
//////        StartCoroutine(LoadDataFromAPIEnum());
//////    }

//////    IEnumerator LoadDataFromAPIEnum()
//////    {
//////        string url = "http://localhost:3000/api/get-data?_=" + Time.time;
//////        UnityWebRequest request = UnityWebRequest.Get(url);
//////        yield return request.SendWebRequest();

//////        if (request.result != UnityWebRequest.Result.Success)
//////        {
//////            Debug.LogError(request.error);
//////            yield break;
//////        }

//////        var json = request.downloadHandler.text;
//////        var data = JsonUtility.FromJson<DataResponse>(FixJson(json));

//////        HashSet<string> serverNoteIds = new HashSet<string>();

//////        foreach (var note in data.notes)
//////        {
//////            serverNoteIds.Add(note._id);

//////            if (noteItems.ContainsKey(note._id))
//////            {
//////                if (noteItems[note._id].noteText.text != note.text)
//////                {
//////                    noteItems[note._id].ShowNote(note.text);
//////                }
//////            }
//////            else
//////            {
//////                var go = Instantiate(prefab, container);
//////                var item = go.GetComponent<NoteOrImageItem>();
//////                item.noteId = note._id;
//////                item.ShowNote(note.text);
//////                noteItems[note._id] = item;
//////            }
//////        }

//////        List<string> notesToRemove = new List<string>();
//////        foreach (var id in noteItems.Keys)
//////        {
//////            if (!serverNoteIds.Contains(id))
//////            {
//////                Destroy(noteItems[id].gameObject);
//////                notesToRemove.Add(id);
//////            }
//////        }
//////        foreach (var id in notesToRemove)
//////        {
//////            noteItems.Remove(id);
//////        }

//////        HashSet<string> serverImageUrls = new HashSet<string>();

//////        foreach (var imageUrl in data.images)
//////        {
//////            serverImageUrls.Add(imageUrl);

//////            if (!imageItems.ContainsKey(imageUrl))
//////            {
//////                StartCoroutine(LoadImage(imageUrl));
//////            }
//////        }

//////        List<string> imagesToRemove = new List<string>();
//////        foreach (var imgUrl in imageItems.Keys)
//////        {
//////            if (!serverImageUrls.Contains(imgUrl))
//////            {
//////                Destroy(imageItems[url].gameObject);
//////                imagesToRemove.Add(imgUrl);
//////            }
//////        }
//////        foreach (var imgUrl in imagesToRemove)
//////        {
//////            imageItems.Remove(imgUrl);
//////        }
//////    }

//////    //IEnumerator LoadImage(string url)
//////    //{
//////    //    UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
//////    //    yield return request.SendWebRequest();

//////    //    if (request.result != UnityWebRequest.Result.Success)
//////    //    {
//////    //        Debug.LogError(request.error);
//////    //        yield break;
//////    //    }

//////    //    var tex = DownloadHandlerTexture.GetContent(request);
//////    //    var go = Instantiate(prefab, container);
//////    //    var item = go.GetComponent<NoteOrImageItem>();
//////    //    item.ShowImage(tex);
//////    //}
//////    IEnumerator LoadImage(string url)
//////    {
//////        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
//////        yield return request.SendWebRequest();

//////        if (request.result != UnityWebRequest.Result.Success)
//////        {
//////            Debug.LogError(request.error);
//////            yield break;
//////        }

//////        var tex = DownloadHandlerTexture.GetContent(request);
//////        var go = Instantiate(prefab, container);
//////        var item = go.GetComponent<NoteOrImageItem>();
//////        item.ShowImage(tex);

//////        imageItems[url] = item;
//////    }


//////    string FixJson(string value)
//////    {
//////        return "{\"notes\":" + JsonHelper.GetJsonArray(value, "notes") +
//////               ",\"images\":" + JsonHelper.GetJsonArray(value, "images") + "}";
//////    }
//////}
// VRContentLoader.cs
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using Siccity.GLTFUtility;

public class VRContentLoader : MonoBehaviour
{
    public GameObject prefab;
    public Transform container;

    public void AddNote(string id, string text)
    {
        GameObject go = Instantiate(prefab, container);
        var item = go.GetComponent<NoteOrImageItem>();
        item.noteId = id;
        item.ShowNote(text);
    }

    public void AddImage(string id, string url)
    {
        StartCoroutine(LoadImageCoroutine(id, url));
    }

    public void AddModel(string id, string url)
    {
        StartCoroutine(LoadModelCoroutine(id, url));
    }

    IEnumerator LoadImageCoroutine(string id, string url)
    {
        UnityWebRequest req = UnityWebRequestTexture.GetTexture(url);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Texture2D tex = DownloadHandlerTexture.GetContent(req);
            GameObject go = Instantiate(prefab, container);
            var item = go.GetComponent<NoteOrImageItem>();
            item.noteId = id;
            item.ShowImage(tex);
        }
        else
        {
            Debug.LogError("⚠ Failed to load image: " + req.error);
        }
    }

    // تحميل موديل GLB وعرضه باستخدام GLTFUtility
    IEnumerator LoadModelCoroutine(string id, string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] modelData = www.downloadHandler.data;

            string path = Path.Combine(Application.persistentDataPath, id + ".glb");
            File.WriteAllBytes(path, modelData);

            GameObject importedModel = Importer.LoadFromFile(path);

            GameObject go = Instantiate(prefab, container);
            var item = go.GetComponent<NoteOrImageItem>();
            item.noteId = id;
            item.ShowModel(importedModel);
        }
        else
        {
            Debug.LogError("⚠ Failed to download model: " + www.error);
        }
    }
}


// VRContentLoader.cs
//using UnityEngine;
//using UnityEngine.Networking;
//using System.Collections;
//using System.IO;
//using System.Threading.Tasks;
//using GLTFast;

//public class VRContentLoader : MonoBehaviour
//{
//    public GameObject prefab;          // البريفاب للنوت والصورة
//    public Transform container;        // الحاوية للنوت والصور
//    public Transform modelContainer;   // مكان وضع الموديلات داخل المشهد (أضفه في الـ Inspector)

//    private GameObject currentModel;   // لحفظ الموديل الحالي

//    public void ProcessWebSocketMessage(string json)
//    {
//        var msg = JsonUtility.FromJson<MessageData>(json);

//        switch (msg.type)
//        {
//            case "note":
//                if (msg.action == "add")
//                    AddNote(msg.data.id, msg.data.text);
//                else if (msg.action == "delete")
//                    DeleteNote(msg.data.id);
//                break;

//            case "image":
//                if (msg.action == "add")
//                    AddImage(msg.data.id, msg.data.url);
//                break;

//            case "model":
//                if (msg.action == "add")
//                    AddModel(msg.data.url, msg.data.fileName);
//                break;
//        }
//    }

//    public void AddModel(string url, string fileName = "model.glb")
//    {
//        _ = DownloadAndLoadGLBAsync(url, fileName);
//    }

//    private async Task DownloadAndLoadGLBAsync(string url, string fileName)
//    {
//        string filePath = Path.Combine(Application.persistentDataPath, fileName);

//        using (UnityWebRequest req = UnityWebRequest.Get(url))
//        {
//            var operation = req.SendWebRequest();
//            while (!operation.isDone)
//                await Task.Yield();

//            if (req.result == UnityWebRequest.Result.Success)
//            {
//                File.WriteAllBytes(filePath, req.downloadHandler.data);
//                Debug.Log($"✅ GLB downloaded to: {filePath}");

//                var gltf = new GltfImport();
//                bool success = await gltf.Load(filePath);

//                if (success)
//                {
//                    if (currentModel != null)
//                        Destroy(currentModel);

//                    currentModel = new GameObject("GLB_Model");
//                    bool instantiated = gltf.InstantiateMainScene(currentModel.transform);
//                    Debug.Log("Instantiate result: " + instantiated);
//                    Debug.Log("Children count: " + currentModel.transform.childCount);
//                    foreach (Transform child in currentModel.transform)
//                    {
//                        Debug.Log("Child: " + child.name);
//                    }

//                    if (modelContainer != null)
//                    {
//                        currentModel.transform.SetParent(modelContainer, false);
//                        currentModel.transform.localPosition = Vector3.zero;
//                        currentModel.transform.localRotation = Quaternion.identity;
//                        currentModel.transform.localScale = Vector3.one;
//                    }
//                    else
//                    {
//                        // إذا لم تحدد مكان الحاوية، ضع النموذج أمام الكاميرا مباشرة
//                        currentModel.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2f;
//                        currentModel.transform.localScale = Vector3.one;
//                    }

//                    Debug.Log("✅ Model loaded and placed in scene.");
//                }
//                else
//                {
//                    Debug.LogError("❌ Failed to load GLB model.");
//                }
//            }
//            else
//            {
//                Debug.LogError($"❌ Failed to download GLB: {req.error}");
//            }
//        }
//    }

//    public void AddNote(string id, string text)
//    {
//        var go = Instantiate(prefab, container);
//        var item = go.GetComponent<NoteOrImageItem>();
//        item.noteId = id;
//        item.ShowNote(text);
//    }

//    public void AddImage(string id, string url)
//    {
//        StartCoroutine(LoadImageCoroutine(id, url));
//    }

//    IEnumerator LoadImageCoroutine(string id, string url)
//    {
//        UnityWebRequest req = UnityWebRequestTexture.GetTexture(url);
//        yield return req.SendWebRequest();

//        if (req.result == UnityWebRequest.Result.Success)
//        {
//            Texture2D tex = DownloadHandlerTexture.GetContent(req);
//            var go = Instantiate(prefab, container);
//            var item = go.GetComponent<NoteOrImageItem>();
//            item.noteId = id;
//            item.ShowImage(tex);
//        }
//        else
//        {
//            Debug.LogError($"Image load failed: {req.error}");
//        }
//    }

//    void DeleteNote(string id)
//    {
//        foreach (Transform child in container)
//        {
//            var item = child.GetComponent<NoteOrImageItem>();
//            if (item != null && item.noteId == id)
//            {
//                Destroy(child.gameObject);
//                break;
//            }
//        }
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
//            public string fileName;
//        }
//    }
//}
