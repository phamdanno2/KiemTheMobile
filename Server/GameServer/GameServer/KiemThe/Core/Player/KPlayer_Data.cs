using GameServer.Core.Executor;
using GameServer.KiemThe;
using GameServer.KiemThe.Core.Activity.CardMonth;
using GameServer.KiemThe.Core.Item;
using GameServer.KiemThe.Core.Title;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Entities.Player;
using GameServer.KiemThe.Logic;

using Server.Data;
using Server.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GameServer.Logic
{
    /// <summary>
    /// Quản lý các thuộc tính cơ bản
    /// </summary>
    public partial class KPlayer
    {
        /// Prams ghi chép toàn bộ lại việc đánh dấu của người chơi



        #region Recore Event

        /// Nhận quà tải game chưa
        public int ReviceBounsDownload { get; set; }

        public DailyDataRecore _DailyDataRecore { get; set; } = new DailyDataRecore();

        public WeekDataRecore _WeekDataRecore { get; set; } = new WeekDataRecore();

        public ForeverRecore _ForeverRecore { get; set; } = new ForeverRecore();

        /// <summary>
        /// Set giá trị lưu trữ vĩnh viễn
        /// Key cần def kỹ để tránh bị trùng với các key khác
        /// Tham khảo tại Enumration để biết quy tắc đặt key sao cho không trùng
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void SetValueOfForeverRecore(RecoreDef Key, int Value)
        {  // Lấy ra ngày trong năm
            lock (_ForeverRecore)
            {
                // Thử xem đã có key này trong nhật ký ghi chép chưa
                if (_ForeverRecore.EventRecoding.ContainsKey((int)Key))
                {
                    // Nếu có rồi thì thay thế key cũ
                    _ForeverRecore.EventRecoding[(int)Key] = Value;
                }
                else // Nếu key này chưa có trong nhật ký ghi chép thì tạo mới
                {
                    _ForeverRecore.EventRecoding.Add((int)Key, Value);
                }
            }
        }
        /// <summary>
        /// Lấy ra giá trị key lưu trũ vĩnh viễn
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public int GetValueOfForeverRecore(RecoreDef Key)
        {
            lock (_ForeverRecore)
            {
                if (_ForeverRecore.EventRecoding.ContainsKey((int)Key))
                {
                    return _ForeverRecore.EventRecoding[(int)Key];
                }
                else
                {
                    return -1;
                }
            }
        }

        public void SetValueOfDailyRecore(int Key, int Value)
        {  // Lấy ra ngày trong năm
            int Day = DateTime.Now.DayOfYear;

            lock (_DailyDataRecore)
            {
                // Check xem nếu mà thời gian khác thì tạo mới thực thể
                if (_DailyDataRecore.DayID != Day)
                {
                    _DailyDataRecore = new DailyDataRecore();
                    _DailyDataRecore.DayID = Day;
                }

                // Thử xem đã có key này trong nhật ký ghi chép chưa
                if (_DailyDataRecore.EventRecoding.ContainsKey(Key))
                {
                    // Nếu có rồi thì thay thế key cũ
                    _DailyDataRecore.EventRecoding[Key] = Value;
                }
                else // Nếu key này chưa có trong nhật ký ghi chép thì tạo mới
                {
                    _DailyDataRecore.EventRecoding.Add(Key, Value);
                }
            }
        }

        public int GetValueOfDailyRecore(int Key)
        {
            // Lấy ra ngày trong năm
            int Day = DateTime.Now.DayOfYear;

            lock (_DailyDataRecore)
            {
                // Nếu thông tin lưu trong nhật ký không phải ngày mới nhất thì thôi luôn
                if (_DailyDataRecore.DayID != Day)
                {
                    _DailyDataRecore = new DailyDataRecore();
                    _DailyDataRecore.DayID = Day;
                    return -1;
                }

                if (_DailyDataRecore.EventRecoding.ContainsKey(Key))
                {
                    return _DailyDataRecore.EventRecoding[Key];
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Funtion xử lý việc ghi chép các giá trị trong tuần
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void SetValueOfWeekRecore(int Key, int Value)
        {
            // Lấy ra tuần trong năm
            int WeekID = TimeUtil.GetIso8601WeekOfYear(DateTime.Now);

            lock (_WeekDataRecore)
            {
                // Check xem nếu mà thời gian khác thì tạo mới thực thể
                if (_WeekDataRecore.WeekID != WeekID)
                {
                    _WeekDataRecore = new WeekDataRecore();
                    _WeekDataRecore.WeekID = WeekID;
                }

                // Thử xem đã có key này trong nhật ký ghi chép chưa
                if (_WeekDataRecore.EventRecoding.ContainsKey(Key))
                {
                    // Nếu có rồi thì thay thế key cũ
                    _WeekDataRecore.EventRecoding[Key] = Value;
                }
                else // Nếu key này chưa có trong nhật ký ghi chép thì tạo mới
                {
                    _WeekDataRecore.EventRecoding.Add(Key, Value);
                }
            }
        }

        public int GetValueOfWeekRecore(int Key)
        {
            // Lấy ra ngày trong năm
            int WeekID = TimeUtil.GetIso8601WeekOfYear(DateTime.Now);

            lock (_WeekDataRecore)
            {
                // Nếu thông tin lưu trong nhật ký không phải ngày mới nhất thì thôi luôn
                if (_WeekDataRecore.WeekID != WeekID)
                {
                    _WeekDataRecore = new WeekDataRecore();
                    _WeekDataRecore.WeekID = WeekID;
                    return -1;
                }

                if (_WeekDataRecore.EventRecoding.ContainsKey(Key))
                {
                    return _WeekDataRecore.EventRecoding[Key];
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Chuyển về string
        /// </summary>
        public string DailyRecoreString
        {
            get
            {
                lock (_DailyDataRecore)
                {
                    byte[] DataByteArray = DataHelper.ObjectToBytes(this._DailyDataRecore);

                    string InfoEncoding = Convert.ToBase64String(DataByteArray);

                    return InfoEncoding;
                }
            }
        }

        public string ForeverRecoreString
        {
            get
            {
                lock (_ForeverRecore)
                {
                    byte[] DataByteArray = DataHelper.ObjectToBytes(this._ForeverRecore);

                    string InfoEncoding = Convert.ToBase64String(DataByteArray);

                    return InfoEncoding;
                }
            }
        }
        /// <summary>
        /// Chuyển về string
        /// </summary>
        public string WeekRecoreString
        {
            get
            {
                lock (_WeekDataRecore)
                {
                    byte[] DataByteArray = DataHelper.ObjectToBytes(this._WeekDataRecore);

                    string InfoEncoding = Convert.ToBase64String(DataByteArray);

                    return InfoEncoding;
                }
            }
        }


        /// <summary>
        /// Trạng thái nút hiện tại
        /// </summary>
        public int BtnState = -1;
        /// <summary>
        /// Nút hiện tại
        /// </summary>
        public int ReviceIndex = -1;




        #endregion Recore Event

        #region Anti-Auto
        /// <summary>
        /// Hàm này gọi sau khi người chơi trả lời Captcha
        /// </summary>
        public Action<bool> AnswerCaptcha { get; set; }

        /// <summary>
        /// Thời điểm ra Captcha lần tới
        /// </summary>
        public long NextCaptchaTicks { get; set; }

        /// <summary>
        /// Thời điểm hiện bảng trả lời Captcha khi ở tù
        /// </summary>
        public long LastJailCaptchaTicks { get; set; }

        /// <summary>
        /// Vị trí mở Shop lần trước có NPC
        /// </summary>
        public NPC LastShopNPC { get; set; }

        /// <summary>
        /// NPC Dã Luyện Đại Sư trước đó
        /// </summary>
        public NPC LastEquipMasterNPC { get; set; }
        #endregion

        #region Mật khẩu cấp 2
        /// <summary>
        /// Mật khẩu cấp 2
        /// </summary>
        public string SecondPassword { get; set; }

        /// <summary>
        /// Đã nhập mật khẩu cấp 2 chưa
        /// </summary>
        public bool HasEnteredSecondPassword { get; set; } = false;
        #endregion

        #region Đoán hoa đăng
        /// <summary>
        /// Thời điểm lần trước mở câu hỏi đoán hoa đăng
        /// </summary>
        public long LastOpenKnowledgeChallengeQuestion { get; set; }
        #endregion

        #region Khu an toàn
        private bool _IsInsideSafeZone = false;
        /// <summary>
        /// Có phải đang ở trong khu an toàn không
        /// <para>Trong khu an toàn không thể sử dụng kỹ năng tấn công</para>
        /// </summary>
        public bool IsInsideSafeZone
        {
            get
            {
                return this._IsInsideSafeZone;
            }
            set
            {
                /// Nếu khác giá trị cũ
                if (value != this._IsInsideSafeZone)
                {
                    /// Nếu là vào khu
                    if (value)
                    {
                        PlayerManager.ShowNotification(this, "Tiến vào khu an toàn!");
                    }
                    /// Nếu là rời khỏi khu
                    else
                    {
                        PlayerManager.ShowNotification(this, "Rời khỏi khu an toàn!");
                    }
                }

                /// Thiết lập lại giá trị
                this._IsInsideSafeZone = value;
            }
        }
        #endregion

        #region Tu Luyện Châu
        private int _XiuLianZhu_Exp;
        /// <summary>
        /// Kinh nghiệm Tu Luyện Châu có
        /// </summary>
        public int XiuLianZhu_Exp
        {
            get
            {
                lock (this)
                {
                    return this._XiuLianZhu_Exp;
                }
            }
            set
            {
                lock (this)
                {
                    this._XiuLianZhu_Exp = value;
                }
            }
        }

        private int _XiuLianZhu_TotalTime;
        /// <summary>
        /// Thời gian Tu Luyện còn lại (Giờ * 10)
        /// </summary>
        public int XiuLianZhu_TotalTime
        {
            get
            {
                /// Ngày hôm nay đã cộng chưa
                int addTime = this.GetValueOfDailyRecore((int)AcitvityRecore.XiuLianZhu_TodayTimeAdded);
                /// Nếu chưa cộng
                if (addTime < 0)
                {
                    addTime = ItemXiuLianZhuManager.GetHourAddPerDay() * 10;
                    this.SetValueOfDailyRecore((int)AcitvityRecore.XiuLianZhu_TodayTimeAdded, 1);
                }
                /// Nếu đã cộng rồi thì thôi
				else
                {
                    addTime = 0;
                }

                lock (this)
                {
                    /// Nếu có lượng cộng thêm
                    if (addTime > 0)
                    {
                        int val = this._XiuLianZhu_TotalTime + addTime;
                        this._XiuLianZhu_TotalTime = val;
                        /// Nếu vượt quá 14
                        if (this._XiuLianZhu_TotalTime > 140)
                        {
                            this._XiuLianZhu_TotalTime = 140;
                        }
                    }
                    /// Trả về kết quả
                    return this._XiuLianZhu_TotalTime;
                }
            }
            set
            {
                /// Ngày hôm nay đã cộng chưa
                int addTime = this.GetValueOfDailyRecore((int)AcitvityRecore.XiuLianZhu_TodayTimeAdded);
                /// Nếu chưa cộng
                if (addTime < 0)
                {
                    addTime = ItemXiuLianZhuManager.GetHourAddPerDay() * 10;
                    this.SetValueOfDailyRecore((int)AcitvityRecore.XiuLianZhu_TodayTimeAdded, 1);
                }
                /// Nếu đã cộng rồi thì thôi
				else
                {
                    addTime = 0;
                }

                lock (this)
                {
                    int val = value + addTime;
                    if (val > 140)
                    {
                        val = 140;
                    }

                    /// Lưu lại kết quả
                    this._XiuLianZhu_TotalTime = val;

                }
            }
        }
        #endregion

        #region Phụ bản

        /// <summary>
        /// Danh sách biến tạm dùng cho phụ bản
        /// </summary>
        private readonly Dictionary<int, int> TempCopySceneParams = new Dictionary<int, int>();

        /// <summary>
        /// Thiết lập biến tạm phụ bản tương ứng
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetTempCopySceneParam(int key, int value)
        {
            lock (this.TempCopySceneParams)
            {
                this.TempCopySceneParams[key] = value;
            }
        }

        /// <summary>
        /// Trả về biến tạm phụ bản tương ứng
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetTempCopySceneParam(int key)
        {
            if (this.TempCopySceneParams.TryGetValue(key, out int value))
            {
                return value;
            }
            return 0;
        }

        #endregion Phụ bản

        #region Tần Lăng
        /// <summary>
        /// Thời điểm Tick Tần Lăng trước
        /// </summary>
        public long LastEmperorTombTicks { get; set; }
        #endregion

        #region Bách Bảo Rương
        /// <summary>
        /// GM thiết lập cho quay vào rương xấu xí ở lượt tiếp theo
        /// </summary>
        public bool GM_SetWillGetTreasureNextTurn { get; set; } = false;

        /// <summary>
        /// GM thiết lập cho quay vào rương xấu xí ở lượt tiếp theo với số cược tương ứng
        /// <para>-1 sẽ bỏ qua, cược bất cứ giá trị nào cũng được</para>
        /// </summary>
        public int GM_SetWillGetTreasureNextTurnWithBet { get; set; } = -1;

        private long _LastSeashellCircleTicks = 0;
        /// <summary>
        /// Thời điểm lần trước quay sò
        /// </summary>
        public long LastSeashellCircleTicks
        {
            get
            {
                return this._LastSeashellCircleTicks;
            }
            set
            {
                this._LastSeashellCircleTicks = value;
            }
        }


        private int _LastSeashellCircleStopPos = -1;

        /// <summary>
        /// Vị trí dừng lại lần trước khi quay sò
        /// </summary>
        public int LastSeashellCircleStopPos
        {
            get
            {
                return this._LastSeashellCircleStopPos;
            }
            set
            {
                this._LastSeashellCircleStopPos = value;
            }
        }

        private int _LastSeashellCircleStage = -1;

        /// <summary>
        /// Tầng lần trước dừng lại khi quay sò
        /// </summary>
        public int LastSeashellCircleStage
        {
            get
            {
                return this._LastSeashellCircleStage;
            }
            set
            {
                this._LastSeashellCircleStage = value;
            }
        }

        private int _LastSeashellCircleBet = -1;

        /// <summary>
        /// Số sò đặt cược lần trước
        /// </summary>
        public int LastSeashellCircleBet
        {
            get
            {
                return this._LastSeashellCircleBet;
            }
            set
            {
                this._LastSeashellCircleBet = value;
            }
        }

        /// <summary>
        /// Đọc thông tin Bách Bảo Rương từ DB
        /// </summary>
        public void ReadSeashellCircleParamFromDB()
        {
            this._LastSeashellCircleStopPos = this.GetValueOfForeverRecore(RecoreDef.SeashellCircle_LastSeashellCircleStopPos);
            this._LastSeashellCircleStage = this.GetValueOfForeverRecore(RecoreDef.SeashellCircle_LastSeashellCircleStage);
            this._LastSeashellCircleBet = this.GetValueOfForeverRecore(RecoreDef.SeashellCircle_LastSeashellCircleBet);
        }

        /// <summary>
        /// Lưu lại thông tin Bách Bảo Rương vào DB
        /// </summary>
        public void SaveSeashellCircleParamToDB()
        {
            this.SetValueOfForeverRecore(RecoreDef.SeashellCircle_LastSeashellCircleStopPos, this._LastSeashellCircleStopPos);
            this.SetValueOfForeverRecore(RecoreDef.SeashellCircle_LastSeashellCircleStage, this._LastSeashellCircleStage);
            this.SetValueOfForeverRecore(RecoreDef.SeashellCircle_LastSeashellCircleBet, this._LastSeashellCircleBet);
        }

        #endregion Bách Bảo Rương

        #region Vòng quay may mắn
        /// <summary>
        /// Tổng số lượt đã quay Vòng quay may mắn
        /// </summary>
        public int LuckyCircle_TotalTurn
        {
            get
            {
                return this.GetValueOfForeverRecore(RecoreDef.LuckyCircle_TotalTurn);
            }
            set
            {
                this.SetValueOfForeverRecore(RecoreDef.LuckyCircle_TotalTurn, value);
            }
        }

        /// <summary>
        /// Vị trí dừng lần cuối trong Vòng quay may mắn
        /// </summary>
        public int LuckyCircle_LastStopPos { get; set; } = -1;

        /// <summary>
        /// Đánh dấu vật phẩm nhận được từ vòng quay may mắn là khóa hay không
        /// </summary>
        public bool LuckyCircle_AwardBound { get; set; } = true;

        /// <summary>
        /// Thời điểm thao tác Vòng quay may mắn trước
        /// </summary>
        public long LuckyCircle_LastTicks { get; set; }
        #endregion

        #region Chúc phúc
        private long _LastPrayTicks = 0;
        /// <summary>
        /// Thời điểm lần trước quay chúc phúc
        /// </summary>
        public long LastPrayTicks
        {
            get
            {
                return this._LastPrayTicks;
            }
            set
            {
                this._LastPrayTicks = value;
            }
        }

        private List<string> _LastPrayResult = null;
        /// <summary>
        /// Kết quả chúc phúc lần trước
        /// </summary>
        public List<string> LastPrayResult
        {
            get
            {
                return this._LastPrayResult;
            }
            set
            {
                this._LastPrayResult = value;
            }
        }

        /// <summary>
        /// Đọc dữ liệu chúc phúc từ DB
        /// </summary>
        public void ReadPrayDataFromDB()
        {
            try
            {
                string paramString = Global.GetRoleParamsStringWithNullFromDB(this, RoleParamName.PrayData);
                if (string.IsNullOrEmpty(paramString))
                {
                    this._LastPrayResult = null;
                }
                else
                {
                    string[] fields = paramString.Split('_');
                    /// Nếu có ký tự không hợp lệ
                    foreach (string str in fields)
                    {
                        int x = int.Parse(str);
                        /// Toác
                        if (x < 1 || x > 5)
                        {
                            throw new Exception();
                        }
                    }
                    this._LastPrayResult = fields.ToList();
                }
            }
            catch (Exception)
            {
                this._LastPrayResult = null;
            }
        }

        /// <summary>
        /// Lưu dữ liệu quay chúc phúc lần trước vào DB
        /// </summary>
        public void SavePrayDataToDB()
        {
            Global.SaveRoleParamsStringWithNullToDB(this, RoleParamName.PrayData, string.Format("{0}", this._LastPrayResult == null ? "" : string.Join("_", this._LastPrayResult)), true);
        }
        #endregion

        #region NHIEMVU

        private List<QuestInfo> _GetQuestInfo = new List<QuestInfo>();

        public List<QuestInfo> GetQuestInfo()
        {
            lock (_GetQuestInfo)
            {
                return _GetQuestInfo;
            }
        }

        public void AddQuestInfo(QuestInfo Quest)
        {
            lock (_GetQuestInfo)
            {
                _GetQuestInfo.Add(Quest);
            }
        }

        public int CurenQuestIDBVD = -1;

        public int CanncelQuestBVD = 0;

        public int QuestBVDStreakCount = 0;

        public int QuestBVDTodayCount = 0;



        #endregion NHIEMVU

        #region Vinh dự
        private int _WorldMartial = 0;
        /// <summary>
        /// Vinh dự võ lâm liên đấu
        /// </summary>
        public int WorldMartial
        {
            get
            {
                return this._WorldMartial;
            }
            set
            {
                lock (this)
                {
                    this._WorldMartial = value;
                }
                /// Thông báo vinh dự võ lâm liên đấu thay đổi
                KT_TCPHandler.NotifyMyselfPrestigeAndWorldHonorChanged(this);
            }
        }

        private int _WorldHonor = 0;
        /// <summary>
        /// Vinh dự võ lâm
        /// </summary>
        public int WorldHonor
        {
            get
            {
                return this._WorldHonor;
            }
            set
            {
                lock (this)
                {
                    this._WorldHonor = value;
                }
                /// Thông báo vinh dự võ lâm thay đổi
                KT_TCPHandler.NotifyMyselfPrestigeAndWorldHonorChanged(this);
            }
        }

        private int _FactionHonor = 0;
        /// <summary>
        /// Vinh dự môn phái
        /// </summary>
        public int FactionHonor
        {
            get
            {
                return this._FactionHonor;
            }
            set
            {
                lock (this)
                {
                    this._FactionHonor = value;
                }
                /// Thông báo vinh dự thi đấu môn phái thay đổi
                KT_TCPHandler.NotifyMyselfPrestigeAndWorldHonorChanged(this);
            }
        }

        #endregion

        #region Tài phú

        /// <summary>
        /// Tổng tài phú
        /// </summary>
        private long totalValue = 0;

        /// <summary>
        /// Thiết lập tổng tài phú
        /// </summary>
        /// <param name="value"></param>
        public void SetTotalValue(long value)
        {
            lock (this)
            {
                this.totalValue = value;
            }

            //Console.WriteLine(string.Format("[SET] {0} (ID: {1}), TotalValue = {2}", this.RoleName, this.RoleID, value));
            //LogManager.WriteLog(LogTypes.GameMapEvents, string.Format("{0} (ID: {1}), TotalValue = {2}, Source:\n{3}", this.RoleName, this.RoleID, this.GetTotalValue(), new System.Diagnostics.StackTrace().ToString()));
        }

        /// <summary>
        /// Trả về tổng tài phú
        /// </summary>
        /// <returns></returns>
        public long GetTotalValue()
        {
            //Console.WriteLine(string.Format("[GET] {0} (ID: {1}), TotalValue = {2}", this.RoleName, this.RoleID, this.totalValue));

            lock (this)
            {
                return this.totalValue;
            }
        }

        #endregion

        #region Danh vọng

        /// <summary>
        /// Danh vọng
        /// </summary>
        private List<ReputeInfo> Repute = new List<ReputeInfo>();

        /// <summary>
        /// Trả về danh sách danh vọng
        /// </summary>
        /// <returns></returns>
        public List<ReputeInfo> GetRepute()
        {
            return this.Repute;
        }

        /// <summary>
        /// Chuyển danh vọng thành chuỗi mã hóa
        /// </summary>
        public string ReputeInfoToString
        {
            get
            {
                byte[] ItemDataByteArray = DataHelper.ObjectToBytes(this.Repute);

                string ReputeInfoEncoding = Convert.ToBase64String(ItemDataByteArray);

                return ReputeInfoEncoding;
            }
        }

        /// <summary>
        /// Thiết lập danh sách danh vọng
        /// </summary>
        /// <param name="repute"></param>
        public void SetReputeInfo(List<ReputeInfo> repute)
        {
            lock (this.Repute)
            {
                this.Repute = repute;
            }
        }

        #endregion Danh vọng

        #region Danh hiệu

        /// <summary>
        /// Danh hiệu
        /// <para>Key: ID danh hiệu</para>
        /// <para>Value: Thời điểm nhận (đơn vị giờ), -1 nghĩa là vĩnh viễn</para>
        /// </summary>
        public ConcurrentDictionary<int, long> RoleTitles { get; set; }

        /// <summary>
        /// Chuỗi mã hóa danh hiệu để lưu vào DB
        /// </summary>
        public string RoleTitlesInfoString
        {
            get
            {
                List<string> titleString = new List<string>();
                foreach (KeyValuePair<int, long> pair in this.RoleTitles)
                {
                    titleString.Add(string.Format("{0}_{1}", pair.Key, pair.Value));
                }

                return string.Format("{0}|{1}", this.CurrentRoleTitleID, string.Join("|", titleString));
            }
        }

        /// <summary>
        /// ID danh hiệu hiện tại
        /// </summary>
        public int CurrentRoleTitleID { get; set; }

        /// <summary>
        /// Thêm danh hiệu tương ứng
        /// </summary>
        /// <param name="titleID"></param>
        public void AddRoleTitle(int titleID)
        {
            /// Nếu không tồn tại
            if (!KTTitleManager.IsTitleExist(titleID))
            {
                return;
            }
            /// Nếu trong danh sách đã tồn tại
            else if (this.RoleTitles.ContainsKey(titleID))
            {
                return;
            }

            /// Thêm vào danh sách
            this.RoleTitles[titleID] = KTGlobal.GetCurrentTimeMilis() / 1000 / 3600;

            /// Gửi về Client
            KT_TCPHandler.SendModifyMyselfCurrentRoleTitle(this, titleID, 1);
        }

        /// <summary>
        /// Xóa danh hiệu tương ứng
        /// </summary>
        /// <param name="titleID"></param>
        public void RemoveRoleTitle(int titleID)
        {
            /// Nếu không tồn tại
            if (!KTTitleManager.IsTitleExist(titleID))
            {
                return;
            }
            /// Nếu trong danh sách không tồn tại
            else if (!this.RoleTitles.ContainsKey(titleID))
            {
                return;
            }

            /// Xóa khỏi danh sách
            this.RoleTitles.TryRemove(titleID, out _);

            /// Gửi về Client
            KT_TCPHandler.SendModifyMyselfCurrentRoleTitle(this, titleID, -1);

            /// Nếu đang là danh hiệu hiện tại
            if (titleID == this.CurrentRoleTitleID)
            {
                /// Hủy danh hiệu hiện tại
                this.CurrentRoleTitleID = -1;
                /// Thông báo tới tất cả người chơi xung quanh
                KT_TCPHandler.NotifyOthersMyCurrentRoleTitleChanged(this);
            }

            /// Danh hiệu tương ứng
            KTitleXML titleData = KTTitleManager.GetTitleData(titleID);
            /// Nếu danh hiệu tồn tại
            if (titleData != null)
            {
                /// Thông báo hủy danh hiệu
                PlayerManager.ShowNotification(this, string.Format("Danh hiệu [{0}] đã bị hủy do hết thời hạn!", titleData.Text));
            }
        }

        /// <summary>
        /// Thiết lập làm danh hiệu hiện tại
        /// </summary>
        /// <param name="titleID"></param>
        public bool SetAsCurrentRoleTitle(int titleID)
        {
            /// Nếu không tồn tại
            if (!KTTitleManager.IsTitleExist(titleID))
            {
                return false;
            }
            /// Nếu trong danh sách không tồn tại
            else if (!this.RoleTitles.ContainsKey(titleID))
            {
                return false;
            }

            /// Thiết lập làm danh hiệu hiện tại
            this.CurrentRoleTitleID = titleID;

            /// Thông báo tới tất cả người chơi xung quanh
            KT_TCPHandler.NotifyOthersMyCurrentRoleTitleChanged(this);

            /// Thành công
            return true;
        }

        /// <summary>
        /// Tự động xóa các danh hiệu đã quá thời hạn
        /// </summary>
        public void AutoRemoveTimeoutTitles()
        {
            List<int> keys = this.RoleTitles.Keys.ToList();
            /// Duyệt danh sách danh hiệu
            foreach (int key in keys)
            {
                /// Danh hiệu tương ứng
                if (!this.RoleTitles.TryGetValue(key, out long startHours))
                {
                    /// Xóa danh hiệu tương ứng
                    this.RemoveRoleTitle(key);
                    continue;
                }
                /// Thông tin danh hiệu tương ứng
                KTitleXML titleData = KTTitleManager.GetTitleData(key);
                /// Nếu không tồn tại
                if (titleData == null)
                {
                    /// Xóa danh hiệu tương ứng
                    this.RemoveRoleTitle(key);
                    continue;
                }

                /// Thời gian lệch so với thời điểm hiện tại (Giờ)
                long hours = KTGlobal.GetCurrentTimeMilis() / 1000 / 3600 - startHours;
                /// Nếu quá số giờ (cho lãi thêm 1 giờ)
                if (hours >= titleData.Duration + 1)
                {
                    /// Xóa danh hiệu tương ứng
                    this.RemoveRoleTitle(key);
                }
            }
        }

        #endregion Danh hiệu

        #region Kỹ năng sống

        /// <summary>
        /// Danh sách kỹ năng sống
        /// </summary>
        private Dictionary<int, LifeSkillPram> LifeSkills = new Dictionary<int, LifeSkillPram>();

        /// <summary>
        /// Trả về danh sách kỹ năng sống tương ứng
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, LifeSkillPram> GetLifeSkills()
        {
            return this.LifeSkills;
        }

        /// <summary>
        /// Thiết lập danh sách kỹ năng sống
        /// </summary>
        /// <param name="lifeSkills"></param>
        public void SetLifeSkills(Dictionary<int, LifeSkillPram> lifeSkills)
        {
            lock (this.LifeSkills)
            {
                if (lifeSkills == null)
                {
                    this.LifeSkills = new Dictionary<int, LifeSkillPram>();
                    for (int i = 1; i < 12; i++)
                    {
                        LifeSkillPram param = new LifeSkillPram();
                        param.LifeSkillID = i;
                        param.LifeSkillLevel = 1;
                        param.LifeSkillExp = 0;
                        this.LifeSkills[i] = param;
                    }
                }
                else
                {
                    this.LifeSkills = lifeSkills;
                }
            }
        }

        /// <summary>
        /// Trả về thông tin kỹ năng sống tương ứng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LifeSkillPram GetLifeSkill(int id)
        {
            if (this.LifeSkills.TryGetValue(id, out LifeSkillPram lifeSkillParam))
            {
                return lifeSkillParam;
            }
            return null;
        }

        /// <summary>
        /// Thiết lập cấp độ kỹ năng sống tương ứng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="level"></param>
        /// <param name="exp"></param>
        public void SetLifeSkillParam(int id, int level, int exp)
        {
            if (this.LifeSkills.TryGetValue(id, out LifeSkillPram lifeSkillParam))
            {
                lifeSkillParam.LifeSkillLevel = level;
                LifeSkillExp lifeSkillExp = ItemCraftingManager._LifeSkill.TotalExp.Where(x => x.Level == level).FirstOrDefault();
                if (lifeSkillExp == null)
                {
                    lifeSkillParam.LifeSkillExp = 0;
                }
                else
                {
                    if (exp < 0)
                    {
                        exp = 0;
                    }
                    if (exp > lifeSkillExp.Exp - 1)
                    {
                        exp = lifeSkillExp.Exp - 1;
                    }
                }

                /// Gửi thông báo về Client
                KT_TCPHandler.NotifySelfLifeSkillLevelAndExpChanged(this, id, level, exp);
            }
        }

        /// <summary>
        /// Chuyển kỹ năng sống thành dạng String để lưu vào DB
        /// </summary>
        public string LifeSkillToString
        {
            get
            {
                byte[] ItemDataByteArray = DataHelper.ObjectToBytes(this.LifeSkills);
                string LifeSkillEncoding = Convert.ToBase64String(ItemDataByteArray);
                return LifeSkillEncoding;
            }
        }

        private bool _IsCrafting = false;

        /// <summary>
        /// Có đang chế đồ không
        /// </summary>
        public bool IsCrafting
        {
            get
            {
                return this._IsCrafting;
            }
            set
            {
                this._IsCrafting = value;
            }
        }

        #endregion Kỹ năng sống

        #region Bạn bè

        /// <summary>
        /// Danh sách bạn bè
        /// </summary>
        public List<FriendData> FriendDataList
        {
            get { return _RoleDataEx.FriendDataList; }
            set { lock (this) _RoleDataEx.FriendDataList = value; }
        }

        /// <summary>
        /// Danh sách người chơi mà đối tượng đang yêu cầu kết bạn
        /// </summary>
        private readonly HashSet<int> AskingToBeFriendWith = new HashSet<int>();

        /// <summary>
        /// Kiểm tra đối tượng có đang yêu cầu kết bạn với người chơi tương ứng không
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsAskingToBeFriendWith(KPlayer player)
        {
            return this.AskingToBeFriendWith.Contains(player.RoleID);
        }

        /// <summary>
        /// Thêm người chơi mà đối tượng đang yêu cầu kết bạn vào danh sách
        /// </summary>
        /// <param name="player"></param>
        public void AddAskingToBeFriend(KPlayer player)
        {
            /// Nếu đã tồn tại thì bỏ qua
            if (this.AskingToBeFriendWith.Contains(player.RoleID))
            {
                return;
            }
            /// Thêm vào danh sách
            this.AskingToBeFriendWith.Add(player.RoleID);
        }

        /// <summary>
        /// Xóa người chơi mà đối tượng đang yêu cầu kết bạn khỏi danh sách
        /// </summary>
        /// <param name="player"></param>
        public void RemoveAskingToBeFriend(KPlayer player)
        {
            /// Nếu không tồn tại thì bỏ qua
            if (!this.AskingToBeFriendWith.Contains(player.RoleID))
            {
                return;
            }
            this.AskingToBeFriendWith.Remove(player.RoleID);
        }

        #endregion Bạn bè

        #region Chat

        /// <summary>
        /// Thời điểm Chat lần trước tại kênh tương ứng
        /// </summary>
        private readonly Dictionary<ChatChannel, long> TickChat = new Dictionary<ChatChannel, long>();

        /// <summary>
        /// Có thể gửi tin nhắn Chat không
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public bool CanChat(ChatChannel channel, out long tickLeft)
        {
            /// Nếu không thể Chat
            if (this.BanChat == 1)
            {
                tickLeft = 99999999;
                return false;
            }

            /// Lấy thời điểm Chat trước
            if (!this.TickChat.TryGetValue(channel, out long lastTick))
            {
                this.TickChat[channel] = 0;
                lastTick = 0;
            }

            /// Thời điểm hiện tại
            long currentTick = KTGlobal.GetCurrentTimeMilis();
            /// Khoảng lệch
            long diffTick = currentTick - lastTick;

            switch (channel)
            {
                case ChatChannel.Near:
                    {
                        tickLeft = 1000 - diffTick;
                        return diffTick >= 1000;
                    }
                case ChatChannel.Team:
                    {
                        tickLeft = 500 - diffTick;
                        return diffTick >= 500;
                    }
                case ChatChannel.Private:
                    {
                        tickLeft = 500 - diffTick;
                        return diffTick >= 500;
                    }
                case ChatChannel.System:
                    {
                        tickLeft = 99999999;
                        return false;
                    }
                case ChatChannel.Faction:
                    {
                        tickLeft = 1000 - diffTick;
                        return diffTick >= 1000;
                    }
                case ChatChannel.Family:
                    {
                        tickLeft = 1000 - diffTick;
                        return diffTick >= 1000;
                    }
                case ChatChannel.Global:
                    {
                        tickLeft = 120000 - diffTick;
                        return diffTick >= 120000;
                    }
                case ChatChannel.KuaFuLine:
                    {
                        tickLeft = 1000 - diffTick;
                        return diffTick >= 1000;
                    }
                case ChatChannel.Guild:
                    {
                        tickLeft = 1000 - diffTick;
                        return diffTick >= 1000;
                    }
                case ChatChannel.Special:
                    {
                        tickLeft = 10000 - diffTick;
                        return diffTick >= 10000;
                    }
                default:
                    {
                        tickLeft = 99999999;
                        return false;
                    }
            }
        }

        /// <summary>
        /// Ghi lại thời điểm chat tại kênh tương ứng
        /// </summary>
        /// <param name="channel"></param>
        public void RecordChatTick(ChatChannel channel)
        {
            this.TickChat[channel] = KTGlobal.GetCurrentTimeMilis();
        }

        #endregion Chat

        #region Dialog
        private long _LastClickDialog = 0;

        /// <summary>
        /// Thời điểm lần trước thực hiện Click chọn chức năng của NPCDialog và ItemDialog
        /// </summary>
        public long LastClickDialog
        {
            get
            {
                lock (this)
                {
                    return this._LastClickDialog;
                }
            }
            set
            {
                lock (this)
                {
                    this._LastClickDialog = value;
                }
            }
        }

        #endregion Dialog

        #region RoleDataEx

        /// <summary>
        /// Thực thể trung gian để giao tiếp với GAMEBD -> VÀ  TOÀN BỘ GAMECLIENT | TOÀN BỘ DỮ LIỆU GIAO TIẾP Ở ATRIBUILD PHẢI ĐẢM BẢO ĐỒNG BỘ VỚI ROLEDATAEX
        /// </summary>
        private RoleDataEx _RoleDataEx = null;

        /// <summary>
        /// Giá trị RoleDataEx
        /// </summary>
        /// <returns></returns>
        public RoleDataEx GetRoleData()
        {
            return _RoleDataEx;
        }

        /// <summary>
        /// Thiết lập RoleDataEx
        /// </summary>
        public RoleDataEx RoleData
        {
            set { lock (this) _RoleDataEx = value; }
        }

        #endregion RoleDataEx

        #region Define

        private Dictionary<int, long> _LastDBCmdTicksDict = new Dictionary<int, long>();

        /// <summary>
        /// Lần cuối tick với DB Service
        /// </summary>
        public Dictionary<int, long> LastDBCmdTicksDict
        {
            get { return _LastDBCmdTicksDict; }
            set { lock (this) _LastDBCmdTicksDict = value; }
        }

        private long _LastDBHeartTicks = TimeUtil.NOW();



        private long _LastBookExpTicks = TimeUtil.NOW();


        public long LastBookExpTicks
        {
            get { return _LastBookExpTicks; }
            set { lock (this) _LastBookExpTicks = value; }
        }

        /// <summary>
        /// Service DB connect
        /// </summary>
        public long LastDBHeartTicks
        {
            get { return _LastDBHeartTicks; }
            set { lock (this) _LastDBHeartTicks = value; }
        }

        /// <summary>
        /// Tin nhắn cuối cùng PUSH
        /// </summary>
        public string PushMessageID
        {
            get { return _RoleDataEx.PushMessageID; }
            set { lock (this) _RoleDataEx.PushMessageID = value; }
        }

        /// <summary>
        /// Toàn bộ dic Sysmboy của ROLE
        /// </summary>
        public Dictionary<string, RoleParamsData> RoleParamsDict
        {
            get { return _RoleDataEx.RoleParamsDict; }
            set { lock (this) _RoleDataEx.RoleParamsDict = value; }
        }

        /// <summary>
        /// ID thư mới
        /// </summary>
        public int LastMailID
        {
            get { return _RoleDataEx.LastMailID; }
            set { lock (this) _RoleDataEx.LastMailID = value; }
        }

        /// <summary>
        /// Xem đã nạp đầu hay chưa
        /// </summary>
        public int CZTaskID
        {
            get { return _RoleDataEx.CZTaskID; }
            set { lock (this) _RoleDataEx.CZTaskID = value; }
        }

        /// <summary>
        /// Hướng quay mặt
        /// </summary>
        public int RoleDirection
        {
            get
            {
                return this._RoleDataEx.RoleDirection;
            }

            set
            {
                lock (this)
                {
                    this._RoleDataEx.RoleDirection = value;
                }
            }
        }

        /// <summary>
        /// Avata nhân vật
        /// </summary>
        public int RolePic
        {
            get
            {
                return this._RoleDataEx.RolePic;
            }
            set
            {
                lock (this)
                {
                    this._RoleDataEx.RolePic = value;
                }
            }
        }

        /// <summary>
        /// Môn phái của người chơi
        /// </summary>
        public KPlayerFaction m_cPlayerFaction { get; private set; }

        /// <summary>
        /// Danh sách đối tượng xung quanh
        /// <para>0: Cần xóa</para>
        /// <para>1: Giữ</para>
        /// <para>2: Cần thêm</para>
        /// <para>3: Tàng hình hoặc gì đó nên không thêm vào danh sách cũng không xóa khỏi danh sách</para>
        /// </summary>
        public ConcurrentDictionary<Object, byte> VisibleGrid9Objects { get; set; } = new ConcurrentDictionary<object, byte>();

        /// <summary>
        /// Danh sách người chơi xung quanh
        /// </summary>
        public ConcurrentDictionary<int, KPlayer> NearbyPlayers { get; set; } = new ConcurrentDictionary<int, KPlayer>();

        /// <summary>
        /// 更新某个角色参数命令的时间
        /// </summary>
        private Dictionary<string, long> _LastDBRoleParamCmdTicksDict = new Dictionary<string, long>();

        /// <summary>
        /// 更新某个角色参数命令的时间
        /// </summary>
        public Dictionary<string, long> LastDBRoleParamCmdTicksDict
        {
            get { return _LastDBRoleParamCmdTicksDict; }
            set { lock (this) _LastDBRoleParamCmdTicksDict = value; }
        }

        /// <summary>
        /// 更新某个装备耐久度命令的时间
        /// </summary>
        private Dictionary<int, long> _LastDBEquipStrongCmdTicksDict { get; set; } = new Dictionary<int, long>();

        /// <summary>
        /// 更新某个装备耐久度命令的时间
        /// </summary>
        public Dictionary<int, long> LastDBEquipStrongCmdTicksDict
        {
            get { return _LastDBEquipStrongCmdTicksDict; }
            set { lock (this) _LastDBEquipStrongCmdTicksDict = value; }
        }

        /// <summary>
        /// 拾取物品的锁定
        /// </summary>
        private object _PickUpGoodsPackMutex = new object();

        /// <summary>
        /// 拾取物品的锁定
        /// </summary>
        public object PickUpGoodsPackMutex
        {
            get { return _PickUpGoodsPackMutex; }
        }

        public List<int> GroupMailRecordList
        {
            get { return _RoleDataEx.GroupMailRecordList; }
            set { lock (this) _RoleDataEx.GroupMailRecordList = value; }
        }


        /// <summary>
        /// Thời điểm truy vấn danh sách Top chiến đội trong Võ lâm liên đấu lần trước
        /// </summary>
        public long LastQueryTeamBattleTicks { get; set; }


        #endregion Define

        #region Tìm kiếm

        private long _LastBrowsePlayersTick = 0;

        /// <summary>
        /// Thời điểm lần trước tìm kiếm người chơi khác thông qua khung tìm kiếm
        /// </summary>
        public long LastBrowsePlayersTick
        {
            get
            {
                return this._LastBrowsePlayersTick;
            }
            set
            {
                this._LastBrowsePlayersTick = value;
            }
        }

        #endregion Tìm kiếm

        #region Tổ đội

        private int _TeamID;

        /// <summary>
        /// ID nhóm
        /// </summary>
        public int TeamID
        {
            get { return _TeamID; }
            set { _TeamID = value; }
        }

        /// <summary>
        /// Nhóm trưởng
        /// </summary>
        public KPlayer TeamLeader
        {
            get
            {
                return KTTeamManager.GetTeamLeader(this.TeamID);
            }
        }

        /// <summary>
        /// Danh sách thành viên nhóm
        /// </summary>
        public List<KPlayer> Teammates
        {
            get
            {
                return KTTeamManager.GetTeamPlayers(this.TeamID);
            }
        }

        #endregion Tổ đội

        #region Nhặt đồ
        private long _LastPickUpDropItemTicks = 0;
        /// <summary>
        /// Thời điểm nhặt đồ rơi dưới đất lần trước
        /// </summary>
        public long LastPickUpDropItemTicks
        {
            get
            {
                lock (this)
                {
                    return this._LastPickUpDropItemTicks;
                }
            }
            set
            {
                lock (this)
                {
                    this._LastPickUpDropItemTicks = value;
                }
            }
        }
        #endregion

        #region Thư

        private long _LastCheckMailTick = 0;

        /// <summary>
        /// Thời điểm đọc thư lần cuối
        /// </summary>
        public long LastCheckMailTick
        {
            get
            {
                return this._LastCheckMailTick;
            }
            set
            {
                this._LastCheckMailTick = value;
            }
        }

        #endregion Thư

        #region THETHANG

        /// <summary>
        /// Thẻ tháng của người chơi
        /// </summary>
        public YueKaDetail YKDetail = new YueKaDetail();

        #endregion THETHANG

        #region TONGKIM

        public int ReviceMedicineOfDay { get; set; }

        #endregion TONGKIM

        #region PK Mode

        /// <summary>
        /// Chưa dùng
        /// </summary>
        public int PKPoint
        {
            get { return _RoleDataEx.PKPoint; }
            set { lock (this) _RoleDataEx.PKPoint = value; }
        }

        private long _LastSiteSubPKPointTicks = 0;

        /// <summary>
        /// Thời gian gần đây nhất giảm PK
        /// </summary>
        public long LastSiteSubPKPointTicks
        {
            get
            {
                return this._LastSiteSubPKPointTicks;
            }
            set
            {
                lock (this)
                {
                    this._LastSiteSubPKPointTicks = value;
                }
            }
        }

        private long _LastChangePKModeToFight = 0;

        /// <summary>
        /// Thời gian lần trước thay đổi trạng thái chiến đấu khác Luyện công
        /// </summary>
        public long LastChangePKModeToFight
        {
            get
            {
                return this._LastChangePKModeToFight;
            }
            set
            {
                lock (this)
                {
                    this._LastChangePKModeToFight = value;
                }
            }
        }

        #endregion PK Mode

        #region CacheData

        public int LastRoleCommonUseIntParamValueListTickCount = 0;

        /// <summary>
        /// Thời gian reset túi đồ gần đây
        /// </summary>
        public long _ResetBagTicks = 0;

        /// <summary>
        /// Thời gian làm mới chợ gần đây
        /// </summary>
        public long _RefreshMarketTicks = 0;

        /// <summary>
        /// Thời gian tick của Sprite
        /// </summary>
        public long _SpriteFightTicks = 0;

        /// <summary>
        /// Thời gina add Friend Tick
        /// </summary>
        public long[] _AddFriendTicks = new long[] { 0, 0, 0 };

        #endregion CacheData

        #region Bang hội

        /// <summary>
        /// Tên bang hội
        /// </summary>
        public string GuildName
        {
            get
            {
                return this._RoleDataEx.GuildName;
            }
            set
            {
                lock (this)
                {
                    this._RoleDataEx.GuildName = value;
                }
            }
        }

        /// <summary>
        /// Thời gina tick của Bang HỘI
        /// </summary>
        public long _AddBHMemberTicks = 0;

        /// <summary>
        /// ID bang hội
        /// </summary>
        public int GuildID
        {
            get
            {
                int tmpVar = this._RoleDataEx.GuildID;
                return tmpVar;
            }

            set
            {
                this._RoleDataEx.GuildID = value;
            }
        }

        /// <summary>
        /// Cấp bậc bang hội
        /// </summary>
        public int GuildRank
        {
            get
            {
                return this._RoleDataEx.GuildRank;
            }
            set
            {
                lock (this)
                {
                    this._RoleDataEx.GuildRank = value;
                }
            }
        }

        /// <summary>
        /// Tiền bang hội
        /// </summary>
        public int RoleGuildMoney
        {
            get
            {
                return this._RoleDataEx.RoleGuildMoney;
            }
            set
            {
                lock (this)
                {
                    this._RoleDataEx.RoleGuildMoney = value;
                }
            }
        }

        /// <summary>
        /// Danh hiệu bang hội
        /// </summary>
        public string GuildTitle
        {
            get
            {
                /// Nếu không có bang
                if (this.GuildID <= 0)
                {
                    return "";
                }

                /// Danh hiệu theo chức vụ
                string guildRankName = "";

                if (this.GuildRank <= (int)GameServer.KiemThe.Entities.GuildRank.Member)
                {
                    guildRankName = "Bang chúng";
                }
                else if (this.GuildRank == (int)GameServer.KiemThe.Entities.GuildRank.Master)
                {
                    guildRankName = "Bang chủ";
                }
                else if (this.GuildRank == (int)GameServer.KiemThe.Entities.GuildRank.ViceMaster)
                {
                    guildRankName = "Phó bang chủ";
                }
                else if (this.GuildRank == (int)GameServer.KiemThe.Entities.GuildRank.Ambassador)
                {
                    guildRankName = "Trưởng lão";
                }
                else if (this.GuildRank == (int)GameServer.KiemThe.Entities.GuildRank.Elite)
                {
                    guildRankName = "Tinh anh";
                }

                /// Trả về kết quả
                return string.Format("[Bang hội] {0} - {1}", this.GuildName, guildRankName);
            }
        }

        /// <summary>
        /// Quan hàm
        /// </summary>
        public int OfficeRank
        {
            get
            {
                return this._RoleDataEx.OfficeRank;
            }
        }

        #endregion Bang hội

        #region Gia Tộc

        /// <summary>
        /// Tên bang hội
        /// </summary>
        public string FamilyName
        {
            get
            {
                return this._RoleDataEx.FamilyName;
            }
            set
            {
                lock (this)
                {
                    this._RoleDataEx.FamilyName = value;
                }
            }
        }

        /// <summary>
        /// ID bang hội
        /// </summary>
        public int FamilyID
        {
            get
            {
                int tmpVar = this._RoleDataEx.FamilyID;
                return tmpVar;
            }

            set
            {
                this._RoleDataEx.FamilyID = value;
            }
        }

        /// <summary>
        /// Cấp bậc bang hội
        /// </summary>
        public int FamilyRank
        {
            get
            {
                return this._RoleDataEx.FamilyRank;
            }
            set
            {
                lock (this)
                {
                    this._RoleDataEx.FamilyRank = value;
                }
            }
        }

        /// <summary>
        /// Danh hiệu bang hội
        /// </summary>
        public string FamilyTitle
        {
            get
            {
                /// Nếu không có tộc
                if (this.FamilyID <= 0)
                {
                    return "";
                }

                /// Danh hiệu theo chức vụ
                string guildRankName = "";

                if (this.FamilyRank <= (int)GameServer.KiemThe.Entities.FamilyRank.Member)
                {
                    guildRankName = "Thành viên";
                }
                else if (this.FamilyRank == (int)GameServer.KiemThe.Entities.FamilyRank.Master)
                {
                    guildRankName = "Tộc trưởng";
                }
                else if (this.FamilyRank == (int)GameServer.KiemThe.Entities.FamilyRank.ViceMaster)
                {
                    guildRankName = "Tộc phó";
                }

                /// Trả về kết quả
                return string.Format("<color=#ffb92e>[Gia tộc] {0} - {1}</color>", this.FamilyName, guildRankName);
            }
        }

        #endregion Gia Tộc

        #region TIENTEINGAME

        /// <summary>
        /// USER MONEY LOCK
        /// </summary>
        private object _TokenMutex = new object();

        /// <summary>
        ///  USER MONEY LOCK
        /// </summary>
        public object TokenMutex
        {
            get { return _TokenMutex; }
        }

        /// <summary>
        /// Bạc khóa
        /// </summary>
        public int BoundMoney
        {
            get { return _RoleDataEx.BoundMoney; }
            set { lock (this) _RoleDataEx.BoundMoney = value; }
        }

        /// <summary>
        /// Điểm Uy Danh Giang hồ
        /// </summary>
        public int Prestige
        {
            get
            {
                return this._RoleDataEx.Prestige;
            }
            set
            {
                lock (this)
                {
                    this._RoleDataEx.Prestige = value;
                }
                /// Thông báo uy danh thay đổi
                KT_TCPHandler.NotifyMyselfPrestigeAndWorldHonorChanged(this);
            }
        }

        /// <summary>
        /// Đối tượng Mutex dùng trong thao tác thêm bạc lưu trữ ở thương khố cho người chơi
        /// </summary>
        public object StoreMoneyMutex { get; } = new object();

        /// <summary>
        /// Bạc lưu trữ ở thương khố
        /// </summary>
        public int StoreMoney
        {
            get { return _RoleDataEx.Store_Money; }
            set { lock (this) _RoleDataEx.Store_Money = value; }
        }

        /// <summary>
        /// Đồng mua trên kỳ trân các
        /// </summary>
        public int Token
        {
            get { return _RoleDataEx.Token; }
            set { lock (this) _RoleDataEx.Token = value; }
        }

        /// <summary>
        /// LOCKK BẠC KHÓA
        /// </summary>
        private object _BoundTokenMutex = new object();

        /// <summary>
        /// LOCKK BẠC KHÓA
        /// </summary>
        private object _MoneyLock = new object();

        public object GetMoneyLock
        {
            get { return _MoneyLock; }
        }

        /// <summary>
        ///  LOCKK BẠC KHÓA => Việc Lock này đảm bảo cho client chỉ có 1 request thay đổi giá trị 1 lúc
        /// </summary>
        public object BoundTokenMutex
        {
            get { return _BoundTokenMutex; }
        }

        /// <summary>
        /// Đòng khóa
        /// </summary>
        public int BoundToken
        {
            get { return _RoleDataEx.BoundToken; }
            set { lock (this) _RoleDataEx.BoundToken = value; }
        }

        /// <summary>
        /// Lock BoundMoney  => Việc Lock này đảm bảo cho client chỉ có 1 request thay đổi giá trị 1 lúc
        /// </summary>
        private object _BoundMoneyMutex = new object();

        /// <summary>
        ///   Lock BoundMoney   => Việc Lock này đảm bảo cho client chỉ có 1 request thay đổi giá trị 1 lúc
        /// </summary>
        public object BoundMoneyMutex
        {
            get { return _BoundMoneyMutex; }
        }

        /// <summary>
        /// Bạc thường
        /// </summary>
        public int Money
        {
            get
            {
                int tmpVar = this._RoleDataEx.Money;
                return tmpVar;
            }

            set
            {
                this._RoleDataEx.Money = value;
            }
        }
        #endregion TIENTEINGAME

        #region ROLEPOSTION
        /// <summary>
        /// Thời điểm lần trước chết
        /// </summary>
        public long LastDeadTicks { get; set; }

        /// <summary>
        /// Lần cuối cùng report lại vị trí
        /// </summary>
        private long _ReportPosTicks = 0;

        /// <summary>
        /// Report lại vị trí tick
        /// </summary>
        public long ReportPosTicks
        {
            get { return _ReportPosTicks; }
            set { lock (this) _ReportPosTicks = value; }
        }

        /// <summary>
        /// ZONE ID CỦA MÁY CHỦ
        /// </summary>
        public int ZoneID
        {
            get
            {
                //lock (this)
                {
                    return this._RoleDataEx.ZoneID;
                }
            }
            set
            {
                lock (this)
                {
                    this._RoleDataEx.ZoneID = value;
                }
            }
        }

        /// <summary>
        /// ID bản đồ hiện tại
        /// </summary>
        public int MapCode
        {
            get
            {
                if (this._RoleDataEx == null)
                {
                    return -1;
                }
                return this._RoleDataEx.MapCode;
            }
            set
            {
                if (this._RoleDataEx == null)
                {
                    return;
                }
                this._RoleDataEx.MapCode = value;
            }
        }

        /// <summary>
        /// Tọa độ X hiện tại
        /// </summary>
        public int PosX
        {
            get
            {
                if (this._RoleDataEx == null)
                {
                    return -1;
                }
                return this._RoleDataEx.PosX;
            }
            set
            {
                if (this._RoleDataEx == null)
                {
                    return;
                }
                this._RoleDataEx.PosX = value;
            }
        }

        /// <summary>
        /// Tọa độ Y hiện tại
        /// </summary>
        public int PosY
        {
            get
            {
                if (this._RoleDataEx == null)
                {
                    return -1;
                }
                return this._RoleDataEx.PosY;
            }
            set
            {
                if (this._RoleDataEx == null)
                {
                    return;
                }
                this._RoleDataEx.PosY = value;
            }
        }


        /// <summary>
        /// Lần cuối thông báo đổi vị trí
        /// </summary>
        public long LastChangePosTicks { get; set; }

        /// <summary>
        /// Thời điểm Tick StoryBoard lần trước
        /// </summary>
        public long LastStoryBoardTicks { get; set; }

        /// <summary>
        /// Lần cuối thông báo đổi bản đồ
        /// </summary>
        public long LastChangeMapTicks { get; set; }

        /// <summary>
        /// Đợi chuyển bản đồ
        /// </summary>
        private bool _WaitingForChangeMap = false;

        /// <summary>
        /// Đợi chuyển bản đồ
        /// </summary>
        public bool WaitingForChangeMap
        {
            get { lock (this) return _WaitingForChangeMap; }
            set { lock (this) _WaitingForChangeMap = value; }
        }

        /// <summary>
        /// ID bản đồ đang đợi dịch đến
        /// </summary>
        public int WaitingChangeMapCode { get; set; }

        /// <summary>
        /// Vị trí X đang chờ dịch sang bản đồ đích
        /// </summary>
        public int WaitingChangeMapPosX { get; set; }

        /// <summary>
        /// Vị trí Y đang chờ dịch sang bản đồ đích
        /// </summary>
        public int WaitingChangeMapPosY { get; set; }

        /// <summary>
        /// Đợi chuyển bản đồ
        /// </summary>
        private bool _WaitingForChangePos = false;

        /// <summary>
        /// Đợi chuyển tọa độ
        /// </summary>
        public bool WaitingForChangePos
        {
            get { return _WaitingForChangePos; }
            set { lock (this) _WaitingForChangePos = value; }
        }

        /// <summary>
        /// Vị trí lần trước được chuyển bởi hệ thống (thông qua hàm chuyển bản đồ hoặc thay đổi vị trí)
        /// </summary>
        public Point LastChangedPosition { get; set; } = new Point(0, 0);

        /// <summary>
        /// Thời điểm cập nhật danh sách quái đặc biệt trong bản đồ lần trước
        /// </summary>
        public long LastUpdateLocalMapMonsterTicks { get; set; }

        /// <summary>
        /// 物品使用限时更新时间
        /// </summary>
        private long _LastGoodsLimitUpdateTicks = TimeUtil.NOW();

        /// <summary>
        /// 物品使用限时更新时间
        /// </summary>
        public long LastGoodsLimitUpdateTicks
        {
            get { return _LastGoodsLimitUpdateTicks; }
            set { lock (this) _LastGoodsLimitUpdateTicks = value; }
        }

        /// </summary>
        /// 背包格子开启时间
        /// </summary>
        private int _OpenGridTime = 0;

        /// <summary>
        /// 背包格子开启时间
        /// </summary>
        public int OpenGridTime
        {
            get { return _OpenGridTime; }
            set { lock (this) _OpenGridTime = value; }
        }

        private int _OnlineActiveVal = 0;

        public int OnlineActiveVal
        {
            get { return _OnlineActiveVal; }
            set { lock (this) _OnlineActiveVal = value; }
        }

        private bool _ForceDisconnect = false;

        /// <summary>
        /// Buộc ngắt kết nối
        /// </summary>
        public bool ForceDisconnect
        {
            get { return _ForceDisconnect; }
            set { lock (this) _ForceDisconnect = value; }
        }

        #endregion ROLEPOSTION

        #region Task_NHIỆMVU

        /// <summary>
        /// Nhiệm vụ
        /// </summary>
        public List<TaskData> TaskDataList
        {
            get { return _RoleDataEx.TaskDataList; }
            set { lock (this) _RoleDataEx.TaskDataList = value; }
        }

        public List<OldTaskData> OldTasks
        {
            get { return _RoleDataEx.OldTasks; }
            set { lock (this) _RoleDataEx.OldTasks = value; }
        }

        /// <summary>
        /// Nhiệm vụ hàng ngày
        /// </summary>
        public DailyTaskData YesterdayDailyTaskData = null;

        /// <summary>
        /// Nhiệm vụ hàng ngày 2
        /// </summary>
        public DailyTaskData YesterdayTaofaTaskData = null;

        /// <summary>
        /// ID Nhiệm vụ chính tuyến hiện tại
        /// </summary>
        public int MainTaskID
        {
            get { return _RoleDataEx.MainTaskID; }
            set { lock (this) _RoleDataEx.MainTaskID = value; }
        }

        /// <summary>
        /// Danh sách nhiệm vụ hàng ngày
        /// </summary>
        public List<DailyTaskData> MyDailyTaskDataList
        {
            get { return _RoleDataEx.MyDailyTaskDataList; }
            set { lock (this) _RoleDataEx.MyDailyTaskDataList = value; }
        }

        #endregion Task_NHIỆMVU

        #region Kỹ năng
        /// <summary>
        /// Thời điểm Tick lần trước tự lưu dữ liệu kỹ năng vào DB
        /// </summary>
        public long LastDBTickSkills { get; set; }

        /// <summary>
        /// Danh sách kỹ năng
        /// </summary>
        public List<SkillData> SkillDataList
        {
            get { return _RoleDataEx.SkillDataList; }
            set { lock (this) _RoleDataEx.SkillDataList = value; }
        }

        /// <summary>
        /// Danh sách kỹ năng ở ô thiết lập nhanh
        /// </summary>
        public string MainQuickBarKeys
        {
            get { return _RoleDataEx.MainQuickBarKeys; }
            set { lock (this) _RoleDataEx.MainQuickBarKeys = value; }
        }

        /// <summary>
        /// Kỹ năng vòng sáng đang được sử dụng
        /// <para>Dạng: ID_TRẠNG THÁI, ID là ID kỹ năng, TRẠNG THÁI 0 tức là không kích hoạt, 1 tức là kích hoạt</para>
        /// </summary>
        public string OtherQuickBarKeys
        {
            get { return _RoleDataEx.OtherQuickBarKeys; }
            set { lock (this) _RoleDataEx.OtherQuickBarKeys = value; }
        }

        #endregion Kỹ năng

        #region LoginRecore

        private bool _FirstPlayStart = true;

        /// <summary>
        ///  Đăng nhập lần đầu
        /// </summary>
        public bool FirstPlayStart
        {
            get { return _FirstPlayStart; }
            set { lock (this) _FirstPlayStart = value; }
        }

        /// <summary>
        /// Thời gian bảo vệ sức khỏe
        /// </summary>
        private int _AntiAddictionTimeType = 0;

        /// <summary>
        /// Thời gian bảo vệ sức khỏe
        /// </summary>
        public int AntiAddictionTimeType
        {
            get { return _AntiAddictionTimeType; }
            set { lock (this) _AntiAddictionTimeType = value; }
        }

        /// <summary>
        /// Cấm chât
        /// </summary>
        public int BanChat
        {
            get { return _RoleDataEx.BanChat; }
            set { lock (this) _RoleDataEx.BanChat = value; }
        }

        /// <summary>
        /// Cấm Login
        /// </summary>
        public int BanLogin
        {
            get { return _RoleDataEx.BanLogin; }
            set { lock (this) _RoleDataEx.BanLogin = value; }
        }

        /// <summary>
        /// Tổng số lần đăng nhập
        /// </summary>
        public int LoginNum
        {
            get { return _RoleDataEx.LoginNum; }
            set { lock (this) _RoleDataEx.LoginNum = value; }
        }

        /// <summary>
        /// Tổng thời gina đăng nhập
        /// </summary>
        public int TotalOnlineSecs
        {
            get { return _RoleDataEx.TotalOnlineSecs; }
            set { lock (this) _RoleDataEx.TotalOnlineSecs = value; }
        }

        /// <summary>
        /// Thời gian offline gần đây
        /// </summary>
        public long LastOfflineTime
        {
            get { return _RoleDataEx.LastOfflineTime; }
            set { lock (this) _RoleDataEx.LastOfflineTime = value; }
        }

        /// <summary>
        /// Thời gian đăng ký
        /// </summary>
        public long RegTime
        {
            get { return _RoleDataEx.RegTime; }
            set { lock (this) _RoleDataEx.RegTime = value; }
        }

        /// <summary>
        /// Ngày đăng nhập
        /// </summary>
        private int _LoginDayID = TimeUtil.NowDateTime().DayOfYear;

        /// <summary>
        /// ID ngày đăng nhập
        /// </summary>
        public int LoginDayID
        {
            get { return _LoginDayID; }
            set { lock (this) _LoginDayID = value; }
        }

        #endregion LoginRecore

        #region GIAODICH_TRADE

        /// <summary>
        /// THỜI GIAN CẤM GIAO DỊCH
        /// </summary>
        public long BanTradeToTicks
        {
            get { { return _RoleDataEx.BantTradeToTicks; } }
            set { lock (this) { _RoleDataEx.BantTradeToTicks = value; } }
        }

        private int _ExchangeID;

        /// <summary>
        /// Id giao dịch
        /// </summary>
        public int ExchangeID
        {
            get { return _ExchangeID; }
            set { lock (this) _ExchangeID = value; }
        }

        private long _ExchangeTicks;

        /// <summary>
        /// Thời gian giao dịch gần đây
        /// </summary>
        public long ExchangeTicks
        {
            get { return _ExchangeTicks; }
            set { lock (this) _ExchangeTicks = value; }
        }

        #endregion GIAODICH_TRADE

        #region Danh sách Buff

        /// <summary>
        /// Toàn bộ dánh ách BUFFF hiện tại
        /// </summary>
        public List<BufferData> BufferDataList
        {
            get { lock (this) return _RoleDataEx.BufferDataList; }
            set { lock (this) _RoleDataEx.BufferDataList = value; }
        }

        #endregion Danh sách Buff

        #region ITEMINGAME

        /// <summary>
        /// Danh sách vật phẩm khóa
        /// </summary>
        private GoodsPackItem _GoodsPackItem = null;

        /// <summary>
        /// Các gói khóa
        /// </summary>
        public GoodsPackItem LockedGoodsPackItem
        {
            get { return _GoodsPackItem; }
            set { lock (this) _GoodsPackItem = value; }
        }

        public List<GoodsLimitData> GoodsLimitDataList
        {
            get { return _RoleDataEx.GoodsLimitDataList; }
            set { lock (this) _RoleDataEx.GoodsLimitDataList = value; }
        }

        /// <summary>
        /// Danh sách vật phẩm đang bày bán
        /// </summary>
        public List<GoodsData> SaleGoodsDataList
        {
            get { lock (this) return _RoleDataEx.SaleGoodsDataList; }
            set { lock (this) _RoleDataEx.SaleGoodsDataList = value; }
        }

        /// <summary>
        /// DANH SÁCH ITEM
        /// </summary>
        public List<GoodsData> GoodsDataList
        {
            get { lock (this) return _RoleDataEx.GoodsDataList; }
            set { lock (this) _RoleDataEx.GoodsDataList = value; }
        }

        #endregion ITEMINGAME


        #region KTCOIN

        public long LastActionReChage = 0;

        public long LastRequestKTCoin = 0;

        public int KTCoin = 0;

        #endregion

        /// <summary>
        /// Đối tượng Mutex dùng khóa Lock
        /// </summary>
        public object PropPointMutex { get; } = new object();

        /// <summary>
        /// Action hiện tại
        /// </summary>
        public int CurrentAction
        {
            get { return (int)this.m_eDoing; }
            set
            {
                lock (this)
                {
                    if ((int)this.m_eDoing != value)
                    {
                        this.m_eDoing = (KE_NPC_DOING)value;
                    }
                }
            }
        }

        #region CLIENT_CONNECT_DISCONNECT

        /// <summary>
        /// Số lần kiểm tra
        /// </summary>
        private int _ClientHeartCount = 0;

        /// <summary>
        /// Số lần kiểm tra
        /// </summary>
        public int ClientHeartCount
        {
            get { return _ClientHeartCount; }
            set { lock (this) _ClientHeartCount = value; }
        }

        /// <summary>
        /// Lần kiểm tra gàn đây nhất
        /// </summary>
        private long _LastClientHeartTicks = TimeUtil.NOW();

        /// <summary>
        /// Lần kiểm tra gần đây nhất
        /// </summary>
        public long LastClientHeartTicks
        {
            get { return _LastClientHeartTicks; }
            set { lock (this) _LastClientHeartTicks = value; }
        }

        /// <summary>
        /// Close client step
        /// </summary>
        private int _ClosingClientStep = 0;

        /// <summary>
        /// Close client step
        /// </summary>
        public int ClosingClientStep
        {
            get { return _ClosingClientStep; }
            set { lock (this) _ClosingClientStep = value; }
        }

        #endregion CLIENT_CONNECT_DISCONNECT

        #region GIFTCODE

        /// <summary>
        /// Thời gian kích hoạt code
        /// </summary>
        private long _GetLiPinMaTicks = 0;

        /// <summary>
        /// Thời gian kích hoạt code
        /// </summary>
        public long GetLiPinMaTicks
        {
            get { return _GetLiPinMaTicks; }
            set { lock (this) _GetLiPinMaTicks = value; }
        }

        #endregion GIFTCODE

        #region Túi đồ

        /// <summary>
        /// Túi phụ
        /// </summary>
        public List<GoodsData> _PortableGoodsDataList = null;

        public List<GoodsData> PortableGoodsDataList
        {
            get { lock (this) return _PortableGoodsDataList; }
            set { lock (this) _PortableGoodsDataList = value; }
        }

        /// <summary>
        /// Tạm set chết số ô là 100
        /// </summary>
        public int BagNum = 100;

        //{
        //    get
        //    {
        //        int tmpVar = _RoleDataEx.BagNum;
        //        return tmpVar;
        //    }

        //    set
        //    {
        //        _RoleDataEx.BagNum = value;
        //    }
        //}

        /// </summary>
        /// Thời giang đang mở túi
        /// </summary>
        private int _OpenPortableGridTime = 0;

        /// <summary>
        /// Thời gian mở túi
        /// </summary>
        public int OpenPortableGridTime
        {
            get { return _OpenPortableGridTime; }
            set { lock (this) _OpenPortableGridTime = value; }
        }

        /// <summary>
        /// Túi phụ
        /// </summary>
        public PortableBagData MyPortableBagData
        {
            get { return _RoleDataEx.MyPortableBagData; }
            set { lock (this) _RoleDataEx.MyPortableBagData = value; }
        }

        /// <summary>
        /// Tọa độ mở kho
        /// </summary>
        public Point OpenPortableBagPoint;

        #endregion Túi đồ

        #region Phụ bản

        /// <summary>
        /// ID phụ bản
        /// </summary>
        private int _CopyMapID = -1;

        /// <summary>
        /// ID phụ bản
        /// </summary>
        public int CopyMapID
        {
            get { { return _CopyMapID; } }
            set { lock (this) _CopyMapID = value; }
        }

        #endregion Phụ bản

        #region QUANAP_QUATANG

        public HuodongData MyHuodongData
        {
            get { return _RoleDataEx.MyHuodongData; }
            set { lock (this) _RoleDataEx.MyHuodongData = value; }
        }

        public RoleDailyData MyRoleDailyData
        {
            get { return _RoleDataEx.MyRoleDailyData; }
            set { lock (this) _RoleDataEx.MyRoleDailyData = value; }
        }

        #endregion QUANAP_QUATANG

        #region BAYBAN_Stall

        private StallData _StallDataItem = null;

        public StallData StallDataItem
        {
            get { return _StallDataItem; }
            set { lock (this) _StallDataItem = value; }
        }

        #endregion BAYBAN_Stall

        #region Sự kiện liên quan đăng nhập

        /// <summary>
        /// 每日在线奖励
        /// </summary>
        private List<GoodsData> _DailyOnLineAwardGift = null;

        /// <summary>
        /// 每日在线奖励
        /// </summary>
        public List<GoodsData> DailyOnLineAwardGift
        {
            get { return _DailyOnLineAwardGift; }
            set { lock (this) _DailyOnLineAwardGift = value; }
        }

        /// <summary>
        /// 连续登陆奖励
        /// </summary>
        private List<GoodsData> _SeriesLoginAwardGift = null;

        /// <summary>
        /// 连续奖励
        /// </summary>
        public List<GoodsData> SeriesLoginAwardGift
        {
            get { return _SeriesLoginAwardGift; }
            set { lock (this) _SeriesLoginAwardGift = value; }
        }



        /// <summary>
        /// Mỗi ngày đăng nhập thời gian (đơn vị giây)
        /// </summary>
        private int _DayOnlineSecond = 10;

        /// <summary>
        /// Mỗi ngày đăng nhập thời gian (đơn vị giây)
        /// </summary>
        public int DayOnlineSecond
        {
            get { return _DayOnlineSecond; }
            set { _DayOnlineSecond = value; }
        }

        /// <summary>
        /// 临时每日登陆时间长 单位-秒
        /// </summary>
        private int _BakDayOnlineSecond = 10;

        /// <summary>
        /// 临时每日登陆时间长 单位-秒
        /// </summary>
        public int BakDayOnlineSecond
        {
            get { return _BakDayOnlineSecond; }
            set { _BakDayOnlineSecond = value; }
        }

        /// <summary>
        /// 登陆时间长开始记录时间 单位-秒
        /// </summary>
        private long _DayOnlineRecSecond = 10;

        /// <summary>
        /// 登陆时间长开始记录时间 单位-秒
        /// </summary>
        public long DayOnlineRecSecond
        {
            get { return _DayOnlineRecSecond; }
            set { _DayOnlineRecSecond = value; }
        }

        /// <summary>
        /// 连续登陆次数 1-7
        /// </summary>
        public int _SeriesLoginNum = 0;

        /// <summary>
        /// Quà login từ 1-7
        /// </summary>
        public int SeriesLoginNum
        {
            get { return _SeriesLoginNum; }
            set { _SeriesLoginNum = value; }
        }

        /// <summary>

        /// </summary>
        private bool _DailyActiveDayLginSetFlag = false;

        /// <summary>

        /// </summary>
        public bool DailyActiveDayLginSetFlag
        {
            get { return _DailyActiveDayLginSetFlag; }
            set { lock (this) _DailyActiveDayLginSetFlag = value; }
        }

        /// <summary>
        /// Tổng số ngày đăng nhập
        /// </summary>
        private uint _TotalDayLoginNum = 0;

        /// <summary>
        /// Tổng số ngày đăng nhập
        /// </summary>
        public uint TotalDayLoginNum
        {
            get { return _TotalDayLoginNum; }
            set { lock (this) _TotalDayLoginNum = value; }
        }

        /// <summary>
        /// Tổng số quái đã giết
        /// </summary>
        private uint _TotalKilledMonsterNum = 0;

        /// <summary>
        /// Ghi lại tích lũy đã giết bao nhieu con quái
        /// </summary>
        public uint TotalKilledMonsterNum
        {
            get { return _TotalKilledMonsterNum; }
            set { lock (this) _TotalKilledMonsterNum = value; }
        }

        /// <summary>
        /// Đăng nhập liên tiếp
        /// </summary>
        private uint _ContinuousDayLoginNum = 0;

        /// <summary>
        /// Đăng nhập liên tiếp
        /// </summary>
        public uint ContinuousDayLoginNum
        {
            get { return _ContinuousDayLoginNum; }
            set { lock (this) _ContinuousDayLoginNum = value; }
        }

        /// <summary>
        /// 玩家活跃值
        /// </summary>
        private int _DailyActiveValues = 0;

        /// <summary>
        /// 玩家活跃值
        /// </summary>
        public int DailyActiveValues
        {
            get { return _DailyActiveValues; }
            set { lock (this) _DailyActiveValues = value; }
        }

        /// <summary>
        /// 玩家活跃dayID
        /// </summary>
        private int _DailyActiveDayID = 0;

        /// <summary>
        /// 玩家活跃dayID
        /// </summary>
        public int DailyActiveDayID
        {
            get { return _DailyActiveDayID; }
            set { lock (this) _DailyActiveDayID = value; }
        }

        /// <summary>
        /// 玩家活跃每日登陆次数
        /// </summary>
        private uint _DailyActiveDayLginCount = 0;

        /// <summary>
        /// 玩家活跃每日登陆次数
        /// </summary>
        public uint DailyActiveDayLginCount
        {
            get { return _DailyActiveDayLginCount; }
            set { lock (this) _DailyActiveDayLginCount = value; }
        }

        /// <summary>
        /// Thời gian ủy thác bạch cầu hoàn tính bằng phút
        /// </summary>
        public int baijuwan { get; set; }

        /// <summary>
        /// Thời gina ủy thác đại bạch cầu hoàn tính bằng phút
        /// </summary>
        public int baijuwanpro { get; set; }

        #endregion Sự kiện liên quan đăng nhập
    }
}