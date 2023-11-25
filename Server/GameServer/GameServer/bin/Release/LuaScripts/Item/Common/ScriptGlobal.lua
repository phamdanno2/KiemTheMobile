--[[
	Script này được tải mặc định cùng hệ thống.
	Toàn bộ các biến, hàm bên dưới đều là Global và có thể được dùng ở mọi Script khác.
]]

-- Danh sách môn phái--
Global_FactionName = {
	[0] = "Không có",
	[1] = "Thiếu Lâm",
	[2] = "Thiên Vương",
	[3] = "Đường Môn",
	[4] = "Ngũ Độc",
	[5] = "Nga My",
	[6] = "Thúy Yên",
	[7] = "Cái Bang",
	[8] = "Thiên Nhẫn",
	[9] = "Võ Đang",
	[10] = "Côn Lôn",
	[11] = "Minh Giáo",
	[12] = "Đoàn Thị",
}

Global_FactionID ={
	[1]={ FactionID = 1 },
	[2]={ FactionID = 2 },
	[3]={ FactionID = 3 },
	[4]={ FactionID = 4 },
	[5]={ FactionID = 5 },
	[6]={ FactionID = 6 },
	[7]={ FactionID = 7 },
	[8]={ FactionID = 8 },
	[9]={ FactionID = 9 },
	[10]={ FactionID = 10 },
	[11]={ FactionID = 11 },
	[12]={ FactionID = 12 },
}
-- danh sách các phái Truyền thống phù Item--
Global_NameMapItemPhaiID={
	[9] = {
		ID = 9, Name = "Thiếu Lâm Phái", PosX = 4695, PosY = 2929 ,FactionID = 1
	},
	[22] = {
		ID = 22, Name = "Thiên Vương Bang", PosX = 4114, PosY = 3372 ,FactionID  =2
	},
	[18] = {
		ID = 18, Name = "Đường Môn ", PosX = 3910, PosY = 2906 ,FactionID  =3
	},
	[20] = {
		ID = 20, Name = "Ngũ Độc Giáo", PosX = 4891, PosY = 1518 ,FactionID = 4
	},
	[16] = {
		ID = 16, Name = "Nga My Phái", PosX = 6476, PosY = 5090 ,FactionID =5
	},
	[17] = {
		ID = 17, Name = "Thúy Yên Môn", PosX = 6849, PosY = 3108 ,FactionID =6
	},
	[15] = {
		ID = 15, Name = "Cái Bang Phái", PosX = 2557, PosY = 2166 ,FactionID =7
	},
	[10] = {
		ID = 10, Name = "Thiên Nhẫn Giáo", PosX = 3643, PosY = 3911 ,FactionID =8
	},
	[14] = {
		ID = 14, Name = "Võ Đang Phái", PosX = 5121, PosY = 3057 ,FactionID =9
	},
	[12] = {
		ID = 12, Name = "Côn Lôn Phái", PosX = 3545, PosY = 2179 ,FactionID =10
	},
	[224] = {
		ID = 224, Name = "Minh Giáo ", PosX = 6035, PosY = 3311 ,FactionID =11
	},
	[19] = {
		ID = 19, Name = "Đoàn Thị Phái", PosX = 3464, PosY = 3218 ,FactionID =12
	},
	

}
--danh sách thành  Truyền thống phù Item--
Global_NameMapItemThanhID ={ 

	[23] = {
		ID = 23, Name = "Biện Kinh Phủ", PosX = 6754, PosY = 3590
	},
	[26] = {
		ID = 26, Name = "Dương Châu Phủ", PosX =8914, PosY = 4374
	},
	
	[29] = {
		ID = 29, Name = "Lâm An Phủ", PosX = 7559, PosY = 4416
	},
	[25] = {
		ID = 25, Name = "Tương Dương Phủ", PosX = 7860, PosY = 4218
	},
	[27] = {
		ID = 27, Name = "Thành Đô Phủ", PosX = 7175, PosY = 3142
	},
	[24] = {
		ID = 24, Name = "Phượng Tường Phủ", PosX = 11345, PosY = 3572
	},
	[28] = {
		ID = 28, Name = "Đại Lý Phủ", PosX =2902, PosY = 2046
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
		ID = 23, Name = "Biện Kinh Phủ", PosX = 4138, PosY = 4820
	},
	[26] = {
		ID = 26, Name = "Dương Châu Phủ", PosX =4757, PosY = 4497
	},
	
	[29] = {
		ID = 29, Name = "Lâm An Phủ", PosX = 11684, PosY = 2391
	},
	[25] = {
		ID = 25, Name = "Tương Dương Phủ", PosX = 11158, PosY = 6175 
	},
	[27] = {
		ID = 27, Name = "Thành Đô Phủ", PosX = 4144, PosY = 2302
	},
	[24] = {
		ID = 24, Name = "Phượng Tường Phủ", PosX = 8754, PosY = 6205
	},
	[28] = {
		ID = 28, Name = "Đại Lý Phủ", PosX =8674, PosY = 4765
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
