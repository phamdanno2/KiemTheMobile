using FSPlay.GameEngine.Logic;
using FSPlay.GameFramework.Logic;
using FSPlay.KiemVu.Utilities.UnityComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace FSPlay.KiemVu.Control.Component
{
    /// <summary>
    /// Đối tượng bản đồ
    /// </summary>
    public partial class Map
    {
        /// <summary>
        /// Tải ảnh map có Layer tương ứng
        /// </summary>
        /// <param name="layerID"></param>
        /// <returns></returns>
        private Sprite LoadMapSprite(int layerID)
        {
            string url = Global.WebPath(string.Format("Data/{0}/{1}.unity3d", FSPlay.KiemVu.Loader.Loader.Maps[this.MapCode].ImageFolder, layerID));
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
            request.SendWebRequest();

            /// Chừng nào chưa hoàn thành thì đợi
            while (!request.isDone)
            {
                /// Do nothing
            }

            /// Bundle tương ứng
            AssetBundle mapImageBundle = DownloadHandlerAssetBundle.GetContent(request);
            /// Giải phóng bộ nhớ
            request.downloadHandler.Dispose();
            request.Dispose();

            /// Nếu tồn tại
            if (mapImageBundle != null)
            {
                Sprite sprite = mapImageBundle.LoadAssetWithSubAssets<Sprite>(layerID.ToString())[0];

                /// Thêm vào danh sách đã tải
                this.ListMapSprites[layerID] = sprite;

                /// Xóa Bundle tương ứng
                mapImageBundle.Unload(false);
                GameObject.Destroy(mapImageBundle);

                /// Trả về kết quả
                return sprite;
            }
            /// Toác
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Tải ảnh map có Layer tương ứng
        /// </summary>
        /// <param name="layerID"></param>
        /// <param name="partID"></param>
        /// <param name="done"></param>
        /// <returns></returns>
        private IEnumerator LoadMapSpriteAsync(int layerID, int partID, Action<int, Sprite> done)
        {
            string url = Global.WebPath(string.Format("Data/{0}/{1}.unity3d", FSPlay.KiemVu.Loader.Loader.Maps[this.MapCode].ImageFolder, layerID));
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
            yield return request.SendWebRequest();

            /// Bundle tương ứng
            AssetBundle mapImageBundle = DownloadHandlerAssetBundle.GetContent(request);
            /// Giải phóng bộ nhớ
            request.downloadHandler.Dispose();
            request.Dispose();

            /// Nếu tồn tại
            if (mapImageBundle != null)
            {
                AssetBundleRequest assetRequest = mapImageBundle.LoadAssetWithSubAssetsAsync<Sprite>(layerID.ToString());
                yield return assetRequest;
                Sprite sprite = assetRequest.allAssets[0] as Sprite;

                /// Thêm vào danh sách đã tải
                this.ListMapSprites[layerID] = sprite;

                /// Xóa Bundle tương ứng
                mapImageBundle.Unload(false);
                GameObject.Destroy(mapImageBundle);

                /// Thực hiện Callback khi hoàn tất
                done?.Invoke(partID, sprite);
            }
            /// Toác
            else
            {
                done?.Invoke(partID, null);
            }
        }

        /// <summary>
        /// Tải tài nguyên bản đồ 2D từ Prefab
        /// </summary>
        private IEnumerator Load2DMap()
        {
            #region Map values
            int mapWidth = FSPlay.KiemVu.Loader.Loader.Maps[this.MapCode].Width;
            int mapHeight = FSPlay.KiemVu.Loader.Loader.Maps[this.MapCode].Height;
            int partWidth = FSPlay.KiemVu.Loader.Loader.Maps[this.MapCode].PartWidth;
            int partHeight = FSPlay.KiemVu.Loader.Loader.Maps[this.MapCode].PartHeight;
            int totalHorizontal = FSPlay.KiemVu.Loader.Loader.Maps[this.MapCode].HorizontalCount;
            int totalVertical = FSPlay.KiemVu.Loader.Loader.Maps[this.MapCode].VerticalCount;
            string folderDir = FSPlay.KiemVu.Loader.Loader.Maps[this.MapCode].ImageFolder;
            int totalParts = totalHorizontal * totalVertical;

            this.ListRenderers = new List<SpriteRenderer>();
            this.ListMapSprites = new Dictionary<int, Sprite>();
            this.name = FSPlay.KiemVu.Loader.Loader.Maps[this.MapCode].Name;

            this.gameObject.AddComponent<AudioSource>();
            this.MapMusic = this.gameObject.AddComponent<AudioPlayer>();
            #endregion

            #region Create scene gameobject
            this.ReportProgress?.Invoke(1, "Đang khởi tạo bản đồ");

            {
                this.gameObject.layer = 8;

                /// Vị trí gốc
                this.gameObject.transform.localPosition = Vector2.zero;

                int layerID = 0;
                /// Duyệt danh sách chiều ngang
                for (int partX = 0; partX < this.DynamicViewCellSize.x; partX++)
                {
                    /// Duyệt danh sách chiều dọc
                    for (int partY = 0; partY < this.DynamicViewCellSize.y; partY++)
                    {
                        layerID++;
                        GameObject go = new GameObject(string.Format("Map dynamic image - {0}", layerID));
                        go.transform.SetParent(this.gameObject.transform, false);
                        go.layer = this.gameObject.layer;
                        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
                        renderer.sprite = null;
                        this.ListRenderers.Add(renderer);
                    }
                }
            }
            yield return null;
            #endregion

            #region Load music
            this.ReportProgress?.Invoke(10, "Đang đọc dữ liệu nhạc nền bản đồ");

            {
                string url = Global.WebPath(string.Format("Data/{0}", FSPlay.KiemVu.Loader.Loader.Maps[this.MapCode].MusicBundle));
                UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
                yield return request.SendWebRequest();

                AssetBundle musicBundle = DownloadHandlerAssetBundle.GetContent(request);
                /// Giải phóng bộ nhớ
                request.downloadHandler.Dispose();
                request.Dispose();

                if (musicBundle != null)
                {

                    this.MapMusic.ActivateAfter = 0f;
                    this.MapMusic.IsRepeat = true;
                    this.MapMusic.RepeatTimer = 10f;

                    AssetBundleRequest _request = musicBundle.LoadAssetAsync<AudioClip>("Music");
                    yield return _request;
                    this.MapMusic.Sound = _request.asset as AudioClip;

                    /// Hủy bundle tương ứng
                    musicBundle.Unload(false);
                    GameObject.Destroy(musicBundle);

                    // KTDebug.LogError("Load asset bundle -> " + FSPlay.KiemVu.Loader.Loader.Maps[this.MapCode].MusicBundle + " - SUCCESS");
                }
                else
                {
                    KTDebug.LogError("Load asset bundle -> " + FSPlay.KiemVu.Loader.Loader.Maps[this.MapCode].MusicBundle + " - FAILD");
                }
            }
            #endregion

            #region Tải các thành phần
            this.ReportProgress?.Invoke(20, "Đang đọc dữ liệu thành phần bản đồ");

            /// Tổng số phần đã tải xong
            int totalLoadedPart = 0;
            int totalRequireParts = this.DynamicViewCellSize.x * this.DynamicViewCellSize.y;
            /// Thực hiện tải
            this.DoFollowLeaderLogic(this.LeaderPosition, (layerID) => {
                /// Tăng số phần đã tải
                totalLoadedPart++;
                /// % đã tải
                int loadedPercent = (int) (totalLoadedPart / (float) totalRequireParts * (95 - 20)) + 20;
                this.ReportProgress?.Invoke(loadedPercent, "Đang đọc dữ liệu thành phần bản đồ");
            });

            /// Chừng nào chưa tải xong
            while (totalLoadedPart < totalRequireParts)
            {
                /// Đợi
                yield return null;
            }
            #endregion

            #region Load minimap
            this.ReportProgress?.Invoke(95, "Đang đọc dữ liệu bản đồ thu nhỏ");

            /// Nếu bản đồ hiện tại có hiện bản đồ nhỏ ở góc
            if (Loader.Loader.Maps[this.MapCode].ShowMiniMap)
            {
                string url = Global.WebPath(string.Format("Data/{0}/{1}.unity3d", FSPlay.KiemVu.Loader.Loader.Maps[this.MapCode].ImageFolder, "Minimap"));
                UnityWebRequest _request = UnityWebRequestAssetBundle.GetAssetBundle(url);
                yield return _request.SendWebRequest();

                AssetBundle mapImageBundle = DownloadHandlerAssetBundle.GetContent(_request);
                /// Giải phóng bộ nớ
                _request.downloadHandler.Dispose();
                _request.Dispose();

                if (mapImageBundle != null)
                {
                    AssetBundleRequest request = mapImageBundle.LoadAssetWithSubAssetsAsync<Sprite>("Minimap");
                    yield return request;

                    Sprite sprite = request.allAssets[0] as Sprite;
                    this.LocalMapSprite = sprite;

                    /// Hủy Bundle tương ứng
                    mapImageBundle.Unload(false);
                    GameObject.Destroy(mapImageBundle);
                }
            }
            else
            {
                this.LocalMapSprite = null;
            }
            #endregion

            #region Hoàn tất
            /// Phát nhạc nền map
            this.MapMusic.Play();

            this.ReportProgress?.Invoke(100, "Đọc dữ liệu bản đồ hoàn tất");
            yield return new WaitForSeconds(0.1f);

            this.Finish?.Invoke();
            Super.DestroyLoadingMap();
            #endregion
        }
    }
}
