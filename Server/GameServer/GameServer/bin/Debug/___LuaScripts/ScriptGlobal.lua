--[[
	Script này được tải mặc định cùng hệ thống.
	Toàn bộ các biến, hàm bên dưới đều là Global và có thể được dùng ở mọi Script khác.
]]

-- Danh sách đối tượng trong hệ thống
Global_ObjectTypes = {
	-- Người chơi
	OT_CLIENT = 0,
	-- Quái
	OT_MONSTER = 1,
	-- NPC
	OT_NPC = 5,
	-- Điểm thu thập
	OT_GROWPOINT = 9,
	-- Khu vực động
	OT_DYNAMIC_AREA = 10,
}

-- Dữ liệu phụ bản
Global_Params = {
	-- Bạch Hổ Đường
	BaiHuTang_BeginRegistered = false,	-- Đang mở báo danh
	BaiHuTang_Stage = 0,					-- Đang ở tầng nào
	-- End
}

-- Trạng thái PK
Global_PKMode = {
	-- Hòa bình
	Peace = 0,
	-- Phe
	Team = 1,
	-- Bang hội
	Guild = 2,
	-- Đồ sát
	All = 3,
	-- Thiện ác
	Moral = 4,
	-- Tùy chọn tùy theo thiết lập sự kiện
	Custom = 5,
}

-- ID hoạt động
Global_Events = {
	-- Bạch Hổ Đường
	BaiHuTang = 10,
}

-- Danh sách ID môn phái
Global_FactionID = {
	-- Không có
	None = 0,
	-- Thiếu Lâm
	ShaoLin = 1,
	-- Thiên Vương
	TianWang = 2,
	-- Đường Môn
	TangMen = 3,
	-- Ngũ Độc
	WuDu = 4,
	-- Nga My
	EMei = 5,
	-- Thúy Yên
	CuiYan = 6,
	-- Cái Bang
	GaiBang = 7,
	-- Thiên Nhẫn
	TianRen = 8,
	-- Võ Đang
	WuDang = 9,
	-- Côn Lôn
	KunLun = 10,
	-- Minh Giáo
	MingJiao = 11,
	-- Đoàn Thị
	DaLiDuanShi = 12,
}
Global_TypeMoney = {
	BacKhoa = 0,
	Bac=1,
	Dong=2,
	DongKhoa=3,
}
Global_TypeMoneyName = {
	[Global_TypeMoney.BacKhoa]=" Bạc Khóa",
	[Global_TypeMoney.Bac]=" Bạc",
	[Global_TypeMoney.Dong]=" Đồng",
	[Global_TypeMoney.DongKhoa]=" Đồng Khóa",
}

-- Chức vị trong bang
Global_GuildRank = {
	-- Bang chúng
	Member = 0,
	-- Bang chủ
	Master = 1,
	-- Phó bang chủ
	ViceMaster = 2,
	-- Trưởng lão
	Ambassador = 3,
	-- Tinh anh
	Elite = 4,
}

-- Chức vị trong tộc
Global_FamilyRank = {
	-- Thành Viên
	Member = 0,
	-- Tộc Trưởng
	Master = 1,
	-- Tộc Phó
	ViceMaster = 2,
}

Global_CAMP = {
	[101]={NameActivity="Lệnh Bài Nghĩa Quân"},
	[102]={NameActivity="Lệnh Bài Quân doanh"},
	[103]={NameActivity="Lệnh Bài Học Tạo Đồ"},
	[201]={NameActivity="Lệnh Bài Dương Châu"},
	[202]={NameActivity="Lệnh Bài Phượng Tường"},
	[203]={NameActivity="Lệnh Bài Tương Dương"},
	[301]={NameActivity="Lệnh Bài Thiếu Lâm"},
	[302]={NameActivity="Lệnh Bài Thiên Vương"},
	[303]={NameActivity="Lệnh Bài Đường Môn"},
	[304]={NameActivity="Lệnh Bài Ngũ Độc"},
	[305]={NameActivity="Lệnh Bài Nga My"},
	[306]={NameActivity="Lệnh Bài Thúy Yên"},
	[307]={NameActivity="Lệnh Bài Cái Bang"},
	[308]={NameActivity="Lệnh Bài Thiên Nhẫn"},
	[309]={NameActivity="Lệnh Bài Võ Đang"},
	[310]={NameActivity="Lệnh Bài Côn Lôn"},
	[311]={NameActivity="Lệnh Bài Minh Giáo"},
	[312]={NameActivity="Lệnh Bài Đại Lý Đoàn Thị"},
	[401]={NameActivity="Lệnh Bài Ải gia tộc"},
	[501]={NameActivity="Lệnh Bài Bạch Hổ-"},
	[502]={NameActivity="Lệnh Bài Thịnh Hạ 2008"},
	[503]={NameActivity="Lệnh Bài Tiêu Dao Cốc"},
	[504]={NameActivity="Lệnh Bài Chúc phúc"},
	[505]={NameActivity="Lệnh Bài Hoạt động Thịnh Hạ 2010"},
	[506]={NameActivity="Lệnh Bài Di tích Hàn Vũ"},
	[507]={NameActivity="Lệnh Bài Mỹ nhân"},
	[508]={NameActivity="Lệnh Bài VIP"},
	[601]={NameActivity="Lệnh Bài Cao thủ (kim)"},
	[602]={NameActivity="Lệnh Bài Cao thủ (mộc)"},
	[603]={NameActivity="Lệnh Bài Cao thủ (thủy)"},
	[604]={NameActivity="Lệnh Bài Cao thủ (hỏa)"},
	[605]={NameActivity="Lệnh Bài Cao thủ (thổ)"},
	[701]={NameActivity="Lệnh Bài Võ Lâm Liên Đấu"},
	[801]={NameActivity="Lệnh Bài Tranh Đoạt Lãnh Thổ"},
	[901]={NameActivity="Lệnh Bài Tần Lăng-Quan Phủ"},
	[902]={NameActivity="Lệnh Bài Tần Lăng-Phát Khâu Môn"},
	[1001]={NameActivity="Lệnh Bài Đoàn viên dân tộc"},
	[1101]={NameActivity="Lệnh Bài Đại Hội Võ Lâm"},
	[1201]={NameActivity="Lệnh Bài Liên đấu liên server"},
}

-- Danh sách môn phái
Global_FactionName = {
	[Global_FactionID.None] = "Không có",
	[Global_FactionID.ShaoLin] = "Thiếu Lâm",
	[Global_FactionID.TianWang] = "Thiên Vương",
	[Global_FactionID.TangMen] = "Đường Môn",
	[Global_FactionID.WuDu] = "Ngũ Độc",
	[Global_FactionID.EMei] = "Nga My",
	[Global_FactionID.CuiYan] = "Thúy Yên",
	[Global_FactionID.GaiBang] = "Cái Bang",
	[Global_FactionID.TianRen] = "Thiên Nhẫn",
	[Global_FactionID.WuDang] = "Võ Đang",
	[Global_FactionID.KunLun] = "Côn Lôn",
	[Global_FactionID.MingJiao] = "Minh Giáo",
	[Global_FactionID.DaLiDuanShi] = "Đoàn Thị",
}




-- danh sách các phái Truyền thống phù Item--
--secret1 = mật tịch 1 - 2;
--press = ngũ hành ấn 1-2;
Global_NameMapItemPhaiID={
	[9] = {
		ID = 9, Name = "Thiếu Lâm Phái", PosX = 4695, PosY = 2929 ,FactionID = 1, secret1 = 3232, secret2= 3233 ,press =3864
	},
	[22] = {
		ID = 22, Name = "Thiên Vương Bang", PosX = 4114, PosY = 3372 ,FactionID  =2 ,secret1 = 3234, secret2= 3235,press =3865
	},
	[18] = {
		ID = 18, Name = "Đường Môn ", PosX = 3725, PosY = 3215 ,FactionID  =3,secret1 = 3236, secret2= 3237,press =3866
	},
	[20] = {
		ID = 20, Name = "Ngũ Độc Giáo", PosX = 4891, PosY = 1518 ,FactionID = 4, secret1 = 3238, secret2= 3239,press =3867
	},
	[16] = {
		ID = 16, Name = "Nga My Phái", PosX = 6476, PosY = 5090 ,FactionID =5, secret1 = 3240, secret2= 3241,press =3868
	},
	[17] = {
		ID = 17, Name = "Thúy Yên Môn", PosX = 6849, PosY = 3108 ,FactionID =6,  secret1 = 3242, secret2= 3243,press =3869
	},
	[15] = {
		ID = 15, Name = "Cái Bang Phái", PosX = 2557, PosY = 2166 ,FactionID =7,secret1 = 3244, secret2= 3245,press =3870
	},
	[10] = {
		ID = 10, Name = "Thiên Nhẫn Giáo", PosX = 3643, PosY = 3911 ,FactionID =8,secret1 = 3246, secret2= 3247,press =3871
	},
	[14] = {
		ID = 14, Name = "Võ Đang Phái", PosX = 5121, PosY = 3057 ,FactionID =9, secret1 = 3248, secret2= 3249,press =3872
	},
	[12] = {
		ID = 12, Name = "Côn Lôn Phái", PosX = 3648, PosY = 2126 ,FactionID =10, secret1 = 3250, secret2= 3251,press =3873
	},
	[224] = {
		ID = 224, Name = "Minh Giáo ", PosX = 4901, PosY = 2699 ,FactionID =11, secret1 = 3252, secret2= 3253,press =3874
	},
	[19] = {
		ID = 19, Name = "Đoàn Thị Phái", PosX = 3464, PosY = 3218 ,FactionID =12, secret1 = 3254, secret2= 3255,press =3875
	},
}
--danh sách thành  Truyền thống phù Item--
Global_NameMapItemThanhID ={ 

	[23] = {
		ID = 23, Name = "Biện Kinh Phủ", PosX = 6754, PosY = 3590
	},
	[26] = {
		ID = 26, Name = "Dương Châu Phủ", PosX =8904, PosY = 4377
	},
	
	[29] = {
		ID = 29, Name = "Lâm An Phủ", PosX = 7549, PosY = 4455
	},
	[25] = {
		ID = 25, Name = "Tương Dương Phủ", PosX = 7860, PosY = 4218
	},
	[27] = {
		ID = 27, Name = "Thành Đô Phủ", PosX = 7155, PosY = 3175
	},
	[24] = {
		ID = 24, Name = "Phượng Tường Phủ", PosX = 8054, PosY = 3581
	},
	[28] = {
		ID = 28, Name = "Đại Lý Phủ", PosX =2929, PosY = 2087
	},
	
}
-- danh sách các thôn Truyền thống phù Item --
Global_NameMapItemThonID={
	[5] ={ 
		ID =5, Name = "Giang Tân Thôn" , PosX =5237, PosY = 2910 
	},
	[3] = {
		ID = 3, Name = "Vĩnh Lạc Trấn ", PosX =4297, PosY = 3689
	},                                                                      
	[8] = {
		ID = 8, Name = "Ba Lăng Huyện ", PosX =5577, PosY = 3740
	},
	[2] = {
		ID = 2, Name = "Long Môn Trấn", PosX =6255, PosY = 2340
	},
	[6] = {
		ID = 6, Name = "Thạch Cổ Trấn ", PosX =7943, PosY = 3577
	},
	[4] = {
		ID = 4, Name = "Đạo Hương Thôn", PosX =5204, PosY = 3174
	},
	[1] = {
		ID = 1, Name = "Vân Trung Trấn", PosX =3199, PosY = 3595
	},
	[7] = {
		ID = 7, Name = "Long Tuyền Thôn ", PosX =4710, PosY = 4288
	},
}
-- danh sách các Phai Xa phu--
Global_NameMapPhaiID={
	[9] = {
		ID = 9, Name = "Thiếu Lâm Phái", PosX = 4695, PosY = 2929  
	},
	[22] = {
		ID = 22, Name = "Thiên Vương Bang", PosX = 4114, PosY = 3372
	},
	[18] = {
		ID = 18, Name = "Đường Môn ", PosX = 3725, PosY = 3215 
	},
	[20] = {
		ID = 20, Name = "Ngũ Độc Giáo", PosX = 4891, PosY = 1518
	},
	[16] = {
		ID = 16, Name = "Nga My Phái", PosX = 6476, PosY = 5090
	},
	[17] = {
		ID = 17, Name = "Thúy Yên Môn", PosX = 6849, PosY = 3108
	},
	[15] = {
		ID = 15, Name = "Cái Bang Phái", PosX = 2557, PosY = 2166
	},
	[10] = {
		ID = 10, Name = "Thiên Nhẫn Giáo", PosX = 3643, PosY = 3911
	},
	[14] = {
		ID = 14, Name = "Võ Đang Phái", PosX = 5121, PosY = 3057
	},
	[12] = {
		ID = 12, Name = "Côn Lôn Phái", PosX = 3648, PosY = 2126
	},
	[224] = {
		ID = 224, Name = "Minh Giáo ", PosX = 4901, PosY = 2699
	},
	[19] = {
		ID = 19, Name = "Đoàn Thị Phái", PosX = 3464, PosY = 3218
	},
	
}
--danh sách thành Các xa phu--
Global_NameMapThanhID ={

	[23] = {
		ID = 23, Name = "Biện Kinh Phủ", PosX = 4041, PosY = 2443
	},
	[26] = {
		ID = 26, Name = "Dương Châu Phủ", PosX =8904, PosY = 4377
	},
	[29] = {
		ID = 29, Name = "Lâm An Phủ", PosX = 7549, PosY = 4455
	},
	[25] = {
		ID = 25, Name = "Tương Dương Phủ", PosX = 10975, PosY = 2871 
	},
	[27] = {
		ID = 27, Name = "Thành Đô Phủ", PosX = 7155, PosY = 3175
	},
	[24] = {
		ID = 24, Name = "Phượng Tường Phủ", PosX = 8054, PosY = 3581
	},
	[28] = {
		ID = 28, Name = "Đại Lý Phủ", PosX =2929, PosY = 2087
	},
	
	
}
--danh sách thành Các Điểm Luyện Công--
Global_NameMapLuyenCongCap5ID ={

	[30] = {
		ID = 30, Name = "Chiến Trường Cổ", PosX = 2771, PosY = 1847
	},
	[31] = {
		ID = 31, Name = "Quán Trọ Long Môn", PosX = 8761, PosY = 6516 --ok
	},	
	[40] = {
		ID = 40, Name = "Đồng Quan", PosX = 3710, PosY = 690 --ok
	},
	[33] = {
		ID = 33, Name = "Mai Hoa Lĩnh", PosX = 7523, PosY = 5583  --ok
	},
	[34] = {
		ID = 34, Name = "Trường Giang Hà Cốc", PosX = 1389, PosY = 6523 --ok
	},
	[35] = {
		ID = 35, Name = "Thị trấn Bạch Tộc", PosX = 2218, PosY = 689 --ok
	},
	[36] = {
		ID = 36, Name = "Nhạn Đãng Long Thu", PosX =2929, PosY = 2087
	},
	[37] = {
		ID = 37, Name = "Bờ Hồ Động Đình", PosX =3704, PosY = 8070 --ok
	},
}
Global_NameMapLuyenCongCap15ID ={

	[38] = {
		ID = 38, Name = "Trấn Đông Mộ Viên", PosX = 970, PosY = 1023
	},
	[39] = {
		ID = 39, Name = "Kỳ Liên Sơn", PosX = 9213, PosY = 5313  --ok
	},	

	[32] = {
		ID = 32, Name = "Quân Mã Trường", PosX = 7658, PosY = 773 --ok
	},
	[41] = {
		ID = 41, Name = "Hoài Thủy Sa Châu", PosX = 3997, PosY = 706 --ok
	},
	[42] = {
		ID = 42, Name = "Thục Nam Trúc Hải", PosX = 8956, PosY = 5534 --ok
	},
	[43] = {
		ID = 43, Name = "Trà Mã Cổ Đạo", PosX = 4145, PosY = 749  --ok
	},
	[44] = {
		ID = 44, Name = "Phường Đúc Kiếm", PosX =2929, PosY = 2087
	},
	[45] = {
		ID = 45, Name = "Nhạc Dương Lâu", PosX =1631, PosY = 1205 --ok
	},
}
Global_NameMapLuyenCongCap25ID ={

	[46] = {
		ID = 46, Name = "Cấm Địa Hậu Sơn", PosX = 7554, PosY = 3621  --ok
	},
	[47] = {
		ID = 47, Name = "Hoàng Lăng Kim Quốc 1", PosX =3889, PosY = 2295  --ok
	},
	[48] = {
		ID = 48, Name = "Kiến Tính Phong", PosX = 8156, PosY = 745 --ok
	},
	[49] = {
		ID = 49, Name = "Thiên Trụ Phong", PosX = 3114, PosY = 693 --ok
	},
	[50] = {
		ID = 50, Name = "Cô Tô Thủy Tạ", PosX = 4328, PosY = 4934  --ok
	},
	[51] = {
		ID = 51, Name = "Cửu Lão Động 1", PosX = 3033, PosY = 1188  --ok
	},
	[52] = {
		ID = 52, Name = "Bách Hoa Cốc", PosX =7074, PosY = 1061   --ok
	},
	[53] = {
		ID = 53, Name = "Hồ Phỉ Thúy", PosX =8827, PosY = 5483  --ok
	},
	[54] = {
		ID = 54, Name = "Bộ Lạc Nam Di", PosX =7470, PosY = 5773 --ok
	},
	[55] = {
		ID = 55, Name = "Thanh Loa Đảo", PosX =7190, PosY = 5948  --ok
	},
}
Global_NameMapLuyenCongCap35ID ={

	[56] = {
		ID = 56, Name = "Đông Tháp Lâm", PosX = 7464, PosY = 3644 --ok
	},
	[57] = {
		ID = 57, Name = "Hoàng Lăng Kim Quốc 2", PosX =3910, PosY = 2284  --ok
	},
	[58] = {
		ID = 58, Name = "Mê Cung Băng Huyệt 1", PosX = 8006, PosY = 789 --ok
	},
	[59] = {
		ID = 59, Name = "Đông Long Hổ Huyễn Cảnh", PosX = 7159, PosY = 672 --ok
	},
	[60] = {
		ID = 60, Name = "Ngoài Yến Tử Ổ", PosX = 4349, PosY = 4968  --ok
	},
	[61] = {
		ID = 61, Name = "Cửu Lão Động 2", PosX = 4730, PosY = 3300  --ok
	},
	[62] = {
		ID = 62, Name = "Ngoài Bách Hoa Trận", PosX =7068, PosY = 1103  --ok
	},
	[63] = {
		ID = 63, Name = "Đông Bờ Hồ Trúc Lâm", PosX =8895, PosY = 5494  --ok
	},
	[64] = {
		ID = 64, Name = "Đông Rừng Nguyên Sinh", PosX =7413, PosY = 5707  --ok
	},
	[65] = {
		ID = 65, Name = "Đông Nam Lư Vĩ Đãng", PosX =7142, PosY = 5924  --ok
	},	
}
Global_NameMapLuyenCongCap45ID ={

	[66] = {
		ID = 66, Name = "Tây Tháp Lâm", PosX = 7529, PosY = 3614  --ok
	},
	[67] = {
		ID = 67, Name = "Hoàng Lăng Kim Quốc 3", PosX = 3859, PosY = 2284  --ok
	},
	[68] = {
		ID = 68, Name = "Mê Cung Băng Huyệt 2", PosX = 8003, PosY = 806  --ok
	},
	[69] = {
		ID = 69, Name = "Tây Long Hổ Huyễn Cảnh", PosX = 7103, PosY = 695 --ok
	},
	[70] = {
		ID = 70, Name = "Giữa Yến Tử Ổ", PosX = 5240, PosY = 3100
	},
	[71] = {
		ID = 71, Name = "Cửu Lão Động 3", PosX = 4730, PosY = 3300
	},
	[72] = {
		ID = 72, Name = "Trong Bách Hoa Trận", PosX = 7095, PosY = 1134  --ok
	},
	[73] = {
		ID = 73, Name = "Tây Bờ Hồ Trúc Lâm", PosX = 8924, PosY = 5476  --ok
	},
	[74] = {
		ID = 74, Name = "Tây Rừng Nguyên Sinh", PosX = 7429, PosY = 5732 --ok
	},
	[75] = {
		ID = 75, Name = "Tây Bắc Lư Vĩ Đãng", PosX = 7174, PosY = 5932  --ok
	},	
}
Global_NameMapLuyenCongCap55ID ={

	[86] = {
		ID = 86, Name = "Thái Hành Cổ Kính", PosX = 7536, PosY = 5157  --ok
	},
	[87] = {
		ID = 87, Name = "Đại Tán Quan", PosX = 1712, PosY = 4675  --ok
	},
	[88] = {
		ID = 88, Name = "Hán Thủy Cổ Độ", PosX = 3034, PosY = 7617  --ok
	},
	[89] = {
		ID = 89, Name = "Hàn Sơn Cổ Sát", PosX = 1062, PosY = 6946 --ok
	},
	[90] = {
		ID = 90, Name = "Cán Hoa Khê", PosX = 6125, PosY = 840  --ok
	},
	[91] = {
		ID = 91, Name = "Nhĩ Hải Ma Nham", PosX = 6622, PosY = 1183  --ok
	},
	[92] = {
		ID = 92, Name = "Thái Thạch Cơ", PosX = 7115, PosY = 5959  --ok
	},
}
Global_NameMapLuyenCongCap65ID ={

	[94] = {
		ID = 94, Name = "Cư Diên Trạch", PosX = 4352, PosY = 784  --ok
	},
	[95] = {
		ID = 95, Name = "Phục Ngưu Sơn", PosX = 7658, PosY = 4200  --ok
	},	
	[96] = {
		ID = 96, Name = "Hổ Khâu Kiếm Trì", PosX = 5404, PosY = 988  --ok
	},
	[97] = {
		ID = 97, Name = "Hưởng Thủy Động", PosX = 915, PosY = 4859 --ok
	},
	[98] = {
		ID = 98, Name = "Điểm Thương Sơn", PosX = 6478, PosY = 528  --ok
	},
	[99] = {
		ID = 99, Name = "Bành Lãi Cổ Trạch", PosX = 8054, PosY = 3581
	},
	-- [28] = {
		-- ID = 28, Name = "Đại Lý Phủ", PosX =2929, PosY = 2087
	-- },
	
}
Global_NameMapLuyenCongCap75ID ={

	[100] = {
		ID = 100, Name = "Phong Lăng Độ", PosX = 1258, PosY = 4525  --ok
	},
	[101] = {
		ID = 101, Name = "Mê Cung Sa Mạc", PosX = 2463, PosY = 597 --ok
	},
	[102] = {
		ID = 102, Name = "Kê Quán Động", PosX = 6144, PosY = 1038  --ok
	},
	[103] = {
		ID = 103, Name = "Thục Cương Sơn", PosX = 3934, PosY = 591 --ok
	},
	[104] = {
		ID = 104, Name = "Kiếm Các Thục Đạo", PosX = 839, PosY = 3015  --ok
	},
	[105] = {
		ID = 105, Name = "Hoàng Lăng Đoàn Thị", PosX = 3932, PosY = 2322  --ok
	},
	[106] = {
		ID = 106, Name = "Cửu Nghi Khê", PosX = 11239, PosY = 7724  --ok
	},
	
	
}
Global_NameMapLuyenCongCap85ID ={

	[107] = {
		ID = 107, Name = "Long Môn Thạch Quật", PosX = 9491, PosY = 4887  --ok
	},
	[108] = {
		ID = 108, Name = "Hoàng Lăng Tây Hạ", PosX = 1205, PosY = 2203  --ok
	},
	[109] = {
		ID = 109, Name = "Hoàng Hạc Lâu", PosX = 6014, PosY = 6126  --ok
	},
	[110] = {
		ID = 110, Name = "Tiến Cúc Động", PosX = 3070, PosY = 1170 --ok
	},
	[111] = {
		ID = 111, Name = "Kiếm Môn Quan", PosX = 7742, PosY = 1122  --ok
	},
	[113] = {
		ID = 113, Name = "Bang Nguyên Bí Động", PosX = 8411, PosY = 6165  --ok
	},
	-- [113] = {
		-- ID = 113, Name = "Đại Lý Phủ", PosX =2929, PosY = 2087
	-- },	
}
Global_NameMapLuyenCongCap95ID ={

	[114] = {
		ID = 114, Name = "Sắc Lặc Xuyên", PosX = 10421, PosY = 1785
	},
	[115] = {
		ID = 115, Name = "Gia Dụ Quan", PosX =9789, PosY = 1146 --ok
	},	
	[116] = {
		ID = 116, Name = "Hoa Sơn", PosX = 7182, PosY = 1139 --ok
	},
	[117] = {
		ID = 117, Name = "Thục Cương Bí Cảnh", PosX = 7015, PosY = 874 --ok
	},
	[118] = {
		ID = 118, Name = "Phong Đô Quỷ Thành", PosX = 1172, PosY = 2306 --ok
	},
	[119] = {
		ID = 119, Name = "Miêu Lĩnh", PosX = 2730, PosY = 630 --ok
	},
	[120] = {
		ID = 120, Name = "Vũ Di Sơn", PosX =7496, PosY = 1753  --ok
	},
	[121] = {
		ID = 121, Name = "Vũ Lăng Sơn", PosX =7741, PosY = 1158 --ok
	},	
}
Global_NameMapLuyenCongCap105ID ={

	[122] = {
		ID = 122, Name = "Mạc Bắc Thảo Nguyên", PosX = 2857, PosY = 702 --ok
	},
	[124] = {
		ID = 124, Name = "Đôn Hoàng Cổ Thành", PosX =9190, PosY = 768  --ok
	},	
	[125] = {
		ID = 125, Name = "Đại Vũ Đài", PosX = 4999, PosY = 4539   --ok
	},
	[127] = {
		ID = 127, Name = "Tam Hiệp Sạn Đạo", PosX = 1157, PosY = 2966  --ok
	},
	[128] = {
		ID = 128, Name = "Tỏa Vân Uyên", PosX = 7587, PosY = 1747
	},
	-- [127] = {
		-- ID = 127, Name = "Phượng Tường Phủ", PosX = 8054, PosY = 3581
	-- },
	-- [128] = {
		-- ID = 128, Name = "Đại Lý Phủ", PosX =2929, PosY = 2087
	-- },	
}
Global_NameMapLuyenCongCap115ID ={

	[131] = {
		ID = 131, Name = "Tàn Tích Cung A Phòng", PosX = 12685, PosY = 2167
	},
	[133] = {
		ID = 133, Name = "Lương Sơn Bạc", PosX = 857, PosY = 2805  --ok
	},
	[134] = {
		ID = 134, Name = "Thần Nữ Phong", PosX = 7549, PosY = 4455
	},
	[136] = {
		ID = 136, Name = "Cổ Lãng Dữ", PosX = 10975, PosY = 2871 
	},
	[137] = {
		ID = 137, Name = "Đào Hoa Nguyên", PosX = 7155, PosY = 3175
	},
	-- [24] = {
		-- ID = 24, Name = "Phượng Tường Phủ", PosX = 8054, PosY = 3581
	-- },
	-- [28] = {
		-- ID = 28, Name = "Đại Lý Phủ", PosX =2929, PosY = 2087
	-- },	
}
-- danh sách các thôn xa phu--
Global_NameMapThonID={
	[5] ={ 
		ID =5, Name = "Giang Tân Thôn" , PosX =6141, PosY = 2227 
	},
	[3] = {
		ID = 3, Name = "Vĩnh Lạc Trấn ", PosX =9989, PosY = 4312
	},                                                                      
	[8] = {
		ID = 8, Name = "Ba Lăng Huyện ", PosX =2972, PosY = 5502
	},
	[2] = {
		ID = 2, Name = "Long Môn Trấn", PosX =3512, PosY = 1760
	},
	[6] = {
		ID = 6, Name = "Thạch Cổ Trấn ", PosX =5880, PosY = 861
	},
	[4] = {
		ID = 4, Name = "Đạo Hương Thôn", PosX =5468, PosY = 2105
	},
	[1] = {
		ID = 1, Name = "Vân Trung Trấn", PosX =8242, PosY = 675
	},
	[7] = {
		ID = 7, Name = "Long Tuyền Thôn ", PosX =7467, PosY = 2988
	},
}
