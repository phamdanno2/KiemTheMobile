using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data
{
    /// <summary>
    /// Toàn bộ thiết lập settings sẽ láy từ GS về
    /// </summary>
    [ProtoContract]
    public class AutoSettingConfig
    {
        /// <summary>
        /// Tự động PK
        /// </summary>
        [ProtoMember(1)]
        public AutoPKConfig _AutoPKConfig { get; set; }

        /// <summary>
        /// Tự động nhặt đồ
        /// </summary>
        [ProtoMember(2)]
        public PickItemConfig _PickItemConfig { get; set; }

        /// <summary>
        /// Tự động train quái
        /// </summary>
        [ProtoMember(3)]
        public AutoTrainConfig _AutoTrainConfig { get; set; }
    }

    /// <summary>
    /// Config của AUTOPK
    /// </summary>
    ///
    [ProtoContract]
    public class AutoPKConfig
    {
        /// <summary>
        /// Tự động bơm máu
        /// </summary>
        [ProtoMember(1)]
        public bool IsAutoHp { get; set; }

        /// <summary>
        /// % HP sẽ bơm
        /// </summary>
        [ProtoMember(2)]
        public int HpPercent { get; set; }
        /// <summary>
        /// % MP sẽ bơm
        /// </summary>
        [ProtoMember(3)]
        public int MpPercent { get; set; }

        /// <summary>
        /// Tự động ăn thức ăn
        /// </summary>
        [ProtoMember(4)]
        public bool IsAutoEat { get; set; }

        /// <summary>
        /// Auto buff nga my
        /// </summary>
        [ProtoMember(5)]
        public bool IsBuffNM { get; set; }
        /// <summary>
        /// Tự động buff NM percent
        /// </summary>
        [ProtoMember(6)]
        public int AutoNMPercent { get; set; }
        /// <summary>
        /// Tự động hồi sinh đồng đội
        /// </summary>
        [ProtoMember(7)]
        public bool IsAutoReviceTeam { get; set; }

        /// <summary>
        /// Tự động phản kháng khi bị PK
        /// </summary>
        [ProtoMember(8)]
        public bool IsAutoPKAgain { get; set; }

        /// <summary>
        /// Tự động chọn mục tiêu thấp máu
        /// </summary>
        [ProtoMember(9)]
        public bool IsLowHpSelect { get; set; }

        /// <summary>
        /// Ưu tiên chọn khắc hệ
        /// </summary>
        [ProtoMember(10)]
        public bool IsElementalSelect { get; set; }

        /// <summary>
        /// Danh sách skill sẽ chọn để PK
        /// </summary>
        [ProtoMember(11)]
        public List<int> SkillPKSelect { get; set; }
    }

    /// <summary>
    /// Config của pick item
    /// </summary>
    ///
    [ProtoContract]
    public class PickItemConfig
    {
        /// <summary>
        /// Tự động nhặt đồ
        /// </summary>
        [ProtoMember(1)]
        public bool IsAutoPickUp { get; set; }

        /// <summary>
        /// Khoảng cách sẽ nhặt
        /// </summary>
        [ProtoMember(2)]
        public int RadiusPick { get; set; }

        /// <summary>
        /// Chỉ nhặt huyền tinh
        /// </summary>
        [ProtoMember(3)]
        public bool IsOnlyPickCrytal { get; set; }

        /// <summary>
        /// Cấp độ sẽ nhặt
        /// </summary>
        [ProtoMember(4)]
        public int CrytalLevel { get; set; }

        /// <summary>
        /// Chỉ nhặt vật phẩm
        /// </summary>
        [ProtoMember(5)]
        public bool IsOnlyPickEquip { get; set; }

        /// <summary>
        /// Số sao sẽ nhặt
        /// </summary>
        [ProtoMember(6)]
        public int StarPick { get; set; }

        /// <summary>
        /// Tự động sắp xếp túi đồ
        /// </summary>
        [ProtoMember(7)]
        public bool IsAutoSort { get; set; }

        /// <summary>
        /// Tự động bán đồ khi đầy
        /// </summary>
        [ProtoMember(8)]
        public bool IsAutoSellItem { get; set; }

        /// <summary>
        /// Số sao tối thiểu sẽ bán
        /// </summary>
        [ProtoMember(9)]
        public int StarWillSell { get; set; }
    }

    /// <summary>
    /// Auto Train
    /// </summary>
    ///
    [ProtoContract]
    public class AutoTrainConfig
    {
        /// <summary>
        /// Có phải đánh quanh điểm không
        /// </summary>
        ///

        [ProtoMember(1)]
        public bool IsRadius { get; set; }

        /// <summary>
        /// Bán kính đnáh quái
        /// </summary>
        [ProtoMember(2)]
        public int Raidus { get; set; }

        /// <summary>
        /// Chế độ đánh
        /// </summary>
        [ProtoMember(3)]
        public int AttackMode { get; set; }

        /// <summary>
        /// Danh sách kỹ năng sẽ đánh quái
        /// </summary>
        [ProtoMember(4)]
        public List<int> SkillSelect { get; set; }

        /// <summary>
        /// Tự động đốt lửa trại
        /// </summary>
        [ProtoMember(5)]
        public bool IsAutoFireCamp { get; set; }

        /// <summary>
        /// Bỏ qua boss
        /// </summary>
        [ProtoMember(6)]
        public bool IsSkipBoss { get; set; }

        /// <summary>
        /// Ưu tiên chọn quái có hp thấp
        /// </summary>
        [ProtoMember(7)]
        public bool IsLowHpSelect { get; set; }
    }
}
