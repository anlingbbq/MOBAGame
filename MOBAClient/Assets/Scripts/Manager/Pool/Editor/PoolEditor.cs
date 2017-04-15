using UnityEngine;
using System.Collections;
using UnityEditor;

public class PoolEditor : MonoBehaviour
{
    [MenuItem("Manager/Create PoolConfig")]
    static void CreatePoolList()
    {
        ObjectPoolList poolList = ScriptableObject.CreateInstance<ObjectPoolList>();
        string path = PoolManager.PoolConfigPath;
        AssetDatabase.CreateAsset(poolList, path);
        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("提示", "创建成功", "好的");
    }
}
