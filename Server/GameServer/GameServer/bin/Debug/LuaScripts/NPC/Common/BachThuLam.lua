-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000000' bên dưới thành ID tương ứng

local NPC_Test = Scripts[000000]
local Record2 = 101120
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
local secretID = {
	[1] = {
		ItemID1 = 3280,			-- Thiếu Lâm Phái
		ItemID2 = 3281,
		
	},
	[2] = {
		ItemID1 = 3282,			-- Thiên Vương Bang
		ItemID2 = 3283,
	},
	[3] = {
		ItemID1 = 3284,			-- Đường Môn
		ItemID2 = 3285,
	},
	[4] = {
		ItemID1 = 3286,			-- Ngũ Độc Giáo
		ItemID2 = 3287,
	},
	[5] = {
		ItemID1 = 3288,			-- Nga My Phái
		ItemID2 = 3289,
	},
	[6] = {
		ItemID1 = 3290,			-- Thúy Yên Môn
		ItemID2 = 3291,
	},
	[7] = {
		ItemID1 = 3292,			-- Cái Bang Phái
		ItemID2 = 3293,
	},
	[8] = {
		ItemID1 = 3294,			-- Thiên Nhẫn Giáo
		ItemID2 = 3295,
	},
	[9] = {
		ItemID1 = 3296,			-- Võ Đang Phái
		ItemID2 = 3297,
	},
	[10] = {
		ItemID1 = 3298,			-- Côn Lôn Phái
		ItemID2 = 3299,
	},
	[11] = {
		ItemID1 = 3300,			-- Minh Giáo
		ItemID2 = 3301,
	},
	[12] = {
		ItemID1 = 3302,			-- Đoàn Thị Phái
		ItemID2 = 3303,
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
		ItemID24 =12237
		
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
		ItemID24 =9688
		
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
	},
}
-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		otherParams: Key-Value {number, string} - Danh sách các tham biến khác
-- ****************************************************** --
function NPC_Test:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	local record2 = Player.GetValueForeverRecore(player, Record2)
	dialog:AddText(""..npc:GetName()..": xin chào!")
	-- if player:GetFactionID()==0 then
	-- dialog:AddSelection(1,"Gia nhập Môn Phái.")
	-- end
	dialog:AddSelection(30000, "Ta muốn đổi tên")
	dialog:AddSelection(30001, "Xóa vật phẩm")
	dialog:AddSelection(30002, "Ghép vật phẩm")
	-- dialog:AddSelection(22222, "EXP BOOK")
	-- dialog:AddSelection(10012, "Nhận cấp 89")
	-- dialog:AddSelection(10024, "Nhận cấp 99")
	-- dialog:AddSelection(10000, "Nhận cấp 109")
	-- dialog:AddSelection(11111, "Nhận cấp 119")
	-- dialog:AddSelection(10023, "Nhận <color=red>Nhận set đồ theo hệ 89 +12</color>")
	-- dialog:AddSelection(10025, "Nhận <color=red>Nhận set đồ theo hệ 89 +8</color>")	
	-- dialog:AddSelection(10004, "Nhận <color=red>Nhận set đồ theo hệ 119 +14</color>")
	--dialog:AddSelection(10001, "Nhận <color=red>500 vạn (đồng)</color>")
	-- dialog:AddSelection(10002, "Nhận <color=red> Mật Tịch (cao) Theo Phái </color>")
	-- dialog:AddSelection(10003, "Nhận <color=red>3 Huyền Tinh 8</color>")
	-- dialog:AddSelection(10005, "Nhận <color=red>Nhận 1000 uy danh </color>")
	-- dialog:AddSelection(10006, "Làm mới số lượt đi phụ bản trong ngày")
	-- dialog:AddSelection(10007, "Nhận 5 bản đồ bí Cảnh")
	-- dialog:AddSelection(10008, "Nhận 100 Nguyệt Ảnh Thạch")
	-- dialog:AddSelection(10009, "Nhận 50 Chiến thư Du Long")
	-- dialog:AddSelection(10010, "Nhận Lệnh bài Thi đấu môn phái (sơ)")
	-- dialog:AddSelection(10015, "Nhận thần thú")
	-- dialog:AddSelection(100020, "Nhận 10000 tinh lực,hoạt lực")
--	dialog:AddSelection(100030, "GiftCode")
	-- if record2 ~= 1 then
		-- dialog:AddSelection(30003, "Nhận <color=green>Thẻ đổi tên </color> ")
	-- end
--	dialog:AddSelection(10011, "Dịch chuyển đến Hoàng Thành liên Server")
	
	
	dialog:Show(npc, player)
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
--		otherParams: Key-Value {number, string} - Danh sách các tham biến khác
-- ****************************************************** --
function NPC_Test:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	local TotalPrestige = player:GetPrestige()
	if selectionID == 30000 then
		-- Gọi hàm đổi tên
		GUI.OpenChangeName(player)
		
		-- Đóng khung
		GUI.CloseDialog(player)
		return
	end
	-- ************************** --
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
		local record = Player.GetValueForeverRecore(player, Record2)
		
		-- Nếu đã nhận rồi
		if record == 1 then
			BachThuLam:ShowDialog(npc, player, "Bằng hữu đã nhận <color=green>Thẻ đổi tên</color> trước đó, không thể nhận thêm!")
			return
		end
		-- Đánh dấu đã nhận rồi
		Player.SetValueOfForeverRecore(player, Record2, 1)
		Player.AddItemLua(player,2167,1,-1,1)
		BachThuLam:ShowDialog(npc, player, "Nhận <color=green>Thẻ đổi tên</color> thành công. Rất cảm ơn bằng hữu đã luôn đồng hành cùng <color=green>Kiếm Thế 2009 - Mobile</color>!")
		return
	end
	if selectionID == 10005 then
	
		player:SetPrestige(1000)
		dialog:AddText(""..npc:GetName().."Bạn đã nhận </color=red>1000 uy danh </color>thành công")
		
		dialog:Show(npc, player)
		return
	end
	
	if selectionID == 22222 then
		Player.SetBookLevelAndExp(player, 100, 0)
		dialog:AddText("Nhận thành công")
		dialog:Show(npc, player)
		return
	end
	
	if selectionID == 10006 then
		player:ResetCopySceneEnterTimes(11111)
		player:ResetCopySceneEnterTimes(11112)
		player:ResetCopySceneEnterTimes(100000)
		player:ResetCopySceneEnterTimes(100001)
		player:ResetCopySceneEnterTimes(100002)
		player:ResetCopySceneEnterTimes(100003)
		dialog:AddText(""..npc:GetName().."Mới số lượt đi phụ bản trong ngày thành công")
		dialog:Show(npc, player)
	end
	if selectionID == 10007 then
	
		Player.AddItemLua(player,590,5,-1,1)
		dialog:AddText(""..npc:GetName().."Bạn đã nhận </color=red>5 bản đồ bí Cảnh </color>thành công")
		dialog:Show(npc, player)
		return
	end
	if selectionID == 10008 then
		Player.AddItemLua(player,928,100,-1,1)
		dialog:AddText(""..npc:GetName().."Bạn đã nhận </color=red>100 Nguyệt ảnh thạch </color>thành công")
		dialog:Show(npc, player)
		return
	end
	if selectionID == 10009 then
		Player.AddItemLua(player,1026,50,-1,1)
		dialog:AddText(""..npc:GetName().."Bạn đã nhận </color=red>50 Chiến Thư-Mật Thất Du Long </color>thành công")
		dialog:Show(npc, player)
		return
	end
	if selectionID == 10010 then
		Player.AddItemLua(player,338,10,-1,1)
		dialog:AddText(""..npc:GetName().."Bạn đã nhận </color=red>10 lệnh bài Thi đấu môn phái(sơ) </color>thành công")
		dialog:Show(npc, player)
		return
	end
	if selectionID == 10011 then
		player:ChangeScene(1616, 7545, 4454)
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 10012 then
		player:SetLevel(89)
		dialog:AddText("Thiết lập cấp độ 89 thành công")
		dialog:Show(npc, player)
	end
	if selectionID == 10024 then
		player:SetLevel(99)
		dialog:AddText("Thiết lập cấp độ 99 thành công")
		dialog:Show(npc, player)
	end
	if selectionID == 10013 then
		Player.OpenShop(npc, player, 226)
		GUI.CloseDialog(player)
		
	end
	if selectionID == 10014 then
		player:SetPrayTimes(100)
		dialog:AddText("nhận 100 lượt quay chúc chúc phúc thành công !")
		dialog:Show(npc, player)
	end
	if selectionID == 10015 then
		Player.AddItemLua(player,3508,1,-1,1)
		dialog:AddText(""..npc:GetName().."Bạn đã nhận </color=red>thần thú </color>thành công")
		dialog:Show(npc, player)
		return
	end
	if selectionID == 100020 then
		Player.AddCurGatherPoint(player,10000)
		Player.AddMakePoint(player,10000)
		dialog:AddText(""..npc:GetName().."Nhận tinh lực hoạt lực thành công")
		dialog:Show(npc, player)
	end
	if selectionID == 100030 then
		GUI.OpenUI(player, "UIGiftCode")
		GUI.CloseDialog(player)
		return
	end
	if selectionID == 10002 then
		if player:GetFactionID()==0 then
			dialog:AddText(""..npc:GetName()..": Bạn chưa gia nhập phái ,hãy gia nhập <color=red> môn phái</color> rồi quay lại <color=red> nhận Nhận Mật Tịch (cao).</color>")
			dialog:Show(npc, player)
		else
			if player:GetFactionID() == 1 then
				Player:AddItemLua(player,secretID[1].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[1].ItemID2,1,Player.GetSeries(player),1)
			elseif player:GetFactionID() == 2 then
				Player:AddItemLua(player,secretID[2].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[2].ItemID2,1,Player.GetSeries(player),1)
			elseif player:GetFactionID() == 3 then
				Player:AddItemLua(player,secretID[3].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[3].ItemID2,1,Player.GetSeries(player),1)
			elseif player:GetFactionID() == 4 then
				Player:AddItemLua(player,secretID[4].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[4].ItemID2,1,Player.GetSeries(player),1)
			elseif player:GetFactionID() == 5 then
				Player:AddItemLua(player,secretID[5].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[5].ItemID2,1,Player.GetSeries(player),1)
			elseif player:GetFactionID() == 6 then
				Player:AddItemLua(player,secretID[6].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[6].ItemID2,1,Player.GetSeries(player),1)
			elseif player:GetFactionID() == 7 then
				Player:AddItemLua(player,secretID[7].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[7].ItemID2,1,Player.GetSeries(player),1)
			elseif player:GetFactionID() == 8 then
				Player:AddItemLua(player,secretID[8].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[8].ItemID2,1,Player.GetSeries(player),1)
			elseif player:GetFactionID() == 9 then
				Player:AddItemLua(player,secretID[9].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[9].ItemID2,1,Player.GetSeries(player),1)
			elseif player:GetFactionID() == 10 then
				Player:AddItemLua(player,secretID[10].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[10].ItemID2,1,Player.GetSeries(player),1)
			elseif player:GetFactionID() == 11 then
				Player:AddItemLua(player,secretID[11].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[11].ItemID2,1,Player.GetSeries(player),1)
			elseif player:GetFactionID() == 12 then
				Player:AddItemLua(player,secretID[12].ItemID1,1,Player.GetSeries(player),1)
				Player:AddItemLua(player,secretID[12].ItemID2,1,Player.GetSeries(player),1)
			end
			dialog:AddText(""..npc:GetName().."Bạn đã nhận </color=red>Mật tịch (cao)</color>thành công")
			dialog:Show(npc, player)
		end
	elseif selectionID == 10001 then
		if Player.CheckMoney(player,2) >= 10000000 then
			dialog:AddText("Trên người ngươi quá nhiều <color=#ffd24d>(Đồng)</color>không thể nhân được nữa.")
			dialog:Show(npc, player)
		else
			Player.AddMoney(player,5000000,2)
			dialog:AddText("Ngươi đã nhận <color=#ffd24d>500 vạn (đồng)</color>thành công")
			dialog:Show(npc, player)
		end
	elseif selectionID == 10000 then
			player:SetLevel(109)
			dialog:AddText("Thiết lập cấp độ 109 thành công")
			dialog:Show(npc, player)
	elseif selectionID == 11111 then
			player:SetLevel(119)
			dialog:AddText("Thiết lập cấp độ 109 thành công")
			dialog:Show(npc, player)
	elseif selectionID == 10003 then
		Player:AddItemLua(player,190,3,0,1)
		dialog:AddText("Bạn dã nhận được 3 viên huyền tinh 8")
		dialog:Show(npc, player)
	elseif selectionID == 10004 then--NPC_Test:SetBelonging(npc, player, ItemID,Number,Series,LockStatus) set do +14
			if player:GetFactionID()==0 then
				dialog:AddText(""..npc:GetName()..": Bạn chưa gia nhập phái ,hãy gia nhập <color=red> môn phái</color> rồi quay lại <color=red> nhận Nhận Mật Tịch (cao).</color>")
				dialog:Show(npc, player)
			elseif player:GetFactionID()~=0 then
				if Player.GetSeries(player)==1 then
					if player:GetSex()==0 then
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID1,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID2,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID3,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID4,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID5,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID6,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID7,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID8,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID9,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID10,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID11,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID12,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID13,1,-1,1)
					elseif player:GetSex()==1 then
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID10,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID11,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID12,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID13,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID14,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID15,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID16,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID17,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID18,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID19,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID20,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID21,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[1].ItemID22,1,-1,1)
					else
						dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
						dialog:Show(npc, player)
					end
				elseif Player.GetSeries(player)==2 then
					if player:GetSex()==0 then
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID1,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID2,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID3,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID4,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID5,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID6,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID7,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID8,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID9,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID10,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID11,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID12,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID13,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID14,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID15,1,-1,1)
						
					elseif player:GetSex()==1 then
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID10,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID11,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID12,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID13,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID14,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID15,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID16,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID17,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID18,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID19,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID20,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID21,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID22,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID23,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[2].ItemID24,1,-1,1)
					else
						dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
						dialog:Show(npc, player)
					end
				elseif Player.GetSeries(player)==3 then
					if player:GetSex()==0 then
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID1,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID2,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID3,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID4,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID5,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID6,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID7,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID8,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID9,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID10,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID11,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID12,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID13,1,-1,1)
					elseif player:GetSex()==1 then
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID10,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID11,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID12,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID13,1,-1,1)
					
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID16,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID17,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID18,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID19,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID20,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID21,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID22,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID23,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[3].ItemID24,1,-1,1)
					else
						dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
						dialog:Show(npc, player)
					end
				elseif Player.GetSeries(player)==4 then
					if player:GetSex()==0 then
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID1,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID2,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID3,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID4,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID5,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID6,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID7,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID8,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID9,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID10,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID11,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID12,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID13,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID30,1,-1,1)
					elseif player:GetSex()==1 then
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID10,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID11,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID12,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID13,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID14,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID15,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID16,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID17,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID18,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID19,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID20,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID21,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID22,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID23,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID24,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID30,1,-1,1)
					else
						dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
						dialog:Show(npc, player)
					end
				elseif Player.GetSeries(player)==5 then
					if player:GetSex()==0 then
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID1,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID2,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID3,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID4,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID5,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID6,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID7,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID8,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID9,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID10,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID11,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID12,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID13,1,-1,1)
					elseif player:GetSex()==1 then
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID10,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID11,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID12,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID13,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID16,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID17,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID18,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID19,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID20,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID21,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID22,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[5].ItemID23,1,-1,1)
						NPC_Test:SetBelonging(npc,player,SetBelongings[4].ItemID24,1,-1,1)
					else
						dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
						dialog:Show(npc, player)
					end

				
				end
			else
				dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
				dialog:Show(npc, player)
			end
		end
		
	if selectionID ==14 then
		GUI.CloseDialog(player)
	end
	if selectionID == 10023 then--NPC_Test:SetBelonging(npc, player, ItemID,Number,Series,LockStatus) set do +12
		if player:GetFactionID()==0 then
			dialog:AddText(""..npc:GetName()..": Bạn chưa gia nhập phái ,hãy gia nhập <color=red> môn phái</color> rồi quay lại <color=red> nhận Nhận Mật Tịch (cao).</color>")
			dialog:Show(npc, player)
		elseif player:GetFactionID()~=0 then
			if Player.GetSeries(player)==1 then
				if player:GetSex()==0 then
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID1,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID2,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID3,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID4,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID5,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID6,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID7,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID8,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID9,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID13,1,-1,1)
				elseif player:GetSex()==1 then
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID13,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID14,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID15,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID16,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID17,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID18,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID19,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID20,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID21,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[1].ItemID22,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(npc, player)
				end
			elseif Player.GetSeries(player)==2 then
				if player:GetSex()==0 then
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID1,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID2,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID3,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID4,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID5,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID6,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID7,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID8,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID9,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID13,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID14,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID15,1,-1,1)
					
				elseif player:GetSex()==1 then
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID13,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID14,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID15,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID16,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID17,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID18,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID19,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID20,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID21,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID22,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID23,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[2].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(npc, player)
				end
			elseif Player.GetSeries(player)==3 then
				if player:GetSex()==0 then
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID1,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID2,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID3,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID4,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID5,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID6,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID7,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID8,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID9,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID13,1,-1,1)
				elseif player:GetSex()==1 then
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID13,1,-1,1)
				
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID16,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID17,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID18,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID19,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID20,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID21,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID22,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID23,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[3].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(npc, player)
				end
			elseif Player.GetSeries(player)==4 then
				if player:GetSex()==0 then
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID1,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID2,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID3,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID4,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID5,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID6,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID7,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID8,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID9,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID13,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID30,1,-1,1)
				elseif player:GetSex()==1 then
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID13,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID14,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID15,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID16,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID17,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID18,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID19,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID20,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID21,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID22,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID23,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID24,1,-1,1)
				 	NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID30,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(npc, player)
				end
			elseif Player.GetSeries(player)==5 then
				if player:GetSex()==0 then
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID1,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID2,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID3,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID4,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID5,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID6,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID7,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID8,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID9,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID13,1,-1,1)
				elseif player:GetSex()==1 then
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID13,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID16,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID17,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID18,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID19,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID20,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID21,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID22,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[5].ItemID23,1,-1,1)
					NPC_Test:SetBelonging89(npc,player,SetBelongings89[4].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(npc, player)
				end

			
			end
		else
			dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
			dialog:Show(npc, player)
		end
	end
	if selectionID == 10025 then--NPC_Test:SetBelonging(npc, player, ItemID,Number,Series,LockStatus) set do  + 8
		if player:GetFactionID()==0 then
			dialog:AddText(""..npc:GetName()..": Bạn chưa gia nhập phái ,hãy gia nhập <color=red> môn phái</color> rồi quay lại <color=red> nhận Nhận Mật Tịch (cao).</color>")
			dialog:Show(npc, player)
		elseif player:GetFactionID()~=0 then
			if Player.GetSeries(player)==1 then
				if player:GetSex()==0 then
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID1,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID2,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID3,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID4,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID5,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID6,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID7,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID8,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID9,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID13,1,-1,1)
				elseif player:GetSex()==1 then
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID13,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID14,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID15,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID16,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID17,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID18,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID19,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID20,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID21,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[1].ItemID22,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(npc, player)
				end
			elseif Player.GetSeries(player)==2 then
				if player:GetSex()==0 then
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID1,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID2,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID3,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID4,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID5,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID6,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID7,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID8,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID9,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID13,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID14,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID15,1,-1,1)
					
				elseif player:GetSex()==1 then
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID13,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID14,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID15,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID16,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID17,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID18,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID19,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID20,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID21,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID22,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID23,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[2].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(npc, player)
				end
			elseif Player.GetSeries(player)==3 then
				if player:GetSex()==0 then
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID1,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID2,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID3,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID4,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID5,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID6,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID7,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID8,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID9,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID13,1,-1,1)
				elseif player:GetSex()==1 then
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID13,1,-1,1)
				
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID16,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID17,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID18,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID19,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID20,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID21,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID22,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID23,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[3].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(npc, player)
				end
			elseif Player.GetSeries(player)==4 then
				if player:GetSex()==0 then
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID1,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID2,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID3,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID4,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID5,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID6,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID7,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID8,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID9,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID13,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID30,1,-1,1)
				elseif player:GetSex()==1 then
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID13,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID14,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID15,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID16,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID17,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID18,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID19,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID20,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID21,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID22,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID23,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID24,1,-1,1)
				 	NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID30,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(npc, player)
				end
			elseif Player.GetSeries(player)==5 then
				if player:GetSex()==0 then
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID1,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID2,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID3,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID4,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID5,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID6,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID7,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID8,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID9,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID13,1,-1,1)
				elseif player:GetSex()==1 then
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID10,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID11,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID12,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID13,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID16,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID17,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID18,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID19,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID20,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID21,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID22,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[5].ItemID23,1,-1,1)
					NPC_Test:SetBelonging89Set8(npc,player,SetBelongings89[4].ItemID24,1,-1,1)
				else
					dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
					dialog:Show(npc, player)
				end

			
			end
		else
			dialog:AddText("<color=red>Lỗi rồi</color>.Hãy báo cho GM để được hỗ trợ!!!")
			dialog:Show(npc, player)
		end
	end
	if selectionID == 1 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Chọn môn phái muốn gia nhập.")
		for key, value in pairs(Global_FactionName) do
			dialog:AddSelection(100 + key, value)
		end
		dialog:Show(npc, player)
		
		return
	end
	-- ************************** --
	if selectionID >= 100 and selectionID <= #Global_FactionName + 100 then
		NPC_Test:JoinFaction(scene, npc, player, selectionID - 100)
		
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
			local npcPos = npc:GetPos()
			local randPosX, randPosY = npcPos:GetX() + math.random(randRange) - math.random(randRange), npcPos:GetY() + math.random(randRange) - math.random(randRange)
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
		
		
		NPC_Test:ShowDialog(npc, player, "Tạo BOT thành công!")
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
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Danh sách vật phẩm kèm Selection")
		dialog:AddSelection(111, "Selection 1")
		dialog:AddSelection(112, "Selection 2")
		dialog:AddItem(132, 100, 1)
		dialog:AddItem(132, 50, 0)
		dialog:AddItem(164, 1, 1)
		dialog:SetItemSelectable(false)
		dialog:Show(npc, player)
		return
	end
	-- ************************** --
	if selectionID == 111 and selectionID == 112 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Chọn Selection 1")
		dialog:Show(npc, player)
		return
	end
	-- ************************** --
	if selectionID == 12 then
		local dialog = GUI.CreateNPCDialog()
		dialog:AddText("Danh sách vật phẩm kèm Selection")
		dialog:AddSelection(111, "Selection 1")
		dialog:AddSelection(112, "Selection 2")
		dialog:AddItem(132, 100, 1)
		dialog:AddItem(132, 50, 0)
		dialog:AddItem(164, 1, 1)
		dialog:SetItemSelectable(true)
		dialog:Show(npc, player)
		return
	end
	-- ************************** --
	if selectionID == 13 then
		player:ChangeScene(25, 6945, 2784)
	end
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		selectedItemInfo: SelectItem - Vật phẩm được chọn
--		otherParams: Key-Value {number, string} - Danh sách các tham biến khác
-- ****************************************************** --
function NPC_Test:OnItemSelected(scene, npc, player, selectedItemInfo, otherParams)

	-- ************************** --
	local itemID = selectedItemInfo:GetItemID()
	local itemNumber = selectedItemInfo:GetItemCount()
	local binding = selectedItemInfo:GetBinding()
	-- ************************** --
	NPC_Test:ShowDialog(npc, player, string.format("Chọn vật phẩm ID: %d, Count: %d, Binding: %d!", itemID, itemNumber, binding))
	-- ************************** --

end

-- ======================================================= --
-- ======================================================= --
function NPC_Test:JoinFaction(scene, npc, player, factionID)
	
	-- ************************** --
	local ret = player:JoinFaction(factionID)
	-- ************************** --
	if ret == -1 then
		NPC_Test:ShowDialog(npc, player, "Người chơi không tồn tại!")
		return
	elseif ret == -2 then
		NPC_Test:ShowDialog(npc, player, "Môn phái không tồn tại!")
		return
	elseif ret == 0 then
		NPC_Test:ShowDialog(npc, player, "Giới tính của bạn không phù hợp với môn phái này!")
		return
	elseif ret == 1 then
		NPC_Test:ShowDialog(npc, player, "Gia nhập phái <color=blue>" .. player:GetFactionName() .. "</color> thành công!")
		return
	else
		NPC_Test:ShowDialog(npc, player, "Chuyển phái thất bại, lỗi chưa rõ!")
		return
	end
	-- ************************** --

end

-- ======================================================= --
-- ======================================================= --
function NPC_Test:GetValueTest(scene, npc, player)
	
	-- ************************** --
	return 1, 2
	-- ************************** --

end

-- ======================================================= --
-- ======================================================= --
function NPC_Test:ShowDialog(npc, player, text)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(text)
	dialog:Show(npc, player)
	-- ************************** --
	
end
function NPC_Test:PlayerSetLevel(npc, player, level)

	-- ************************** --
	player:SetLevel(level)
	NPC_Test:ShowDialog(npc, player, "Thiết lập cấp độ ".. level .." thành công")
	-- ************************** --

end
function NPC_Test:SetBelonging(npc, player, ItemID,Number,Series,LockStatus)

	if ItemID == nil then
		return
	end
	if Series == nil then
		return
	end
-- ************************** --
	Player.AddItemLua(player,ItemID,Number,Series,LockStatus,14)
	NPC_Test:ShowDialog(npc, player,"Nhận phẩm thành công theo hệ <color=red>Thành công !+14</color>")
	-- ************************** --
end
function NPC_Test:SetBelonging89(npc, player, ItemID,Number,Series,LockStatus)

	if ItemID == nil then
		return
	end
	if Series == nil then
		return
	end
	-- ************************** --
	Player.AddItemLua(player,ItemID,Number,Series,LockStatus,12)
	NPC_Test:ShowDialog(npc, player,"Nhận phẩm thành công theo hệ <color=red>Thành công !+12</color>")
	-- ************************** --
end
function NPC_Test:EquipEnhance(npc, player, ItemID,enhLevel)
	-- ************************** --
	Player.EquipEnhance(player, ItemID, enhLevel)-- ************************** --
end

