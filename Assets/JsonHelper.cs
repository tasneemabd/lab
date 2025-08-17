public static class JsonHelper
{
    public static string GetJsonArray(string json, string key)
    {
        int start = json.IndexOf($"\"{key}\":[");
        if (start == -1) return "[]";
        start += key.Length + 3;
        int end = json.IndexOf("]", start);
        var array = json.Substring(start, end - start + 1);
        return array;
    }
}
