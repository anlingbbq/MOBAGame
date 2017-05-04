using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceListener
{
    /// <summary>
    /// 资源加载完成后的回掉 
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="asset"></param>
    /// <param name="assetType"></param>
    void OnLoaded(string assetName, object asset);
}
