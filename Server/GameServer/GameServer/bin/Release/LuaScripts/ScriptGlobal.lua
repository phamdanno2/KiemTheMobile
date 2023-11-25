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
		ID = 18, Name = "Đường Môn ", PosX = 3910, PosY = 2906 ,FactionID  =3,secret1 = 3236, secret2= 3237,press =3866
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
		ID = 12, Name = "Côn Lôn Phái", PosX = 4457, PosY = 2066 ,FactionID =10, secret1 = 3250, secret2= 3251,press =3873
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
		ID = 26, Name = "Dương Châu Phủ", PosX =7200, PosY = 3530
	},
	
	[29] = {
		ID = 29, Name = "Lâm An Phủ", PosX = 8302, PosY = 4624
	},
	[25] = {
		ID = 25, Name = "Tương Dương Phủ", PosX = 7860, PosY = 4218
	},
	[27] = {
		ID = 27, Name = "Thành Đô Phủ", PosX = 5953, PosY = 3739
	},
	[24] = {
		ID = 24, Name = "Phượng Tường Phủ", PosX = 8653, PosY = 4026
	},
	[28] = {
		ID = 28, Name = "Đại Lý Phủ", PosX =5667, PosY = 3387
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
		ID = 2, Name = "Long Môn Trấn", PosX =6864, PosY = 3084
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
		ID = 18, Name = "Đường Môn ", PosX = 3910, PosY = 2906 
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
		ID = 12, Name = "Côn Lôn Phái", PosX = 3545, PosY = 2179
	},
	[224] = {
		ID = 224, Name = "Minh Giáo ", PosX = 6035, PosY = 3311
	},
	[19] = {
		ID = 19, Name = "Đoàn Thị Phái", PosX = 3464, PosY = 3218
	},
	
}
--danh sách thành Các xa phu--
Global_NameMapThanhID ={

	[23] = {
		ID = 23, Name = "Biện Kinh Phủ", PosX = 8783, PosY = 4417,
	},
	[26] = {
		ID = 26, Name = "Dương Châu Phủ", PosX =8783, PosY = 4417
	},
	
	[29] = {
		ID = 29, Name = "Lâm An Phủ", PosX = 11684, PosY = 2391
	},
	[25] = {
		ID = 25, Name = "Tương Dương Phủ", PosX = 5072, PosY = 7169 
	},
	[27] = {
		ID = 27, Name = "Thành Đô Phủ", PosX = 7175, PosY = 3142
	},
	[24] = {
		ID = 24, Name = "Phượng Tường Phủ", PosX = 13039, PosY = 6282
	},
	[28] = {
		ID = 28, Name = "Đại Lý Phủ", PosX =2278, PosY = 3832
	},
	
	
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
		ID = 2, Name = "Long Môn Trấn", PosX =4028, PosY = 2550
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
