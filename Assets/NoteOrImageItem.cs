using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NoteOrImageItem : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI noteText;  
    public RawImage image;            
    public Transform modelHolder;     

    [HideInInspector]
    public string noteId;             

    public void ShowNote(string text)
    {
        noteText.text = text;
        noteText.gameObject.SetActive(true);
        image.gameObject.SetActive(false);
        ClearModel();
    }

    public void ShowImage(Texture2D tex)
    {
        image.texture = tex;
        image.gameObject.SetActive(true);
        noteText.gameObject.SetActive(false);
        ClearModel();
    }

    public void ShowModel(GameObject model)
    {
        if (modelHolder == null)
        {
            Debug.LogError("❌ modelHolder not assigned in Inspector!");
            Destroy(model);
            return;
        }

        ClearModel();

        model.transform.SetParent(modelHolder, false);
        model.SetActive(true);

        noteText.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
    }

    private void ClearModel()
    {
        foreach (Transform child in modelHolder)
        {
            Destroy(child.gameObject);
        }
    }


}



// NoteOrImageItem.cs
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using UnityEngine.Networking;
//using System.Collections;

//public class NoteOrImageItem : MonoBehaviour
//{
//    public TextMeshProUGUI noteText;
//    public RawImage image;
//    public string noteId;

//    // جديد: رابط الموديل (إذا تبي تستخدمه)
//    public string modelUrl;

//    // جديد: GameObject لتمثيل الموديل المحمّل
//    public GameObject loadedModel;

//    public void ShowNote(string text)
//    {
//        ClearDisplay();
//        noteText.text = text;
//        noteText.gameObject.SetActive(true);
//    }

//    public void ShowImage(Texture2D tex)
//    {
//        ClearDisplay();
//        image.texture = tex;
//        image.gameObject.SetActive(true);
//    }

//    public void ShowModel(string url)
//    {
//        ClearDisplay();
//        modelUrl = url;
//        StartCoroutine(LoadModelCoroutine(url));
//    }

//    void ClearDisplay()
//    {
//        noteText.gameObject.SetActive(false);
//        image.gameObject.SetActive(false);
//        if (loadedModel != null)
//        {
//            Destroy(loadedModel);
//            loadedModel = null;
//        }
//    }

//    IEnumerator LoadModelCoroutine(string url)
//    {
//        // مثال باستخدام تحميل AssetBundle
//        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
//        yield return www.SendWebRequest();

//        if (www.result != UnityWebRequest.Result.Success)
//        {
//            Debug.LogError("Model load failed: " + www.error);
//            yield break;
//        }

//        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
//        if (bundle == null)
//        {
//            Debug.LogError("Failed to load AssetBundle");
//            yield break;
//        }

//        // نفترض أن اسم الـ prefab داخل الأسبكت باندل هو "Model"
//        var prefab = bundle.LoadAsset<GameObject>("Model");
//        if (prefab != null)
//        {
//            loadedModel = Instantiate(prefab, transform);
//        }
//        else
//        {
//            Debug.LogError("Model prefab not found in AssetBundle");
//        }

//        bundle.Unload(false);
//    }

//    public void DeleteItem()
//    {
//        StartCoroutine(DeleteFromServer());
//    }

//    IEnumerator DeleteFromServer()
//    {
//        string url = "http://localhost:3000/api/delete-note/" + noteId;
//        UnityWebRequest req = UnityWebRequest.Delete(url);
//        yield return req.SendWebRequest();
//        if (req.result == UnityWebRequest.Result.Success)
//        {
//            Destroy(gameObject);
//        }
//        else
//        {
//            Debug.LogError("Delete failed: " + req.error);
//        }
//    }
//}
