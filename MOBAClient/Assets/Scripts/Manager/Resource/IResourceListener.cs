using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceListener
{
    // 资源加载完成后的回掉
    void OnLoaded(string assetName, object asset, AssetType assetType);
}
