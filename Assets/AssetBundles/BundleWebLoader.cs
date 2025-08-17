//using System.Collections;
//using UnityEngine;
//using UnityEngine.Networking;

//public class BundleWebLoader : MonoBehaviour
//{
//    public string bundleUrl = "https://drive.google.com/uc?export=download&id=1_G3W1HwIAtNB-6oIfXFinrjF0LKH5dzA";
//    public string assetName = "assets/assetbundles/mynewbulder/sphere.prefab"; // 👈 الاسم الصحيح

//    IEnumerator Start()
//    {
//        UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl);
//        yield return uwr.SendWebRequest();

//        if (uwr.result != UnityWebRequest.Result.Success)
//        {
//            Debug.LogError("Failed to download AssetBundle: " + uwr.error);
//            yield break;
//        }

//        AssetBundle remoteAssetBundle = DownloadHandlerAssetBundle.GetContent(uwr);
//        if (remoteAssetBundle == null)
//        {
//            Debug.LogError("AssetBundle is null!");
//            yield break;
//        }

//        طباعة الأسماء للتأكيد
//        foreach (string name in remoteAssetBundle.GetAllAssetNames())
//        {
//            Debug.Log("Asset found in bundle: " + name);
//        }

//        GameObject prefab = remoteAssetBundle.LoadAsset<GameObject>(assetName);
//        if (prefab != null)
//        {
//            Instantiate(prefab);
//        }
//        else
//        {
//            Debug.LogError("Asset not found or not a GameObject: " + assetName);
//        }

//        remoteAssetBundle.Unload(false);
//    }
//}
//_______________________________________________________________________________________



////using UnityEngine;
////using System.Collections;
////using UnityEngine.Networking;


////public class WebSocketBundleLoader : MonoBehaviour
////{
////    private WebSocket ws;

////    void Start()
////    {
////        ws = new WebSocket("ws://localhost:3000");
////        ws.OnMessage += OnMessageReceived;
////        ws.Connect();
////    }

////    private void OnMessageReceived(object sender, MessageEventArgs e)
////    {
////        Debug.Log("📡 Message received from server: " + e.Data);
////        StartCoroutine(LoadBundleFromWeb(e.Data));
////    }

////    IEnumerator LoadBundleFromWeb(string url)
////    {
////        UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(url);
////        yield return uwr.SendWebRequest();

////        if (uwr.result != UnityWebRequest.Result.Success)
////        {
////            Debug.LogError("❌ Failed to download bundle: " + uwr.error);
////            yield break;
////        }

////        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
////        GameObject prefab = bundle.LoadAsset<GameObject>(bundle.GetAllAssetNames()[0]);
////        Instantiate(prefab);
////        bundle.Unload(false);
////    }

////    void OnDestroy()
////    {
////        if (ws != null && ws.IsAlive)
////            ws.Close();
////    }
////}
