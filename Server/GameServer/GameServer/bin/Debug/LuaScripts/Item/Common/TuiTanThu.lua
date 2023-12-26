-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200005' bên dưới thành ID tương ứng
local TuiTanThu = Scripts[200095]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - item tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function TuiTanThu:OnPreCheckCondition(scene, item, player, otherParams)

    -- ************************** --
    -- player:AddNotification("Enter -> TuiTanThu:OnPreCheckCondition")--
    -- ************************** --
    return true
    -- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi để thực thi Logic khi người sử dụng vật phẩm, sau khi đã thỏa mãn hàm kiểm tra điều kiện
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - item tương ứng
-- ****************************************************** --
local Record1 = 101119
local Record2 = 101120
local Record3 = 101121
local Record4 = 101122
local Record5 = 101123

local Record6 = 101124
local Record7 = 101125
local Record8 = 101126
local Record9 = 101127
local Record10 = 101128
local Record11 = 101129
function TuiTanThu:OnUse(scene, item, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	dialog:AddText("Xin chào "..player:GetName().."\nĐua top <color=red>Cấp độ</color> từ <color=green>20h00 17/11/2023 đến 23h00 24/12/2023</color>\nĐua top <color=red>Tài Phú</color> từ <color=green>20h00 17/11/2023 đến 23h00 31/12/2023</color> !")

	if player:GetFactionID()==0 then
		dialog:AddSelection(1,"Gia nhập Môn Phái.")
	else
		dialog:AddSelection(100030, "GiftCode")
		
		local record2 = Player.GetValueForeverRecore(player, Record2)
		if record2 ~= 1 then
			dialog:AddSelection(39901, "Hỗ Trợ Tân Thủ")
		end

		local record6 = Player.GetValueForeverRecore(player, Record6)	
		if record6 ~= 1 then
			dialog:AddSelection(10025, "Nhận <color=green>danh vọng</color> các loại")
		end

		--dialog:AddSelection(39900, "Hỗ Trợ Alpha Test")
		dialog:AddSelection(30003, "Mở shop Trang bị Danh Vọng (Yêu cầu Thạch Cổ Trấn)")

		--local record3 = Player.GetValueForeverRecore(player, Record3)
		--if record3 ~= 1 then
		--	dialog:AddSelection(39902, "Trang bị Tân Thủ")
		--end

		--local nCheck10 = Player.GetValueForeverRecore(player, Record10)
		--if nCheck10 ~= 1 then
		--	dialog:AddSelection(100035, "Nhận <color=green>Thẻ Đổi Tên</color>")
		--end
		
		local nCheck11 = Player.GetValueForeverRecore(player, Record11)
		if nCheck11 ~= 1 then
			dialog:AddSelection(100036, "Nhận <color=green>Thẻ Đổi Phái</color>")
		end
		
		--local record9 = Player.GetValueForeverRecore(player, Record9)
		--if record9 ~= 1 then
			dialog:AddSelection(10003, "Nhận <color=green>Phi Phong</color>")
		--end
		--local record8 = Player.GetValueForeverRecore(player, Record8)
		--if record8 ~= 1 then
			dialog:AddSelection(989898, "Nhận <color=green>Mật Tịch</color> theo phái")
		--end
		dialog:AddSelection(11111, "Nhận <color=green>Max Kinh Nghiệm Mật Tịch</color>")

		local record7 = Player.GetValueForeverRecore(player, Record7)
		if record7 ~= 1 then
			dialog:AddSelection(22222, "Nhận <color=green>Ngũ Hành Ấn</color>")
		end
		dialog:AddSelection(200031, "Nhận thưởng và xem đua top Cấp Độ và Tài Phú")
		dialog:AddSelection(30000, "Ta muốn đổi tên")
		dialog:AddSelection(30001, "Xóa vật phẩm")
		dialog:AddSelection(30002, "Ghép vật phẩm")	
	end	
	if player:IsGM() == 1 then
		dialog:AddSelection(100031, "Lưu danh sach top")
	end
	dialog:AddSelection(77777, "Kết thúc đối thoại")
	
	--	dialog:AddSelection(10011, "Dịch chuyển đến Hoàng Thành liên Server")
	
	dialog:Show(item, player)
	-- ************************** --
end
local AcitvityRecore = {
		--hai tac
        HaiTac = 11111,
		--Bach Ho 
		BVD = 11112,
        -- Tiêu Dao Cốc
		XoYo = 100000,
--		Tổng số ải đã vượt qua ngày hôm nay
		XoYo_StagesPassedToday = 100001,
		--Bí cảnh
         MiJing = 100002,
		-- Du Long Các
		YouLong = 100003,
    }
local SetTanThu80CoNgua ={
	[1] = {
		ItemID1 = 11605,--liên			--THiếu Lâm Kim
		ItemID2 = 12859,--nhẫn
		ItemID3 = 12005,--bội
		ItemID4 = 6360,--giày
		ItemID5 = 6882,--tay
		ItemID6 = 5668,--lưng
		ItemID7 = 4428,--ao
		ItemID8 = 3976,--phù
		ItemID9 = 9237,--mũ
		ItemID10 = 10218,--vk 1
		ItemID11 = 10228,--vk2
		ItemID12 = 3256,--mật tịch trung
		ItemID13 = 3257,--mật tịch trung

		ItemID14 = 11605,--liên
		ItemID15 = 12859,--nhẫn
		ItemID16 = 12015,--hương nang
		ItemID17 = 6370,--giày
		ItemID18 = 6892,--tay
		ItemID19 = 5678,--lưng
		ItemID20 = 4438,--ao
		ItemID21 = 3976,--phù
		ItemID22 = 9247,--mũ
		ItemID23 = 3461,--ngựa 1
		ItemID24 = 3460,--ngựa 2

	},
	[2] = {
		ItemID1 = 11605,			--Thiên Vương Kim
		ItemID2 = 12859,--nhẫn
		ItemID3 = 12005,--bội
		ItemID4 = 6360,--giày
		ItemID5 = 6882,--tay
		ItemID6 = 5668,--lưng
		ItemID7 = 4428,--ao
		ItemID8 = 3976,--phù
		ItemID9 = 9237,--mũ
		ItemID10 = 3258,--mật tịch trung
		ItemID11 = 3259,--mật tịch trung
		ItemID12 = 10238,--vk 3
		ItemID13 = 10248,--vk 4

		ItemID14 = 11605,
		ItemID15 = 12859,
		ItemID16 = 12015,--hương nang
		ItemID17 = 6370,--giày
		ItemID18 = 6892,--tay
		ItemID19 = 5678,--lưng
		ItemID20 = 4438,--ao
		ItemID21 = 3976,--phù
		ItemID22 = 9247,--mũ
		ItemID23 = 3461,--ngựa 1
		ItemID24 = 3460,--ngựa 2

	},
	[3] = {
		ItemID1 = 11615,--liên			--Đường Môn  Mộc
		ItemID2 = 12869,--nhẫn
		ItemID3 = 12025,--bội
		ItemID4 = 6380,--giày
		ItemID5 = 6902,--tay
		ItemID6 = 5688,--lưng
		ItemID7 = 4468,--ao
		ItemID8 = 3986,--phù
		ItemID9 = 9277,--nón
		
		ItemID10 = 3260,--mật tịch trung
		ItemID11 = 3261,--mật tịch trung
		ItemID12 = 12655,
		ItemID13 = 12665,
		
		ItemID16 = 11615,--liên
		ItemID17 = 12869,--nhẫn
		ItemID18 = 12035,--hương nang
		ItemID19 = 6390,--giày
		ItemID20 = 6912,--tay
		ItemID21 = 5698,--lưng
		ItemID22 = 4478,--ao
		ItemID23 = 3986,--phù
		ItemID24 = 9287,--nón
		ItemID25 = 3461,--ngựa 1
		ItemID26 = 3460,--ngựa 2
		
	},
	[4] = {
		ItemID1 = 11615,--liên			--Ngu Độc  Mộc
		ItemID2 = 12869,--nhẫn
		ItemID3 = 12025,--bội
		ItemID4 = 6380,--giày
		ItemID5 = 6902,--tay
		ItemID6 = 5688,--lưng
		ItemID7 = 4468,--ao
		ItemID8 = 3986,--phù
		ItemID9 = 9277,--nón
		
		ItemID10 = 10258,	
		ItemID11 = 10268,
		ItemID14 = 3262,--mật tịch trung
		ItemID15 = 3263,--mật tịch trung
		
		ItemID16 = 11615,--liên
		ItemID17 = 12869,--nhẫn
		ItemID18 = 12035,--hương nang
		ItemID19 = 6390,--giày
		ItemID20 = 6912,--tay
		ItemID21 = 5698,--lưng
		ItemID22 = 4478,--ao
		ItemID23 = 3986,--phù
		ItemID24 = 9287,--nón
		ItemID25 = 3461,--ngựa 1
		ItemID26 = 3460,--ngựa 2
		
	},
	[5] = {
		ItemID1 = 11625,			--Nga My  thủy
		ItemID2 = 12879,--nhẫn
		ItemID3 = 12045,--bội
		ItemID4 = 6400,--giày
		ItemID5 = 6922,--tay
		ItemID6 = 5708,--lưng
		ItemID7 = 4508,--áo
		ItemID8 = 3996,--phù
		ItemID9 = 9317,--nón

		ItemID10 = 3264,
		ItemID11 = 3265,
		ItemID12 = 10298,
		ItemID13 = 10308,

		
		ItemID16 = 11625,--liên
		ItemID17 = 12879,--nhẫn
		ItemID18 = 12055,--bội
		ItemID19 = 6410,--giày
		ItemID20 = 6932,--tay
		ItemID21 = 5718,--lưng
		ItemID22 = 4498,--áo
		ItemID23 = 3996,--phù
		ItemID24 = 9527,--nón
		ItemID25 = 3461,
		ItemID26 = 3460,
	},
	[6] = {
		ItemID1 = 11625,			--Thúy Yên  thủy
		ItemID2 = 12879,--nhẫn
		ItemID3 = 12045,--bội
		ItemID4 = 6400,--giày
		ItemID5 = 6922,--tay
		ItemID6 = 5708,--lưng
		ItemID7 = 4508,--áo
		ItemID8 = 3996,--phù
		ItemID9 = 9317,--nón

		ItemID10 = 10278,
		ItemID11 = 3266,
		ItemID12 = 3267,
		ItemID13 = 10308,

		
		ItemID16 = 11625,--liên
		ItemID17 = 12879,--nhẫn
		ItemID18 = 12055,--bội
		ItemID19 = 6410,--giày
		ItemID20 = 6932,--tay
		ItemID21 = 5718,--lưng
		ItemID22 = 4498,--áo
		ItemID23 = 3996,--phù
		ItemID24 = 9527,--nón
		ItemID25 = 3461,
		ItemID26 = 3460,
	},
	[7] = {
		ItemID1 = 11635,--liên			--  Cái Bang Hỏa
		ItemID2 = 12889,--nhẫn
		ItemID3 = 12065,--bội
		ItemID4 = 6420,--giày
		ItemID5 = 6942,--tay
		ItemID6 = 5728,--lưng
		ItemID7 = 4528,--áo
		ItemID8 = 4006,--phù
		ItemID9 = 9557,--nón

		ItemID10 = 3268,
		ItemID11 = 10318,
		ItemID12 = 3269,
		ItemID13 = 10338,
		
		ItemID16 = 11635,--liên
		ItemID17 = 12889,--nhẫn
		ItemID18 = 12075,--bội
		ItemID19 = 6430,--giày
		ItemID20 = 6952,--tay
		ItemID21 = 5738,--lưng
		ItemID22 = 4538,--áo
		ItemID23 = 4006,--phù
		ItemID24 = 9407,--nón
		ItemID25 = 3461,
		ItemID26 = 3460,
	},
	[8] = {
		ItemID1 = 11635,--liên			-- THiên Nhẫn Hỏa
		ItemID2 = 12889,--nhẫn
		ItemID3 = 12065,--bội
		ItemID4 = 6420,--giày
		ItemID5 = 6942,--tay
		ItemID6 = 5728,--lưng
		ItemID7 = 4528,--áo
		ItemID8 = 4006,--phù
		ItemID9 = 9557,--nón

		ItemID10 = 10348,
		ItemID11 = 3271,
		ItemID12 = 10328,
		ItemID13 = 3270,
		
		ItemID16 = 11635,--liên
		ItemID17 = 12889,--nhẫn
		ItemID18 = 12075,--bội
		ItemID19 = 6430,--giày
		ItemID20 = 6952,--tay
		ItemID21 = 5738,--lưng
		ItemID22 = 4538,--áo
		ItemID23 = 4006,--phù
		ItemID24 = 9407,--nón
		ItemID25 = 3461,
		ItemID26 = 3460,
	},
	[9] = {
		ItemID1 = 11645,--liên		-- Võ Đang Thổ
		ItemID2 = 12899,--nhẫn
		ItemID3 = 12085,--bội
		ItemID4 = 6440,--giày
		ItemID5 = 6962,--tay
		ItemID6 = 5748,--lưng
		ItemID7 = 4568,--giáp
		ItemID8 = 4016,--phù
		ItemID9 = 9597,--nón

		ItemID10 = 3272,
		ItemID11 = 10368,
		ItemID12 = 10378,
		ItemID13 = 3273,
		
		ItemID16 = 11645,--liên
		ItemID17 = 12899,--nhẫn
		ItemID18 = 12095,--bội
		ItemID19 = 6450,--giày
		ItemID20 = 6972,--tay
		ItemID21 = 5758,--lưng
		ItemID22 = 4578,--giáp
		ItemID23 = 4016,--phù
		ItemID24 = 9407,--nón
		ItemID25 = 3461,
		ItemID26 = 3460,
	},
	[10] = {
		ItemID1 = 11645,--liên		-- Côn Lôn Thổ
		ItemID2 = 12899,--nhẫn
		ItemID3 = 12085,--bội
		ItemID4 = 6440,--giày
		ItemID5 = 6962,--tay
		ItemID6 = 5748,--lưng
		ItemID7 = 4568,--giáp
		ItemID8 = 4016,--phù
		ItemID9 = 9597,--nón

		ItemID10 = 10358,
		ItemID11 = 3275,
		ItemID12 = 3274,
		ItemID13 = 10388,
		
		ItemID16 = 11645,--liên
		ItemID17 = 12899,--nhẫn
		ItemID18 = 12095,--bội
		ItemID19 = 6450,--giày
		ItemID20 = 6972,--tay
		ItemID21 = 5758,--lưng
		ItemID22 = 4578,--giáp
		ItemID23 = 4016,--phù
		ItemID24 = 9407,--nón
		ItemID25 = 3461,
		ItemID26 = 3460,
	},
	[11] = {
		ItemID1 = 11615,--liên			--Minh Giáo  Mộc
		ItemID2 = 12869,--nhẫn
		ItemID3 = 12025,--bội
		ItemID4 = 6380,--giày
		ItemID5 = 6902,--tay
		ItemID6 = 5688,--lưng
		ItemID7 = 4468,--ao
		ItemID8 = 3986,--phù
		ItemID9 = 9277,--nón
		
		ItemID10 = 3276,
		ItemID11 = 3277,
		ItemID12 = 10958,
		ItemID13 = 10968,
		
		ItemID16 = 11615,--liên
		ItemID17 = 12869,--nhẫn
		ItemID18 = 12035,--hương nang
		ItemID19 = 6390,--giày
		ItemID20 = 6912,--tay
		ItemID21 = 5698,--lưng
		ItemID22 = 5366,--ao
		ItemID23 = 3986,--phù
		ItemID24 = 9287,--nón
		ItemID25 = 3461,--ngựa 1
		ItemID26 = 3460,--ngựa 2
		
	},
	[12] = {
		ItemID1 = 11625,			--Đoàn Thị  thủy
		ItemID2 = 12879,--nhẫn
		ItemID3 = 12045,--bội
		ItemID4 = 6400,--giày
		ItemID5 = 6922,--tay
		ItemID6 = 5708,--lưng
		ItemID7 = 4508,--áo
		ItemID8 = 3996,--phù
		ItemID9 = 9337,--nón

		ItemID10 = 3278,
		ItemID11 = 10288,
		ItemID12 = 3279,
		ItemID13 = 10308,

		
		ItemID16 = 11625,--liên
		ItemID17 = 12879,--nhẫn
		ItemID18 = 12055,--bội
		ItemID19 = 6410,--giày
		ItemID20 = 6932,--tay
		ItemID21 = 5718,--lưng
		ItemID22 = 4498,--áo
		ItemID23 = 3996,--phù
		ItemID24 = 9527,--nón
		ItemID25 = 3461,
		ItemID26 = 3460,
	},
}
local secretID = {
	[1] = {
		ItemID1 = 3232,			-- Thiếu Lâm Phái - so
		ItemID2 = 3233,
		
		ItemID3 = 3256,		---Mật Tịch Trung
		ItemID4 = 3257,		---Mật Tịch Trung	
		
		ItemID5 = 3280, -- Cao
		ItemID6 = 3281,
	},
	[2] = {
		ItemID1 = 3234,			-- Thiên Vương Bang
		ItemID2 = 3235,
		
		ItemID3 = 3258,		---Mật Tịch Trung
		ItemID4 = 3259,		---Mật Tịch Trung

		ItemID5 = 3282, -- Cao
		ItemID6 = 3283,
	},
	[3] = {
		ItemID1 = 3236,			-- Đường Môn
		ItemID2 = 3237,
		
		ItemID3 = 3261,		---Mật Tịch Trung
		ItemID4 = 3260,		---Mật Tịch Trung

		ItemID5 = 3284, -- Cao
		ItemID6 = 3285,
	},
	[4] = {
		ItemID1 = 3238,			-- Ngũ Độc Giáo
		ItemID2 = 3239,
		
		ItemID3 = 3262,		---Mật Tịch Trung
		ItemID4 = 3263,		---Mật Tịch Trung

		ItemID5 = 3286, -- Cao
		ItemID6 = 3287,
	},
	[5] = {
		ItemID1 = 3240,			-- Nga My Phái
		ItemID2 = 3241,
		
		ItemID3 = 3264,		---Mật Tịch Trung
		ItemID4 = 3265,		---Mật Tịch Trung

		ItemID5 = 3288, -- Cao
		ItemID6 = 3289,
	},
	[6] = {
		ItemID1 = 3242,			-- Thúy Yên Môn
		ItemID2 = 3243,
		
		ItemID3 = 3266,		---Mật Tịch Trung
		ItemID4 = 3267,		---Mật Tịch Trung

		ItemID5 = 3290, -- Cao
		ItemID6 = 3291,
	},
	[7] = {
		ItemID1 = 3244,			-- Cái Bang Phái
		ItemID2 = 3245,
		
		ItemID3 = 3268,		---Mật Tịch Trung
		ItemID4 = 3269,		---Mật Tịch Trung

		ItemID5 = 3292, -- Cao
		ItemID6 = 3293,
	},
	[8] = {
		ItemID1 = 3246,			-- Thiên Nhẫn Giáo
		ItemID2 = 3247,
		
		ItemID3 = 3270,		---Mật Tịch Trung
		ItemID4 = 3271,		---Mật Tịch Trung

		ItemID5 = 3294, -- Cao
		ItemID6 = 3295,
	},
	[9] = {
		ItemID1 = 3248,			-- Võ Đang Phái
		ItemID2 = 3249,
		
		ItemID3 = 3272,		---Mật Tịch Trung
		ItemID4 = 3273,		---Mật Tịch Trung

		ItemID5 = 3296, -- Cao
		ItemID6 = 3297,
	},
	[10] = {
		ItemID1 = 3250,			-- Côn Lôn Phái
		ItemID2 = 3251,
		
		ItemID3 = 3274,		---Mật Tịch Trung
		ItemID4 = 3275,		---Mật Tịch Trung

		ItemID5 = 3298, -- Cao
		ItemID6 = 3299,
	},
	[11] = {
		ItemID1 = 3252,			-- Minh Giáo
		ItemID2 = 3253,
		
		ItemID3 = 3276,		---Mật Tịch Trung
		ItemID4 = 3277,		---Mật Tịch Trung

		ItemID5 = 3300, -- Cao
		ItemID6 = 3301,
	},
	[12] = {
		ItemID1 = 3254,			-- Đoàn Thị Phái
		ItemID2 = 3255,
		
		ItemID3 = 3278,		---Mật Tịch Trung
		ItemID4 = 3279,		---Mật Tịch Trung

		ItemID5 = 3302, -- Cao
		ItemID6 = 3303,
	},
}
local SetBelongings ={
	[1] = {
		ItemID1 = 9639,			-- Thiếu Kim
		ItemID2 = 4810,
		ItemID3 = 5870,
		ItemID4 = 7084,
		ItemID5 = 6562,
		ItemID6 = 11707,
		ItemID7 = 12961,
		ItemID8 = 12207,
		ItemID9 = 4078,
		ItemID10 = 11174,
		ItemID11 = 10600,
		ItemID12 = 10610,
		ItemID13 = 10770,

		ItemID14 = 9649,
		ItemID15 = 4820,
		ItemID16 = 5880,
		ItemID17 = 6572,
		ItemID18 = 12217,
		ItemID19 = 11707,
		ItemID20 = 12961,
		ItemID21 = 4078,
		ItemID22 = 7094,
		ItemID23 = 3482,

	},
	[2] = {
		ItemID1 = 9679,			-- Thiếu Mộc
		ItemID2 = 4850,
		ItemID3 = 5890,
		ItemID4 = 7104,
		ItemID5 = 6582,
		ItemID6 = 11717,
		ItemID7 = 12971,
		ItemID8 = 12227,
		ItemID9 = 4088,
		ItemID10 = 10620,
		ItemID11 = 11030,
		ItemID12 = 10630,
		ItemID13 = 12707,
		ItemID14 = 12717,
		ItemID15 = 11020,
		ItemID30 = 10350,
		
		ItemID16 = 9709,
		ItemID17 = 4860,
		ItemID18 = 5900,
		ItemID19 = 7114,
		ItemID20 = 6592,
		ItemID21 = 11717,
		ItemID22 = 12971,
		ItemID23 = 4088,
		ItemID24 =12237,
		ItemID25 = 3482,
		
	},
	[3] = {
		ItemID1 = 9739,			-- Thiếu thủy
		ItemID2 = 4890,
		ItemID3 = 5910,
		ItemID4 = 7124,
		ItemID5 = 6602,
		ItemID6 = 11727,
		ItemID7 = 12981,
		ItemID8 = 12247,
		ItemID9 = 4098,
		ItemID10 = 10640,
		ItemID11 = 10670,
		ItemID12 = 10650,
		ItemID13 = 10660,

		
		ItemID16 = 9749,
		ItemID17 = 4900,
		ItemID18 = 5920,
		ItemID19 = 7134,
		ItemID20 = 6612,
		ItemID21 = 11727,
		ItemID22 = 12981,
		ItemID23 = 12257,
		ItemID24 = 4098,
		ItemID25 = 3482,
	},
	[4] = {
		ItemID1 = 9779,			-- Thiếu Hỏa
		ItemID2 = 4930,
		ItemID3 = 5930,
		ItemID4 = 7144,
		ItemID5 = 6622,
		ItemID6 = 11737,
		ItemID7 = 12991,
		ItemID8 = 12267,
		ItemID9 = 4108,
		ItemID10 = 10710,
		ItemID11 = 10690,
		ItemID12 = 10680,
		ItemID13 = 10700,
		ItemID30 = 10350,
		
		ItemID16 = 9789,
		ItemID17 = 4940,
		ItemID18 = 5940,
		ItemID19 = 7154,
		ItemID20 = 6632,
		ItemID21 = 11737,
		ItemID22 = 12991,
		ItemID23 = 12277,
		ItemID24 = 4108,
		ItemID25 = 3482,
	},
	[5] = {
		ItemID1 = 9819,			-- Thiếu Thổ
		ItemID2 = 4970,
		ItemID3 = 5950,
		ItemID4 = 7164,
		ItemID5 = 6642,
		ItemID6 = 11747,
		ItemID7 = 13001,
		ItemID8 = 12287,
		ItemID9 = 4118,
		ItemID10 = 10720,
		ItemID11 = 10730,
		ItemID12 = 10740,
		ItemID13 = 10750,
		
		ItemID16 = 9829,
		ItemID17 = 4980,
		ItemID18 = 5960,
		ItemID19 = 7174,
		ItemID20 = 6652,
		ItemID21 = 11747,
		ItemID22 = 13001,
		ItemID23 = 12297,
		ItemID24 = 4118,
		ItemID25 = 3482,
	},
}
local SetBelongings89 ={
	[1] = {
		ItemID1 = 11756,			-- Thiếu Kim
		ItemID2 = 13067,
		ItemID3 = 12543,
		ItemID4 = 6661,
		ItemID5 = 7183,
		ItemID6 = 5969,
		ItemID7 = 5029,
		ItemID8 = 4127,
		ItemID9 = 9638,
		ItemID10 = 10758,
		ItemID11 = 10768,
		ItemID12 = 10778,
		ItemID13 = 10788,

		ItemID14 = 11756,
		ItemID15 = 13067,
		ItemID16 = 12550,
		ItemID17 = 6671,
		ItemID18 = 7193,
		ItemID19 = 5979,
		ItemID20 = 5039,
		ItemID21 = 4127,
		ItemID22 = 9648,
		ItemID23 = 3482,

	},
	[2] = {
		ItemID1 = 11766,			-- Thiếu Mộc
		ItemID2 = 13068,
		ItemID3 = 12557,
		ItemID4 = 6681,
		ItemID5 = 7203,
		ItemID6 = 5989,
		ItemID7 = 5069,
		ItemID8 = 4137,
		ItemID9 = 9678,
		
		ItemID10 = 10798,
		ItemID11 = 10998,
		ItemID12 = 10808,
		ItemID13 = 11008,
		ItemID14 = 12635,
		ItemID15 = 12645,
		ItemID30 = 10350,
		
		ItemID16 = 11766,
		ItemID17 = 13068,
		ItemID18 = 12564,
		ItemID19 = 6691,
		ItemID20 = 7213,
		ItemID21 = 5999,
		ItemID22 = 5079,
		ItemID23 = 4137,
		ItemID24 =9688,
		ItemID25 = 3482,
		
	},
	[3] = {
		ItemID1 = 11776,			-- Thiếu thủy
		ItemID2 = 13069,
		ItemID3 = 12571,
		ItemID4 = 6701,
		ItemID5 = 7223,
		ItemID6 = 6009,
		ItemID7 = 5089,
		ItemID8 = 4147,
		ItemID9 = 9718,

		ItemID10 = 10818,
		ItemID11 = 10828,
		ItemID12 = 10838,
		ItemID13 = 10848,

		
		ItemID16 = 11776,
		ItemID17 = 13069,
		ItemID18 = 12578,
		ItemID19 = 6711,
		ItemID20 = 7233,
		ItemID21 = 6019,
		ItemID22 = 5099,
		ItemID23 = 4147,
		ItemID24 = 9728,
		ItemID25 = 3482,
	},
	[4] = {
		ItemID1 = 11786,			-- Thiếu Hỏa
		ItemID2 = 13070,
		ItemID3 = 12585,
		ItemID4 = 6721,
		ItemID5 = 7243,
		ItemID6 = 6029,
		ItemID7 = 5129,
		ItemID8 = 4157,
		ItemID9 = 9758,

		ItemID10 = 10888,
		ItemID11 = 10858,
		ItemID12 = 10868,
		ItemID13 = 10878,
		ItemID30 = 10350,
		
		ItemID16 = 11786,
		ItemID17 = 13070,
		ItemID18 = 12592,
		ItemID19 = 6731,
		ItemID20 = 7253,
		ItemID21 = 6039,
		ItemID22 = 5139,
		ItemID23 = 4157,
		ItemID24 = 9768,
		ItemID25 = 3482,
	},
	[5] = {
		ItemID1 = 11796,			-- Thiếu Thổ
		ItemID2 = 13071,
		ItemID3 = 12599,
		ItemID4 = 6741,
		ItemID5 = 7263,
		ItemID6 = 6049,
		ItemID7 = 5169,
		ItemID8 = 4167,
		ItemID9 = 9798,

		ItemID10 = 10898,
		ItemID11 = 10908,
		ItemID12 = 10918,
		ItemID13 = 10928,
		
		ItemID16 = 11796,
		ItemID17 = 13071,
		ItemID18 = 12606,
		ItemID19 = 6751,
		ItemID20 = 7273,
		ItemID21 = 6059,
		ItemID22 = 5179,
		ItemID23 = 4167,
		ItemID24 = 9808,
		ItemID25 = 3482,
	},
}
local SetBelongingsHK ={
	[1] = { -- Thiếu Lâm Kim
		ItemID1 = 7746,			---Y Phục
		ItemID2 = 8165,			---Nón
		ItemID3 = 6221,			---Đai Lưng
		ItemID4 = 7485,			---Hộ Uyển
		ItemID5 = 6763,			---Giày
		ItemID6 = 4174,			---Phù
		ItemID7 = 12618,		---Ngọc Bội
		ItemID8 = 11893,		---Hạng Liên
		ItemID9 = 13062,		---Nhẫn

		ItemID10 = 11501,		---Vũ Khí 1
		ItemID11 = 11502,		---Vũ Khí 2
		ItemID12 = 3256,		---Mật Tịch Trung
		ItemID13 = 3257,		---Mật Tịch Trung
		ItemID14 = 3280,		---Mật Tịch Cao
		ItemID15 = 3281,		---Mật Tịch Cao

		ItemID16 = 3628,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7736,		---Y Phục Nữ
		ItemID19 = 8166,		---Nón Nữ
		ItemID20 = 6226,		---Đai Lưng Nữ
		ItemID21 = 7486,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6764,		---Giày Nữ
		ItemID23 = 12619,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3638,		---Phi Phong Nữ

	},
	[2] = { -- Thiên Vương  Kim
		ItemID1 = 7746,			---Y Phục
		ItemID2 = 8165,			---Nón
		ItemID3 = 6221,			---Đai Lưng
		ItemID4 = 7485,			---Hộ Uyển
		ItemID5 = 6763,			---Giày
		ItemID6 = 4174,			---Phù
		ItemID7 = 12618,		---Ngọc Bội
		ItemID8 = 11893,		---Hạng Liên
		ItemID9 = 13062,		---Nhẫn

		ItemID10 = 11498,		---Vũ Khí 1
		ItemID11 = 11499,		---Vũ Khí 2
		ItemID12 = 3258,		---Mật Tịch Trung
		ItemID13 = 3259,		---Mật Tịch Trung
		ItemID14 = 3282,		---Mật Tịch Cao
		ItemID15 = 3283,		---Mật Tịch Cao

		ItemID16 = 3628,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7736,		---Y Phục Nữ
		ItemID19 = 8166,		---Nón Nữ
		ItemID20 = 6226,		---Đai Lưng Nữ
		ItemID21 = 7486,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6764,		---Giày Nữ
		ItemID23 = 12619,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3638,		---Phi Phong Nữ

	},
	[3] = {-- Đường Môn Mộc
		ItemID1 = 7747,			---Y Phục
		ItemID2 = 8167,			---Nón
		ItemID3 = 6242,			---Đai Lưng
		ItemID4 = 7487,			---Hộ Uyển
		ItemID5 = 6765,			---Giày
		ItemID6 = 4175,			---Phù
		ItemID7 = 12620,		---Ngọc Bội
		ItemID8 = 11894,		---Hạng Liên
		ItemID9 = 13063,		---Nhẫn

		ItemID10 = 12798,		---Vũ Khí 1
		ItemID11 = 12793,		---Vũ Khí 2
		ItemID12 = 3261,		---Mật Tịch Trung
		ItemID13 = 3260,		---Mật Tịch Trung
		ItemID14 = 3284,		---Mật Tịch Cao
		ItemID15 = 3285,		---Mật Tịch Cao

		ItemID16 = 3648,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7737,		---Y Phục Nữ
		ItemID19 = 8168,		---Nón Nữ
		ItemID20 = 6227,		---Đai Lưng Nữ
		ItemID21 = 7488,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6766,		---Giày Nữ
		ItemID23 = 12621,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3658,		---Phi Phong Nữ
		
	},
	[4] = {-- Ngũ Độc Mộc
		ItemID1 = 7747,			---Y Phục
		ItemID2 = 8167,			---Nón
		ItemID3 = 6242,			---Đai Lưng
		ItemID4 = 7487,			---Hộ Uyển
		ItemID5 = 6765,			---Giày
		ItemID6 = 4175,			---Phù
		ItemID7 = 12620,		---Ngọc Bội
		ItemID8 = 11894,		---Hạng Liên
		ItemID9 = 13063,		---Nhẫn

		ItemID10 = 11512,		---Vũ Khí 1
		ItemID11 = 11517,		---Vũ Khí 2
		ItemID12 = 3262,		---Mật Tịch Trung
		ItemID13 = 3263,		---Mật Tịch Trung
		ItemID14 = 3286,		---Mật Tịch Cao
		ItemID15 = 3287,		---Mật Tịch Cao

		ItemID16 = 3648,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7737,		---Y Phục Nữ
		ItemID19 = 8168,		---Nón Nữ
		ItemID20 = 6227,		---Đai Lưng Nữ
		ItemID21 = 7488,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6766,		---Giày Nữ
		ItemID23 = 12621,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3658,		---Phi Phong Nữ
		
	},
	[5] = {-- Nga My thủy
		ItemID1 = 7748,			---Y Phục
		ItemID2 = 8169,			---Nón
		ItemID3 = 6223,			---Đai Lưng
		ItemID4 = 7489,			---Hộ Uyển
		ItemID5 = 6767,			---Giày
		ItemID6 = 4176,			---Phù
		ItemID7 = 12622,		---Ngọc Bội
		ItemID8 = 11895,		---Hạng Liên
		ItemID9 = 13064,		---Nhẫn

		ItemID10 = 11526,		---Vũ Khí 1
		ItemID11 = 11527,		---Vũ Khí 2
		ItemID12 = 3265,		---Mật Tịch Trung
		ItemID13 = 3264,		---Mật Tịch Trung
		ItemID14 = 3288,		---Mật Tịch Cao
		ItemID15 = 3289,		---Mật Tịch Cao

		ItemID16 = 3668,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7738,		---Y Phục Nữ
		ItemID19 = 8170,		---Nón Nữ
		ItemID20 = 6228,		---Đai Lưng Nữ
		ItemID21 = 7490,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6768,		---Giày Nữ
		ItemID23 = 12623,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3678,		---Phi Phong Nữ

	},
	[6] = {-- Thúy Yên thủy
		ItemID1 = 7748,			---Y Phục
		ItemID2 = 8169,			---Nón
		ItemID3 = 6223,			---Đai Lưng
		ItemID4 = 7489,			---Hộ Uyển
		ItemID5 = 6767,			---Giày
		ItemID6 = 4176,			---Phù
		ItemID7 = 12622,		---Ngọc Bội
		ItemID8 = 11895,		---Hạng Liên
		ItemID9 = 13064,		---Nhẫn

		ItemID10 = 11526,		---Vũ Khí 1
		ItemID11 = 11522,		---Vũ Khí 2
		ItemID12 = 3266,		---Mật Tịch Trung
		ItemID13 = 3267,		---Mật Tịch Trung
		ItemID14 = 3290,		---Mật Tịch Cao
		ItemID15 = 3291,		---Mật Tịch Cao

		ItemID16 = 3668,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7738,		---Y Phục Nữ
		ItemID19 = 8170,		---Nón Nữ
		ItemID20 = 6228,		---Đai Lưng Nữ
		ItemID21 = 7490,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6768,		---Giày Nữ
		ItemID23 = 12623,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3678,		---Phi Phong Nữ

	},
	[7] = {-- Cái Bang Hỏa
		ItemID1 = 7749,			---Y Phục
		ItemID2 = 8171,			---Nón
		ItemID3 = 6224,			---Đai Lưng
		ItemID4 = 7491,			---Hộ Uyển
		ItemID5 = 6769,			---Giày
		ItemID6 = 4177,			---Phù
		ItemID7 = 12624,		---Ngọc Bội
		ItemID8 = 11896,		---Hạng Liên
		ItemID9 = 13065,		---Nhẫn

		ItemID10 = 11531,		---Vũ Khí 1
		ItemID11 = 11537,		---Vũ Khí 2
		ItemID12 = 3268,		---Mật Tịch Trung
		ItemID13 = 3269,		---Mật Tịch Trung
		ItemID14 = 3292,		---Mật Tịch Cao
		ItemID15 = 3293,		---Mật Tịch Cao

		ItemID16 = 3688,		---Phi Phong Nam
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7739,		---Y Phục Nữ
		ItemID19 = 8172,		---Nón Nữ
		ItemID20 = 6229,		---Đai Lưng Nữ
		ItemID21 = 7492,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6770,		---Giày Nữ
		ItemID23 = 12625,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3698,		---Phi Phong Nữ

	},
	[8] = {-- Thiên Nhẫn Hỏa
		ItemID1 = 7749,			---Y Phục
		ItemID2 = 8171,			---Nón
		ItemID3 = 6224,			---Đai Lưng
		ItemID4 = 7491,			---Hộ Uyển
		ItemID5 = 6769,			---Giày
		ItemID6 = 4177,			---Phù
		ItemID7 = 12624,		---Ngọc Bội
		ItemID8 = 11896,		---Hạng Liên
		ItemID9 = 13065,		---Nhẫn

		ItemID10 = 11534,		---Vũ Khí 1
		ItemID11 = 11535,		---Vũ Khí 2
		ItemID12 = 3270,		---Mật Tịch Trung
		ItemID13 = 3271,		---Mật Tịch Trung
		ItemID14 = 3294,		---Mật Tịch Cao
		ItemID15 = 3295,		---Mật Tịch Cao

		ItemID16 = 3688,		---Phi Phong Nam
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7739,		---Y Phục Nữ
		ItemID19 = 8172,		---Nón Nữ
		ItemID20 = 6229,		---Đai Lưng Nữ
		ItemID21 = 7492,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6770,		---Giày Nữ
		ItemID23 = 12625,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3698,		---Phi Phong Nữ

	},
	[9] = {-- Võ Đang Thổ
		ItemID1 = 7750,			---Y Phục
		ItemID2 = 8173,			---Nón
		ItemID3 = 6225,			---Đai Lưng
		ItemID4 = 7493,			---Hộ Uyển
		ItemID5 = 6771,			---Giày
		ItemID6 = 4178,			---Phù
		ItemID7 = 12626,		---Ngọc Bội
		ItemID8 = 11897,		---Hạng Liên
		ItemID9 = 13066,		---Nhẫn

		ItemID10 = 11543,		---Vũ Khí 1
		ItemID11 = 11546,		---Vũ Khí 2
		ItemID12 = 3272,		---Mật Tịch Trung
		ItemID13 = 3273,		---Mật Tịch Trung
		ItemID14 = 3296,		---Mật Tịch Cao
		ItemID15 = 3297,		---Mật Tịch Cao

		ItemID16 = 3708,		---Phi Phong Nam
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7740,		---Y Phục Nữ
		ItemID19 = 8174,		---Nón Nữ
		ItemID20 = 6230,		---Đai Lưng Nữ
		ItemID21 = 7494,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6772,		---Giày Nữ
		ItemID23 = 12627,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3718,		---Phi Phong Nữ

	},
	[10] = {-- Côn Lôn Thổ
		ItemID1 = 7750,			---Y Phục
		ItemID2 = 8173,			---Nón
		ItemID3 = 6225,			---Đai Lưng
		ItemID4 = 7493,			---Hộ Uyển
		ItemID5 = 6771,			---Giày
		ItemID6 = 4178,			---Phù
		ItemID7 = 12626,		---Ngọc Bội
		ItemID8 = 11897,		---Hạng Liên
		ItemID9 = 13066,		---Nhẫn

		ItemID10 = 11542,		---Vũ Khí 1
		ItemID11 = 11546,		---Vũ Khí 2
		ItemID12 = 3274,		---Mật Tịch Trung
		ItemID13 = 3275,		---Mật Tịch Trung
		ItemID14 = 3298,		---Mật Tịch Cao
		ItemID15 = 3299,		---Mật Tịch Cao

		ItemID16 = 3708,		---Phi Phong Nam
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7740,		---Y Phục Nữ
		ItemID19 = 8174,		---Nón Nữ
		ItemID20 = 6230,		---Đai Lưng Nữ
		ItemID21 = 7494,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6772,		---Giày Nữ
		ItemID23 = 12627,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3718,		---Phi Phong Nữ

	},
	[11] = {-- Minh Giáo mộc
		ItemID1 = 7747,			---Y Phục
		ItemID2 = 8167,			---Nón
		ItemID3 = 6242,			---Đai Lưng
		ItemID4 = 7487,			---Hộ Uyển
		ItemID5 = 6765,			---Giày
		ItemID6 = 4175,			---Phù
		ItemID7 = 12620,		---Ngọc Bội
		ItemID8 = 11894,		---Hạng Liên
		ItemID9 = 13063,		---Nhẫn

		ItemID10 = 11509,		---Vũ Khí 1
		ItemID11 = 11516,		---Vũ Khí 2
		ItemID12 = 3276,		---Mật Tịch Trung
		ItemID13 = 3277,		---Mật Tịch Trung
		ItemID14 = 3300,		---Mật Tịch Cao
		ItemID15 = 3301,		---Mật Tịch Cao

		ItemID16 = 3648,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7737,		---Y Phục Nữ
		ItemID19 = 8168,		---Nón Nữ
		ItemID20 = 6227,		---Đai Lưng Nữ
		ItemID21 = 7488,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6766,		---Giày Nữ
		ItemID23 = 12621,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3658,		---Phi Phong Nữ
	},
	[12] = {-- Đoàn Thị thủy
		ItemID1 = 7748,			---Y Phục Nam
		ItemID2 = 8169,			---Nón Nam
		ItemID3 = 6223,			---Đai Lưng Nam
		ItemID4 = 7489,			---Hộ Uyển Nam
		ItemID5 = 6767,			---Giày Nam
		ItemID6 = 4176,			---Phù Chung
		ItemID7 = 12622,		---Ngọc Bội Nam
		ItemID8 = 11895,		---Hạng Liên Chung
		ItemID9 = 13064,		---Nhẫn Chung

		ItemID10 = 11526,		---Vũ Khí 1
		ItemID11 = 11520,		---Vũ Khí 2
		ItemID12 = 3278,		---Mật Tịch Trung
		ItemID13 = 3279,		---Mật Tịch Trung
		ItemID14 = 3302,		---Mật Tịch Cao
		ItemID15 = 3303,		---Mật Tịch Cao

		ItemID16 = 3668,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7738,		---Y Phục Nữ
		ItemID19 = 8170,		---Nón Nữ
		ItemID20 = 6228,		---Đai Lưng Nữ
		ItemID21 = 7490,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6768,		---Giày Nữ
		ItemID23 = 12623,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3678,		---Phi Phong Nữ

	},
}
-- ****************************************************** --
local SetDoTanThuTheoHeCoNgua ={
	[1] = { -- Thiếu Lâm Kim
		ItemID1 = 7746,			---Y Phục
		ItemID2 = 8165,			---Nón
		ItemID3 = 6221,			---Đai Lưng
		ItemID4 = 7485,			---Hộ Uyển
		ItemID5 = 6763,			---Giày
		ItemID6 = 4174,			---Phù
		ItemID7 = 12618,		---Ngọc Bội
		ItemID8 = 11893,		---Hạng Liên
		ItemID9 = 13062,		---Nhẫn

		ItemID10 = 11501,		---Vũ Khí 1
		ItemID11 = 11502,		---Vũ Khí 2
		ItemID12 = 3256,		---Mật Tịch Trung
		ItemID13 = 3257,		---Mật Tịch Trung
		ItemID14 = 3280,		---Mật Tịch Cao
		ItemID15 = 3281,		---Mật Tịch Cao

		ItemID16 = 3628,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7736,		---Y Phục Nữ
		ItemID19 = 8166,		---Nón Nữ
		ItemID20 = 6226,		---Đai Lưng Nữ
		ItemID21 = 7486,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6764,		---Giày Nữ
		ItemID23 = 12619,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3638,		---Phi Phong Nữ

	},
	[2] = { -- Thiên Vương  Kim
		ItemID1 = 7746,			---Y Phục
		ItemID2 = 8165,			---Nón
		ItemID3 = 6221,			---Đai Lưng
		ItemID4 = 7485,			---Hộ Uyển
		ItemID5 = 6763,			---Giày
		ItemID6 = 4174,			---Phù
		ItemID7 = 12618,		---Ngọc Bội
		ItemID8 = 11893,		---Hạng Liên
		ItemID9 = 13062,		---Nhẫn

		ItemID10 = 11498,		---Vũ Khí 1
		ItemID11 = 11499,		---Vũ Khí 2
		ItemID12 = 3258,		---Mật Tịch Trung
		ItemID13 = 3259,		---Mật Tịch Trung
		ItemID14 = 3282,		---Mật Tịch Cao
		ItemID15 = 3283,		---Mật Tịch Cao

		ItemID16 = 3628,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7736,		---Y Phục Nữ
		ItemID19 = 8166,		---Nón Nữ
		ItemID20 = 6226,		---Đai Lưng Nữ
		ItemID21 = 7486,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6764,		---Giày Nữ
		ItemID23 = 12619,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3638,		---Phi Phong Nữ

	},
	[3] = {-- Đường Môn Mộc
		ItemID1 = 7747,			---Y Phục
		ItemID2 = 8167,			---Nón
		ItemID3 = 6242,			---Đai Lưng
		ItemID4 = 7487,			---Hộ Uyển
		ItemID5 = 6765,			---Giày
		ItemID6 = 4175,			---Phù
		ItemID7 = 12620,		---Ngọc Bội
		ItemID8 = 11894,		---Hạng Liên
		ItemID9 = 13063,		---Nhẫn

		ItemID10 = 12798,		---Vũ Khí 1
		ItemID11 = 12793,		---Vũ Khí 2
		ItemID12 = 3261,		---Mật Tịch Trung
		ItemID13 = 3260,		---Mật Tịch Trung
		ItemID14 = 3284,		---Mật Tịch Cao
		ItemID15 = 3285,		---Mật Tịch Cao

		ItemID16 = 3648,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7737,		---Y Phục Nữ
		ItemID19 = 8168,		---Nón Nữ
		ItemID20 = 6227,		---Đai Lưng Nữ
		ItemID21 = 7488,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6766,		---Giày Nữ
		ItemID23 = 12621,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3658,		---Phi Phong Nữ
		
	},
	[4] = {-- Ngũ Độc Mộc
		ItemID1 = 7747,			---Y Phục
		ItemID2 = 8167,			---Nón
		ItemID3 = 6242,			---Đai Lưng
		ItemID4 = 7487,			---Hộ Uyển
		ItemID5 = 6765,			---Giày
		ItemID6 = 4175,			---Phù
		ItemID7 = 12620,		---Ngọc Bội
		ItemID8 = 11894,		---Hạng Liên
		ItemID9 = 13063,		---Nhẫn

		ItemID10 = 11512,		---Vũ Khí 1
		ItemID11 = 11517,		---Vũ Khí 2
		ItemID12 = 3262,		---Mật Tịch Trung
		ItemID13 = 3263,		---Mật Tịch Trung
		ItemID14 = 3286,		---Mật Tịch Cao
		ItemID15 = 3287,		---Mật Tịch Cao

		ItemID16 = 3648,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7737,		---Y Phục Nữ
		ItemID19 = 8168,		---Nón Nữ
		ItemID20 = 6227,		---Đai Lưng Nữ
		ItemID21 = 7488,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6766,		---Giày Nữ
		ItemID23 = 12621,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3658,		---Phi Phong Nữ
		
	},
	[5] = {-- Nga My thủy
		ItemID1 = 7748,			---Y Phục
		ItemID2 = 8169,			---Nón
		ItemID3 = 6223,			---Đai Lưng
		ItemID4 = 7489,			---Hộ Uyển
		ItemID5 = 6767,			---Giày
		ItemID6 = 4176,			---Phù
		ItemID7 = 12622,		---Ngọc Bội
		ItemID8 = 11895,		---Hạng Liên
		ItemID9 = 13064,		---Nhẫn

		ItemID10 = 11526,		---Vũ Khí 1
		ItemID11 = 11527,		---Vũ Khí 2
		ItemID12 = 3265,		---Mật Tịch Trung
		ItemID13 = 3264,		---Mật Tịch Trung
		ItemID14 = 3288,		---Mật Tịch Cao
		ItemID15 = 3289,		---Mật Tịch Cao

		ItemID16 = 3668,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7738,		---Y Phục Nữ
		ItemID19 = 8170,		---Nón Nữ
		ItemID20 = 6228,		---Đai Lưng Nữ
		ItemID21 = 7490,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6768,		---Giày Nữ
		ItemID23 = 12623,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3678,		---Phi Phong Nữ

	},
	[6] = {-- Thúy Yên thủy
		ItemID1 = 7748,			---Y Phục
		ItemID2 = 8169,			---Nón
		ItemID3 = 6223,			---Đai Lưng
		ItemID4 = 7489,			---Hộ Uyển
		ItemID5 = 6767,			---Giày
		ItemID6 = 4176,			---Phù
		ItemID7 = 12622,		---Ngọc Bội
		ItemID8 = 11895,		---Hạng Liên
		ItemID9 = 13064,		---Nhẫn

		ItemID10 = 11526,		---Vũ Khí 1
		ItemID11 = 11522,		---Vũ Khí 2
		ItemID12 = 3266,		---Mật Tịch Trung
		ItemID13 = 3267,		---Mật Tịch Trung
		ItemID14 = 3290,		---Mật Tịch Cao
		ItemID15 = 3291,		---Mật Tịch Cao

		ItemID16 = 3668,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7738,		---Y Phục Nữ
		ItemID19 = 8170,		---Nón Nữ
		ItemID20 = 6228,		---Đai Lưng Nữ
		ItemID21 = 7490,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6768,		---Giày Nữ
		ItemID23 = 12623,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3678,		---Phi Phong Nữ

	},
	[7] = {-- Cái Bang Hỏa
		ItemID1 = 7749,			---Y Phục
		ItemID2 = 8171,			---Nón
		ItemID3 = 6224,			---Đai Lưng
		ItemID4 = 7491,			---Hộ Uyển
		ItemID5 = 6769,			---Giày
		ItemID6 = 4177,			---Phù
		ItemID7 = 12624,		---Ngọc Bội
		ItemID8 = 11896,		---Hạng Liên
		ItemID9 = 13065,		---Nhẫn

		ItemID10 = 11531,		---Vũ Khí 1
		ItemID11 = 11537,		---Vũ Khí 2
		ItemID12 = 3268,		---Mật Tịch Trung
		ItemID13 = 3269,		---Mật Tịch Trung
		ItemID14 = 3292,		---Mật Tịch Cao
		ItemID15 = 3293,		---Mật Tịch Cao

		ItemID16 = 3688,		---Phi Phong Nam
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7739,		---Y Phục Nữ
		ItemID19 = 8172,		---Nón Nữ
		ItemID20 = 6229,		---Đai Lưng Nữ
		ItemID21 = 7492,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6770,		---Giày Nữ
		ItemID23 = 12625,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3698,		---Phi Phong Nữ

	},
	[8] = {-- Thiên Nhẫn Hỏa
		ItemID1 = 7749,			---Y Phục
		ItemID2 = 8171,			---Nón
		ItemID3 = 6224,			---Đai Lưng
		ItemID4 = 7491,			---Hộ Uyển
		ItemID5 = 6769,			---Giày
		ItemID6 = 4177,			---Phù
		ItemID7 = 12624,		---Ngọc Bội
		ItemID8 = 11896,		---Hạng Liên
		ItemID9 = 13065,		---Nhẫn

		ItemID10 = 11534,		---Vũ Khí 1
		ItemID11 = 11535,		---Vũ Khí 2
		ItemID12 = 3270,		---Mật Tịch Trung
		ItemID13 = 3271,		---Mật Tịch Trung
		ItemID14 = 3294,		---Mật Tịch Cao
		ItemID15 = 3295,		---Mật Tịch Cao

		ItemID16 = 3688,		---Phi Phong Nam
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7739,		---Y Phục Nữ
		ItemID19 = 8172,		---Nón Nữ
		ItemID20 = 6229,		---Đai Lưng Nữ
		ItemID21 = 7492,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6770,		---Giày Nữ
		ItemID23 = 12625,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3698,		---Phi Phong Nữ

	},
	[9] = {-- Võ Đang Thổ
		ItemID1 = 7750,			---Y Phục
		ItemID2 = 8173,			---Nón
		ItemID3 = 6225,			---Đai Lưng
		ItemID4 = 7493,			---Hộ Uyển
		ItemID5 = 6771,			---Giày
		ItemID6 = 4178,			---Phù
		ItemID7 = 12626,		---Ngọc Bội
		ItemID8 = 11897,		---Hạng Liên
		ItemID9 = 13066,		---Nhẫn

		ItemID10 = 11543,		---Vũ Khí 1
		ItemID11 = 11546,		---Vũ Khí 2
		ItemID12 = 3272,		---Mật Tịch Trung
		ItemID13 = 3273,		---Mật Tịch Trung
		ItemID14 = 3296,		---Mật Tịch Cao
		ItemID15 = 3297,		---Mật Tịch Cao

		ItemID16 = 3708,		---Phi Phong Nam
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7740,		---Y Phục Nữ
		ItemID19 = 8174,		---Nón Nữ
		ItemID20 = 6230,		---Đai Lưng Nữ
		ItemID21 = 7494,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6772,		---Giày Nữ
		ItemID23 = 12627,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3718,		---Phi Phong Nữ

	},
	[10] = {-- Côn Lôn Thổ
		ItemID1 = 7750,			---Y Phục
		ItemID2 = 8173,			---Nón
		ItemID3 = 6225,			---Đai Lưng
		ItemID4 = 7493,			---Hộ Uyển
		ItemID5 = 6771,			---Giày
		ItemID6 = 4178,			---Phù
		ItemID7 = 12626,		---Ngọc Bội
		ItemID8 = 11897,		---Hạng Liên
		ItemID9 = 13066,		---Nhẫn

		ItemID10 = 11542,		---Vũ Khí 1
		ItemID11 = 11546,		---Vũ Khí 2
		ItemID12 = 3274,		---Mật Tịch Trung
		ItemID13 = 3275,		---Mật Tịch Trung
		ItemID14 = 3298,		---Mật Tịch Cao
		ItemID15 = 3299,		---Mật Tịch Cao

		ItemID16 = 3708,		---Phi Phong Nam
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7740,		---Y Phục Nữ
		ItemID19 = 8174,		---Nón Nữ
		ItemID20 = 6230,		---Đai Lưng Nữ
		ItemID21 = 7494,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6772,		---Giày Nữ
		ItemID23 = 12627,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3718,		---Phi Phong Nữ

	},
	[11] = {-- Minh Giáo mộc
		ItemID1 = 7747,			---Y Phục
		ItemID2 = 8167,			---Nón
		ItemID3 = 6242,			---Đai Lưng
		ItemID4 = 7487,			---Hộ Uyển
		ItemID5 = 6765,			---Giày
		ItemID6 = 4175,			---Phù
		ItemID7 = 12620,		---Ngọc Bội
		ItemID8 = 11894,		---Hạng Liên
		ItemID9 = 13063,		---Nhẫn

		ItemID10 = 11509,		---Vũ Khí 1
		ItemID11 = 11516,		---Vũ Khí 2
		ItemID12 = 3276,		---Mật Tịch Trung
		ItemID13 = 3277,		---Mật Tịch Trung
		ItemID14 = 3300,		---Mật Tịch Cao
		ItemID15 = 3301,		---Mật Tịch Cao

		ItemID16 = 3648,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7737,		---Y Phục Nữ
		ItemID19 = 8168,		---Nón Nữ
		ItemID20 = 6227,		---Đai Lưng Nữ
		ItemID21 = 7488,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6766,		---Giày Nữ
		ItemID23 = 12621,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3658,		---Phi Phong Nữ
	},
	[12] = {-- Đoàn Thị thủy
		ItemID1 = 7748,			---Y Phục Nam
		ItemID2 = 8169,			---Nón Nam
		ItemID3 = 6223,			---Đai Lưng Nam
		ItemID4 = 7489,			---Hộ Uyển Nam
		ItemID5 = 6767,			---Giày Nam
		ItemID6 = 4176,			---Phù Chung
		ItemID7 = 12622,		---Ngọc Bội Nam
		ItemID8 = 11895,		---Hạng Liên Chung
		ItemID9 = 13064,		---Nhẫn Chung

		ItemID10 = 11526,		---Vũ Khí 1
		ItemID11 = 11520,		---Vũ Khí 2
		ItemID12 = 3278,		---Mật Tịch Trung
		ItemID13 = 3279,		---Mật Tịch Trung
		ItemID14 = 3302,		---Mật Tịch Cao
		ItemID15 = 3303,		---Mật Tịch Cao

		ItemID16 = 3668,		---Phi Phong
		ItemID17 = 3482,		---Ngựa
		ItemID18 = 7738,		---Y Phục Nữ
		ItemID19 = 8170,		---Nón Nữ
		ItemID20 = 6228,		---Đai Lưng Nữ
		ItemID21 = 7490,		---Hộ Uyển Nữ /Thủ Trạc
		ItemID22 = 6768,		---Giày Nữ
		ItemID23 = 12623,		---Ngọc Bội Nữ /Hương Nang
		ItemID24 = 3678,		---Phi Phong Nữ

	},
}
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi item thông qua item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: item - item tương ứng
--		player: Player - item tương ứng
--		selectionID: number - ID chức năng
--		otherParams: Key-Value {number, string} - Danh sách các tham biến khác
-- ****************************************************** --
function TuiTanThu:ItemSet()
end
function TuiTanThu:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	local TotalPrestige = player:GetPrestige()
	if selectionID == 77777 then
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 100031 then
		GUI.CloseDialog(player)
		player:ExportTop()
		return
	end
	if selectionID == 100035 then
		GUI.CloseDialog(player)
		if Player.HasFreeBagSpaces(player, 2) == false then
            TuiTanThu:ShowDialog(item, player, string.format("Bằng hữu cần sắp xếp tối thiểu <color=green>%d ô trống</color> trong túi đồ!",2))
            return false
        end
		local nCheck = Player.GetValueForeverRecore(player, Record10)
		if nCheck ~= 1 then
			Player.SetValueOfForeverRecore(player, Record10, 1)
			Player.AddItemLua(player,2167,1,-1,1)	-- thẻ đổi tên
			player:AddNotification("Nhận Thẻ Đổi Tên thành công")
		end
	end
	if selectionID == 100036 then
		GUI.CloseDialog(player)
		if Player.HasFreeBagSpaces(player, 2) == false then
            TuiTanThu:ShowDialog(item, player, string.format("Bằng hữu cần sắp xếp tối thiểu <color=green>%d ô trống</color> trong túi đồ!",2))
            return false
        end
		local nCheck = Player.GetValueForeverRecore(player, Record11)
		if nCheck ~= 1 then
			Player.SetValueOfForeverRecore(player, Record11, 1)
			Player.AddItemLua(player,2168,1,-1,1)	-- thẻ đổi môn phái
			player:AddNotification("Nhận Thẻ Đổi Phái thành công")
		end
	end
	if selectionID == 30000 then
		if Player.CountItemInBag(player, 2167) <= 0 then
            TuiTanThu:ShowDialog(item, player,"Chức năng này yêu cầu <color=yellow>[Thẻ Đổi Tên]</color>. Khi nào có hãy đến tìm ta.")
            return
        end
		GUI.OpenChangeName(player)
        GUI.CloseDialog(player)
		return
	end

	-- ************************** --
	if selectionID == 39900 then
		dialog:AddText("<color=red>Nhận hỗ trợ Alpha Test Kiếm Thế Mobile</color>")
		dialog:AddSelection(979797, "Nhận <color=green>cấp 119</color>")
		dialog:AddSelection(989898, "Nhận <color=green>mật tịch theo phái</color>")
		dialog:AddSelection(10001, "Nhận <color=green>tiền tệ các loại</color>")
		dialog:AddSelection(10015, "Nhận <color=green>thần thú</color>")
		dialog:AddSelection(10016, "Nhận <color=green>tiền vàng Du Long</color>")
		dialog:AddSelection(10003, "Nhận <color=green>phi phong</color>")
		dialog:AddSelection(10005, "Nhận <color=green>1000 uy danh</color>")
		dialog:AddSelection(10014, "Nhận <color=green>500 vỏ sò</color>")
		dialog:AddSelection(999999, "Nhận <color=green>huyền tinh</color>")
		dialog:AddSelection(10025, "Nhận <color=green>danh vọng</color>")
		dialog:AddSelection(22222, "Nhận <color=green>ngũ hành ấn</color>")
		
		--dialog:AddSelection(999998, "Nhận Kỹ Năng Sống (Cấp 60)")		
		dialog:Show(item, player)
		return
	end
	if selectionID == 22222 then
		GUI.CloseDialog(player)
		if Player.HasFreeBagSpaces(player, 2) == false then
            TuiTanThu:ShowDialog(item, player, string.format("Bằng hữu cần sắp xếp tối thiểu <color=green>%d ô trống</color> trong túi đồ!",2))
            return false
        end
		
		local factions = player:GetFactionID()
		if factions == 1 then
			Player.AddItemLua(player,3864,1,-1,1)
		elseif factions == 2 then
			Player.AddItemLua(player,3865,1,-1,1)
		elseif factions == 3 then
			Player.AddItemLua(player,3866,1,-1,1)
		elseif factions == 4 then
			Player.AddItemLua(player,3867,1,-1,1)
		elseif factions == 5 then
			Player.AddItemLua(player,3868,1,-1,1)
		elseif factions == 6 then
			Player.AddItemLua(player,3869,1,-1,1)
		elseif factions == 7 then
			Player.AddItemLua(player,3870,1,-1,1)
		elseif factions == 8 then
			Player.AddItemLua(player,3871,1,-1,1)
		elseif factions == 9 then
			Player.AddItemLua(player,3872,1,-1,1)
		elseif factions == 10 then
			Player.AddItemLua(player,3873,1,-1,1)
		elseif factions == 11 then
			Player.AddItemLua(player,3874,1,-1,1)
		elseif factions == 12 then
			Player.AddItemLua(player,3875,1,-1,1)
		end
		player:AddNotification("Nhận <color=green>ngũ hành ấn</color> thành công")
		Player.SetValueOfForeverRecore(player, Record7, 1)
		return
	end

	if selectionID == 10025 then	
		GUI.CloseDialog(player)
		Player.AddRetupeValue(player,504,300)   --DV Chúc Phúc
		Player.AddRetupeValue(player,504,300)   --DV Chúc Phúc
		Player.AddRetupeValue(player,504,8000)   --DV Chúc Phúc
		Player.AddRetupeValue(player,504,10000)   --DV Chúc Phúc
		Player.AddRetupeValue(player,504,50000)   --DV Chúc Phúc
		
		Player.AddRetupeValue(player,502,10000)   --Thịnh Hạ 2008
		Player.AddRetupeValue(player,505,10000)   --Thịnh Hạ 2010
		Player.AddRetupeValue(player,502,10000)   --Thịnh Hạ 2008
		Player.AddRetupeValue(player,505,10000)   --Thịnh Hạ 2010

		Player.AddRetupeValue(player,1001,10000)  --DV Đoàn Viên Gia Tộc
		Player.AddRetupeValue(player,1001,10000)  --DV Đoàn Viên Gia Tộc
		Player.AddRetupeValue(player,1001,10000)  --DV Đoàn Viên Gia Tộc

		Player.AddRetupeValue(player,801,1500)   -- Danh Vọng TDLT
		Player.AddRetupeValue(player,801,3000)   -- Danh Vọng TDLT		
		Player.AddRetupeValue(player,801,4500)   -- Danh Vọng TDLT
		Player.AddRetupeValue(player,801,35000)   -- Danh Vọng TDLT
		Player.AddRetupeValue(player,801,70000)   -- Danh Vọng TDLT
		
		Player.AddRetupeValue(player,701,3000)   -- Danh Vọng VLLD
		Player.AddRetupeValue(player,701,6000)   -- Danh Vọng VLLD
		Player.AddRetupeValue(player,701,10000)   -- Danh Vọng VLLD
		Player.AddRetupeValue(player,701,75000)	-- Danh Vọng VLLD
		Player.AddRetupeValue(player,701,150000)-- Danh Vọng VLLD
		
		Player.AddRetupeValue(player,506,10000)   -- Danh Vọng Di Tích Hàn Vũ
		
		Player.AddRetupeValue(player,1201,4800)  -- Liên đấu liên server
		Player.AddRetupeValue(player,1201,9600)  -- Liên đấu liên server
		
		Player.AddRetupeValue(player,1101,4800)  --DV Đại Hội Võ Lâm
		Player.AddRetupeValue(player,1101,9600)  --DV Đại Hội Võ Lâm
		
		Player.AddRetupeValue(player,901,10000)   -- Tần Lăng Quan Phủ
		Player.AddRetupeValue(player,901,10000)   -- Tần Lăng Quan Phủ
		Player.AddRetupeValue(player,901,10000)   -- Tần Lăng Quan Phủ
		
		Player.AddRetupeValue(player,902,20000)	  -- Tần Lăng Phát Khấu Môn
		Player.AddRetupeValue(player,902,30000)   -- Tần Lăng Phát Khấu Môn
		
		Player.AddRetupeValue(player,503,10000)   -- Tiêu Dao Cốc
		Player.AddRetupeValue(player,503,10000)   -- Tiêu Dao Cốc
		Player.AddRetupeValue(player,503,10000)   -- Tiêu Dao Cốc
		
		Player.AddRetupeValue(player,601,10000)  --- VLCT Kim
		Player.AddRetupeValue(player,602,10000)  --- VLCT Mộc
		Player.AddRetupeValue(player,603,10000)  --- VLCT Thủy
		Player.AddRetupeValue(player,604,10000)  --- VLCT Hỏa
		Player.AddRetupeValue(player,605,10000)  --- VLCT Thổ
		
		-- Player.AddRetupeValue(player,201,10000)
		-- Player.AddRetupeValue(player,202,10000)
		-- Player.AddRetupeValue(player,203,10000)

		-- Player.AddRetupeValue(player,300 + player:GetFactionID(),10000)
		-- Player.AddRetupeValue(player,401,10000)
		-- Player.AddRetupeValue(player,402,10000)
		

		-- Player.AddRetupeValue(player,501,10000)
		-- Player.AddRetupeValue(player,503,10000)
		
		
		player:AddNotification("Nhận Danh Vọng Các Loại Thành Công")
		Player.SetValueOfForeverRecore(player, Record6, 1)
		return
	end
	if selectionID == 999999 then
		dialog:AddText("Vui lòng chọn loại bạn cần")
		dialog:AddSelection(9999991, "Nhận huyền tinh cấp 1")
		dialog:AddSelection(9999992, "Nhận huyền tinh cấp 2")
		dialog:AddSelection(9999993, "Nhận huyền tinh cấp 3")
		dialog:AddSelection(9999994, "Nhận huyền tinh cấp 4")
		dialog:AddSelection(9999995, "Nhận huyền tinh cấp 5")
		dialog:AddSelection(9999996, "Nhận huyền tinh cấp 6")
		dialog:AddSelection(9999997, "Nhận huyền tinh cấp 7")
		dialog:AddSelection(9999998, "Nhận huyền tinh cấp 8")
		dialog:AddSelection(9999999, "Nhận huyền tinh cấp 9")
		dialog:AddSelection(99999910, "Nhận huyền tinh cấp 10")
		dialog:AddSelection(99999911, "Nhận huyền tinh cấp 11")
		dialog:AddSelection(99999912, "Nhận huyền tinh cấp 12")
		dialog:Show(item, player)
		return
	end
	if selectionID == 9999991 then
		Player.AddItemLua(player,385,10,-1,1)
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 9999992 then
		Player.AddItemLua(player,386,10,-1,1)
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 9999993 then
		Player.AddItemLua(player,387,10,-1,1)
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 9999994 then
		Player.AddItemLua(player,388,10,-1,1)
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 9999995 then
		Player.AddItemLua(player,389,10,-1,1)
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 9999996 then
		Player.AddItemLua(player,390,10,-1,1)
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 9999997 then
		Player.AddItemLua(player,391,10,-1,1)
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 9999998 then
		Player.AddItemLua(player,392,10,-1,1)
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 9999999 then
		Player.AddItemLua(player,393,10,-1,1)
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 99999910 then
		Player.AddItemLua(player,394,10,-1,1)
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 99999911 then
		Player.AddItemLua(player,395,10,-1,1)
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 99999912 then
		Player.AddItemLua(player,396,10,-1,1)
		GUI.CloseDialog(player)
		return
	end	
	if selectionID == 979797 then
		player:SetLevel(119)

		GUI.CloseDialog(player)
		player:AddNotification("Chúc mừng "..player:GetName().."  nhận hỗ trợ cấp thành công")
		return
	end
	if selectionID == 989898 then
		GUI.CloseDialog(player)
		if Player.HasFreeBagSpaces(player, 8) == false then
            TuiTanThu:ShowDialog(item, player, string.format("Bằng hữu cần sắp xếp tối thiểu <color=green>%d ô trống</color> trong túi đồ!",8))
            return false
        end
		
		local factions = player:GetFactionID()
		local series = Player.GetSeries(player)
		if factions == 1 then
			Player.AddItemLua(player,secretID[1].ItemID1,1,series,1)
			Player.AddItemLua(player,secretID[1].ItemID2,1,series,1)
			Player.AddItemLua(player,secretID[1].ItemID3,1,series,1)
			Player.AddItemLua(player,secretID[1].ItemID4,1,series,1)
			Player.AddItemLua(player,secretID[1].ItemID5,1,series,1)
			Player.AddItemLua(player,secretID[1].ItemID6,1,series,1)
		elseif factions == 2 then
			Player.AddItemLua(player,secretID[2].ItemID1,1,series,1)
			Player.AddItemLua(player,secretID[2].ItemID2,1,series,1)
			Player.AddItemLua(player,secretID[2].ItemID3,1,series,1)
			Player.AddItemLua(player,secretID[2].ItemID4,1,series,1)
			Player.AddItemLua(player,secretID[2].ItemID5,1,series,1)
			Player.AddItemLua(player,secretID[2].ItemID6,1,series,1)
		elseif factions == 3 then
			Player.AddItemLua(player,secretID[3].ItemID1,1,series,1)
			Player.AddItemLua(player,secretID[3].ItemID2,1,series,1)
			Player.AddItemLua(player,secretID[3].ItemID3,1,series,1)
			Player.AddItemLua(player,secretID[3].ItemID4,1,series,1)
			Player.AddItemLua(player,secretID[3].ItemID5,1,series,1)
			Player.AddItemLua(player,secretID[3].ItemID6,1,series,1)
		elseif factions == 4 then
			Player.AddItemLua(player,secretID[4].ItemID1,1,series,1)
			Player.AddItemLua(player,secretID[4].ItemID2,1,series,1)
			Player.AddItemLua(player,secretID[4].ItemID3,1,series,1)
			Player.AddItemLua(player,secretID[4].ItemID4,1,series,1)
			Player.AddItemLua(player,secretID[4].ItemID5,1,series,1)
			Player.AddItemLua(player,secretID[4].ItemID6,1,series,1)
		elseif factions == 5 then
			Player.AddItemLua(player,secretID[5].ItemID1,1,series,1)
			Player.AddItemLua(player,secretID[5].ItemID2,1,series,1)
			Player.AddItemLua(player,secretID[5].ItemID3,1,series,1)
			Player.AddItemLua(player,secretID[5].ItemID4,1,series,1)
			Player.AddItemLua(player,secretID[5].ItemID5,1,series,1)
			Player.AddItemLua(player,secretID[5].ItemID6,1,series,1)
		elseif factions == 6 then
			Player.AddItemLua(player,secretID[6].ItemID1,1,series,1)
			Player.AddItemLua(player,secretID[6].ItemID2,1,series,1)
			Player.AddItemLua(player,secretID[6].ItemID3,1,series,1)
			Player.AddItemLua(player,secretID[6].ItemID4,1,series,1)
			Player.AddItemLua(player,secretID[6].ItemID5,1,series,1)
			Player.AddItemLua(player,secretID[6].ItemID6,1,series,1)
		elseif factions == 7 then
			Player.AddItemLua(player,secretID[7].ItemID1,1,series,1)
			Player.AddItemLua(player,secretID[7].ItemID2,1,series,1)
			Player.AddItemLua(player,secretID[7].ItemID3,1,series,1)
			Player.AddItemLua(player,secretID[7].ItemID4,1,series,1)
			Player.AddItemLua(player,secretID[7].ItemID5,1,series,1)
			Player.AddItemLua(player,secretID[7].ItemID6,1,series,1)
		elseif factions == 8 then
			Player.AddItemLua(player,secretID[8].ItemID1,1,series,1)
			Player.AddItemLua(player,secretID[8].ItemID2,1,series,1)
			Player.AddItemLua(player,secretID[8].ItemID3,1,series,1)
			Player.AddItemLua(player,secretID[8].ItemID4,1,series,1)
			Player.AddItemLua(player,secretID[8].ItemID5,1,series,1)
			Player.AddItemLua(player,secretID[8].ItemID6,1,series,1)
		elseif factions == 9 then
			Player.AddItemLua(player,secretID[9].ItemID1,1,series,1)
			Player.AddItemLua(player,secretID[9].ItemID2,1,series,1)
			Player.AddItemLua(player,secretID[9].ItemID3,1,series,1)
			Player.AddItemLua(player,secretID[9].ItemID4,1,series,1)
			Player.AddItemLua(player,secretID[9].ItemID5,1,series,1)
			Player.AddItemLua(player,secretID[9].ItemID6,1,series,1)
		elseif factions == 10 then
			Player.AddItemLua(player,secretID[10].ItemID1,1,series,1)
			Player.AddItemLua(player,secretID[10].ItemID2,1,series,1)
			Player.AddItemLua(player,secretID[10].ItemID3,1,series,1)
			Player.AddItemLua(player,secretID[10].ItemID4,1,series,1)
			Player.AddItemLua(player,secretID[10].ItemID5,1,series,1)
			Player.AddItemLua(player,secretID[10].ItemID6,1,series,1)
		elseif factions == 11 then
			Player.AddItemLua(player,secretID[11].ItemID1,1,series,1)
			Player.AddItemLua(player,secretID[11].ItemID2,1,series,1)
			Player.AddItemLua(player,secretID[11].ItemID3,1,series,1)
			Player.AddItemLua(player,secretID[11].ItemID4,1,series,1)
			Player.AddItemLua(player,secretID[11].ItemID5,1,series,1)
			Player.AddItemLua(player,secretID[11].ItemID6,1,series,1)
		else--if player:GetFactionID() == 12 then
			Player.AddItemLua(player,secretID[12].ItemID1,1,series,1)
			Player.AddItemLua(player,secretID[12].ItemID2,1,series,1)
			Player.AddItemLua(player,secretID[12].ItemID3,1,series,1)
			Player.AddItemLua(player,secretID[12].ItemID4,1,series,1)
			Player.AddItemLua(player,secretID[12].ItemID5,1,series,1)
			Player.AddItemLua(player,secretID[12].ItemID6,1,series,1)
		end
		player:AddNotification("Nhận Mật Tịch thành công")
		Player.SetValueOfForeverRecore(player, Record8, 1)
		return
	end
	if selectionID == 11111 then
		GUI.CloseDialog(player)
		Player.SetBookLevelAndExp(player, 100, 0)
		player:AddNotification("Nhận thành công Kinh Nghiệm Mật Tịch")
		return
	end
	
	if selectionID == 10001 then
		local str = "";
		if Player.CheckMoney(player,0) < 10000000 then
			Player.AddMoney(player,10000000,0)
			str = str .. "<color=#ffd24d>1000 vạn (bạc khóa)</color>";
		end
		if Player.CheckMoney(player,1) < 10000000 then
			Player.AddMoney(player,10000000,1)
			str = str .. "<color=#ffd24d>1000 vạn (bạc)</color>";
		end
		if Player.CheckMoney(player,2) < 10000000 then
			Player.AddMoney(player,10000000,2)
			str = str .. "<color=#ffd24d>1000 vạn (đồng)</color>";
		end
		if Player.CheckMoney(player,3) < 10000000 then
			Player.AddMoney(player,10000000,3)
			str = str .. "<color=#ffd24d>1000 vạn (đồng khóa)</color>";
		end
		dialog:AddText("Ngươi đã nhận "..str.." thành công")
		dialog:Show(item, player)
		return
	end
	if selectionID == 10015 then
		Player.AddItemLua(player,3459,1,-1,1)--Ma bai--
		Player.AddItemLua(player,3483,1,-1,1)--Hoan Hoan--Hoan Hoan
		Player.AddItemLua(player,3484,1,-1,1)--Hy Hy--Okie
		Player.AddItemLua(player,3485,1,-1,1)--Ho Cat Tuong--Okie
		Player.AddItemLua(player,3500,1,-1,1)--Uc Van--Okie
		Player.AddItemLua(player,3501,1,-1,1)--Tuyet Hon--Okie
		Player.AddItemLua(player,3502,1,-1,1)--Su Gia Truy Phong--Okie
		Player.AddItemLua(player,3508,1,-1,1)--Hoa Ky Lan--Okie

		GUI.CloseDialog(player)
		player:AddNotification(""..player:GetName().."Bạn đã nhận <color=red>thần thú</color> thành công")		
		return
	end
	if selectionID == 10016 then
		Player.AddItemLua(player,1033,10,-1,1)
		Player.AddItemLua(player,1034,10,-1,1)		
		Player.AddItemLua(player,1074,10000,-1,1)

		GUI.CloseDialog(player)
		player:AddNotification(""..player:GetName().." nhận Tiền Vàng Du Long thành công")
		return
	end
	if selectionID == 10003 then
		GUI.CloseDialog(player)
		if Player.HasFreeBagSpaces(player, 12) == false then
            TuiTanThu:ShowDialog(item, player, string.format("Bằng hữu cần sắp xếp tối thiểu <color=green>%d ô trống</color> trong túi đồ!",12))
            return false
        end
		local series = Player.GetSeries(player)
		if series == 1 then
			if player:GetSex()==0 then
				Player.AddItemLua(player,3619,1,-1,1,0,43200)--sieu pham kim nam
				Player.AddItemLua(player,3620,1,-1,1,0,43200)--xuat tran kim nam
				Player.AddItemLua(player,3621,1,-1,1,0,43200)--lang tuyet kim nam
				Player.AddItemLua(player,3622,1,-1,1,0,43200)--kinh the kim nam
				Player.AddItemLua(player,3623,1,-1,1,0,43200)--ngu khong kim nam
				Player.AddItemLua(player,3624,1,-1,1,0,43200)--hon thien kim nam
				Player.AddItemLua(player,3625,1,-1,1,0,43200)--so phuong kim nam
				Player.AddItemLua(player,3626,1,-1,1,0,43200)--tiem long kim nam
				Player.AddItemLua(player,3627,1,-1,1,0,43200)--chi ton kim nam
				Player.AddItemLua(player,3628,1,-1,1,0,43200)--vo song kim nam
			else
				Player.AddItemLua(player,3629,1,-1,1,0,43200)--sieu pham kim nu
				Player.AddItemLua(player,3630,1,-1,1,0,43200)--xuat tran kim nu
				Player.AddItemLua(player,3631,1,-1,1,0,43200)--lang tuyet kim nu
				Player.AddItemLua(player,3632,1,-1,1,0,43200)--kinh the kim nu
				Player.AddItemLua(player,3633,1,-1,1,0,43200)--ngu khong kim nu
				Player.AddItemLua(player,3634,1,-1,1,0,43200)--hon thien kim nu
				Player.AddItemLua(player,3635,1,-1,1,0,43200)--so phuong kim nu
				Player.AddItemLua(player,3636,1,-1,1,0,43200)--tiem long kim nu
				Player.AddItemLua(player,3637,1,-1,1,0,43200)--chi ton kim nu
				Player.AddItemLua(player,3638,1,-1,1,0,43200)--vo song kim nu
			end
		elseif series == 2 then
			if player:GetSex()==0 then
				Player.AddItemLua(player,3639,1,-1,1,0,43200)--sieu pham moc nam
				Player.AddItemLua(player,3640,1,-1,1,0,43200)--xuat tran moc nam
				Player.AddItemLua(player,3641,1,-1,1,0,43200)--lang tuyet moc nam
				Player.AddItemLua(player,3642,1,-1,1,0,43200)--kinh the moc nam
				Player.AddItemLua(player,3643,1,-1,1,0,43200)--ngu khong moc nam
				Player.AddItemLua(player,3644,1,-1,1,0,43200)--hon thien moc nam
				Player.AddItemLua(player,3645,1,-1,1,0,43200)--so phuong moc nam
				Player.AddItemLua(player,3646,1,-1,1,0,43200)--tiem long moc nam
				Player.AddItemLua(player,3647,1,-1,1,0,43200)--chi ton moc nam
				Player.AddItemLua(player,3648,1,-1,1,0,43200)--vo song moc nam
			else
				Player.AddItemLua(player,3649,1,-1,1,0,43200)--sieu pham moc nu
				Player.AddItemLua(player,3650,1,-1,1,0,43200)--xuat tran moc nu
				Player.AddItemLua(player,3651,1,-1,1,0,43200)--lang tuyet moc nu
				Player.AddItemLua(player,3652,1,-1,1,0,43200)--kinh the moc nu
				Player.AddItemLua(player,3653,1,-1,1,0,43200)--ngu khong moc nu
				Player.AddItemLua(player,3654,1,-1,1,0,43200)--hon thien moc nu
				Player.AddItemLua(player,3655,1,-1,1,0,43200)--so phuong moc nu
				Player.AddItemLua(player,3656,1,-1,1,0,43200)--tiem long moc nu
				Player.AddItemLua(player,3657,1,-1,1,0,43200)--chi ton moc nu
				Player.AddItemLua(player,3658,1,-1,1,0,43200)--vo song moc nu
			end
		elseif series == 3 then
			if player:GetSex()==0 then
				Player.AddItemLua(player,3659,1,-1,1,0,43200)--sieu pham thuy nam
				Player.AddItemLua(player,3660,1,-1,1,0,43200)--xuat tran thuy nam
				Player.AddItemLua(player,3661,1,-1,1,0,43200)--lang tuyet thuy nam
				Player.AddItemLua(player,3662,1,-1,1,0,43200)--kinh the thuy nam
				Player.AddItemLua(player,3663,1,-1,1,0,43200)--ngu khong thuy nam
				Player.AddItemLua(player,3664,1,-1,1,0,43200)--hon thien thuy nam
				Player.AddItemLua(player,3665,1,-1,1,0,43200)--so phuong thuy nam
				Player.AddItemLua(player,3666,1,-1,1,0,43200)--tiem long thuy nam
				Player.AddItemLua(player,3667,1,-1,1,0,43200)--chi ton thuy nam
				Player.AddItemLua(player,3668,1,-1,1,0,43200)--vo song thuy nam
			else
				Player.AddItemLua(player,3669,1,-1,1,0,43200)--sieu pham thuy nu
				Player.AddItemLua(player,3670,1,-1,1,0,43200)--xuat tran thuy nu
				Player.AddItemLua(player,3671,1,-1,1,0,43200)--lang tuyet thuy nu
				Player.AddItemLua(player,3672,1,-1,1,0,43200)--kinh the thuy nu
				Player.AddItemLua(player,3673,1,-1,1,0,43200)--ngu khong thuy nu
				Player.AddItemLua(player,3674,1,-1,1,0,43200)--hon thien thuy nu
				Player.AddItemLua(player,3675,1,-1,1,0,43200)--so phuong thuy nu
				Player.AddItemLua(player,3676,1,-1,1,0,43200)--tiem long thuy nu
				Player.AddItemLua(player,3677,1,-1,1,0,43200)--chi ton thuy nu
				Player.AddItemLua(player,3678,1,-1,1,0,43200)--vo song thuy nu
			end
		elseif series == 4 then
			if player:GetSex()==0 then
				Player.AddItemLua(player,3679,1,-1,1,0,43200)--sieu pham hoa nam
				Player.AddItemLua(player,3680,1,-1,1,0,43200)--xuat tran hoa nam
				Player.AddItemLua(player,3681,1,-1,1,0,43200)--lang tuyet hoa nam
				Player.AddItemLua(player,3682,1,-1,1,0,43200)--kinh the hoa nam
				Player.AddItemLua(player,3683,1,-1,1,0,43200)--ngu khong hoa nam
				Player.AddItemLua(player,3684,1,-1,1,0,43200)--hon thien hoa nam
				Player.AddItemLua(player,3685,1,-1,1,0,43200)--so phuong hoa nam
				Player.AddItemLua(player,3686,1,-1,1,0,43200)--tiem long hoa nam
				Player.AddItemLua(player,3687,1,-1,1,0,43200)--chi ton hoa nam
				Player.AddItemLua(player,3688,1,-1,1,0,43200)--vo song hoa nam
			else
				Player.AddItemLua(player,3689,1,-1,1,0,43200)--sieu pham hoa nu
				Player.AddItemLua(player,3690,1,-1,1,0,43200)--xuat tran hoa nu
				Player.AddItemLua(player,3691,1,-1,1,0,43200)--lang tuyet hoa nu
				Player.AddItemLua(player,3692,1,-1,1,0,43200)--kinh the hoa nu
				Player.AddItemLua(player,3693,1,-1,1,0,43200)--ngu khong hoa nu
				Player.AddItemLua(player,3694,1,-1,1,0,43200)--hon thien hoa nu
				Player.AddItemLua(player,3695,1,-1,1,0,43200)--so phuong hoa nu
				Player.AddItemLua(player,3696,1,-1,1,0,43200)--tiem long hoa nu
				Player.AddItemLua(player,3697,1,-1,1,0,43200)--chi ton hoa nu
				Player.AddItemLua(player,3698,1,-1,1,0,43200)--vo song hoa nu
			end
		else
			if player:GetSex()==0 then
				Player.AddItemLua(player,3699,1,-1,1,0,43200)--sieu pham tho nam
				Player.AddItemLua(player,3700,1,-1,1,0,43200)--xuat tran tho nam
				Player.AddItemLua(player,3701,1,-1,1,0,43200)--lang tuyet tho nam
				Player.AddItemLua(player,3702,1,-1,1,0,43200)--kinh the tho nam
				Player.AddItemLua(player,3703,1,-1,1,0,43200)--ngu khong tho nam
				Player.AddItemLua(player,3704,1,-1,1,0,43200)--hon thien tho nam
				Player.AddItemLua(player,3705,1,-1,1,0,43200)--so phuong tho nam
				Player.AddItemLua(player,3706,1,-1,1,0,43200)--tiem long tho nam
				Player.AddItemLua(player,3707,1,-1,1,0,43200)--chi ton tho nam
				Player.AddItemLua(player,3708,1,-1,1,0,43200)--vo song tho nam
			else
				Player.AddItemLua(player,3709,1,-1,1,0,43200)--sieu pham tho nu
				Player.AddItemLua(player,3700,1,-1,1,0,43200)--xuat tran tho nu
				Player.AddItemLua(player,3711,1,-1,1,0,43200)--lang tuyet tho nu
				Player.AddItemLua(player,3712,1,-1,1,0,43200)--kinh the tho nu
				Player.AddItemLua(player,3713,1,-1,1,0,43200)--ngu khong tho nu
				Player.AddItemLua(player,3714,1,-1,1,0,43200)--hon thien tho nu
				Player.AddItemLua(player,3715,1,-1,1,0,43200)--so phuong tho nu
				Player.AddItemLua(player,3716,1,-1,1,0,43200)--tiem long tho nu
				Player.AddItemLua(player,3717,1,-1,1,0,43200)--chi ton tho nu
				Player.AddItemLua(player,3718,1,-1,1,0,43200)--vo song tho nu
			end
		end
		player:AddNotification("Nhận Phi Phong thành công")
		Player.SetValueOfForeverRecore(player, Record9, 1)
		return
	end
	if selectionID == 10005 then
		GUI.CloseDialog(player)

		player:SetPrestige(1000)
		player:AddNotification(""..player:GetName().."Bạn đã nhận <color=red>1000 uy danh</color> thành công")		
		return
	end
	if selectionID == 10014 then
		GUI.CloseDialog(player)
		Player.AddItemLua(player,746,500,-1,1)
		--Player.AddItemLua(player,337,50,-1,1)
		--Player.AddItemLua(player,796,500,-1,1)
		--Player.AddItemLua(player,3344,1,-1,1)		
		
		-- Player.AddItemLua(player,3345,1,-1,1)
		-- Player.AddItemLua(player,3346,1,-1,1)
		-- Player.AddItemLua(player,3347,1,-1,1)
		-- Player.AddItemLua(player,3348,1,-1,1)
		-- Player.AddItemLua(player,337,50,-1,1)
		-- Player.AddItemLua(player,337,50,-1,1)
		
		player:AddNotification("Nhận 500 Vỏ Sò Vàng Thành Công")
		return
	end
	if selectionID == 200031 then
		dialog:AddText("Xin chào "..player:GetName().."\nĐua top <color=red>Cấp độ</color> từ <color=green>20h00 17/11/2023 đến 23h00 24/12/2023</color>\nĐua top <color=red>Tài Phú</color> từ <color=green>20h00 17/11/2023 đến 23h00 31/12/2023</color> !")
		local nCheck4 = Player.GetValueForeverRecore(player, Record4)
		if nCheck4 <= 0 then
			dialog:AddSelection(200032, "Nhận thưởng Đua Top Cấp Độ")
		end
		local nCheck5 = Player.GetValueForeverRecore(player, Record5)
		if nCheck5 <= 0 then
			dialog:AddSelection(200033, "Nhận thưởng Đua Top Tài Phú")
		end
		dialog:AddSelection(100032, "Xem thưởng Top Tài Phú")
		dialog:AddSelection(100034, "Xem thưởng Top Cấp Độ")
		dialog:AddSelection(77777, "Kết thúc đối thoại")
		dialog:Show(item, player)
		return;
	end
	
	if selectionID == 200032 then
		GUI.CloseDialog(player)
		if Player.HasFreeBagSpaces(player, 10) == false then
            TuiTanThu:ShowDialog(item, player, string.format("Bằng hữu cần sắp xếp tối thiểu <color=green>%d ô trống</color> trong túi đồ!",10))
            return
        end
		local record = Player.GetValueForeverRecore(player, Record4)
        if record >= 1 then
            TuiTanThu:ShowDialog(item, player, "Bạn đã nhận đua top cấp độ này rồi.")
            return
        end
		local rank = player:GetTopLevel()
		if rank <=0 then
			TuiTanThu:ShowDialog(item, player, "Người chơi "..player:GetName().." không trong danh sách nhận thưởng")
			return
		end
		---------------------------
		if rank == 1 then
			Player.SetValueOfForeverRecore(player, Record4, 1)
			Player.AddItemLua(player,583,20,-1,1)-- Ngu Hon Thach Khoa 1000c
			Player.AddItemLua(player,15001,20,-1,1)-- Phieu Dong Khoa 1v
			Player.AddItemLua(player,15008,2,-1,1)-- Phieu Giam Gia 30%
			Player.AddItemLua(player,403,20,-1,1)-- Thoi vang (dai)
			player:AddNotification("Chúc mừng "..player:GetName().."  nhận thưởng Top Cấp Độ thành công")
		elseif rank == 2 then
			Player.SetValueOfForeverRecore(player, Record4, 1)
			Player.AddItemLua(player,583,15,-1,1)-- Ngu Hon Thach Khoa 1000c
			Player.AddItemLua(player,15001,15,-1,1)-- Phieu Dong Khoa 1v
			Player.AddItemLua(player,15008,1,-1,1)-- Phieu Giam Gia 30%
			Player.AddItemLua(player,403,15,-1,1)-- Thoi vang (dai)
			player:AddNotification("Chúc mừng "..player:GetName().."  nhận thưởng Top Cấp Độ thành công")
		elseif rank == 3 then
			Player.SetValueOfForeverRecore(player, Record4, 1)
			Player.AddItemLua(player,583,10,-1,1)-- Ngu Hon Thach Khoa 1000c
			Player.AddItemLua(player,15001,10,-1,1)-- Phieu Dong Khoa 1v
			Player.AddItemLua(player,15007,2,-1,1)-- Phieu Giam Gia 20%
			Player.AddItemLua(player,403,15,-1,1)-- Thoi vang (dai)
			player:AddNotification("Chúc mừng "..player:GetName().."  nhận thưởng Top Cấp Độ thành công")
		elseif rank <= 10 then
			Player.SetValueOfForeverRecore(player, Record4, 1)
			Player.AddItemLua(player,583,5,-1,1)-- Ngu Hon Thach Khoa 1000c
			Player.AddItemLua(player,15001,5,-1,1)-- Phieu Dong Khoa 1v
			Player.AddItemLua(player,15007,2,-1,1)-- Phieu Giam Gia 20%
			Player.AddItemLua(player,403,10,-1,1)-- Thoi vang (dai)
			player:AddNotification("Chúc mừng "..player:GetName().."  nhận thưởng Top Cấp Độ thành công")
		elseif rank <= 20 then
			Player.SetValueOfForeverRecore(player, Record4, 1)
			Player.AddItemLua(player,15001,2,-1,1)-- Phieu Dong Khoa 1v
			Player.AddItemLua(player,15006,1,-1,1)-- Phieu Giam Gia 20%
			Player.AddItemLua(player,403,5,-1,1)-- Thoi vang (dai)
			player:AddNotification("Chúc mừng "..player:GetName().."  nhận thưởng Top Cấp Độ thành công")
		elseif rank <= 30 then
			Player.SetValueOfForeverRecore(player, Record4, 1)
			Player.AddItemLua(player,15001,2,-1,1)-- Phieu Dong Khoa 1v
			Player.AddItemLua(player,15006,1,-1,1)-- Phieu Giam Gia 20%
			Player.AddItemLua(player,402,10,-1,1)-- Thoi vang (dai)
			player:AddNotification("Chúc mừng "..player:GetName().."  nhận thưởng Top Cấp Độ thành công")
		else
			player:AddNotification("Người chơi "..player:GetName().." không trong danh sách nhận thưởng Top Cấp Độ")
		end
		return
	end
	if selectionID == 200033 then
		GUI.CloseDialog(player)
		if Player.HasFreeBagSpaces(player, 10) == false then
            TuiTanThu:ShowDialog(item, player, string.format("Bằng hữu cần sắp xếp tối thiểu <color=green>%d ô trống</color> trong túi đồ!",10))
            return
        end
		local record = Player.GetValueForeverRecore(player, Record5)
        if record >= 1 then
            TuiTanThu:ShowDialog(item, player, "Bạn đã nhận đua top tài phú này rồi.")
            return
        end
		local rank = player:GetTopMoney()
		if rank <=0 then
			TuiTanThu:ShowDialog(item, player, "Người chơi "..player:GetName().." không trong danh sách nhận thưởng")
			return
		end
		---------------------------
		if rank == 1 then
			Player.SetValueOfForeverRecore(player, Record5, 1)
			player:SetTitle(1)
			Player.AddItemLua(player,3508,1,-1,1)--Hoa Ky Lan--Okie
			Player.AddItemLua(player,492,5,-1,1)-- Tay Tuy Kinh (so)
			Player.AddItemLua(player,490,5,-1,1)-- Vo Lam Mat Tich (so)
			Player.AddItemLua(player,15001,30,-1,1)-- Phieu Dong Khoa 1v
			Player.AddItemLua(player,403,10,-1,1)-- Thoi vang (dai)
			Player.AddItemLua(player,1034,20,-1,1)-- Dinh vang Du Long
			player:AddNotification("Chúc mừng "..player:GetName().."  nhận thưởng Top Tài Phú thành công")
		elseif rank == 2 then
			Player.SetValueOfForeverRecore(player, Record5, 1)
			player:SetTitle(2)
			Player.AddItemLua(player,3500,1,-1,1)--Uc Van--Okie
			Player.AddItemLua(player,492,5,-1,1)-- Tay Tuy Kinh (so)
			Player.AddItemLua(player,490,5,-1,1)-- Vo Lam Mat Tich (so)
			Player.AddItemLua(player,15001,20,-1,1)-- Phieu Dong Khoa 1v
			Player.AddItemLua(player,403,6,-1,1)-- Thoi vang (dai)
			Player.AddItemLua(player,1034,10,-1,1)-- Dinh vang Du Long
			player:AddNotification("Chúc mừng "..player:GetName().."  nhận thưởng Top Tài Phú thành công")
		elseif rank == 3 then
			Player.SetValueOfForeverRecore(player, Record5, 1)
			player:SetTitle(3)
			Player.AddItemLua(player,3502,1,-1,1)--Su Gia Truy Phong--Okie
			Player.AddItemLua(player,492,5,-1,1)-- Tay Tuy Kinh (so)
			Player.AddItemLua(player,490,5,-1,1)-- Vo Lam Mat Tich (so)
			Player.AddItemLua(player,15001,15,-1,1)-- Phieu Dong Khoa 1v
			Player.AddItemLua(player,403,3,-1,1)-- Thoi vang (dai)
			Player.AddItemLua(player,1034,4,-1,1)-- Dinh vang Du Long
			player:AddNotification("Chúc mừng "..player:GetName().."  nhận thưởng Top Tài Phú thành công")
		elseif rank <= 10 then
			Player.SetValueOfForeverRecore(player, Record5, 1)
			Player.AddItemLua(player,3487,1,-1,1)--Ma bai Lang Thien
			Player.AddItemLua(player,492,4,-1,1)-- Tay Tuy Kinh (so)
			Player.AddItemLua(player,490,4,-1,1)-- Vo Lam Mat Tich (so)
			Player.AddItemLua(player,15001,10,-1,1)-- Phieu Dong Khoa 1v
			Player.AddItemLua(player,403,2,-1,1)-- Thoi vang (dai)
			Player.AddItemLua(player,15000,10,-1,1)-- Phieu Bac Khoa 1v
			player:AddNotification("Chúc mừng "..player:GetName().."  nhận thưởng Top Tài Phú thành công")
		elseif rank <= 20 then
			Player.SetValueOfForeverRecore(player, Record5, 1)
			Player.AddItemLua(player,3486,1,-1,1)--Ma bai Truc Nhat
			Player.AddItemLua(player,492,2,-1,1)-- Tay Tuy Kinh (so)
			Player.AddItemLua(player,490,2,-1,1)-- Vo Lam Mat Tich (so)
			Player.AddItemLua(player,15001,7,-1,1)-- Phieu Dong Khoa 1v
			Player.AddItemLua(player,403,2,-1,1)-- Thoi vang (dai)
			Player.AddItemLua(player,15000,10,-1,1)-- Phieu Bac Khoa 1v
			player:AddNotification("Chúc mừng "..player:GetName().."  nhận thưởng Top Tài Phú thành công")
		elseif rank <= 30 then
			Player.SetValueOfForeverRecore(player, Record5, 1)
			Player.AddItemLua(player,15001,5,-1,1)-- Phieu Dong Khoa 1v
			Player.AddItemLua(player,15000,10,-1,1)-- Phieu Bac Khoa 1v
			player:AddNotification("Chúc mừng "..player:GetName().."  nhận thưởng Top Tài Phú thành công")
		elseif rank <= 100 then
			Player.SetValueOfForeverRecore(player, Record5, 1)
			Player.AddItemLua(player,15001,2,-1,1)-- Phieu Dong Khoa 1v
			Player.AddItemLua(player,15000,10,-1,1)-- Phieu Bac Khoa 1v
			player:AddNotification("Chúc mừng "..player:GetName().."  nhận thưởng Top Tài Phú thành công")
		else
			player:AddNotification("Người chơi "..player:GetName().." không trong danh sách nhận thưởng Top Tài Phú")
		end
	end
	if selectionID == 100034 then
		dialog:AddText("<color=green>Đại hiệp muốn xem phần thưởng cấp độ hạng mấy ?</color>")
		dialog:AddSelection(1000341, "Thưởng hạng 1")
		dialog:AddSelection(1000342, "Thưởng hạng 2")
		dialog:AddSelection(1000343, "Thưởng hạng 3")
		dialog:AddSelection(1000344, "Thưởng hạng 4-10")
		dialog:AddSelection(1000345, "Thưởng hạng 11-20")
		dialog:AddSelection(1000346, "Thưởng hạng 21-30")
		dialog:Show(item, player)
		return;
	end
	if selectionID == 1000341 then
		dialog:AddText("<color=yellow>Phần thưởng Hạng 1 top Cấp Độ gồm:</color>\n<color=green>20 Ngũ Hành Hồn Thạch Khóa (1000)\n20 Phiếu Đồng Khóa 1v\n2 Phiếu Giảm Giá 30%\n20 Thỏi Vàng (đại)</color>")
		dialog:AddSelection(100034, "Xem thưởng Top Cấp Độ")
		dialog:Show(item, player)
		return;
	end
	if selectionID == 1000342 then
		dialog:AddText("<color=yellow>Phần thưởng Hạng 2 top Cấp Độ gồm:</color>\n<color=green>15 Ngũ Hành Hồn Thạch Khóa (1000)\n15 Phiếu Đồng Khóa 1v\n1 Phiếu Giảm Giá 30%\n15 Thỏi Vàng (đại)</color>")
		dialog:AddSelection(100034, "Xem thưởng Top Cấp Độ")
		dialog:Show(item, player)
		return;
	end
	if selectionID == 1000343 then
		dialog:AddText("<color=yellow>Phần thưởng Hạng 3 top Cấp Độ gồm:</color>\n<color=green>10 Ngũ Hành Hồn Thạch Khóa (1000)\n10 Phiếu Đồng Khóa 1v\n2 Phiếu Giảm Giá 20%\n15 Thỏi Vàng (đại)</color>")
		dialog:AddSelection(100034, "Xem thưởng Top Cấp Độ")
		dialog:Show(item, player)
		return;
	end
	if selectionID == 1000344 then
		dialog:AddText("<color=yellow>Phần thưởng Hạng 4-10 top Cấp Độ gồm:</color>\n<color=green>5 Ngũ Hành Hồn Thạch Khóa (1000)\n5 Phiếu Đồng Khóa 1v\n2 Phiếu Giảm Giá 10%\n10 Thỏi Vàng (đại)</color>")
		dialog:AddSelection(100034, "Xem thưởng Top Cấp Độ")
		dialog:Show(item, player)
		return;
	end
	if selectionID == 1000345 then
		dialog:AddText("<color=yellow>Phần thưởng Hạng 11-20 top Cấp Độ gồm:</color>\n<color=green>2 Phiếu Đồng Khóa 1v\n1 Phiếu Giảm Giá 10%\n5 Thỏi Vàng (đại)</color>")
		dialog:AddSelection(100034, "Xem thưởng Top Cấp Độ")
		dialog:Show(item, player)
		return;
	end
	if selectionID == 1000346 then
		dialog:AddText("<color=yellow>Phần thưởng Hạng 11-20 top Cấp Độ gồm:</color>\n<color=green>2 Phiếu Đồng Khóa 1v\n1 Phiếu Giảm Giá 10%\n10 Thỏi Vàng</color>")
		dialog:AddSelection(100034, "Xem thưởng Top Cấp Độ")
		dialog:Show(item, player)
		return;
	end
	if selectionID == 100032 then
		dialog:AddText("<color=green>Đại hiệp muốn xem phần thưởng tài phú hạng mấy ?</color>")
		dialog:AddSelection(1000321, "Thưởng hạng 1")
		dialog:AddSelection(1000322, "Thưởng hạng 2")
		dialog:AddSelection(1000323, "Thưởng hạng 3")
		dialog:AddSelection(1000324, "Thưởng hạng 4-10")
		dialog:AddSelection(1000325, "Thưởng hạng 11-20")
		dialog:AddSelection(1000326, "Thưởng hạng 21-30")
		dialog:AddSelection(1000327, "Thưởng hạng 40-100")
		dialog:Show(item, player)
		return;
	end
	if selectionID == 1000321 then
		dialog:AddText("<color=yellow>Phần thưởng Hạng 1 top Tài Phú gồm:</color>\n<color=red>Danh hiệu vĩnh viễn: Minh Chủ Võ Lâm</color>\n<color=green>1 thú cưỡi Hỏa Kỳ Lân\n5 Tẩy Tủy Kinh (Sơ)\n5 Võ Lâm Mật Tịch (Sơ)\n30 Phiếu Đồng Khóa 1v\n10 Thỏi Vàng (đại)\n20 Đỉnh Vàng Du Long</color>")
		dialog:AddSelection(100032, "Xem thưởng Top Tài Phú")
		dialog:Show(item, player)
		return;
	end
	if selectionID == 1000322 then
		dialog:AddText("<color=yellow>Phần thưởng Hạng 2 top Tài Phú gồm:</color>\n<color=red>Danh hiệu vĩnh viễn: Xưng Bá Một Phương</color>\n<color=green>1 thú cưỡi Ức Vân\n5 Tẩy Tủy Kinh (Sơ)\n5 Võ Lâm Mật Tịch (Sơ)\n20 Phiếu Đồng Khóa 1v\n6 Thỏi Vàng (đại)\n10 Đỉnh Vàng Du Long</color>")
		dialog:AddSelection(100032, "Xem thưởng Top Tài Phú")
		dialog:Show(item, player)
		return;
	end
	if selectionID == 1000323 then
		dialog:AddText("<color=yellow>Phần thưởng Hạng 3 top Tài Phú gồm:</color>\n<color=red>Danh hiệu vĩnh viễn: Võ Công Cái Thế</color>\n<color=green>1 thú cưỡi Sứ Giả Truy Phong\n5 Tẩy Tủy Kinh (Sơ)\n5 Võ Lâm Mật Tịch (Sơ)\n15 Phiếu Đồng Khóa 1v\n3 Thỏi Vàng (đại)\n4 Đỉnh Vàng Du Long</color>")
		dialog:AddSelection(100032, "Xem thưởng Top Tài Phú")
		dialog:Show(item, player)
		return;
	end
	if selectionID == 1000324 then
		dialog:AddText("<color=yellow>Phần thưởng Hạng 4-10 top Tài Phú gồm:</color>\n<color=green>1 thú cưỡi Mã Bài (Lăng Thiên)\n4 Tẩy Tủy Kinh (Sơ)\n4 Võ Lâm Mật Tịch (Sơ)\n10 Phiếu Đồng Khóa 1v\n2 Thỏi Vàng (đại)\n10 Phiếu Bạc Khóa 1v</color>")
		dialog:AddSelection(100032, "Xem thưởng Top Tài Phú")
		dialog:Show(item, player)
		return;
	end
	if selectionID == 1000325 then
		dialog:AddText("<color=yellow>Phần thưởng Hạng 11-20 top Tài Phú gồm:</color>\n<color=green>1 thú cưỡi Mã Bài (Trục Nhật)\n2 Tẩy Tủy Kinh (Sơ)\n2 Võ Lâm Mật Tịch (Sơ)\n7 Phiếu Đồng Khóa 1v\n2 Thỏi Vàng (đại)\n10 Phiếu Bạc Khóa 1v</color>")
		dialog:AddSelection(100032, "Xem thưởng Top Tài Phú")
		dialog:Show(item, player)
		return;
	end
	if selectionID == 1000326 then
		dialog:AddText("<color=yellow>Phần thưởng Hạng 21-30 top Tài Phú gồm:</color>\n<color=green>5 Phiếu Đồng Khóa 1v\n10 Phiếu Bạc Khóa 1v</color>")
		dialog:AddSelection(100032, "Xem thưởng Top Tài Phú")
		dialog:Show(item, player)
		return;
	end
	if selectionID == 1000327 then
		dialog:AddText("<color=yellow>Phần thưởng Hạng 21-30 top Tài Phú gồm:</color>\n<color=green>2 Phiếu Đồng Khóa 1v\n10 Phiếu Bạc Khóa 1v</color>")
		dialog:AddSelection(100032, "Xem thưởng Top Tài Phú")
		dialog:Show(item, player)
		return;
	end
	if selectionID == 39901 then
		--local record2 = Player.GetValueForeverRecore(player, Record2)
		--local record3 = Player.GetValueForeverRecore(player, Record3)
		GUI.CloseDialog(player)
		--	dialog:AddText("<color=green>Hỗ Trợ Tân Thủ KT 2009 Mobile</color>")
		--	if player:GetLevel() < 80 then
		--		dialog:AddSelection(11111, "Nhận <color=green>Nhận hỗ trợ cấp độ Tân Thủ </color>")
		--	end
		--	if Record3 ~= 1  then
		--		dialog:AddSelection(17023, "Nhận Set Đồ Theo Hệ </color>")
		--	end
			-- dialog:AddSelection(999998, "Nhận Kỹ Năng Sống (Cấp 60)")
		--	dialog:AddSelection(77777, "Ta sẽ quay lại sau !!!")
			-- dialog:AddSelection(10023, "Nhận Set Đồ Theo Hệ </color>")
		--	dialog:Show(item, player)
		player:SetLevel(119)		
		Player.AddItemLua(player,3484,1,-1,1)--Hy Hy--Okie
		Player.AddItemLua(player,739,1,-1,1)--Luc Thao Tap Chu
		Player.AddItemLua(player,555,1,-1,1)--Vo Han Truyen Tong Phu
		Player.AddItemLua(player,583,1,-1,1)--Ruong Hon Thach
		Player.AddItemLua(player,344,1,-1,1)--Can Khon Phu (10)
		Player.AddItemLua(player,538,1,-1,1)--Cuu Chuyen Tuc Menh Hoan
		Player.AddItemLua(player,336,1,-1,1)--Lenh bai Tay Tuy Dao
		Player.AddItemLua(player,348,5,-1,1)--Tinh Khi Tan
		Player.AddItemLua(player,351,5,-1,1)--Hoat Khi Tan
		Player.AddItemLua(player,15000,10,-1,1)--Phieu Bac Khoa 1 van
		Player.AddItemLua(player,15001,1,-1,1)--Phieu Dong Khoa 1 van
		Player.AddItemLua(player,2167,1,-1,1)--The Doi Ten
		Player.AddMoney(player,5000000,0)-- 500v bạc khoa
		Player.AddMoney(player,5000,1) --5000 bac
		
		Player.AddItemLua(player,132,999,-1,1)--Ngoc Truc Mai Hoa
		Player.AddItemLua(player,174,999,-1,1)--Ngu Hoa Ngoc Lo Hoan
		Player.AddItemLua(player,215,1,-1,1)--Tu Luyen Chau

		player:AddNotification(""..player:GetName().."  Nhận Hỗ Trợ Tân Thủ Thành Công")
		Player.SetValueOfForeverRecore(player, Record2, 1)
		return;
	end
	if selectionID == 39902 then
		if player:GetFactionID()==0 then
			dialog:AddText(""..player:GetName()..": Bạn chưa gia nhập phái ,hãy gia nhập <color=red> môn phái</color> rồi quay lại <color=red> nhận Nhận Hỗ Trợ Đồ Tân Thủ.</color>")
			dialog:Show(item, player)
		elseif player:GetFactionID()~=0 then
			GUI.CloseDialog(player)
			if player:GetFactionID()==1 then
				if player:GetSex()==0 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID14,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID15,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==2 then
				if player:GetSex()==0 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID14,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID15,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==3 then
				if player:GetSex()==0 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)				
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==4 then
				if player:GetSex()==0 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID14,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID15,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					-- TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					-- TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID14,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID15,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==5 then
				if player:GetSex()==0 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==6 then
				if player:GetSex()==0 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==7 then
				if player:GetSex()==0 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==8 then
				if player:GetSex()==0 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==9 then
				if player:GetSex()==0 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==10 then
				if player:GetSex()==0 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==11 then
				if player:GetSex()==0 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			else--if player:GetFactionID()==12 then
				if player:GetSex()==0 then
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1,5)
					Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1,5)
					--Player.AddItemLua(player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1,5)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end			
			end
		else
			dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
			dialog:Show(item, player)
		end
		return
	end	
	-- ************************** --	
	if selectionID == 999998 then
	 for i=1,10 do 
		Player.SaveLifeSkillLevel(player,i, 60);
	 end
	-- ************************** SetLifeSkillParam(lifeSkillID, level, exp)player.RoleID--
	player:AddNotification(""..player:GetName().."Bạn đã nhận cấp 50 cho tất cả </color=red>Kỹ Năng Sống </color>thành công")
	GUI.CloseDialog(player)
	end
	if selectionID == 10012 then--TuiTanThu:SetBelonging(item, player, ItemID,Number,Series,LockStatus) set do +12
		if player:GetFactionID()==0 then
			dialog:AddText(""..player:GetName()..": Bạn chưa gia nhập phái ,hãy gia nhập <color=red> môn phái</color> rồi quay lại <color=red> nhận Nhận Mật Tịch (cao).</color>")
			dialog:Show(item, player)
		elseif player:GetFactionID()~=0 then
			if player:GetFactionID()==1 then  ---- Thiếu Lâm
				if player:GetSex()==0 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end			
			elseif player:GetFactionID()==2 then
				if player:GetSex()==0 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==3 then
				if player:GetSex()==0 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==4 then
				if player:GetSex()==0 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==5 then
				if player:GetSex()==0 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==6 then
				if player:GetSex()==0 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==7 then
				if player:GetSex()==0 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==8 then
				if player:GetSex()==0 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==9 then
				if player:GetSex()==0 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==10 then
				if player:GetSex()==0 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==11 then
				if player:GetSex()==0 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==12 then
				if player:GetSex()==0 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetHKMP16(item,player,SetBelongingsHK[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetBelongingsHK[player:GetFactionID()].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end			
			end
		else
			dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
			dialog:Show(item, player)
		end
	end	
	
	----------------------Set HKMP ---------------------------------	
	if selectionID == 30001 then
		-- Mở khung xóa vật phẩm
		GUI.OpenRemoveItems(player)
		
		-- Đóng khung
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 30002 then
		-- Mở khung ghép vật phẩm
		GUI.OpenMergeItems(player)
		
		-- Đóng khung
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 30003 then
		if player:NpcClick(2993) == 1 then		
			GUI.CloseDialog(player)
		else
			dialog:AddText("Shop danh vọng chỉ mở ở Thạch Cổ Trấn !")
			dialog:Show(item, player)
		end
		return
	end
		
	if selectionID == 10006 then
		player:ResetCopySceneEnterTimes(11111)
		player:ResetCopySceneEnterTimes(11112)
		player:ResetCopySceneEnterTimes(100000)
		player:ResetCopySceneEnterTimes(100001)
		player:ResetCopySceneEnterTimes(100002)
		player:ResetCopySceneEnterTimes(100003)
		player:AddNotification(""..player:GetName().."Mới số lượt đi phụ bản trong ngày thành công")
		GUI.CloseDialog(player)
	end
	if selectionID == 10007 then
	
		-- Player.AddItemLua(player,590,5,-1,1)
		
		Player.AddItemLua(player,3332,1,-1,1) -- Quan Ấn Hoàng Đế Hệ Kim
		Player.AddItemLua(player,3340,1,-1,1) -- Quan Ấn Hoàng Đế Hệ Mộc
		Player.AddItemLua(player,3348,1,-1,1) -- Quan Ấn Hoàng Đế Hệ Thủy
		Player.AddItemLua(player,3356,1,-1,1) -- Quan Ấn Hoàng Đế Hệ Hỏa
		Player.AddItemLua(player,3364,1,-1,1) -- Quan Ấn Hoàng Đế Hệ Thổ
		player:AddNotification(""..player:GetName().."Bạn đã nhận </color=red>5 Quan Ấn Hoàng Đế 5 Hệ </color>thành công")
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 10008 then
		Player.AddItemLua(player,819,500,-1,1)
		player:AddNotification(""..player:GetName().."Bạn đã nhận thành công 500 Hòa thị bích")
		GUI.CloseDialog(player)				 
		return
	end
	if selectionID == 10009 then
		Player.AddItemLua(player,1026,500,-1,1)
		Player.AddItemLua(player,11213,1,-1,1,16)
		player:AddNotification(""..player:GetName().."Bạn đã nhận </color=red>500 Chiến Thư-Mật Thất Du Long </color>thành công")
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 10010 then
		Player.AddItemLua(player,338,10,-1,1)
		player:AddNotification(""..player:GetName().."Bạn đã nhận </color=red>10 lệnh bài Thi đấu môn phái(sơ) </color>thành công")
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 10011 then
		player:ChangeScene(1616, 7545, 4454)
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 10212 then
		player:SetLevel(89)
		player:AddNotification("Thiết lập cấp độ 89 thành công")
		GUI.CloseDialog(player)
	end
	if selectionID == 10013 then
		Player.OpenShop(item, player, 226)
		GUI.CloseDialog(player)
		
	end
	if selectionID == 10024 then
		player:SetPrayTimes(100)
		player:AddNotification("nhận 100 lượt quay chúc chúc phúc thành công !")
		GUI.CloseDialog(player)
	end	
	if selectionID == 100020 then
		Player.AddCurGatherPoint(player,10000)
		Player.AddMakePoint(player,10000)
		player:AddNotification(""..player:GetName().."Nhận tinh lực hoạt lực thành công")
		GUI.CloseDialog(player)
	end
	if selectionID == 100030 then
		if Player.HasFreeBagSpaces(player, 5) == false then
			GUI.CloseDialog(player)
            TuiTanThu:ShowDialog(item, player, string.format("Bằng hữu cần sắp xếp tối thiểu <color=green>%d ô trống</color> trong túi đồ!",5))
            return false
        end
		GUI.OpenUI(player, "UIGiftCode")
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 10002 then
		if player:GetFactionID()==0 then
			dialog:AddText(""..player:GetName()..": Bạn chưa gia nhập phái ,hãy gia nhập <color=red> môn phái</color> rồi quay lại <color=red> nhận Nhận Mật Tịch (cao).</color>")
			dialog:Show(item, player)
		else
			if player:GetFactionID() == 1 then
				Player:AddItemLua(player,secretID[1].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[1].ItemID2,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[1].ItemID3,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[1].ItemID4,1,Player.GetSeries(player),1)
				-- Thêm kỹ năng mật tịch
				-- player:AddSkill(1200)
				-- player:AddSkill(1201)
				-- player:AddSkill(1241)
				-- player:AddSkill(1242)
			elseif player:GetFactionID() == 2 then
				Player:AddItemLua(player,secretID[2].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[2].ItemID2,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[2].ItemID3,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[2].ItemID4,1,Player.GetSeries(player),1)
				-- Thêm kỹ năng mật tịch
				-- player:AddSkill(1202)
				-- player:AddSkill(1202)
				-- player:AddSkill(1243)
				-- player:AddSkill(1244)
			elseif player:GetFactionID() == 3 then
				Player:AddItemLua(player,secretID[3].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[3].ItemID2,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[3].ItemID3,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[3].ItemID4,1,Player.GetSeries(player),1)
				-- Thêm kỹ năng mật tịch
				-- player:AddSkill(1204)
				-- player:AddSkill(1203)
				-- player:AddSkill(1245)
				-- player:AddSkill(1246)
			elseif player:GetFactionID() == 4 then
				Player:AddItemLua(player,secretID[4].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[4].ItemID2,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[4].ItemID3,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[4].ItemID4,1,Player.GetSeries(player),1)
				-- Thêm kỹ năng mật tịch
				-- player:AddSkill(1206)
				-- player:AddSkill(1205)
				-- player:AddSkill(1247)
				-- player:AddSkill(1248)
			elseif player:GetFactionID() == 5 then
				Player:AddItemLua(player,secretID[5].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[5].ItemID2,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[5].ItemID3,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[5].ItemID4,1,Player.GetSeries(player),1)
				-- Thêm kỹ năng mật tịch
				-- player:AddSkill(1207)
				-- player:AddSkill(1208)
				-- player:AddSkill(1249)
				-- player:AddSkill(1250)
			elseif player:GetFactionID() == 6 then
				Player:AddItemLua(player,secretID[6].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[6].ItemID2,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[6].ItemID3,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[6].ItemID4,1,Player.GetSeries(player),1)
				-- Thêm kỹ năng mật tịch
				-- player:AddSkill(1209)
				-- player:AddSkill(1210)
				-- player:AddSkill(1251)
				-- player:AddSkill(1252)
			elseif player:GetFactionID() == 7 then
				Player:AddItemLua(player,secretID[7].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[7].ItemID2,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[7].ItemID3,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[7].ItemID4,1,Player.GetSeries(player),1)
				-- Thêm kỹ năng mật tịch
				-- player:AddSkill(1211)
				-- player:AddSkill(1212)
				-- player:AddSkill(1253)
				-- player:AddSkill(1254)
			elseif player:GetFactionID() == 8 then
				Player:AddItemLua(player,secretID[8].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[8].ItemID2,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[8].ItemID3,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[8].ItemID4,1,Player.GetSeries(player),1)
				-- Thêm kỹ năng mật tịch
				-- player:AddSkill(1213)
				-- player:AddSkill(1214)
				-- player:AddSkill(1255)
				-- player:AddSkill(1256)
			elseif player:GetFactionID() == 9 then
				Player:AddItemLua(player,secretID[9].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[9].ItemID2,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[9].ItemID3,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[9].ItemID4,1,Player.GetSeries(player),1)
				-- Thêm kỹ năng mật tịch
				-- player:AddSkill(1215)
				-- player:AddSkill(1216)
				-- player:AddSkill(1257)
				-- player:AddSkill(1258)
			elseif player:GetFactionID() == 10 then
				Player:AddItemLua(player,secretID[10].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[10].ItemID2,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[10].ItemID3,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[10].ItemID4,1,Player.GetSeries(player),1)
				-- Thêm kỹ năng mật tịch
				-- player:AddSkill(1217)
				-- player:AddSkill(1218)
				-- player:AddSkill(1259)
				-- player:AddSkill(1260)
			elseif player:GetFactionID() == 11 then
				Player:AddItemLua(player,secretID[11].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[11].ItemID2,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[11].ItemID3,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[11].ItemID4,1,Player.GetSeries(player),1)
				-- Thêm kỹ năng mật tịch
				-- player:AddSkill(1219)
				-- player:AddSkill(1220)
				-- player:AddSkill(1261)
				-- player:AddSkill(1262)
			elseif player:GetFactionID() == 12 then
				Player:AddItemLua(player,secretID[12].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[12].ItemID2,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[12].ItemID3,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[12].ItemID4,1,Player.GetSeries(player),1)
				-- Thêm kỹ năng mật tịch
				-- player:AddSkill(1221)
				-- player:AddSkill(1222)
				-- player:AddSkill(1263)
				-- player:AddSkill(1264)
			end
			dialog:AddText(""..player:GetName().."Bạn đã nhận </color=red>Mật tịch (trung)(cao)</color>thành công")
			dialog:Show(item, player)
		end
	elseif selectionID == 10000 then
			player:SetLevel(109)
			dialog:AddText("Thiết lập cấp độ 109 thành công")
			dialog:Show(item, player)
	elseif selectionID == 11111 then
		local record4 = Player.GetValueForeverRecore(player, Record4)	
		-- Hỗ Trợ Đồ TT
		if record4 == 1 then
			TuiTanThu:ShowDialog(item, player, "Bằng hữu đã nhận <color=green>Đồ hỗ trợ tân thủ</color> trước đó, không thể nhận thêm!")
			return
		end
			Player:AddItemLua(player,388,5,0,1) --5HT4
			Player:AddItemLua(player,389,5,0,1)	--5HT5
			Player:AddItemLua(player,390,5,0,1) --5HT6
			if player:GetLevel() < 80 then
			player:SetLevel(80)
			end
			Player.AddMoney(player,100000,0)
			 for i=1,10 do 
				Player.SaveLifeSkillLevel(player,i, 60);
			 end
			dialog:AddText("Bạn đã nhận Hỗ Trợ Tân Thủ Của KT 2009 Mobile Thành Công !!!")
			Player.SetValueOfForeverRecore(player, Record4, 1)
			dialog:Show(item, player)
	elseif selectionID == 111111 then
			player:SetLevel(119)
			dialog:AddText("Thiết lập cấp độ 100 thành công")
			dialog:Show(item, player)
	elseif selectionID == 111112 then
			player:SetLevel(130)
			dialog:AddText("Thiết lập cấp độ 130 thành công")
			dialog:Show(item, player)
	elseif selectionID == 10003 then
		-- Gọi hàm đổi tên
	local dialog = GUI.CreateItemDialog()
		dialog:AddText("<color=green>Hỗ Trợ Tân Thủ</color>")
		dialog:AddSelection(10002, "Nhận <color=red> Mật Tịch (trung)(cao) Theo Phái </color>")
		dialog:AddSelection(19080, "Nhận <color=green>Huyền Tinh Các Cấp</color>")
		dialog:AddSelection(10005, "Nhận <color=red> 1000 uy danh </color>")
		dialog:AddSelection(10014, "Nhận Vỏ Sò Vàng</color>")
		dialog:AddSelection(10006, "Làm mới số lượt đi phụ bản trong ngày")
		dialog:AddSelection(10007, "Quan Ấn Hoàng Đế")
		dialog:AddSelection(10008, "Nhận 500 Hòa Thị Bích")
		-- dialog:AddSelection(10009, "Vũ Khi +16")
		-- dialog:AddSelection(10010, "Nhận Lệnh bài Thi đấu môn phái (sơ)")
		dialog:AddSelection(10015, "Nhận thần thú")
		dialog:AddSelection(100020, "Nhận 10000 tinh lực,hoạt lực")
	dialog:Show(item, player)
	-- ************************** --	
	elseif selectionID == 19080 then
		-- Gọi hàm đổi tên
	local dialog = GUI.CreateItemDialog()
		dialog:AddText("<color=green>Hỗ Trợ Tân Thủ</color>")
		dialog:AddSelection(19081, "Nhận <color=green>Huyền Tinh Cấp 1</color>")
		dialog:AddSelection(19082, "Nhận <color=green>Huyền Tinh Cấp 2</color>")
		dialog:AddSelection(19083, "Nhận <color=green>Huyền Tinh Cấp 3</color>")
		dialog:AddSelection(19084, "Nhận <color=green>Huyền Tinh Cấp 4</color>")
		dialog:AddSelection(19085, "Nhận <color=green>Huyền Tinh Cấp 5</color>")
		dialog:AddSelection(19086, "Nhận <color=green>Huyền Tinh Cấp 6</color>")
		dialog:AddSelection(19087, "Nhận <color=green>Huyền Tinh Cấp 7</color>")
		dialog:AddSelection(19088, "Nhận <color=green>Huyền Tinh Cấp 8</color>")
		dialog:AddSelection(19089, "Nhận <color=green>Huyền Tinh Cấp 9</color>")
		dialog:AddSelection(19090, "Nhận <color=green>Huyền Tinh Cấp 10</color>")
		dialog:AddSelection(19091, "Nhận <color=green>Huyền Tinh Cấp 11</color>")
		dialog:AddSelection(19092, "Nhận <color=green>Huyền Tinh Cấp 12</color>")
	dialog:Show(item, player)
	-- ************************** --	
	elseif selectionID == 19081 then
	-- ************************** --	
		Player:AddItemLua(player,385,7,0,1)
		player:AddNotification("Nhận 7 Huyền Tinh Cấp 1 Thành Công")
		GUI.CloseDialog(player)
	-- ************************** --
	elseif selectionID == 19082 then
	-- ************************** --	
		Player:AddItemLua(player,386,7,0,1)
		player:AddNotification("Nhận 7 Huyền Tinh Cấp 2 Thành Công")
		GUI.CloseDialog(player)
	-- ************************** --
	elseif selectionID == 19083 then
	-- ************************** --	
		Player:AddItemLua(player,387,7,0,1)
		player:AddNotification("Nhận 7 Huyền Tinh Cấp 3 Thành Công")
		GUI.CloseDialog(player)
	-- ************************** --
	elseif selectionID == 19084 then
	-- ************************** --	
		Player:AddItemLua(player,388,7,0,1)
		player:AddNotification("Nhận 7 Huyền Tinh Cấp 4 Thành Công")
		GUI.CloseDialog(player)
	-- ************************** --
	elseif selectionID == 19085 then
	-- ************************** --	
		Player:AddItemLua(player,389,7,0,1)
		player:AddNotification("Nhận 7 Huyền Tinh Cấp 5 Thành Công")
		GUI.CloseDialog(player)
	-- ************************** --
	elseif selectionID == 19086 then
	-- ************************** --	
		Player:AddItemLua(player,390,7,0,1)
		player:AddNotification("Nhận 7 Huyền Tinh Cấp 6 Thành Công")
		GUI.CloseDialog(player)
	-- ************************** --
	elseif selectionID == 19087 then
	-- ************************** --	
		Player:AddItemLua(player,391,7,0,1)
		player:AddNotification("Nhận 7 Huyền Tinh Cấp 7 Thành Công")
		GUI.CloseDialog(player)
	-- ************************** --
	elseif selectionID == 19088 then
	-- ************************** --	
		Player:AddItemLua(player,392,7,0,1)
		player:AddNotification("Nhận 7 Huyền Tinh Cấp 8 Thành Công")
		GUI.CloseDialog(player)
	-- ************************** --
	elseif selectionID == 19089 then
	-- ************************** --	
		Player:AddItemLua(player,393,7,0,1)
		player:AddNotification("Nhận 7 Huyền Tinh Cấp 9 Thành Công")
		GUI.CloseDialog(player)
	-- ************************** --
	elseif selectionID == 19090 then
	-- ************************** --	
		Player:AddItemLua(player,394,7,0,1)
		player:AddNotification("Nhận 7 Huyền Tinh Cấp 10 Thành Công")
		GUI.CloseDialog(player)
	-- ************************** --
	elseif selectionID == 19091 then
	-- ************************** --	
		Player:AddItemLua(player,395,7,0,1)
		player:AddNotification("Nhận 7 Huyền Tinh Cấp 11 Thành Công")
		GUI.CloseDialog(player)
	-- ************************** --
	elseif selectionID == 19092 then
	-- ************************** --	
		Player:AddItemLua(player,396,7,0,1)
		player:AddNotification("Nhận 7 Huyền Tinh Cấp 12 Thành Công")
		GUI.CloseDialog(player)
	-- ************************** --
	elseif selectionID == 10004 then
			if player:GetFactionID()==0 then
				dialog:AddText(""..player:GetName()..": Bạn chưa gia nhập phái ,hãy gia nhập <color=red> môn phái</color> rồi quay lại <color=red> nhận Nhận Mật Tịch (cao).</color>")
				dialog:Show(item, player)
			elseif player:GetFactionID()~=0 then
				if Player.GetSeries(player)==1 then
					if player:GetSex()==0 then
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID1,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID2,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID3,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID4,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID5,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID6,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID7,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID8,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID9,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID10,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID11,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID12,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID13,1,-1,1)
						TuiTanThu:SetItemKhongCH(item,player,SetBelongings[1].ItemID23,1,-1,1)
					elseif player:GetSex()==1 then
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID10,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID11,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID12,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID13,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID14,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID15,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID16,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID17,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID18,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID19,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID20,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID21,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[1].ItemID22,1,-1,1)
						TuiTanThu:SetItemKhongCH(item,player,SetBelongings[1].ItemID23,1,-1,1)
					else
						dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
						dialog:Show(item, player)
					end
				elseif Player.GetSeries(player)==2 then
					if player:GetSex()==0 then
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID1,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID2,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID3,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID4,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID5,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID6,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID7,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID8,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID9,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID10,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID11,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID12,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID13,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID14,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID15,1,-1,1)
						TuiTanThu:SetItemKhongCH(item,player,SetBelongings[2].ItemID25,1,-1,1)
						
					elseif player:GetSex()==1 then
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID10,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID11,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID12,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID13,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID14,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID15,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID16,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID17,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID18,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID19,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID20,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID21,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID22,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID23,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[2].ItemID24,1,-1,1)
						TuiTanThu:SetItemKhongCH(item,player,SetBelongings[2].ItemID25,1,-1,1)
					else
						dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
						dialog:Show(item, player)
					end
				elseif Player.GetSeries(player)==3 then
					if player:GetSex()==0 then
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID1,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID2,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID3,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID4,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID5,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID6,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID7,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID8,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID9,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID10,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID11,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID12,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID13,1,-1,1)
						TuiTanThu:SetItemKhongCH(item,player,SetBelongings[3].ItemID25,1,-1,1)
					elseif player:GetSex()==1 then
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID10,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID11,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID12,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID13,1,-1,1)
					
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID16,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID17,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID18,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID19,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID20,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID21,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID22,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID23,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[3].ItemID24,1,-1,1)
						TuiTanThu:SetItemKhongCH(item,player,SetBelongings[3].ItemID25,1,-1,1)
					else
						dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
						dialog:Show(item, player)
					end
				elseif Player.GetSeries(player)==4 then
					if player:GetSex()==0 then
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID1,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID2,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID3,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID4,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID5,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID6,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID7,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID8,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID9,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID10,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID11,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID12,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID13,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID30,1,-1,1)
						TuiTanThu:SetItemKhongCH(item,player,SetBelongings[4].ItemID25,1,-1,1)
					elseif player:GetSex()==1 then
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID10,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID11,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID12,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID13,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID14,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID15,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID16,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID17,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID18,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID19,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID20,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID21,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID22,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID23,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID24,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[4].ItemID30,1,-1,1)
						TuiTanThu:SetItemKhongCH(item,player,SetBelongings[4].ItemID25,1,-1,1)
					else
						dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
						dialog:Show(item, player)
					end
				elseif Player.GetSeries(player)==5 then
					if player:GetSex()==0 then
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID1,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID2,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID3,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID4,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID5,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID6,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID7,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID8,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID9,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID10,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID11,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID12,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID13,1,-1,1)
						TuiTanThu:SetItemKhongCH(item,player,SetBelongings[5].ItemID25,1,-1,1)
					elseif player:GetSex()==1 then
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID10,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID11,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID12,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID13,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID16,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID17,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID18,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID19,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID20,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID21,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID22,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID23,1,-1,1)
						TuiTanThu:SetBelonging(item,player,SetBelongings[5].ItemID24,1,-1,1)
						TuiTanThu:SetItemKhongCH(item,player,SetBelongings[25].ItemID25,1,-1,1)
					else
						dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
						dialog:Show(item, player)
					end

				
				end
			else
				dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
				dialog:Show(item, player)
			end
		end
		
	if selectionID ==14 then
		GUI.CloseDialog(player)
	end
	if selectionID == 17023 then
		local record3 = Player.GetValueForeverRecore(player, Record3)	
		-- Nếu đã nhận rồi
		if record3 == 1 then
			TuiTanThu:ShowDialog(item, player, "Bằng hữu đã nhận <color=green>Đồ hỗ trợ tân thủ</color> trước đó, không thể nhận thêm!")
			return
		end
		if player:GetFactionID()==0 then
			dialog:AddText(""..player:GetName()..": Bạn chưa gia nhập phái ,hãy gia nhập <color=red> môn phái</color> rồi quay lại <color=red> nhận Nhận Hỗ Trợ Đồ Tân Thủ.</color>")
			dialog:Show(item, player)
		elseif player:GetFactionID()~=0 then
			if player:GetFactionID()==1 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==2 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==3 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)				
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==4 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					-- TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					-- TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID14,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID15,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==5 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==6 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==7 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==8 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==9 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==10 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==11 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif player:GetFactionID()==12 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID24,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID25,1,-1,1)
					-- TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[player:GetFactionID()].ItemID26,1,-1,1)
					Player.SetValueOfForeverRecore(player, Record3, 1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end

			
			end
		else
			dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
			dialog:Show(item, player)
		end
	end
	if selectionID == 10023 then
		if player:GetFactionID()==0 then
			dialog:AddText(""..player:GetName()..": Bạn chưa gia nhập phái ,hãy gia nhập <color=red> môn phái</color> rồi quay lại <color=red> nhận Nhận Mật Tịch (cao).</color>")
			dialog:Show(item, player)
		elseif player:GetFactionID()~=0 then
			if Player.GetSeries(player)==1 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[1].ItemID23,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID14,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID15,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[1].ItemID22,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[1].ItemID23,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif Player.GetSeries(player)==2 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID14,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID15,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[2].ItemID25,1,-1,1)
					
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID14,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID15,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[2].ItemID24,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[2].ItemID25,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif Player.GetSeries(player)==3 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[3].ItemID25,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID13,1,-1,1)				
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[3].ItemID24,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[3].ItemID25,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif Player.GetSeries(player)==4 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID30,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[4].ItemID25,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID14,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID15,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID24,1,-1,1)
				 	TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[4].ItemID30,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[4].ItemID25,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif Player.GetSeries(player)==5 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID13,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[5].ItemID25,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89CH7(item,player,SetTanThu80CoNgua[5].ItemID24,1,-1,1)
					TuiTanThu:SetItemKhongCH(item,player,SetTanThu80CoNgua[5].ItemID25,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end

			
			end
		else
			dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
			dialog:Show(item, player)
		end
			-- player:SetLevel(89)
			-- dialog:AddText("Thiết lập cấp độ 89 thành công")
	end
	if selectionID == 10999025 then--TuiTanThu:SetBelonging(item, player, ItemID,Number,Series,LockStatus) set do  + 8
		if player:GetFactionID()==0 then
			dialog:AddText(""..player:GetName()..": Bạn chưa gia nhập phái ,hãy gia nhập <color=red> môn phái</color> rồi quay lại <color=red> nhận Nhận Mật Tịch (cao).</color>")
			dialog:Show(item, player)
		elseif player:GetFactionID()~=0 then
			if Player.GetSeries(player)==1 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID13,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID14,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID15,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[1].ItemID22,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif Player.GetSeries(player)==2 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID14,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID15,1,-1,1)
					
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID14,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID15,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[2].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif Player.GetSeries(player)==3 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID13,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID13,1,-1,1)
				
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[3].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif Player.GetSeries(player)==4 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID30,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID14,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID15,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID24,1,-1,1)
				 	TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID30,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end
			elseif Player.GetSeries(player)==5 then
				if player:GetSex()==0 then
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID1,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID2,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID3,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID4,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID5,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID6,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID7,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID8,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID9,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID13,1,-1,1)
				elseif player:GetSex()==1 then
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID10,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID11,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID12,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID13,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID16,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID17,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID18,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID19,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID20,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID21,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID22,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[5].ItemID23,1,-1,1)
					TuiTanThu:SetBelonging89Set8(item,player,SetBelongings89[4].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(item, player)
				end

			
			end
		else
			dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
			dialog:Show(item, player)
		end
	end
	if selectionID == 1 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Chọn môn phái muốn gia nhập.")
		for key, value in pairs(Global_FactionName) do
			dialog:AddSelection(100 + key, value)
		end
		dialog:Show(item, player)
		
		return
	end
	-- ************************** --
	if selectionID >= 100 and selectionID <= #Global_FactionName + 100 then
		TuiTanThu:JoinFaction(scene, item, player, selectionID - 100)
		
		return
	end
	-- ************************** --
	if selectionID == 2 or selectionID == 3 then
		local randomArmor = {
			Female = {3181, 3182, 3183, 3184, 3185, 3186, 3187, 3188, 3189, 3190, 3191},
			Male = {3171, 3172, 3173, 3174, 3175, 3176, 3177, 3178, 3179, 3180},
		}
		local randomWeapon = {3766, 3776, 3786, 3736, 3746, 3756}
		local randomHelm = {
			Female = {3429, 3430, 3431, 3432, 3433, 3434, 3435, 3436, 3437, 3438},
			Male = {3419, 3420, 3421, 3422, 3423, 3424, 3425, 3426, 3427, 3428},
		}
		local randomMantle = {
			Female = {3669, 3670, 3671, 3672, 3673, 3674, 3675, 3676, 3677, 3678},
			Male = {3619, 3620, 3621, 3622, 3623, 3624, 3625, 3626, 3627, 3628},
		}
		local randomHorse = {3459, 3460, 3461, 3462, 3463, 3464, 3465, 3466, 3467, 3468, 3469, 3470}
		local randRange = 500
		
		local count = 1
		if selectionID == 2 then
			count = 10
		elseif selectionID == 3 then
			count = 20
		end
		
		for i = 1, count do
			local itemPos = item:GetPos()
			local randPosX, randPosY = itemPos:GetX() + math.random(randRange) - math.random(randRange), itemPos:GetY() + math.random(randRange) - math.random(randRange)
			local randSex = math.random(0, 1)
			local randRiding = math.random(0, 1)
			local randMaxHP = math.random(100, 2000)
			local randHP = math.random(50, randMaxHP)
			local randSeries = math.random(1, 5)
		
			local botBuilder = EventManager.CreateBotBuilder(scene)
			botBuilder:SetPos(randPosX, randPosY)
			botBuilder:SetName("Bot")
			botBuilder:SetTitle("Bot Title")
			botBuilder:SetLevel(100)
			botBuilder:SetFaction(3, 1)
			botBuilder:SetSex(randSex)
			if randSex == 0 then
				botBuilder:SetAvarta(2)
			else
				botBuilder:SetAvarta(101)
			end
			if randRiding == 0 then
				botBuilder:SetIsRiding(false)
			else
				botBuilder:SetIsRiding(true)
			end
			botBuilder:SetAIID(-1)
			botBuilder:SetDirection(-1)
			botBuilder:AddEquip(randomWeapon[math.random(#randomWeapon)], randSeries, 16)
			botBuilder:AddEquip(randomHorse[math.random(#randomHorse)], 0, 0)
			if randSex == 0 then
				botBuilder:AddEquip(randomArmor.Male[math.random(#randomArmor.Male)], randSeries, 0)
				botBuilder:AddEquip(randomHelm.Male[math.random(#randomHelm.Male)], randSeries, 0)
				botBuilder:AddEquip(randomMantle.Male[math.random(#randomMantle.Male)], 0, 0)
			else
				botBuilder:AddEquip(randomArmor.Female[math.random(#randomArmor.Female)], randSeries, 0)
				botBuilder:AddEquip(randomHelm.Female[math.random(#randomHelm.Female)], randSeries, 0)
				botBuilder:AddEquip(randomMantle.Female[math.random(#randomMantle.Female)], 0, 0)
			end
			botBuilder:SetHP(randHP)
			botBuilder:SetHPMax(randMaxHP)
			botBuilder:Build()
		end
		
		
		TuiTanThu:ShowDialog(item, player, "Tạo BOT thành công!")
		return
	end
	-- ************************** --
	if selectionID == 4 then
		player:ChangeScene(25, 11530, 2070)
		return
	end
	-- ************************** --
	if selectionID == 5 then
		GUI.OpenUI(player, "UISignetEnhance")
		
		return
	end
	-- ************************** --
	if selectionID == 6 then
		GUI.OpenUI(player, "UIEnhance")
		
		return
	end
	-- ************************** --
	if selectionID == 7 then
		GUI.OpenUI(player, "UICrystalStoneSynthesis")
		
		return
	end
	-- ************************** --
	if selectionID == 8 then
		GUI.OpenUI(player, "UISplitEquipCrystalStones", 8, 80)
		
		return
	end
	-- ************************** --
	if selectionID == 9 then
		GUI.OpenUI(player, "UISplitCrystalStone", 10, 80)
		
		return
	end
	-- ************************** --
	if selectionID == 10 then
		-- Boss
		local Boss = {
			ID = 2661,
			Name = "",
			Title = "",
			BaseHP = 10000,
			HPIncreaseEachLevel = 10000,
			AIType = 2,
			SpawnAfter = 10000,
			ScriptID = -1,
		}
		
		local level = 120

		local monsterBuilder = EventManager.CreateMonsterBuilder(scene)
		monsterBuilder:SetResID(Boss.ID)
		monsterBuilder:SetPos(player:GetPos())
		monsterBuilder:SetName(Boss.Name)
		monsterBuilder:SetTitle(Boss.Title)
		monsterBuilder:SetLevel(level)
		monsterBuilder:SetMaxHP(Boss.BaseHP + level * Boss.HPIncreaseEachLevel)
		monsterBuilder:SetDirection(-1)
		monsterBuilder:SetSeries(0)
		monsterBuilder:SetAIType(Boss.AIType)
		monsterBuilder:SetScriptID(Boss.ScriptID)
		monsterBuilder:SetTag("Boss")
		monsterBuilder:SetRespawnTick(-1)
		monsterBuilder:Build()
		
		return
	end
	-- ************************** --
	if selectionID == 11 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Danh sách vật phẩm kèm Selection")
		dialog:AddSelection(111, "Selection 1")
		dialog:AddSelection(112, "Selection 2")
		dialog:AddItem(132, 100, 1)
		dialog:AddItem(132, 50, 0)
		dialog:AddItem(164, 1, 1)
		dialog:SetItemSelectable(false)
		dialog:Show(item, player)
		return
	end
	-- ************************** --
	if selectionID == 111 and selectionID == 112 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Chọn Selection 1")
		dialog:Show(item, player)
		return
	end
	-- ************************** --
	if selectionID == 12 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Danh sách vật phẩm kèm Selection")
		dialog:AddSelection(111, "Selection 1")
		dialog:AddSelection(112, "Selection 2")
		dialog:AddItem(132, 100, 1)
		dialog:AddItem(132, 50, 0)
		dialog:AddItem(164, 1, 1)
		dialog:SetItemSelectable(true)
		dialog:Show(item, player)
		return
	end
	-- ************************** --
	if selectionID == 13 then
		player:ChangeScene(25, 6945, 2784)
	end
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi item thông qua item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: item - item tương ứng
--		player: Player - item tương ứng
--		selectedItemInfo: SelectItem - Vật phẩm được chọn
--		otherParams: Key-Value {number, string} - Danh sách các tham biến khác
-- ****************************************************** --
function TuiTanThu:OnItemSelected(scene, item, player, selectedItemInfo, otherParams)

	-- ************************** --
	-- local itemID = selectedItemInfo:GetItemID()
	-- local itemNumber = selectedItemInfo:GetItemCount()
	-- local binding = selectedItemInfo:GetBinding()
	-- ************************** --
	-- TuiTanThu:ShowDialog(item, player, string.format("Chọn vật phẩm ID: %d, Count: %d, Binding: %d!", itemID, itemNumber, binding))
	-- ************************** --

end

-- ======================================================= --
-- ======================================================= --
function TuiTanThu:JoinFaction(scene, item, player, factionID)
	
	-- ************************** --
	local ret = player:JoinFaction(factionID)
	-- ************************** --
	if ret == -1 then
		TuiTanThu:ShowDialog(item, player, "Người chơi không tồn tại!")
		return
	elseif ret == -2 then
		TuiTanThu:ShowDialog(item, player, "Môn phái không tồn tại!")
		return
	elseif ret == 0 then
		TuiTanThu:ShowDialog(item, player, "Giới tính của bạn không phù hợp với môn phái này!")
		return
	elseif ret == 1 then
		TuiTanThu:ShowDialog(item, player, "Gia nhập phái <color=blue>" .. player:GetFactionName() .. "</color> thành công!")
		return
	else
		TuiTanThu:ShowDialog(item, player, "Chuyển phái thất bại, lỗi chưa rõ!")
		return
	end
	-- ************************** --

end

-- ======================================================= --
-- ======================================================= --
function TuiTanThu:GetValueTest(scene, item, player)
	
	-- ************************** --
	return 1, 2
	-- ************************** --

end

-- ======================================================= --
-- ======================================================= --
function TuiTanThu:ShowDialog(item, player, text)

	-- ************************** --
	local dialog = GUI.CreateItemDialog()
	dialog:AddText(text)
	dialog:Show(item, player)
	-- ************************** --
	
end
function TuiTanThu:PlayerSetLevel(item, player, level)

	-- ************************** --
	player:SetLevel(level)
	TuiTanThu:ShowDialog(item, player, "Thiết lập cấp độ ".. level .." thành công")
	-- ************************** --

end
function TuiTanThu:SetBelonging(item, player, ItemID,Number,Series,LockStatus)

	if ItemID == nil then
		return
	end
	if Series == nil then
		return
	end
-- ************************** --
	Player.AddItemLua(player,ItemID,Number,Series,LockStatus,14)
	TuiTanThu:ShowDialog(item, player,"Nhận phẩm thành công theo hệ <color=red>Thành công !+14</color>")
	-- ************************** --
end
function TuiTanThu:SetBelonging89(item, player, ItemID,Number,Series,LockStatus)

	if ItemID == nil then
		return
	end
	if Series == nil then
		return
	end
	-- ************************** --
	Player.AddItemLua(player,ItemID,Number,Series,LockStatus,12)
	TuiTanThu:ShowDialog(item, player,"Nhận phẩm thành công theo hệ <color=red>Thành công !+12</color>")
	-- ************************** --
end
function TuiTanThu:SetHKMP16(item, player, ItemID,Number,Series,LockStatus)

	if ItemID == nil then
		return
	end
	if Series == nil then
		return
	end
	-- ************************** --
	Player.AddItemLua(player,ItemID,Number,Series,LockStatus,16)
	TuiTanThu:ShowDialog(item, player,"Nhận phẩm thành công theo hệ <color=red>Thành công !+16</color>")
	-- ************************** --
end
function TuiTanThu:SetItemKhongCH(item, player, ItemID,Number,Series,LockStatus)

	if ItemID == nil then
		return
	end
	if Series == nil then
		return
	end
	-- ************************** --
	Player.AddItemLua(player,ItemID,Number,-1,1)
--	Player.AddItemLua(player,ItemID,Number,Series,LockStatus,16)
	-- TuiTanThu:ShowDialog(item, player,"Nhận phẩm thành công theo hệ <color=red>Thành công !+12</color>")
	-- ************************** --
end
function TuiTanThu:EquipEnhance(item, player, ItemID,enhLevel)
	-- ************************** --
	Player.EquipEnhance(player, ItemID, enhLevel)-- ************************** --
end
function TuiTanThu:SetBelonging89CH7(item, player, ItemID,Number,Series,LockStatus)

	if ItemID == nil then
		return
	end
	if Series == nil then
		return
	end
	-- ************************** --
	Player.AddItemLua(player,ItemID,Number,Series,LockStatus,5)
	TuiTanThu:ShowDialog(item, player,"Nhận phẩm thành công theo hệ <color=red>Thành công !+5</color>")
	-- ************************** --
end
