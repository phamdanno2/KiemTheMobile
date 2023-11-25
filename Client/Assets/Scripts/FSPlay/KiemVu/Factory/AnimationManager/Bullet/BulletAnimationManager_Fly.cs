using FSPlay.KiemVu.Entities.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.Factory.AnimationManager
{
	/// <summary>
	/// Quản lý hiệu ứng đạn
	/// </summary>
	public partial class BulletAnimationManager
	{
        /// <summary>
        /// Trả về danh sách ảnh động tác bay tương ứng
        /// </summary>
        /// <param name="resID"></param>
        /// <param name="dir16"></param>
        /// <returns></returns>
        public Dictionary<string, UnityEngine.Object> GetFlySprites(int resID, Direction16 dir16)
        {
            /// File quy định đường dẫn tương ứng
            BulletActionSetXML actionSetXML = Loader.Loader.BulletActionSetXML;

            /// Nếu Res không tồn tại
            if (!actionSetXML.ResDatas.TryGetValue(resID, out BulletActionSetXML.BulletResData bulletData))
            {
                return null;
            }

            /// Nếu không có hiệu ứng bay
            if (!bulletData.HasFlyAction)
            {
                return null;
            }

            /// Tên động tác
            string actionName;
            /// Nếu dùng 16 hướng
            if (actionSetXML.ResDatas[resID].Use16Dir)
            {
                actionName = string.Format("AnimFile2_{0}", (int) dir16);
            }
            else
            {
                actionName = "AnimFile2";
            }

            /// File AssetBundle tương ứng
            string bundleDir = string.Format("{0}/{1}.unity3d", bulletData.BundleDir, actionName);

            /// Trả về kết quả
            return KTResourceManager.Instance.GetSubAssets(bundleDir, actionName);
        }

        /// <summary>
        /// Tải xuống Sprite và thêm vào danh sách trong Cache (nếu chưa được tải xuống)
        /// </summary>
        /// <param name="resID"></param>
        /// <param name="dir16"></param>
        /// <param name="callbackIfNeedToLoad"></param>
        /// <returns></returns>
        public IEnumerator LoadFlySprites(int resID, Direction16 dir16, Action callbackIfNeedToLoad = null)
        {
            /// Thông tin Res
            BulletActionSetXML actionSetXML = Loader.Loader.BulletActionSetXML;

            /// Nếu Res không tồn tại
            if (!actionSetXML.ResDatas.TryGetValue(resID, out BulletActionSetXML.BulletResData bulletData))
            {
                yield break;
            }

            /// Nếu không có hiệu ứng bay
            if (!bulletData.HasFlyAction)
            {
                yield break;
            }

            /// Tên động tác
            string actionName;
            /// Nếu dùng 16 hướng
            if (bulletData.Use16Dir)
            {
                actionName = string.Format("AnimFile2_{0}", (int) dir16);
            }
            else
            {
                actionName = "AnimFile2";
            }

            /// File AssetBundle tương ứng
            string bundleDir = string.Format("{0}/{1}.unity3d", bulletData.BundleDir, actionName);

            /// Nếu Bundle chưa được tải xuống
            if (!KTResourceManager.Instance.HasBundle(bundleDir))
            {
                /// Thực hiện Callback đánh dấu cần Load
                callbackIfNeedToLoad?.Invoke();

                /// Nếu sử dụng phương thức Async
                if (BulletAnimationManager.UseAsyncLoad)
                {
                    /// Tải xuống AssetBundle
                    yield return KTResourceManager.Instance.LoadAssetBundleAsync(bundleDir, false, KTResourceManager.KTResourceCacheType.CachedUntilChangeScene);
                    /// Tải xuống Asset tương ứng
                    yield return KTResourceManager.Instance.LoadAssetWithSubAssetsAsync<Sprite>(bundleDir, actionName, true, null, KTResourceManager.KTResourceCacheType.CachedUntilChangeScene);
                }
                /// Nếu sử dụng phương thức tuần tự
                else
                {
                    /// Tải xuống AssetBundle
                    KTResourceManager.Instance.LoadAssetBundle(bundleDir, false, KTResourceManager.KTResourceCacheType.CachedUntilChangeScene);
                    /// Tải xuống Asset tương ứng
                    KTResourceManager.Instance.LoadAssetWithSubAssets<Sprite>(bundleDir, actionName, true, KTResourceManager.KTResourceCacheType.CachedUntilChangeScene);
                }
            }
            /// Nếu Bundle đã được tải xuống
            else
            {
                /// Tăng tham chiếu Bundle
                KTResourceManager.Instance.GetAssetBundle(bundleDir);

                /// Nếu Asset chưa được tải xuống
                if (!KTResourceManager.Instance.HasAsset(bundleDir, actionName))
                {
                    /// Nếu sử dụng phương thức Async
                    if (BulletAnimationManager.UseAsyncLoad)
                    {
                        /// Tải xuống Asset tương ứng
                        yield return KTResourceManager.Instance.LoadAssetWithSubAssetsAsync<Sprite>(bundleDir, actionName, true, null, KTResourceManager.KTResourceCacheType.CachedUntilChangeScene);
                    }
                    /// Nếu sử dụng phương thức tuần tự
                    else
                    {
                        /// Tải xuống Asset tương ứng
                        KTResourceManager.Instance.LoadAssetWithSubAssets<Sprite>(bundleDir, actionName, true, KTResourceManager.KTResourceCacheType.CachedUntilChangeScene);
                    }
                }
            }
        }

        /// <summary>
        /// Giải phóng Sprite đã tải xuống
        /// </summary>
        /// <param name="resID"></param>
        /// <param name="dir16"></param>
        public void UnloadFlySprites(int resID, Direction16 dir16)
        {
            /// Thông tin Res
            BulletActionSetXML actionSetXML = Loader.Loader.BulletActionSetXML;

            /// Nếu Res không tồn tại
            if (!actionSetXML.ResDatas.TryGetValue(resID, out BulletActionSetXML.BulletResData bulletData))
            {
                return;
            }

            /// Nếu không có hiệu ứng bay
            if (!bulletData.HasFlyAction)
            {
                return;
            }

            /// Tên động tác
            string actionName;
            /// Nếu dùng 16 hướng
            if (bulletData.Use16Dir)
            {
                actionName = string.Format("AnimFile2_{0}", (int) dir16);
            }
            else
            {
                actionName = "AnimFile2";
            }

            /// File AssetBundle tương ứng
            string bundleDir = string.Format("{0}/{1}.unity3d", bulletData.BundleDir, actionName);

            /// Giải phóng AssetBundle
            KTResourceManager.Instance.ReleaseBundle(bundleDir);
        }
    }
}
