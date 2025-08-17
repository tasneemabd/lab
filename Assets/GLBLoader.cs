////using UnityEngine;
////using UnityEngine.Networking;
////using System.IO;
////using System.Threading.Tasks;
////using GLTFast;

////public class GLBLoader : MonoBehaviour
////{
////    public Transform spawnPoint;

////    public async void LoadGLBFromUrl(string url)
////    {
////        string fileName = Path.GetFileName(url);
////        string filePath = Path.Combine(Application.persistentDataPath, fileName);

////        // إذا الملف موجود مسبقاً، امسحه (اختياري)
////        if (File.Exists(filePath))
////        {
////            File.Delete(filePath);
////            Debug.Log("🗑️ Deleted old model file: " + filePath);
////        }

////        // تحميل ملف GLB من الانترنت
////        using (UnityWebRequest uwr = UnityWebRequest.Get(url))
////        {
////            var operation = uwr.SendWebRequest();
////            while (!operation.isDone)
////                await Task.Yield();

////            if (uwr.result != UnityWebRequest.Result.Success)
////            {
////                Debug.LogError("❌ Failed to download model: " + uwr.error);
////                return;
////            }

////            File.WriteAllBytes(filePath, uwr.downloadHandler.data);
////            Debug.Log("✅ Model saved to: " + filePath);
////        }

////        // تحميل الموديل باستخدام GLTFast
////        var gltf = new GltfImport();
////        bool success = await gltf.Load(filePath);

////        if (success)
////        {
////            GameObject model = new GameObject("GLB_Model");
////            gltf.InstantiateMainScene(model.transform);
////            model.transform.position = spawnPoint != null ? spawnPoint.position : Vector3.zero;
////            Debug.Log("✅ Model instantiated.");
////        }
////        else
////        {
////            Debug.LogError("❌ Failed to load GLB model.");
////        }
////    }
////}
//// Assets/Scripts/ModelLoader.cs
//using UnityEngine;
//using UnityEngine.Networking;
//using System.IO;
//using System.Threading.Tasks;
//using GLTFast;

//public class ModelLoader : MonoBehaviour
//{
//    public string modelId;  // حط هنا الـ ID الخاص بالموديل (موجود في MongoDB)
//    public Transform spawnPoint;

//    private string GetModelUrl()
//    {
//        return $"http://localhost:3000/model/{modelId}";
//    }

//    public async void LoadModel()
//    {
//        string url = GetModelUrl();
//        string fileName = modelId + ".glb";  // اسم مؤقت لتخزين الملف
//        string filePath = Path.Combine(Application.persistentDataPath, fileName);

//        // تحميل الملف من الرابط وحفظه
//        using (UnityWebRequest www = UnityWebRequest.Get(url))
//        {
//            var operation = www.SendWebRequest();
//            while (!operation.isDone)
//                await Task.Yield();

//            if (www.result != UnityWebRequest.Result.Success)
//            {
//                Debug.LogError("Failed to download model: " + www.error);
//                return;
//            }

//            File.WriteAllBytes(filePath, www.downloadHandler.data);
//            Debug.Log("Model downloaded and saved to: " + filePath);
//        }

//        // تحميل وعرض الموديل باستخدام GLTFast
//        var gltf = new GltfImport();
//        bool success = await gltf.Load(filePath);
//        if (success)
//        {
//            GameObject modelRoot = new GameObject("LoadedModel");
//            gltf.InstantiateMainScene(modelRoot.transform);
//            modelRoot.transform.position = spawnPoint != null ? spawnPoint.position : Vector3.zero;
//            Debug.Log("Model loaded and instantiated");
//        }
//        else
//        {
//            Debug.LogError("Failed to load GLB model.");
//        }
//    }
//}

