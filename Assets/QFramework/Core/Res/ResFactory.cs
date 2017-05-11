﻿using System;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using SCFramework;

namespace QFramework
{
    public class ResFactory
    {
        private delegate IRes ResCreator(string name);

        static ResFactory()
        {
            Log.i("Init[ResFactory]");
            ObjectPool<AssetBundleRes>.Instance.MaxCacheCount = 20;
			ObjectPool<AssetRes>.Instance.MaxCacheCount = 40;
			ObjectPool<InternalRes>.Instance.MaxCacheCount = 40;
			ObjectPool<NetImageRes>.Instance.MaxCacheCount = 20;
        }

        public static IRes Create(string name)
        {
            short assetType = 0;
            if (name.StartsWith("Resources/"))
            {
                assetType = eResType.kInternal;
            }
            else if (name.StartsWith("NetImage:"))
            {
                assetType = eResType.kNetImageRes;
            }
            else
            {
                AssetData data = AssetDataTable.Instance.GetAssetData(name);
                if (data == null)
                {
                    Log.e("Failed to Create Res. Not Find AssetData:" + name);
                    return null;
                }
                else
                {
                    assetType = data.assetType;
                }
            }

            return Create(name, assetType);
        }

        public static IRes Create(string name, short assetType)
        {
            switch (assetType)
            {
                case eResType.kAssetBundle:
                    return AssetBundleRes.Allocate(name);
                case eResType.kABAsset:
                    return AssetRes.Allocate(name);
                case eResType.kABScene:
                    return SceneRes.Allocate(name);
                case eResType.kInternal:
                    return InternalRes.Allocate(name);
                case eResType.kNetImageRes:
                    return NetImageRes.Allocate(name);
                default:
                    Log.e("Invalid assetType :" + assetType);
                    return null;
            }
        }
    }
}
