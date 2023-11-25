using Assets.Scripts.FSPlay.KiemVu.Entities.Config;
using FSPlay.KiemVu.UI.Main.ActivityList;
using FSPlay.KiemVu.Utilities.UnityUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSPlay.KiemVu.UI.Main
{
    /// <summary>
    /// Khung danh sách hoạt động
    /// </summary>
    public class UIActivityList : MonoBehaviour
    {
        #region Define
        /// <summary>
        /// Thông tin ngày
        /// </summary>
        [Serializable]
        private class DayInfo
        {
            /// <summary>
            /// Ngày
            /// </summary>
            public int Day;

            /// <summary>
            /// Toggle
            /// </summary>
            public UIToggleSprite UIToggle;
        }

        /// <summary>
        /// Button đóng khung
        /// </summary>
        [SerializeField]
        private UnityEngine.UI.Button UIButton_Close;

        /// <summary>
        /// Prefab thông tin hoạt động
        /// </summary>
        [SerializeField]
        private UIActivityList_ActivityInfo UI_ActivityInfoPrefab;

        /// <summary>
        /// Danh sách theo ngày
        /// </summary>
        [SerializeField]
        private DayInfo[] UI_DayOfWeek;
        #endregion

        #region Private fields
        /// <summary>
        /// RectTransform danh sách hoạt động
        /// </summary>
        private RectTransform transformActivityList;
        #endregion

        #region Properties
        /// <summary>
        /// Sự kiện đóng khung
        /// </summary>
        public Action Close { get; set; }
        #endregion

        #region Core MonoBahaviour
        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Awake()
        {
            this.transformActivityList = this.UI_ActivityInfoPrefab.transform.parent.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Hàm này gọi ở Frame đầu tiên
        /// </summary>
        private void Start()
        {
            this.InitPrefabs();
        }
        #endregion

        #region Code UI
        /// <summary>
        /// Khởi tạo ban đầu
        /// </summary>
        private void InitPrefabs()
        {
            this.UIButton_Close.onClick.AddListener(this.ButtonClose_Clicked);
            /// Duyệt danh sách Toggle ngày trong tuần
            foreach (DayInfo dayInfo in this.UI_DayOfWeek)
            {
                /// Thiết lập sự kiện
                dayInfo.UIToggle.OnSelected = (isSelected) => {
                    if (isSelected)
                    {
                        this.ToggleDay_Selected(dayInfo.Day);
                    }
                };
            }

            /// Xem ngày hôm nay là thứ mấy
            DayOfWeek todayDay = new DateTime(KTGlobal.GetServerTime() * TimeSpan.TicksPerMillisecond).DayOfWeek;
            int todayDayInt = -1;
            switch (todayDay)
            {
                case DayOfWeek.Monday:
                {
                    todayDayInt = 2;
                    break;
                }
                case DayOfWeek.Tuesday:
                {
                    todayDayInt = 3;
                    break;
                }
                case DayOfWeek.Wednesday:
                {
                    todayDayInt = 4;
                    break;
                }
                case DayOfWeek.Thursday:
                {
                    todayDayInt = 5;
                    break;
                }
                case DayOfWeek.Friday:
                {
                    todayDayInt = 6;
                    break;
                }
                case DayOfWeek.Saturday:
                {
                    todayDayInt = 7;
                    break;
                }
                case DayOfWeek.Sunday:
                {
                    todayDayInt = 8;
                    break;
                }
            }
            /// Chọn ngày hôm nay
            this.ToggleDay_Selected(todayDayInt);
        }

        /// <summary>
        /// Sự kiện khi Button đóng khung được ấn
        /// </summary>
        private void ButtonClose_Clicked()
        {
            this.Close?.Invoke();
        }

        /// <summary>
        /// Sự kiện khi Toggle ngày tương ứng được chọn
        /// </summary>
        /// <param name="day"></param>
        private void ToggleDay_Selected(int day)
        {
            /// Thông tin ngày tương ứng
            DayInfo dayInfo = this.UI_DayOfWeek.Where(x => x.Day == day).FirstOrDefault();
            /// Nếu tìm thấy
            if (dayInfo != null)
            {
                /// Đánh dấu Toggle ngày hôm nay
                dayInfo.UIToggle.Active = true;
            }
            /// Làm mới hiển thị
            this.Refresh();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Thực thi sự kiện bỏ qua một số Frame
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="work"></param>
        /// <returns></returns>
        private IEnumerator ExecuteSkipFrames(int skip, Action work)
        {
            for (int i = 1; i <= skip; i++)
            {
                yield return null;
            }
            work?.Invoke();
        }

        /// <summary>
        /// Xây lại giao diện danh sách vật phẩm
        /// </summary>
        private void RebuildLayout()
        {
            /// Nếu đối tượng không được kích hoạt
            if (!this.gameObject.activeSelf)
            {
                return;
            }
            /// Thực thi xây lại giao diện ở Frame tiếp theo
            this.StartCoroutine(this.ExecuteSkipFrames(1, () => {
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transformActivityList);
            }));
        }

        /// <summary>
        /// Làm rỗng danh sách hoạt động
        /// </summary>
        private void ClearActivityList()
        {
            foreach (Transform child in this.transformActivityList.transform)
            {
                if (child.gameObject != this.UI_ActivityInfoPrefab.gameObject)
                {
                    UIActivityList_ActivityInfo uiActivityInfo = child.GetComponent<UIActivityList_ActivityInfo>();
                    uiActivityInfo.Data = null;
                    uiActivityInfo.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// TÌm một vị trí trống chưa có hoạt động
        /// </summary>
        /// <returns></returns>
        private UIActivityList_ActivityInfo FindEmptySlot()
        {
            foreach (Transform child in this.transformActivityList.transform)
            {
                if (child.gameObject != this.UI_ActivityInfoPrefab.gameObject)
                {
                    UIActivityList_ActivityInfo uiActivityInfo = child.GetComponent<UIActivityList_ActivityInfo>();
                    if (uiActivityInfo.Data == null)
                    {
                        return uiActivityInfo;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Thêm hoạt động tương ứng vào danh sách
        /// </summary>
        /// <param name="activityInfo"></param>
        private void AppendActivity(ActivityXML activityInfo)
        {
            /// Vị trí trống
            UIActivityList_ActivityInfo uiActivityInfo = this.FindEmptySlot();
            /// Nếu tìm thấy
            if (uiActivityInfo != null)
            {
                uiActivityInfo.Data = activityInfo;
                uiActivityInfo.gameObject.SetActive(true);
            }
            /// Nếu không tìm thấy
            else
            {
                /// Tạo mới
                uiActivityInfo = GameObject.Instantiate<UIActivityList_ActivityInfo>(this.UI_ActivityInfoPrefab);
                uiActivityInfo.transform.SetParent(this.transformActivityList, false);
                uiActivityInfo.gameObject.SetActive(true);
                uiActivityInfo.Data = activityInfo;
            }
        }

        /// <summary>
        /// Làm mới hiển thị
        /// </summary>
        private void Refresh()
        {
            /// Làm rỗng danh sách hoạt động
            this.ClearActivityList();

            /// Đang chọn thứ mấy
            DayInfo dayInfo = this.UI_DayOfWeek.Where(x => x.UIToggle.Active).FirstOrDefault();
            /// Nếu không chọn thì chọn ngày đầu tiên
            if (dayInfo == null)
            {
                dayInfo = this.UI_DayOfWeek.FirstOrDefault();
            }

            /// Danh sách hoạt động có trong thứ tương ứng
            List<ActivityXML> activities = Loader.Loader.Activities.Values.Where(x => x.DayOfWeek.Contains(dayInfo.Day)).ToList();
            /// Duyệt danh sách hoạt động
            foreach (ActivityXML activityInfo in activities)
            {
                this.AppendActivity(activityInfo);
            }

            /// Xây lại giao diện
            this.RebuildLayout();
        }
        #endregion
    }
}
