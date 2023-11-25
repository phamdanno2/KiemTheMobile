﻿using FSPlay.KiemVu.Entities.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static FSPlay.KiemVu.Entities.Enum;

namespace FSPlay.KiemVu.Factory.Animation
{
    /// <summary>
    /// Quản lý phi phong
    /// </summary>
    public partial class CharacterAnimationManager
    {
        /// <summary>
        /// Trả về danh sách ảnh phi phong tương ứng
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="resID"></param>
        /// <param name="weaponID"></param>
        /// <param name="sex"></param>
        /// <param name="isRiding"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public Dictionary<string, UnityEngine.Object> GetMantleSprites(PlayerActionType actionType, string resID, string weaponID, Sex sex, bool isRiding, Direction dir)
        {
            /// Nếu không tồn tại ResID
            if (string.IsNullOrEmpty(resID))
            {
                return null;
            }

            /// File quy định đường dẫn tương ứng
            CharacterActionSetXML actionSetXML = Loader.Loader.CharacterActionSetXML;

            /// Tên động tác
            string actionName = string.Format("{0}_{1}", this.GetActionName(actionType, weaponID, isRiding), (int) dir);
            /// Đường dẫn AssetBundle chứa động tác
            string bundleDir = string.Format("{0}/{1}.unity3d", (sex == Sex.FEMALE ? actionSetXML.FemaleMantle[resID].BundleDir : actionSetXML.MaleMantle[resID].BundleDir), actionName);

            /// Trả về kết quả
            return KTResourceManager.Instance.GetSubAssets(bundleDir, actionName);
        }

        /// <summary>
        /// Tải xuống ảnh phi phong tương ứng
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="resID"></param>
        /// <param name="weaponID"></param>
        /// <param name="sex"></param>
        /// <param name="isRiding"></param>
        /// <param name="dir"></param>
        /// <param name="callbackIfNeedToLoad"></param>
        /// <returns></returns>
        public IEnumerator LoadMantleSprites(PlayerActionType actionType, string resID, string weaponID, Sex sex, bool isRiding, Direction dir, Action callbackIfNeedToLoad)
        {
            /// Nếu không tồn tại ResID
            if (string.IsNullOrEmpty(resID))
            {
                yield break;
            }

            /// File quy định đường dẫn tương ứng
            CharacterActionSetXML actionSetXML = Loader.Loader.CharacterActionSetXML;

            /// Tên động tác
            string actionName = string.Format("{0}_{1}", this.GetActionName(actionType, weaponID, isRiding), (int) dir);
            /// Đường dẫn AssetBundle chứa động tác
            string bundleDir = string.Format("{0}/{1}.unity3d", (sex == Sex.FEMALE ? actionSetXML.FemaleMantle[resID].BundleDir : actionSetXML.MaleMantle[resID].BundleDir), actionName);

            /// Nếu Bundle chưa được tải xuống
            if (!KTResourceManager.Instance.HasBundle(bundleDir))
            {
                /// Thực hiện Callback đánh dấu cần Load
                callbackIfNeedToLoad?.Invoke();

                /// Nếu sử dụng phương thức Async
                if (CharacterAnimationManager.UseAsyncLoad)
                {
                    /// Tải xuống AssetBundle
                    yield return KTResourceManager.Instance.LoadAssetBundleAsync(bundleDir, false, KTResourceManager.KTResourceCacheType.CachedUntilChangeScene);
                    /// Tải xuống Asset tương ứng
                    yield return KTResourceManager.Instance.LoadAssetWithSubAssetsAsync<Sprite>(bundleDir, actionName, true, null, KTResourceManager.KTResourceCacheType.CachedUntilChangeScene);
                }
                /// Nếu sử dụng phương thức tải tuần tự
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
                /// Tăng tham chiếu
                KTResourceManager.Instance.GetAssetBundle(bundleDir);
                /// Nếu Asset chưa được tải xuống
                if (!KTResourceManager.Instance.HasAsset(bundleDir, actionName))
                {
                    /// Nếu sử dụng phương thức Async
                    if (CharacterAnimationManager.UseAsyncLoad)
                    {
                        /// Tải xuống Asset tương ứng
                        yield return KTResourceManager.Instance.LoadAssetWithSubAssetsAsync<Sprite>(bundleDir, actionName, true, null, KTResourceManager.KTResourceCacheType.CachedUntilChangeScene);
                    }
                    /// Nếu sử dụng phương thức tải tuần tự
                    else
                    {
                        /// Tải xuống Asset tương ứng
                        KTResourceManager.Instance.LoadAssetWithSubAssets<Sprite>(bundleDir, actionName, true, KTResourceManager.KTResourceCacheType.CachedUntilChangeScene);
                    }
                }
            }
        }

        /// <summary>
        /// Giải phóng Sprite phi phong đã tải xuống
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="resID"></param>
        /// <param name="weaponID"></param>
        /// <param name="sex"></param>
        /// <param name="isRiding"></param>
        /// <param name="dir"></param>
        public void UnloadMantleSprites(PlayerActionType actionType, string resID, string weaponID, Sex sex, bool isRiding, Direction dir)
        {
            /// Nếu không tồn tại ResID
            if (string.IsNullOrEmpty(resID))
            {
                return;
            }

            /// File quy định đường dẫn tương ứng
            CharacterActionSetXML actionSetXML = Loader.Loader.CharacterActionSetXML;

            /// Tên động tác
            string actionName = string.Format("{0}_{1}", this.GetActionName(actionType, weaponID, isRiding), (int) dir);
            /// Đường dẫn AssetBundle chứa động tác
            string bundleDir = string.Format("{0}/{1}.unity3d", (sex == Sex.FEMALE ? actionSetXML.FemaleMantle[resID].BundleDir : actionSetXML.MaleMantle[resID].BundleDir), actionName);

            /// Giải phóng AssetBundle
            KTResourceManager.Instance.ReleaseBundle(bundleDir);
        }
    }
}
