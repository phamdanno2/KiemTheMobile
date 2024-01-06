local TuiTanThu = Scripts[200095]
function TuiTanThu:OnPreCheckCondition(scene, item, player, otherParams)
    return true
end

-- ****************************************************** --
local Record1 = 101119	---bỏ
local Record2 = 101120	---Hỗ Trợ Tân Thủ
local Record3 = 101121	---bỏ
local Record4 = 101122	---đua top cấp
local Record5 = 101123	---đua top tài phú
local Record6 = 101124	---nhận danh vọng
local Record7 = 101125	---nhận Ngũ Hành Ấn
local Record8 = 101126	---bỏ
local Record9 = 101127	---bỏ
local Record10 = 101128	---bỏ
local Record11 = 101129	---Thẻ Đổi Phái

function TuiTanThu:OnUse(scene, item, player, otherParams)
	local dialog = GUI.CreateItemDialog()
	dialog:AddText("Xin chào "..player:GetName().."\nĐua top <color=red>Cấp độ</color> từ <color=green>20h00 17/11/2023 đến 23h00 24/12/2023</color>\nĐua top <color=red>Tài Phú</color> từ <color=green>20h00 17/11/2023 đến 23h00 31/12/2023</color> !")

	if player:GetFactionID()==0 then
		dialog:AddSelection(1,"Gia nhập Môn Phái.")
	else
		dialog:AddSelection(40008, "GiftCode")
		local record2 = Player.GetValueForeverRecore(player, Record2)
		if record2 ~= 1 then
			dialog:AddSelection(40000, "Hỗ Trợ Tân Thủ")
		end
		local record6 = Player.GetValueForeverRecore(player, Record6)	
		if record6 ~= 1 then
			dialog:AddSelection(40007, "Nhận <color=green>danh vọng</color> các loại")
		end

		dialog:AddSelection(40006, "Mở shop Trang bị Danh Vọng (Yêu cầu Thạch Cổ Trấn)")
		local nCheck11 = Player.GetValueForeverRecore(player, Record11)
		if nCheck11 ~= 1 then
			dialog:AddSelection(40005, "Nhận <color=green>Thẻ Đổi Phái</color>")
		end
		dialog:AddSelection(40002, "Nhận <color=green>Phi Phong</color>")
		dialog:AddSelection(40001, "Nhận <color=green>Mật Tịch</color> theo phái")
		dialog:AddSelection(40003, "Nhận <color=green>Max Kinh Nghiệm Mật Tịch</color>")
		local record7 = Player.GetValueForeverRecore(player, Record7)
		if record7 ~= 1 then
			dialog:AddSelection(40004, "Nhận <color=green>Ngũ Hành Ấn</color>")
		end
		dialog:AddSelection(200031, "Nhận thưởng và xem đua top Cấp Độ và Tài Phú")
		dialog:AddSelection(40009, "Ta muốn đổi tên")
		dialog:AddSelection(40010, "Xóa vật phẩm")
		dialog:AddSelection(40011, "Ghép vật phẩm")	
	end	
	if player:IsGM() == 1 then
		dialog:AddSelection(999999, "Lưu danh sach top")
	end
	dialog:AddSelection(77777, "Kết thúc đối thoại")
	dialog:Show(item, player)
end

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

function TuiTanThu:ItemSet()
end

function TuiTanThu:OnSelection(scene, item, player, selectionID, otherParams)
	local dialog = GUI.CreateItemDialog()
	local TotalPrestige = player:GetPrestige()
	if selectionID == 77777 then
		GUI.CloseDialog(player)
		return
	end
	
	if selectionID == 999999 then
		GUI.CloseDialog(player)
		player:ExportTop()
		return
	end

	if selectionID == 40005 then
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
	
	if selectionID == 40009 then
		if Player.CountItemInBag(player, 2167) <= 0 then
            TuiTanThu:ShowDialog(item, player,"Chức năng này yêu cầu <color=yellow>[Thẻ Đổi Tên]</color>. Khi nào có hãy đến tìm ta.")
            return
        end
		GUI.OpenChangeName(player)
        GUI.CloseDialog(player)
		return
	end

	if selectionID == 40004 then
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

	if selectionID == 40007 then	
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

	if selectionID == 40001 then
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
		return
	end
	
	if selectionID == 40003 then
		GUI.CloseDialog(player)
		Player.SetBookLevelAndExp(player, 100, 0)
		player:AddNotification("Nhận thành công Kinh Nghiệm Mật Tịch")
		return
	end
	
	if selectionID == 40002 then
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
		return
	end
	
	if selectionID == 200031 then
		dialog:AddText("Xin chào "..player:GetName().."\nĐua top <color=red>Cấp độ</color> từ <color=green>20h00 17/11/2023 đến 23h00 24/12/2023</color>\nĐua top <color=red>Tài Phú</color> từ <color=green>20h00 17/11/2023 đến 23h00 31/12/2023</color> !")
		local nCheck4 = Player.GetValueForeverRecore(player, Record4)
		----if nCheck4 <= 0 then
			dialog:AddSelection(200032, "Nhận thưởng Đua Top Cấp Độ")
		----end
		----local nCheck5 = Player.GetValueForeverRecore(player, Record5)
		----if nCheck5 <= 0 then
			dialog:AddSelection(200033, "Nhận thưởng Đua Top Tài Phú")
		----end
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
        ---if record >= 1 then
        ---    TuiTanThu:ShowDialog(item, player, "Bạn đã nhận đua top tài phú này rồi.")
        ---    return
        ---end
		local rank = player:GetTopMoney()
		rank = 100;
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
		player:BXHCapDo()
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
		player:BXHTaiPhu()
		
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
	
	if selectionID == 40000 then
		GUI.CloseDialog(player)
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
	
	if selectionID == 40010 then
		-- Mở khung xóa vật phẩm
		GUI.OpenRemoveItems(player)
		
		-- Đóng khung
		GUI.CloseDialog(player)
		return
	end
	
	if selectionID == 40011 then
		-- Mở khung ghép vật phẩm
		GUI.OpenMergeItems(player)
		
		-- Đóng khung
		GUI.CloseDialog(player)
		return
	end
	
	if selectionID == 40006 then
		----if player:NpcClick(2993) == 1 then		
		----	GUI.CloseDialog(player)
		----else
		----	dialog:AddText("Shop danh vọng chỉ mở ở Thạch Cổ Trấn !")
		----	dialog:Show(item, player)
		----end
		Player.OpenShopNew(player, 128)
		GUI.CloseDialog(player)
		return
	end

	if selectionID == 40008 then
		if Player.HasFreeBagSpaces(player, 5) == false then
			GUI.CloseDialog(player)
            TuiTanThu:ShowDialog(item, player, string.format("Bằng hữu cần sắp xếp tối thiểu <color=green>%d ô trống</color> trong túi đồ!",5))
            return false
        end
		GUI.OpenUI(player, "UIGiftCode")
		GUI.CloseDialog(player)
		return
	end
	
	-----------------gia nhập môn phái
	if selectionID == 1 then
		local dialog = GUI.CreateItemDialog()
		dialog:AddText("Chọn môn phái muốn gia nhập.")
		for key, value in pairs(Global_FactionName) do
			dialog:AddSelection(100 + key, value)
		end
		dialog:Show(item, player)
		
		return
	end

	if selectionID >= 100 and selectionID <= #Global_FactionName + 100 then
		TuiTanThu:JoinFaction(scene, item, player, selectionID - 100)
		return
	end
end

function TuiTanThu:OnItemSelected(scene, item, player, selectedItemInfo, otherParams)

end

function TuiTanThu:JoinFaction(scene, item, player, factionID)
	local ret = player:JoinFaction(factionID)
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
end

function TuiTanThu:ShowDialog(item, player, text)
	local dialog = GUI.CreateItemDialog()
	dialog:AddText(text)
	dialog:Show(item, player)
end

