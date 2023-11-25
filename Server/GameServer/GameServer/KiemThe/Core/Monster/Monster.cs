using GameServer.Core.Executor;
using GameServer.KiemThe;
using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic;
using Server.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows;

namespace GameServer.Logic
{
    /// <summary>
    /// Đối tượng quái vật
    /// </summary>
    public partial class Monster : GameObject, IDisposable
    {
        /// <summary>
        /// Tạo mới đối tượng quái
        /// </summary>
        public Monster()
        {
            /// Tăng ID tự động
            this.Buffs = new BuffTree(this);
        }

        /// <summary>
        /// Quái tĩnh, không di chuyển
        /// </summary>
        public bool IsStatic { get; set; } = false;

        private bool _UpdatePositionToLocalMapContinuously;
        /// <summary>
        /// Cập nhật vị trí lên LocalMap liên tục
        /// </summary>
        public bool UpdatePositionToLocalMapContinuously
        {
            get
            {
                return this._UpdatePositionToLocalMapContinuously;
            }
            set
            {
                this._UpdatePositionToLocalMapContinuously = value;
                if (value)
                {
                    GameManager.MonsterMgr.AddContinuouslyUpdateMiniMap(this);
                }
                else
                {
                    GameManager.MonsterMgr.RemoveContinuouslyUpdateMiniMap(this);
                }
            }
        }

        /// <summary>
        /// Tên hiện trên bản đồ khu vực
        /// </summary>
        public string LocalMapName { get; set; } = "";

        /// <summary>
        /// Tìm đường sửa dụng thuật toán A*
        /// </summary>
        public bool UseAStarPathFinder { get; set;  } = false;

        /// <summary>
        /// Sự kiện Tick
        /// </summary>
        public Action OnTick { get; set; } = null;

        /// <summary>
        /// Sự kiện khi đối tượng bị tấn công
        /// </summary>
        public Action<GameObject, int> OnTakeDamage { get; set; } = null;

        #region Core
        /// <summary>
        /// Hàm này gọi liên tục 0.5s một lần
        /// </summary>
        public override void Tick()
        {
            base.Tick();

            /// Thực thi sự kiện Tick
            this.OnTick?.Invoke();
        }
        #endregion

        #region Clone

        /// <summary>
        /// Copy dữ liệu quái hiện tại chuyển qua đối tượng mới
        /// </summary>
        /// <returns></returns>
        public Monster Clone()
        {
            Monster monster = new Monster();
            monster.Title = this.Title;
            monster.FormatSystemName = this.FormatSystemName;
            monster.MonsterZoneNode = this.MonsterZoneNode;
            monster.MonsterInfo = this.MonsterInfo;
            monster.IsStatic = this.IsStatic;
            if (null == monster.MonsterInfo)
            {
                monster.MonsterInfo = this.MonsterZoneNode.GetMonsterInfo();
                monster.Camp = monster.MonsterInfo.Camp;
            }

            monster.RoleID = this.RoleID;
            monster.RoleName = this.MonsterInfo.Name;
            monster.m_CurrentLife = this.m_CurrentLife;
            monster.ChangeLifeMax(this.m_LifeMax, 0, 0);
            monster.ChangeRunSpeed(this.m_RunSpeed, this.m_RunSpeedAddP, this.m_RunSpeedAddV);
            monster.MonsterType = this.MonsterType;
            monster.Camp = this.Camp;
            monster.m_Level = this.m_Level;
            monster.ScriptID = this.MonsterInfo.ScriptID;
            monster.AIID = this.MonsterInfo.AIID;
            monster.SeekRange = this.MonsterInfo.SeekRange;
            monster.ChangeAttackSpeed(this.MonsterInfo.AtkSpeed, 0);
            monster.ChangeCastSpeed(this.MonsterInfo.AtkSpeed, 0);
            monster.MonsterType = this.MonsterInfo.MonsterType;

            /// Ngũ hành
            if (this.MonsterInfo.Elemental <= KE_SERIES_TYPE.series_none || this.MonsterInfo.Elemental >= KE_SERIES_TYPE.series_num)
            {
                int VALUE = KTGlobal.GetRandomNumber((int)KE_SERIES_TYPE.series_none + 1, (int)KE_SERIES_TYPE.series_num - 1);

                monster.m_Series = (KE_SERIES_TYPE)VALUE;
            }
            else
            {
                monster.m_Series = this.MonsterInfo.Elemental;
            }

            for (int idx = 0; idx < (int)DAMAGE_TYPE.damage_num; idx++)
            {
                DAMAGE_TYPE damageType = (DAMAGE_TYPE)idx;

                monster.SetSeriesDamagePhysics(damageType, this.GetSeriesDamagePhysics(damageType));
                monster.SetResist(damageType, this.GetResist(damageType));
            }
            Monster.IncMonsterCount();

            return monster;
        }

        #endregion Clone

        #region Dữ liệu quái vật

        /// <summary>
        /// Trả về dữ liệu quái
        /// </summary>
        /// <returns></returns>
        public MonsterData GetMonsterData()
        {
            MonsterData _MonsterData = new MonsterData();

            _MonsterData.RoleID = this.RoleID;
            _MonsterData.RoleName = this.RoleName;
            _MonsterData.ExtensionID = this.MonsterInfo.ExtensionID;
            _MonsterData.Level = this.m_Level;
            _MonsterData.MaxHP = this.m_CurrentLifeMax;
            _MonsterData.Camp = this.Camp;
            _MonsterData.PosX = (int)this.CurrentPos.X;
            _MonsterData.PosY = (int)this.CurrentPos.Y;
            _MonsterData.Direction = (int)this.CurrentDir;
            _MonsterData.HP = this.m_CurrentLife;
            _MonsterData.Elemental = (int)this.m_Series;
            _MonsterData.MoveSpeed = this.GetCurrentRunSpeed();
            _MonsterData.Title = this.Title;
            _MonsterData.AttackSpeed = this.GetCurrentAttackSpeed();
            _MonsterData.MonsterType = (int)this.MonsterType;


            /// Danh sách Buff
            _MonsterData.ListBuffs = this.Buffs.ToBufferData();

            return _MonsterData;
        }

        /// <summary>
        /// Tái sinh quái vật
        /// </summary>
        public Point Relive()
        {
            this.UniqueID = Global.GetUniqueID();

            /// Ngừng di chuyển
            if (KTMonsterStoryBoardEx.Instance != null)
            {
                KTMonsterStoryBoardEx.Instance.Remove(this);
            }

            this._LastDeadTicks = 0;
            this.m_CurrentLife = (int)this.MonsterInfo.MaxHP;
            this.m_eDoing = KiemThe.Entities.KE_NPC_DOING.do_stand;

            this.WhoKillMeID = 0;
            this.WhoKillMeName = "";

            this.Awake();

            Point toPoint = Global.GetMapPointByGridXY(ObjectTypes.OT_MONSTER, MonsterZoneNode.MapCode, MonsterZoneNode.ToX, MonsterZoneNode.ToY, MonsterZoneNode.Radius, 0, true);

            this.CurrentPos = toPoint;
            this.CurrentDir = (KiemThe.Entities.Direction)Global.GetRandomNumber(0, 8);
            this.ToPos = toPoint;
            this.StartPos = this.CurrentPos;
            this.InitializePos = this.InitializePos;

            /// Làm mới ngũ hành
            if (this.MonsterInfo.Elemental <= KE_SERIES_TYPE.series_none || this.MonsterInfo.Elemental >= KE_SERIES_TYPE.series_num)
            {
                this.m_Series = (KE_SERIES_TYPE)KTGlobal.GetRandomNumber((int)KE_SERIES_TYPE.series_none + 1, (int)KE_SERIES_TYPE.series_num - 1);
            }
            else
            {
                this.m_Series = this.MonsterInfo.Elemental;
            }

            return toPoint;
        }

        /// <summary>
        /// Sự kiện khi đối tượng chết
        /// </summary>
        public void OnDead()
        {
            /// Ngừng StoryBoard
            KTMonsterStoryBoardEx.Instance.Remove(this);

            ///Xóa khỏi danh sách quản lý
            GameManager.MapGridMgr.DictGrids[MonsterZoneNode.MapCode].RemoveObject(this);

            /// Chuyển động tác sang chết
            this.m_eDoing = KiemThe.Entities.KE_NPC_DOING.do_death;
            this._LastDeadTicks = TimeUtil.NOW() * 10000;

            /// Đánh dấu đối tượng đã chết
            this.Alive = false;

            /// Xóa toàn bộ Buff và vòng sáng tương ứng
            this.Buffs.RemoveAllBuffs();
            this.Buffs.RemoveAllAruas();
            this.Buffs.RemoveAllAvoidBuffs();

            /// Xóa toàn bộ kỹ năng tự kích hoạt tương ứng
            this.RemoveAllAutoSkills();

            /// Làm mới AI
            this.ResetAI();
        }

        #endregion Dữ liệu quái vật

        #region Quản lý hệ thống

        /// <summary>
        /// Khu vực xuất hiện
        /// </summary>
        public MonsterZone MonsterZoneNode
        {
            get;
            set;
        }

        /// <summary>
        /// Thông tin quái
        /// </summary>
        public MonsterStaticInfo MonsterInfo
        {
            get;
            set;
        }

        private string _RoleName = null;
        /// <summary>
        /// Tên quái
        /// </summary>
        public override string RoleName
        {
            get
            {
                return this._RoleName;
            }
            set
            {
                string oldValue = this._RoleName;
                this._RoleName = value;

                if (oldValue != null)
                {
                    /// Thông báo tên thay đổi
                    KT_TCPHandler.NotifyOthersMyNameChanged(this);
                }
            }
        }

        private string _Title = null;
        /// <summary>
        /// Danh hiệu của đối tượng
        /// </summary>
        public override string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                string oldValue = this._Title;
                this._Title = value;

                if (oldValue != null)
                {
                    /// Thông báo về Client
                    KT_TCPHandler.NotifyOthersMyTitleChanged(this);
                }
            }
        }

        /// <summary>
        /// Còn sống không
        /// </summary>
        public bool Alive { get; set; } = false;

        /// <summary>
        /// Thời gian thực thi Timer lần trước
        /// </summary>
        public long LastExecTimerTicks { get; set; }

        /// <summary>
        /// Số lượng người chơi xung quanh
        /// </summary>
        public int VisibleClientsNum { get; set; }

        /// <summary>
        /// Sự kiện khi quái chết
        /// </summary>
        public Action<GameObject> OnDieCallback { get; set; }

        /// <summary>
        /// ID Script điều khiển
        /// </summary>
        public int ScriptID { get; set; } = -1;

        /// <summary>
        /// ID Script AI điều khiển
        /// </summary>
        public int AIID { get; set; } = -1;

        /// <summary>
        /// Tag của đối tượng
        /// </summary>
        public string Tag { get; set; } = "";

        /// <summary>
        /// Phạm vi đuổi mục tiêu
        /// </summary>
        public int SeekRange { get; set; }

        #endregion Quản lý hệ thống

        #region IObject

        /// <summary>
        /// Loại đối tượng
        /// </summary>
        public override ObjectTypes ObjectType
        {
            get { return ObjectTypes.OT_MONSTER; }
        }

        /// <summary>
        /// Vị trí hiện tại theo tọa độ lưới
        /// </summary>
        public override Point CurrentGrid
        {
            get
            {
                GameMap gameMap = GameManager.MapMgr.DictMaps[this.MonsterZoneNode.MapCode];
                return new Point((int)(this.CurrentPos.X / gameMap.MapGridWidth), (int)(this.CurrentPos.Y / gameMap.MapGridHeight));
            }

            set
            {
                GameMap gameMap = GameManager.MapMgr.DictMaps[this.MonsterZoneNode.MapCode];
                this.CurrentPos = new Point(value.X * gameMap.MapGridWidth + gameMap.MapGridWidth / 2, value.Y * gameMap.MapGridHeight + gameMap.MapGridHeight / 2);
            }
        }

        /// <summary>
        /// ID bản đồ hiện tại
        /// </summary>
        public override int CurrentMapCode
        {
            get
            {
                if (this.MonsterZoneNode == null)
                {
                    return -1;
                }
                return this.MonsterZoneNode.MapCode;
            }
        }

        /// <summary>
        /// ID phụ bản hiện tại
        /// </summary>
        public override int CurrentCopyMapID { get; set; } = -1;

        #endregion IObject

        #region Thuộc tính đối tượng

        /// <summary>
        /// Biến toàn cục ID duy nhất của đối tượng
        /// </summary>
        public long UniqueID { get; set; }

        /// <summary>
        /// Danh sách các biến cục bộ của đối tượng
        /// <para>Sử dụng ở Script Lua</para>
        /// </summary>
        private readonly ConcurrentDictionary<int, long> LocalVariables = new ConcurrentDictionary<int, long>();

        /// <summary>
        /// Trả về biến cục bộ tại vị trí tương ứng của đối tượng
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public long GetLocalParam(int idx)
        {
            if (this.LocalVariables.TryGetValue(idx, out long value))
            {
                return value;
            }
            else
            {
                this.LocalVariables[idx] = 0;
                return 0;
            }
        }

        /// <summary>
        /// Thiết lập biến cục bộ tại vị trí tương ứng của đối tượng
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetLocalParam(int idx, long value)
        {
            this.LocalVariables[idx] = value;
        }

        /// <summary>
        /// Xóa rỗng toàn bộ danh sách biến cục bộ
        /// </summary>
        public void RemoveAllLocalParams()
        {
            this.LocalVariables.Clear();
        }

        #endregion Thuộc tính đối tượng

        #region Thuộc tính cơ bản

        /// <summary>
        /// Loại quái
        /// </summary>
        public MonsterAIType MonsterType { get; set; }

        /// <summary>
        /// ID đối tượng giết
        /// </summary>
        public int WhoKillMeID { get; set; }

        /// <summary>
        /// Tên đối tượng giết
        /// </summary>
        public string WhoKillMeName { get; set; }

        #endregion Thuộc tính cơ bản

        #region Thuộc tính di chuyển
        /// <summary>
        /// Thời gian chết lần trước
        /// </summary>
        private long _LastDeadTicks = 0;

        /// <summary>
        /// Thời gian chết lần trước
        /// </summary>
        public long LastDeadTicks
        {
            get { return _LastDeadTicks; }
        }

        /// <summary>
        /// Đối tượng có đang di chuyển không
        /// </summary>
        public bool IsMoving
        {
            get
            {
                return this.m_eDoing == KE_NPC_DOING.do_walk || this.m_eDoing == KE_NPC_DOING.do_run;
            }
        }

        #endregion Thuộc tính di chuyển

        #region Sự kiện
        /// <summary>
        /// Sự kiện khi quái chết
        /// </summary>
        /// <param name="killer"></param>
        public override void OnDie(GameObject killer)
        {
            /// Làm rỗng danh sách biến cục bộ
            this.RemoveAllLocalParams();
            /// Làm mới AI
            this.ResetAI();
            /// Xóa luồng thực thi
            KTMonsterTimerManager.Instance.Remove(this);

            base.OnDie(killer);

            this.OnDieCallback?.Invoke(killer);
        }

        /// <summary>
        /// Sự kiện khi quái giết đối tượng khác
        /// </summary>
        /// <param name="deadObj"></param>
        public override void OnKillObject(GameObject deadObj)
        {
            base.OnKillObject(deadObj);
        }

        /// <summary>
        /// Sự kiện khi quái đánh trúng
        /// </summary>
        /// <param name="target"></param>
        /// <param name="nDamage"></param>
        public override void OnHitTarget(GameObject target, int nDamage)
        {
            base.OnHitTarget(target, nDamage);
        }

        /// <summary>
        /// Sự kiện khi quái bị đánh trúng
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="nDamage"></param>
        public override void OnBeHit(GameObject attacker, int nDamage)
        {
            base.OnBeHit(attacker, nDamage);
        }

        /// <summary>
        /// Hàm này gọi đến khi đối tượng được tạo ra
        /// </summary>
        public void Awake()
        {
            /// Đánh dấu đối tượng còn sống
            this.Alive = true;

            /// Nếu là loại quái đặc biệt hoặc NPC di động
            if (this.MonsterType == MonsterAIType.DynamicNPC || this.MonsterType == MonsterAIType.Special_Boss || this.MonsterType != MonsterAIType.Special_Normal)
            {
                /// Thực thi Timer của quái
                KTMonsterTimerManager.Instance.Add(this);
            }
        }

        /// <summary>
        /// Hàm này gọi mỗi khi Timer quái được thêm vào
        /// </summary>
        public void Start()
        {
            /// Nếu là Boss hoặc hải tặc thì cho miễn dịch trạng thái
            if (this.MonsterType == MonsterAIType.Boss || this.MonsterType == MonsterAIType.Pirate)
            {
                this.m_IgnoreAllSeriesStates = true;
            }
            /// Nếu là quái tĩnh miễn dịch toàn bộ
            else if (this.MonsterType == MonsterAIType.Static_ImmuneAll)
            {
                this.m_IgnoreAllSeriesStates = true;
            }

            /// Nếu là quái tĩnh
            if (this.MonsterType == MonsterAIType.Static || this.MonsterType == MonsterAIType.Static_ImmuneAll)
            {
                this.IsStatic = true;
            }

            ///// Nếu là Boss hoặc quái đặc biệt - Test only
            //if (this.MonsterType == MonsterAIType.DynamicNPC)
            //{
            //    this.UpdatePositionToLocalMapContinuously = true;
            //}

            /// Danh sách vòng sáng
            if (!string.IsNullOrEmpty(this.MonsterInfo.Auras))
            {
                foreach (string auraSkillString in this.MonsterInfo.Auras.Split(';'))
                {
                    string[] fields = auraSkillString.Split('_');
                    int skillID = int.Parse(fields[0]);
                    int skilllevel = int.Parse(fields[1]);

                    SkillDataEx skillData = KSkill.GetSkillData(skillID);
                    SkillLevelRef skill = new SkillLevelRef()
                    {
                        Data = skillData,
                        AddedLevel = skilllevel,
                    };

                    /// Kích hoạt vòng sáng
                    this.UseSkill(skillID, skilllevel, this);
                }
            }

            /// Danh sách kỹ năng
            if (!string.IsNullOrEmpty(this.MonsterInfo.Skills))
            {
                foreach (string skillString in this.MonsterInfo.Skills.Split(';'))
                {
                    string[] fields = skillString.Split('_');
                    int skillID = int.Parse(fields[0]);
                    int skilllevel = int.Parse(fields[1]);
                    int cooldown = int.Parse(fields[2]);

                    SkillDataEx skillData = KSkill.GetSkillData(skillID);
                    SkillLevelRef skill = new SkillLevelRef()
                    {
                        Data = skillData,
                        AddedLevel = skilllevel,
                        Exp = cooldown,
                    };
                    this.CustomAISkills.Add(skill);
                }
            }
        }
        #endregion Sự kiện

        #region Damage Note





        public class DamgeGroup
        {

            public bool IsTeam { get; set; }
            public int ID { get; set; }
            public long TotalDamage { get; set; }
            public long LastUpdateTime { get; set; }

        }


        public void ResetDamge(List<DamgeGroup> Total)
        {
            foreach (DamgeGroup _Damge in Total)
            {
                _Damge.TotalDamage = 0;
                // Reset damge của thằng này
                this.DamageTakeRecord[_Damge.ID] = _Damge;
            }
        }
        /// <summary>
        /// Ghi lại lượng sát thương nhận được gây ra bởi đối tượng có ID tương ứng
        /// </summary>
        public ConcurrentDictionary<int, DamgeGroup> DamageTakeRecord { get; set; } = new ConcurrentDictionary<int, DamgeGroup>();

        /// <summary>
        /// Thêm sát thương nhận được bởi đối tượng tương ứng vào danh sách, dùng để tính kinh nghiệm nhận được
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="nDamage"></param>
        public void AddDmageToQueue(int roleID, int nDamage, bool IsTeam)
        {
            long UpdateTime = TimeUtil.NOW();
            if (this.DamageTakeRecord.TryGetValue(roleID, out DamgeGroup totalDamageDealt))
            {
                if (totalDamageDealt != null)
                {
                    totalDamageDealt.ID = roleID;
                    totalDamageDealt.TotalDamage = totalDamageDealt.TotalDamage + nDamage;
                    totalDamageDealt.LastUpdateTime = UpdateTime;
                }
                this.DamageTakeRecord[roleID] = totalDamageDealt;
            }
            else
            {
                // Add vào nhóm
                DamgeGroup _Recore = new DamgeGroup();
                _Recore.IsTeam = IsTeam;
                _Recore.LastUpdateTime = UpdateTime;
                _Recore.TotalDamage = nDamage;

                this.DamageTakeRecord.TryAdd(roleID, _Recore);

            }
        }

        #endregion

        #region Dead Queue

        /// <summary>
        /// Thêm đối tượng vào hàng đợi chờ thực hiện hàm Dead
        /// </summary>
        public long AddToDeadQueueTicks
        {
            get;
            set;
        }

        #endregion Dead Queue

        #region Thống kê

        /// <summary>
        /// Mutex dùng trong khóa Lock
        /// </summary>
        private static object CountLock = new object();

        /// <summary>
        /// Tổng số quái
        /// </summary>
        private static int TotalMonsterCount = 0;

        /// <summary>
        /// Tăng tổng số quái
        /// </summary>
        public static void IncMonsterCount()
        {
            lock (Monster.CountLock)
            {
                Monster.TotalMonsterCount++;
            }
        }

        /// <summary>
        /// Giảm tổng số quái
        /// </summary>
        public static void DecMonsterCount()
        {
            lock (Monster.CountLock)
            {
                Monster.TotalMonsterCount--;
            }
        }

        /// <summary>
        /// Trả về tổng số quái hiện tại
        /// </summary>
        public static int GetMonsterCount()
        {
            int count = 0;
            lock (Monster.CountLock)
            {
                count = Monster.TotalMonsterCount;
            }

            return count;
        }

        #endregion Thống kê

        /// <summary>
        /// Hủy đối tượng
        /// </summary>
        public void Dispose()
        {
            /// Đánh dấu đã chết
            this.m_CurrentLife = 0;
            this.m_eDoing = KE_NPC_DOING.do_death;

            this.DamageTakeRecord.Clear();
            this.LocalVariables.Clear();
            this.CustomAISkills.Clear();

            /// Xóa toàn bộ Buff và vòng sáng tương ứng
            this.Buffs.RemoveAllBuffs(false, false);
            this.Buffs.RemoveAllAruas(false, false);

            /// Ngừng Storyboard tương ứng
            KTMonsterStoryBoardEx.Instance.Remove(this);

            /// Ngừng Timer tương ứng
            KTMonsterTimerManager.Instance.Remove(this);
        }
    }
}