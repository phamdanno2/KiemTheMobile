using GameServer.KiemThe.Entities;
using GameServer.KiemThe.Logic;
using GameServer.Logic;
using Server.Data;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.KiemThe.Core.Item
{
    /// <summary>
    /// Định nghĩa khi gọi 1 item mới in game để tính toán +- thuộc tính
    /// </summary>
    public class KItem
    {
        public int ItemDBID
        {
            get
            {
                return _GoodDatas.Id;
            }
        }

        public int SuiteID
        {
            get
            {
                return GetBaseItem.SuiteID;
            }
        }

        public ItemData GetBaseItem
        {
            get
            {
                return _ItemData;
            }
        }

        public bool active_all_ornament { get; set; }

        public bool active_suit { get; set; }

        public List<KMagicAttrib> TotalBaseAttrib { get; set; }

        public List<KREQUIRE_ATTR> TotalRequest { get; set; }

        public List<KSOCKET> TotalEnhance { get; set; }

        public List<KMagicAttrib> TotalGreenProp { get; set; }

        public List<KSOCKET> TotalHiddenProp { get; set; }

        public List<KMagicAttrib> BookAtriibute { get; set; }

        public List<KMagicAttrib> TotalRiderProp { get; set; }

        /// <summary>
        /// Các kỹ năng có được từ mật tịch
        /// </summary>
        public List<int> SkillsFromBook
        {
            get
            {
                /// Nếu BookProperty NULL thì toác
                if (this._ItemData.BookProperty == null)
                {
                    return null;
                }

                return new List<int>()
                {
                    this._ItemData.BookProperty.SkillID1,
                    this._ItemData.BookProperty.SkillID2,
                    this._ItemData.BookProperty.SkillID3,
                    this._ItemData.BookProperty.SkillID4,
                };
            }
        }

        public ItemData _ItemData { get; set; }

        public KE_ITEM_EQUIP_DETAILTYPE TypeItem { get; set; }

        public bool ISEQUIP { get; set; }

        /// <summary>
        /// Cấp độ cường hóa
        /// </summary>
        public int Forge_Level { get; set; }

        public GoodsData _GoodDatas { get; set; }

        /// <summary>
        /// Attack Effect TO PLAYER
        /// </summary>
        /// <param name="_Player"></param>
        public void AttackEffect(KPlayer _Player, bool IsDetack)
        {
            /// Nếu là Ngũ Hành Ấn
            if (ItemManager.KD_ISSIGNET(this._ItemData.DetailType))
            {
                /// Lấy ra thông tin dòng cường hóa ngũ hành tương khắc
                if (this._GoodDatas.OtherParams.TryGetValue(ItemPramenter.Pram_1, out string seriesEnhance))
                {
                    try
                    {
                        string[] fields = seriesEnhance.Split('|');
                        int level = int.Parse(fields[0]);

                        /// Tạo KMagicAttrib
                        KMagicAttrib magicAttrib = new KMagicAttrib()
                        {
                            nAttribType = MAGIC_ATTRIB.magic_seriesenhance,
                            nValue = new int[] { level, 0, 0 }
                        };
                        /// Attach thuộc tính
                        KTAttributesModifier.AttachProperty(magicAttrib, _Player, IsDetack);
                    }
                    catch (Exception ex)
                    {
                        LogManager.WriteLog(LogTypes.Exception, ex.ToString());
                    }
                }
                /// Lấy ra thông tin dòng nhược hóa ngũ hành tương khắc
                if (this._GoodDatas.OtherParams.TryGetValue(ItemPramenter.Pram_2, out string seriesConque))
                {
                    try
                    {
                        string[] fields = seriesConque.Split('|');
                        int level = int.Parse(fields[0]);

                        /// Tạo KMagicAttrib
                        KMagicAttrib magicAttrib = new KMagicAttrib()
                        {
                            nAttribType = MAGIC_ATTRIB.magic_seriesabate,
                            nValue = new int[] { level, 0, 0 }
                        };
                        /// Attach thuộc tính
                        KTAttributesModifier.AttachProperty(magicAttrib, _Player, IsDetack);
                    }
                    catch (Exception ex)
                    {
                        LogManager.WriteLog(LogTypes.Exception, ex.ToString());
                    }
                }
            }
            else
            {
                if (this.TotalBaseAttrib != null)
                {
                    if (this.TotalBaseAttrib.Count > 0)
                    {
                        // Attack Base EFFECT
                        foreach (KMagicAttrib _KMagic in this.TotalBaseAttrib)
                        {
                            KTAttributesModifier.AttachProperty(_KMagic, _Player, IsDetack);
                        }
                    }
                }
            }

            if (TotalGreenProp != null)
            {
                if (this.TotalGreenProp.Count > 0)
                {
                    //Attack Green Prob
                    foreach (KMagicAttrib _KMagic in this.TotalGreenProp)
                    {
                        KTAttributesModifier.AttachProperty(_KMagic, _Player, IsDetack);
                    }
                }
            }

            if (this.TotalEnhance != null)
            {
                if (this.TotalEnhance.Count > 0)
                {
                    // Attack Enhance Efect
                    foreach (KSOCKET _KSocket in this.TotalEnhance)
                    {
                        if (_KSocket.bActive)
                        {
                            KTAttributesModifier.AttachProperty(_KSocket.sMagicAttrib, _Player, IsDetack);
                        }
                    }
                }
            }
            if (this.TotalHiddenProp != null)
            {
                if (this.TotalHiddenProp.Count > 0)
                {
                    // Attack Hidden Prob
                    foreach (KSOCKET _KSocket in this.TotalHiddenProp)
                    {
                        if (_KSocket.bActive)
                        {
                            KTAttributesModifier.AttachProperty(_KSocket.sMagicAttrib, _Player, IsDetack);
                        }
                    }
                }
            }

            if (this.BookAtriibute != null)
            {
                if (this.BookAtriibute.Count > 0)
                {   // Attack Efffect OffBOOK
                    foreach (KMagicAttrib _KMagic in this.BookAtriibute)
                    {
                        KTAttributesModifier.AttachProperty(_KMagic, _Player, IsDetack);
                    }
                }

                /// Duyệt danh sách kỹ năng có
                foreach (int skillID in this.SkillsFromBook)
                {
                    /// Nếu kỹ năng không tồn tại
                    if (KSkill.GetSkillData(skillID) == null)
                    {
                        continue;
                    }

                    /// Nếu chưa có kỹ năng trên người
                    if (!_Player.Skills.HasSkill(skillID))
                    {
                        /// Thêm kỹ năng
                        _Player.Skills.AddSkill(skillID);
                        _Player.Skills.AddSkillLevel(skillID, 1);
                    }
                }
            }
        }

        /// <summary>
        /// Kích hoạt thuộc tính cưỡi ngựa
        /// </summary>
        /// <param name="player"></param>
        /// <param name="isDetach"></param>
        public void AttachRiderEffect(KPlayer player, bool isDetach)
        {
            if (this.TotalRiderProp.Count > 0)
            {
                foreach (KMagicAttrib _KMagic in this.TotalRiderProp)
                {
                    KTAttributesModifier.AttachProperty(_KMagic, player, isDetach);
                }
            }
        }

        public void InitHorseProperty()
        {
            List<RiderProp> RiderProp = this._ItemData.RiderProp;
            List<KMagicAttrib> TotalRiderPropEx = new List<KMagicAttrib>();

            if (RiderProp.Count > 0)
            {
                foreach (RiderProp _Prob in RiderProp)
                {
                    KMagicAttrib Atribute = new KMagicAttrib();

                    Atribute.Init(_Prob.RidePropType, _Prob.RidePropPA1Min, _Prob.RidePropPA2Min, _Prob.RidePropPA3Min);
                    TotalRiderPropEx.Add(Atribute);
                }
            }

            this.TotalRiderProp = TotalRiderPropEx;
        }

        /// <summary>
        /// DONE
        /// </summary>
        public void InitTotalEnhance()
        {
            this.Forge_Level = _GoodDatas.Forge_level;

            List<KSOCKET> TotalEnhance = new List<KSOCKET>();

            List<ENH> ListEnhance = this._ItemData.ListEnhance;

            if (ListEnhance.Count > 0)
            {
                foreach (ENH _Ech in ListEnhance)
                {
                    KSOCKET _Sock = new KSOCKET();

                    _Sock.Index = _Ech.Index;

                    KMagicAttrib Magic = new KMagicAttrib();

                    if (Magic.Init(_Ech.EnhMAName, _Ech.EnhMAPA1Min, _Ech.EnhMAPA2Min, _Ech.EnhMAPA3Min))
                    {
                        _Sock.sMagicAttrib = Magic;
                        if (this.Forge_Level >= _Ech.EnhTimes)
                        {
                            if (_Ech.EnhMAName == "active_all_ornament")
                            {
                                active_all_ornament = true;
                            }
                            if (_Ech.EnhMAName == "active_suit")
                            {
                                active_suit = true;
                            }

                            /// Nếu là dòng ngũ hành tương khắc hệ gì đó
                            if (_Ech.EnhMAName == "damage_series_resist")
                            {
                                Magic.nValue[2] = GetResValue(this._GoodDatas.Series);
                            }
                            /// Nếu là dòng tỷ lệ bỏ qua kháng
                            else if (_Ech.EnhMAName == "ignoreresist_p")
                            {
                                Magic.nValue[2] = this._GoodDatas.Series;
                            }

                            _Sock.bActive = true;
                        }
                        else
                        {
                            _Sock.bActive = false;
                        }

                        TotalEnhance.Add(_Sock);
                    }
                    else
                    {
                        throw new System.ArgumentException("Symbol not found : " + _Ech.EnhMAName);
                    }
                }
            }

            this.TotalEnhance = TotalEnhance;
        }

        public void Update(KPlayer _Player, bool IsActiveOrnament = false, bool IsActiveAllSuite = false)
        {
            int ACTIVE = 0;

            int PlayerSeri = (int)_Player.m_Series;

            int ItemSeri = this._GoodDatas.Series;

            // Nếu nhân vật và vật phẩm có quan hệ tương sinh thì + 1
            if (KTGlobal.g_IsAccrue(PlayerSeri, ItemSeri))
            {
                ACTIVE++;
            }

            int nPlace = ItemManager.g_anEquipPos[_ItemData.DetailType];

            /// Nếu không có dòng ẩn
            if (!ItemManager.g_anEquipActive.TryGetValue(nPlace, out ActiveByItem activeCheck))
            {
                return;
            }

            int Postion1 = activeCheck.Pos1;

            int Postion2 = activeCheck.Pos2;

            KItem _Item1 = _Player._KPlayerEquipBody.GetItemByPostion(Postion1);

            if (_Item1 != null)
            {
                // Nếu vật phẩm và vật phẩm thứ 1 yêu cầu kích hoạt có quan hệ tương sinh thì +1
                if (KTGlobal.g_IsAccrue(_Item1._GoodDatas.Series, ItemSeri))
                {
                    ACTIVE++;
                }
            }

            KItem _Item2 = _Player._KPlayerEquipBody.GetItemByPostion(Postion2);
            if (_Item2 != null)
            {
                // Nếu vật phẩm và vật phẩm thứ 2 yêu cầu kích hoạt có quan hệ tương sinh thì +1
                if (KTGlobal.g_IsAccrue(_Item2._GoodDatas.Series, ItemSeri))
                {
                    ACTIVE++;
                }
            }

            // DETACK ALL SOCKET
            if (this.TotalHiddenProp != null)
            {
                if (this.TotalHiddenProp.Count > 0)
                {
                    foreach (KSOCKET _Socket in this.TotalHiddenProp)
                    {
                        _Socket.bActive = false;
                    }
                }
            }

            if (ACTIVE >= 1)
            {
                if (this.TotalHiddenProp != null)
                {
                    if (this.TotalHiddenProp.Count > 0)
                    {
                        this.TotalHiddenProp[0].bActive = true;
                    }
                }
            }

            if (ACTIVE >= 2)
            {
                if (this.TotalHiddenProp != null)
                {
                    if (this.TotalHiddenProp.Count > 1)
                    {
                        this.TotalHiddenProp[1].bActive = true;
                    }
                }
            }
            if (ACTIVE >= 3)
            {
                if (this.TotalHiddenProp != null)
                {
                    if (this.TotalHiddenProp.Count > 2)
                    {
                        this.TotalHiddenProp[2].bActive = true;
                    }
                }
            }

            if (IsActiveOrnament)
            {
                if (ItemManager.KD_ISORNAMENT(this._ItemData.DetailType))
                {
                    if (this.TotalHiddenProp != null)
                    {
                        if (this.TotalHiddenProp.Count > 0)
                        {
                            this.TotalHiddenProp[0].bActive = true;
                        }

                        if (this.TotalHiddenProp.Count > 1)
                        {
                            this.TotalHiddenProp[1].bActive = true;
                        }
                        if (this.TotalHiddenProp.Count > 2)
                        {
                            this.TotalHiddenProp[2].bActive = true;
                        }
                    }
                }
            }

            if (IsActiveAllSuite)
            {
                if (this.TotalHiddenProp != null)
                {
                    if (this.TotalHiddenProp.Count > 0)
                    {
                        this.TotalHiddenProp[0].bActive = true;
                    }

                    if (this.TotalHiddenProp.Count > 1)
                    {
                        this.TotalHiddenProp[1].bActive = true;
                    }
                    if (this.TotalHiddenProp.Count > 2)
                    {
                        this.TotalHiddenProp[2].bActive = true;
                    }
                }
            }
        }

        public void InitHiddenProbs()
        {
            byte[] Base64Decode = Convert.FromBase64String(_GoodDatas.Props);

            ItemGenByteData _ItemBuild = DataHelper.BytesToObject<ItemGenByteData>(Base64Decode, 0, Base64Decode.Length);

            if (_ItemBuild != null)
            {
                if (_ItemBuild.HiddenProbsCount > 0)
                {
                    List<KSOCKET> TotalHiddenEffect = new List<KSOCKET>();

                    List<PropMagic> List = _ItemData.HiddenProp.OrderBy(x => x.Index).ToList();

                    for (int i = 0; i < _ItemBuild.HiddenProbsCount; i++)
                    {
                        KSOCKET _Sock = new KSOCKET();

                        int PostionGet = (i * 3);

                        KMagicAttrib Atribute = new KMagicAttrib();

                        int VALUE0 = _ItemBuild.HiddenProbsValue[PostionGet];
                        int VALUE1 = _ItemBuild.HiddenProbsValue[PostionGet + 1];
                        int VALUE2 = _ItemBuild.HiddenProbsValue[PostionGet + 2];

                        int LevelMagic = this._GoodDatas.Forge_level + List[i].MagicLevel;

                        //ORIGIN VALUE
                        MagicAttribLevel FindOrginalValue = ItemManager.TotalMagicAttribLevel.Where(x => x.MagicName == List[i].MagicName && x.Level == List[i].MagicLevel).FirstOrDefault();

                        MagicAttribLevel FindMagic = ItemManager.TotalMagicAttribLevel.Where(x => x.MagicName == List[i].MagicName && x.Level == LevelMagic).FirstOrDefault();

                        int _Value0 = 0, _Value1 = 0, _Value2 = 0;

                        if (VALUE0 != -1)
                        {
                            if (FindMagic != null)
                            {
                                int Percent0 = RecaculationPercent(FindOrginalValue.MA1Min, FindOrginalValue.MA1Max, VALUE0);

                                if (Percent0 > 0)
                                {
                                    int AddValue = ((FindMagic.MA1Max - FindMagic.MA1Min) * Percent0) / 100;

                                    _Value0 = FindMagic.MA1Min + AddValue;
                                }
                                else
                                {
                                    _Value0 = FindMagic.MA1Min;
                                }
                            }
                        }

                        if (VALUE1 != -1)
                        {
                            if (FindMagic != null)
                            {
                                int Percent1 = RecaculationPercent(FindOrginalValue.MA2Min, FindOrginalValue.MA2Max, VALUE1);
                                if (Percent1 > 0)
                                {
                                    int AddValue = ((FindMagic.MA2Max - FindMagic.MA2Min) * Percent1) / 100;

                                    _Value1 = FindMagic.MA2Min + AddValue;
                                }
                                else
                                {
                                    _Value1 = FindMagic.MA2Min;
                                }
                            }
                        }
                        if (VALUE2 != -1)
                        {
                            if (FindMagic != null)
                            {
                                int Percent2 = RecaculationPercent(FindOrginalValue.MA3Min, FindOrginalValue.MA3Max, VALUE2);

                                if (Percent2 > 0)
                                {
                                    int AddValue = ((FindMagic.MA3Max - FindMagic.MA3Min) * Percent2) / 100;

                                    _Value2 = FindMagic.MA3Min + AddValue;
                                }
                                else
                                {
                                    _Value2 = FindMagic.MA3Min;
                                }
                            }
                        }

                        /// Nếu là dòng ngũ hành tương khắc hệ gì đó
                        if (List[i].MagicName == "damage_series_resist")
                        {
                            _Value2 = GetResValue(this._GoodDatas.Series);
                        }
                        /// Nếu là dòng tỷ lệ bỏ qua kháng
                        else if (List[i].MagicName == "ignoreresist_p")
                        {
                            _Value2 = this._GoodDatas.Series;
                        }

                        if (Atribute.Init(List[i].MagicName, _Value0, _Value1, _Value2))
                        {
                            // SET ALL ACTIVE FAIL
                            _Sock.Index = _ItemData.HiddenProp[i].Index;
                            _Sock.sMagicAttrib = Atribute;
                            _Sock.bActive = false;

                            TotalHiddenEffect.Add(_Sock);
                        }
                        else
                        {
                            throw new System.ArgumentException("Symboy not found : " + List[i].MagicName);
                        }
                    }

                    this.TotalHiddenProp = TotalHiddenEffect;
                }
            }
        }

        public void InitBookProperty()
        {
            byte[] Base64Decode = Convert.FromBase64String(_GoodDatas.Props);

            ItemGenByteData _ItemBuild = DataHelper.BytesToObject<ItemGenByteData>(Base64Decode, 0, Base64Decode.Length);
            List<KMagicAttrib> BookProfile = new List<KMagicAttrib>();

            if (_ItemBuild.HaveBookProperties)
            {
                if (_GoodDatas.OtherParams.TryGetValue(ItemPramenter.Pram_1, out string BookLevel))
                {
                    int LevelBook = Int32.Parse(BookLevel);

                    BookAttr FindBook = ItemManager.GetItemTemplate(_GoodDatas.GoodsID).BookProperty;

                    int StrBase = FindBook.StrInitMin / 100;
                    int DevBase = FindBook.DexInitMin / 100;
                    int VitBase = FindBook.VitInitMin / 100;
                    int EngBase = FindBook.EngInitMin / 100;

                    if (_ItemBuild.BookPropertyValue[0] != -1)
                    {
                        int PowerDiv = _ItemData.FightPower;

                        int PointRevice = PowerDiv * LevelBook;

                        int STRFILNALADD = StrBase + _ItemBuild.BookPropertyValue[0] + PointRevice;

                        KMagicAttrib Atribute = new KMagicAttrib();
                        Atribute.nAttribType = MAGIC_ATTRIB.magic_strength_v;
                        Atribute.nValue[0] = STRFILNALADD;
                        Atribute.nValue[1] = -1;
                        Atribute.nValue[2] = -1;

                        BookProfile.Add(Atribute);
                    }

                    if (_ItemBuild.BookPropertyValue[1] != -1)
                    {
                        int PowerDiv = _ItemData.FightPower;

                        int PointRevice = PowerDiv * LevelBook;

                        int DEVFINAL = DevBase + _ItemBuild.BookPropertyValue[1] + PointRevice;

                        KMagicAttrib Atribute = new KMagicAttrib();
                        Atribute.nAttribType = MAGIC_ATTRIB.magic_dexterity_v;
                        Atribute.nValue[0] = DEVFINAL;
                        Atribute.nValue[1] = -1;
                        Atribute.nValue[2] = -1;

                        BookProfile.Add(Atribute);
                    }

                    if (_ItemBuild.BookPropertyValue[2] != -1)
                    {
                        int PowerDiv = _ItemData.FightPower;

                        int PointRevice = PowerDiv * LevelBook;

                        int VitFinal = VitBase + _ItemBuild.BookPropertyValue[2] + PointRevice;

                        KMagicAttrib Atribute = new KMagicAttrib();
                        Atribute.nAttribType = MAGIC_ATTRIB.magic_vitality_v;
                        Atribute.nValue[0] = VitFinal;
                        Atribute.nValue[1] = -1;
                        Atribute.nValue[2] = -1;

                        BookProfile.Add(Atribute);
                    }
                    if (_ItemBuild.BookPropertyValue[3] != -1)
                    {
                        int PowerDiv = _ItemData.FightPower;

                        int PointRevice = PowerDiv * LevelBook;

                        int EngFinal = EngBase + _ItemBuild.BookPropertyValue[3] + PointRevice;

                        KMagicAttrib Atribute = new KMagicAttrib();
                        Atribute.nAttribType = MAGIC_ATTRIB.magic_energy_v;
                        Atribute.nValue[0] = EngFinal;
                        Atribute.nValue[1] = -1;
                        Atribute.nValue[2] = -1;

                        BookProfile.Add(Atribute);
                    }
                }
            }
            this.BookAtriibute = BookProfile;
        }

        public int RecaculationPercent(int MinValue, int MaxValue, int CurenValue)
        {
            if (MaxValue == MinValue)
            {
                return 0;
            }
            return (CurenValue * 100) / (MaxValue - MinValue);
        }

        /// <summary>
        /// Khởi tạo thuộc tính Base của Item
        /// Bao gồm Base Atrib + Green Atrib
        /// </summary>
        public void InitBaseAttribParse()
        {
            byte[] Base64Decode = Convert.FromBase64String(_GoodDatas.Props);

            ItemGenByteData _ItemBuild = DataHelper.BytesToObject<ItemGenByteData>(Base64Decode, 0, Base64Decode.Length);

            if (_ItemBuild != null)
            {
                if (_ItemBuild.BasicPropCount > 0)
                {
                    List<KMagicAttrib> TotalBaseAttrib = new List<KMagicAttrib>();

                    List<BasicProp> List = _ItemData.ListBasicProp.OrderBy(x => x.Index).ToList();

                    for (int i = 0; i < _ItemBuild.BasicPropCount; i++)
                    {
                        int PostionGet = (i * 3);

                        KMagicAttrib Atribute = new KMagicAttrib();

                        int VALUE0 = _ItemBuild.BasicPropValue[PostionGet];
                        int VALUE1 = _ItemBuild.BasicPropValue[PostionGet + 1];
                        int VALUE2 = _ItemBuild.BasicPropValue[PostionGet + 2];

                        int _Value0 = 0, _Value1 = 0, _Value2 = 0;

                        if (VALUE0 != -1)
                        {
                            _Value0 = List[i].BasicPropPA1Min + VALUE0;
                        }

                        if (VALUE1 != -1)
                        {
                            _Value1 = List[i].BasicPropPA2Min + VALUE1;
                        }
                        if (VALUE2 != -1)
                        {
                            _Value2 = List[i].BasicPropPA3Min + VALUE2;
                        }

                        /// Nếu là dòng ngũ hành tương khắc hệ gì đó
                        if (List[i].BasicPropType == "damage_series_resist")
                        {
                            _Value2 = GetResValue(this._GoodDatas.Series);
                        }
                        /// Nếu là dòng tỷ lệ bỏ qua kháng
                        else if (List[i].BasicPropType == "ignoreresist_p")
                        {
                            _Value2 = this._GoodDatas.Series;
                        }

                        if (Atribute.Init(List[i].BasicPropType, _Value0, _Value1, _Value2))
                        {
                            TotalBaseAttrib.Add(Atribute);
                        }
                        else
                        {
                            throw new System.ArgumentException("Symbol not found : " + List[i].BasicPropType);
                        }
                    }

                    this.TotalBaseAttrib = TotalBaseAttrib;
                }

                //TODO CÁI NÀY CẦN TÍNH MIN MAX
                if (_ItemBuild.GreenPropCount > 0)
                {
                    List<KMagicAttrib> TotalGreenProb = new List<KMagicAttrib>();

                    List<PropMagic> List = _ItemData.GreenProp.OrderBy(x => x.Index).ToList();

                    for (int i = 0; i < _ItemBuild.GreenPropCount; i++)
                    {
                        int PostionGet = (i * 3);

                        KMagicAttrib Atribute = new KMagicAttrib();

                        int VALUE0 = _ItemBuild.GreenPropValue[PostionGet];
                        int VALUE1 = _ItemBuild.GreenPropValue[PostionGet + 1];
                        int VALUE2 = _ItemBuild.GreenPropValue[PostionGet + 2];

                        // tính toán ra percent max min

                        MagicAttribLevel FindOrginalValue = ItemManager.TotalMagicAttribLevel.Where(x => x.MagicName == List[i].MagicName && x.Level == List[i].MagicLevel).FirstOrDefault();

                        int _Value0 = 0, _Value1 = 0, _Value2 = 0;

                        int LevelMagic = this._GoodDatas.Forge_level + List[i].MagicLevel;

                        MagicAttribLevel FindMagic = ItemManager.TotalMagicAttribLevel.Where(x => x.MagicName == List[i].MagicName && x.Level == LevelMagic).FirstOrDefault();

                        if (VALUE0 != -1)
                        {
                            if (FindMagic != null)
                            {
                                int Percent0 = RecaculationPercent(FindOrginalValue.MA1Min, FindOrginalValue.MA1Max, VALUE0);

                                if (Percent0 > 0)
                                {
                                    int AddValue = ((FindMagic.MA1Max - FindMagic.MA1Min) * Percent0) / 100;

                                    _Value0 = FindMagic.MA1Min + AddValue;
                                }
                                else
                                {
                                    _Value0 = FindMagic.MA1Min;
                                }
                            }
                        }

                        if (VALUE1 != -1)
                        {
                            int Percent1 = RecaculationPercent(FindOrginalValue.MA2Min, FindOrginalValue.MA2Max, VALUE1);
                            if (Percent1 > 0)
                            {
                                int AddValue = ((FindMagic.MA2Max - FindMagic.MA2Min) * Percent1) / 100;

                                _Value1 = FindMagic.MA2Min + AddValue;
                            }
                            else
                            {
                                _Value1 = FindMagic.MA2Min;
                            }
                        }
                        if (VALUE2 != -1)
                        {
                            int Percent2 = RecaculationPercent(FindOrginalValue.MA3Min, FindOrginalValue.MA3Max, VALUE2);

                            if (Percent2 > 0)
                            {
                                int AddValue = ((FindMagic.MA3Max - FindMagic.MA3Min) * Percent2) / 100;

                                _Value2 = FindMagic.MA3Min + AddValue;
                            }
                            else
                            {
                                _Value2 = FindMagic.MA3Min;
                            }
                        }

                        /// Nếu là dòng ngũ hành tương khắc hệ gì đó
                        if (List[i].MagicName == "damage_series_resist")
                        {
                            _Value2 = GetResValue(this._GoodDatas.Series);
                        }
                        /// Nếu là dòng tỷ lệ bỏ qua kháng
                        else if (List[i].MagicName == "ignoreresist_p")
                        {
                            _Value2 = this._GoodDatas.Series;
                        }

                        if (Atribute.Init(List[i].MagicName, _Value0, _Value1, _Value2))
                        {
                            TotalGreenProb.Add(Atribute);
                        }
                        else
                        {
                            throw new System.ArgumentException("Symboy not found : " + List[i].MagicName);
                        }
                    }

                    this.TotalGreenProp = TotalGreenProb;
                }
            }
        }

        /// <summary>
        /// Khởi tạo thuộc tính Base của Item
        /// Bao gồm Base Atrib
        /// </summary>
        public void InitBaseAttribParseHorse()
        {
            try
            {
                byte[] Base64Decode = Convert.FromBase64String(_GoodDatas.Props);
                ItemGenByteData _ItemBuild = DataHelper.BytesToObject<ItemGenByteData>(Base64Decode, 0, Base64Decode.Length);
                if (_ItemBuild != null)
                {
                    if (_ItemBuild.BasicPropCount > 0)
                    {
                        List<KMagicAttrib> TotalBaseAttrib = new List<KMagicAttrib>();

                        List<BasicProp> List = _ItemData.ListBasicProp.OrderBy(x => x.Index).ToList();

                        for (int i = 0; i < _ItemBuild.BasicPropCount; i++)
                        {
                            int PostionGet = (i * 3);

                            KMagicAttrib Atribute = new KMagicAttrib();

                            int VALUE0 = _ItemBuild.BasicPropValue[PostionGet];
                            int VALUE1 = _ItemBuild.BasicPropValue[PostionGet + 1];
                            int VALUE2 = _ItemBuild.BasicPropValue[PostionGet + 2];

                            int _Value0 = 0, _Value1 = 0, _Value2 = 0;

                            if (VALUE0 != -1)
                            {
                                _Value0 = List[i].BasicPropPA1Min + VALUE0;
                            }

                            if (VALUE1 != -1)
                            {
                                _Value1 = List[i].BasicPropPA2Min + VALUE1;
                            }
                            if (VALUE2 != -1)
                            {
                                _Value2 = List[i].BasicPropPA3Min + VALUE2;
                            }

                            /// Nếu là dòng ngũ hành tương khắc hệ gì đó
                            if (List[i].BasicPropType == "damage_series_resist")
                            {
                                _Value2 = GetResValue(this._GoodDatas.Series);
                            }
                            /// Nếu là dòng tỷ lệ bỏ qua kháng
                            else if (List[i].BasicPropType == "ignoreresist_p")
                            {
                                _Value2 = this._GoodDatas.Series;
                            }

                            if (Atribute.Init(List[i].BasicPropType, _Value0, _Value1, _Value2))
                            {
                                TotalBaseAttrib.Add(Atribute);
                            }
                            else
                            {
                                throw new System.ArgumentException("Symbol not found : " + List[i].BasicPropType);
                            }
                        }

                        this.TotalBaseAttrib = TotalBaseAttrib;
                    }
                }
            } catch { }
        }

        private static int GetResValue(int series)
        {
            int resValue = 0;

            switch (series)
            {
                case 1:
                    resValue = 4;
                    break;

                case 2:
                    resValue = 3;
                    break;

                case 3:
                    resValue = 1;
                    break;

                case 4:
                    resValue = 0;
                    break;

                case 5:
                    resValue = 2;
                    break;
            }

            return resValue;
        }

        public void InitRequestItem()
        {
            List<KREQUIRE_ATTR> TotalRequest = new List<KREQUIRE_ATTR>();

            if (_ItemData.ListReqProp.Count > 0)
            {
                foreach (ReqProp req in _ItemData.ListReqProp)
                {
                    KREQUIRE_ATTR _Att = new KREQUIRE_ATTR();
                    _Att.eRequire = (KE_ITEM_REQUIREMENT)req.ReqPropType;
                    _Att.nValue = req.ReqPropValue;
                    TotalRequest.Add(_Att);
                }
                this.TotalRequest = TotalRequest;
            }
        }

        /// <summary>
        /// Khởi tạo thuộc tính Base của Item
        /// Bao gồm Base Atrib + Green Atrib
        /// </summary>
        public void InitBaseAttribSignetParse()
        {
            byte[] Base64Decode = Convert.FromBase64String(_GoodDatas.Props);

            ItemGenByteData _ItemBuild = DataHelper.BytesToObject<ItemGenByteData>(Base64Decode, 0, Base64Decode.Length);

            if (_ItemBuild != null)
            {
                if (_ItemBuild.BasicPropCount > 0)
                {
                    List<KMagicAttrib> TotalBaseAttrib = new List<KMagicAttrib>();

                    List<BasicProp> List = _ItemData.ListBasicProp.OrderBy(x => x.Index).ToList();

                    for (int i = 0; i < _ItemBuild.BasicPropCount; i++)
                    {
                        int PostionGet = (i * 3);

                        KMagicAttrib Atribute = new KMagicAttrib();

                        int VALUE0 = _ItemBuild.BasicPropValue[PostionGet];
                        int VALUE1 = _ItemBuild.BasicPropValue[PostionGet + 1];
                        int VALUE2 = _ItemBuild.BasicPropValue[PostionGet + 2];

                        int _Value0 = 0, _Value1 = 0, _Value2 = 0;

                        if (VALUE0 != -1)
                        {
                            _Value0 = List[i].BasicPropPA1Min + VALUE0;
                        }

                        if (VALUE1 != -1)
                        {
                            _Value1 = List[i].BasicPropPA2Min + VALUE1;
                        }
                        if (VALUE2 != -1)
                        {
                            _Value2 = List[i].BasicPropPA3Min + VALUE2;
                        }

                        /// Nếu là dòng ngũ hành tương khắc hệ gì đó
                        if (List[i].BasicPropType == "damage_series_resist")
                        {
                            _Value2 = GetResValue(this._GoodDatas.Series);
                        }
                        /// Nếu là dòng tỷ lệ bỏ qua kháng
                        else if (List[i].BasicPropType == "ignoreresist_p")
                        {
                            _Value2 = this._GoodDatas.Series;
                        }

                        if (List[i].BasicPropType == "seriesenhance")
                        {
                            if (_GoodDatas.OtherParams.TryGetValue(ItemPramenter.Pram_1, out string enhance))
                            {
                                string[] enhancePram = enhance.Split('|');

                                int nLevel = Int32.Parse(enhancePram[0]);

                                if (nLevel > 1)
                                {
                                    VALUE0 = VALUE0 + nLevel - 1;
                                }
                            }
                        }

                        if (List[i].BasicPropType == "seriesabate")
                        {
                            if (_GoodDatas.OtherParams.TryGetValue(ItemPramenter.Pram_2, out string enhance))
                            {
                                string[] enhancePram = enhance.Split('|');

                                int nLevel = Int32.Parse(enhancePram[0]);

                                if (nLevel > 1)
                                {
                                    VALUE0 = VALUE0 + nLevel - 1;
                                }
                            }
                        }

                        if (Atribute.Init(List[i].BasicPropType, _Value0, _Value1, _Value2))
                        {
                            TotalBaseAttrib.Add(Atribute);
                        }
                        else
                        {
                            throw new System.ArgumentException("Symboy not found : " + List[i].BasicPropType);
                        }
                    }

                    this.TotalBaseAttrib = TotalBaseAttrib;
                }
            }
        }

        public void AbradeInDeath(int nPercent, KPlayer client)
        {
            // nếu độ bên fhiện tịa đã nhỏ hơn không thì bỏ qua
            if (this._GoodDatas.Strong <= 0)
                return;

            int nSub = this._GoodDatas.Strong * nPercent / 100;

            if (nSub > 0)
            {
                nSub = 1;

                this._GoodDatas.Strong = this._GoodDatas.Strong - 1;

                Dictionary<UPDATEITEM, object> TotalUpdate = new Dictionary<UPDATEITEM, object>();

                TotalUpdate.Add(UPDATEITEM.ROLEID, client.RoleID);
                TotalUpdate.Add(UPDATEITEM.ITEMDBID, this._GoodDatas.Id);
                TotalUpdate.Add(UPDATEITEM.STRONG, this._GoodDatas.Strong);

                ItemManager.UpdateItemPrammenter(TotalUpdate, this._GoodDatas, client, "", false);

                PlayerManager.ShowNotification(client, "Độ bền của [" + ItemManager.GetNameItem(this._GoodDatas) + "] giảm đi 1");

                // Nếu độ bền mà khác không thì reload lại các chỉ số
                if (this._GoodDatas.Strong <= 0)
                {
                    client.GetPlayEquipBody().ClearAllEffecEquipBody();

                    client.GetPlayEquipBody().AttackAllEquipBody();
                }
            }
        }

        public void Abrade(int Value, KPlayer client)
        {
            // nếu độ bên fhiện tịa đã nhỏ hơn không thì bỏ qua
            if (this._GoodDatas.Strong <= 0)
                return;

            this._GoodDatas.Strong = this._GoodDatas.Strong - Value;

            Dictionary<UPDATEITEM, object> TotalUpdate = new Dictionary<UPDATEITEM, object>();

            TotalUpdate.Add(UPDATEITEM.ROLEID, client.RoleID);
            TotalUpdate.Add(UPDATEITEM.ITEMDBID, this._GoodDatas.Id);
            TotalUpdate.Add(UPDATEITEM.STRONG, this._GoodDatas.Strong);

            ItemManager.UpdateItemPrammenter(TotalUpdate, this._GoodDatas, client, "", false);

            PlayerManager.ShowNotification(client, "Độ bền của [" + ItemManager.GetNameItem(this._GoodDatas) + "] giảm đi 1");

            // Nếu độ bền mà khác không thì reload lại các chỉ số
            if (this._GoodDatas.Strong <= 0)
            {
                client.GetPlayEquipBody().ClearAllEffecEquipBody();

                client.GetPlayEquipBody().AttackAllEquipBody();
            }
        }

        public KItem(GoodsData goodsData)
        {
            // SetItemData
            _ItemData = ItemManager.GetItemTemplate(goodsData.GoodsID);

            this.ISEQUIP = ItemManager.KD_ISEQUIP(_ItemData.Genre);
            this._GoodDatas = goodsData;
            this.TypeItem = ItemManager.GetItemType(_ItemData.DetailType);
            if (ISEQUIP)
            {
                this.active_all_ornament = false;
                this.active_suit = false;

                switch (TypeItem)
                {
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_meleeweapon:
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_rangeweapon:
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_armor:
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_ring:
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_necklace:
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_amulet:
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_boots:
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_belt:
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_helm:
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_cuff:
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_mantle:
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_chop:
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_pendant:
                        {
                            InitBaseAttribParse();
                            InitRequestItem();
                            InitTotalEnhance();
                            InitHiddenProbs();
                            break;
                        }
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_book:
                        {
                            InitRequestItem();
                            InitBookProperty();

                            break;
                        }
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_horse:
                        {
                            //-----------fix jackson thêm thuộc tính ListBasicProp cho ngựa
                            InitBaseAttribParseHorse();
                            InitRequestItem();
                            InitHorseProperty();

                            break;
                        }
                    case KE_ITEM_EQUIP_DETAILTYPE.equip_signet:
                        {
                            InitRequestItem();
                            InitBaseAttribSignetParse();

                            break;
                        }
                }
            }
            else // NẾU Không phải vật phẩm để mặc thì GS ko quan tâm. Client chỉ cần lấy để hiển thị ra các dòng DESC. Các vật phẩm kích hoạt sẽ sử dụng lua để kích hoạt script
            {
            }
        }
    }
}