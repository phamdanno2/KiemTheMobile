﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using FSPlay.KiemVu.Utilities.UnityUI;
using FSPlay.KiemVu.Utilities.UnityComponent;
using System.Collections;
using Server.Data;

namespace FSPlay.KiemVu.UI.Main
{
    /// <summary>
    /// Khung thông tin vật phẩm khi nhặt được
    /// </summary>
    public class UIHintItemText : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Image Icon vật phẩm
        /// </summary>
        [SerializeField]
        private SpriteFromAssetBundle UIImage_ItemIcon;

        /// <summary>
        /// Text tên vật phẩm
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_ItemName;

        /// <summary>
        /// Text số lượng vật phẩm
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI UIText_ItemQuantity;

        /// <summary>
        /// Vị trí bay lên
        /// </summary>
        [SerializeField]
        private Vector2 _UpPos;

        /// <summary>
        /// Vị trí kết thúc
        /// </summary>
        [SerializeField]
        private Vector2 _EndPos;

        /// <summary>
        /// Thời gian Delay trước khi thực thi hiệu ứng
        /// </summary>
        [SerializeField]
        private float _Delay = 0.5f;

        /// <summary>
        /// Thời gian thực thi hiệu ứng phóng to
        /// </summary>
        [SerializeField]
        private float _ZoomInDuration = 0.5f;

        /// <summary>
        /// Thời gian thực thi hiệu ứng thu nhỏ và bay về Button túi đồ
        /// </summary>
        [SerializeField]
        private float _ZoomOutDuration = 1f;

        /// <summary>
        /// Vector phóng to đến giá trị
        /// </summary>
        [SerializeField]
        private Vector3 _ScaleTo = new Vector3(1.5f, 1.5f, 1.5f);
        #endregion

        #region Private fields
        /// <summary>
        /// Thành phần Scale theo nhóm
        /// </summary>
        private GroupScale groupScale = null;

        /// <summary>
        /// RectTransform của khung
        /// </summary>
        private RectTransform rectTransform = null;
        #endregion

        #region Properties
        private GoodsData _Data;
        /// <summary>
        /// Dữ liệu vật phẩm
        /// </summary>
        public GoodsData Data
        {
            get
            {
                return this._Data;
            }
            set
            {
                this._Data = value;

                /// Nếu không có Data
                if (value == null)
                {
                    this.Destroy();
                }
                /// Nếu có ItemData
                else if (Loader.Loader.Items.TryGetValue(value.GoodsID, out Entities.Config.ItemData itemData))
                {
                    this.UIText_ItemName.text = KTGlobal.GetItemName(value);
                    this.UIText_ItemName.color = KTGlobal.GetItemColor(value);
                    this.UIImage_ItemIcon.BundleDir = itemData.IconBundleDir;
                    this.UIImage_ItemIcon.AtlasName = itemData.IconAtlasName;
                    this.UIImage_ItemIcon.SpriteName = itemData.Icon;
                    try
                    {
                        this.UIImage_ItemIcon.Load();
                    }
                    catch (Exception ex)
                    {
                        KTDebug.LogError(ex.ToString());

                        /// Load icon mặc định
                        this.UIImage_ItemIcon.SpriteName = "hoicham";
                        this.UIImage_ItemIcon.BundleDir = "Icon/EquipIcon1.unity3d";
                        this.UIImage_ItemIcon.AtlasName = "EquipIcon1";
                        this.UIImage_ItemIcon.Load();
                    }
                }
                /// Nếu không có ItemData
                else
                {
                    this.Destroy();
                } 
            }
        }

        private int _ItemQuantity = 1;
        /// <summary>
        /// Số lượng vật phẩm
        /// </summary>
        public int ItemQuantity
        {
            get
            {
                return this._ItemQuantity;
            }
            set
            {
                this._ItemQuantity = value;
                this.UIText_ItemQuantity.text = string.Format("SL: {0}", this._ItemQuantity);
            }
        }

        /// <summary>
        /// Thời gian thực thi hiệu ứng phóng to
        /// </summary>
        public float ZoomInDuration
        {
            get
            {
                return this._ZoomInDuration;
            }
            set
            {
                this._ZoomInDuration = value;
            }
        }

        /// <summary>
        /// Thời gian thực thi hiệu ứng thu nhỏ
        /// </summary>
        public float ZoomOutDuration
        {
            get
            {
                return this._ZoomOutDuration;
            }
            set
            {
                this._ZoomOutDuration = value;
            }
        }

        /// <summary>
        /// Phóng to đến giá trị
        /// </summary>
        public Vector3 ScaleTo
        {
            get
            {
                return this._ScaleTo;
            }
            set
            {
                this._ScaleTo = value;
            }
        }
        #endregion

        #region Core MonoBehaviour
        /// <summary>
        /// Hàm này gọi khi đối tượng được tạo ra
        /// </summary>
        private void Awake()
        {
            this.groupScale = this.gameObject.GetComponent<GroupScale>();
            this.rectTransform = this.gameObject.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
            this.RebuildLayout();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {

        }
        #endregion

        #region Private methods
        /// <summary>
        /// Xây lại giao diện
        /// </summary>
        private void RebuildLayout()
        {
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTransform);
        }

        /// <summary>
        /// Luồng thực thi hiệu ứng phóng to, thu nhỏ
        /// </summary>
        /// <returns></returns>
        /// <param name="fromVector"></param>
        /// <param name="toVector"></param>
        /// <param name="duration"></param>
        private IEnumerator DoZoom(Vector3 fromVector, Vector3 toVector, float duration)
        {
            this.groupScale.Scale = fromVector;
            float lifeTime = 0f;
            this.RebuildLayout();
            yield return null;

            Vector3 diffVector = toVector - fromVector;
            while (true)
            {
                lifeTime += Time.deltaTime;
                if (lifeTime >= duration)
                {
                    break;
                }

                float percent = lifeTime / duration;
                this.groupScale.Scale = fromVector + diffVector * percent;
                this.RebuildLayout();

                yield return null;
            }
            this.groupScale.Scale = toVector;
            this.RebuildLayout();
            yield return null;
        }

        /// <summary>
        /// Luồng thực hiện di chuyển đối tượng giữa 2 vị trí
        /// </summary>
        /// <param name="fromPos"></param>
        /// <param name="toPos"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator MoveTo(Vector2 fromPos, Vector2 toPos, float duration)
        {
            this.rectTransform.anchoredPosition = fromPos;
            float lifeTime = 0f;
            yield return null;

            Vector2 diffVector = toPos - fromPos;
            while (true)
            {
                lifeTime += Time.deltaTime;
                if (lifeTime >= duration)
                {
                    break;
                }

                float percent = lifeTime / duration;
                this.rectTransform.anchoredPosition = fromPos + diffVector * percent;

                yield return null;
            }
            this.rectTransform.anchoredPosition = toPos;
            yield return null;
        }

        /// <summary>
        /// Luồng thực hiện hiệu ứng
        /// </summary>
        /// <returns></returns>
        private IEnumerator DoPlay()
        {
            /// Delay rồi mới thực hiện Animation
            yield return new WaitForSeconds(this._Delay);
            /// Thực hiện di chuyển đến vị trí bay lên trong khoảng thời gian thực hiện hiệu ứng phóng to
            this.StartCoroutine(this.MoveTo(this.rectTransform.anchoredPosition, this._UpPos, this.ZoomOutDuration));
            /// Thực hiện phóng to từ kích thước hiện tại
            yield return this.DoZoom(this.rectTransform.localScale, this._ScaleTo, this._ZoomInDuration);
            /// Thực hiện di chuyển đến vị trí kết thúc trong khoảng thời gian thực hiện hiệu ứng thu nhỏ
            this.StartCoroutine(this.MoveTo(this.rectTransform.anchoredPosition, this._EndPos, this._ZoomOutDuration));
            /// Thực hiện hiệu ứng thu nhỏ
            yield return this.DoZoom(this.rectTransform.localScale, Vector3.zero, this._ZoomOutDuration);

            /// Thực thi xong thì hủy đối tượng
            this.Destroy();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Thực hiện biểu diễn
        /// </summary>
        public void Play()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.DoPlay());
        }

        /// <summary>
        /// Xóa đối tượng
        /// </summary>
        public void Destroy()
        {
            GameObject.Destroy(this.gameObject);
        }
        #endregion
    }
}
