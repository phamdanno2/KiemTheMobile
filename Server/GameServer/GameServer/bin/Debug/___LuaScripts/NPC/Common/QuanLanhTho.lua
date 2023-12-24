-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000016' bên dưới thành ID tương ứng
local QuanLanhTho = Scripts[000016]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function QuanLanhTho:OnOpen(scene, npc, player, otherParams)


    local State = Player.CheckGuildWarState();
	
	local TxtAdd = ""
	
	if State == 0 then
		TxtAdd = "Hoạt động tranh đoạt chưa bắt đầu vui lòng quay lại sau."
	elseif State == 1 or State == 2 then
		TxtAdd = "Thời gian tuyên chiến đã bắt đầu."
	elseif State == 3 or State == 4 then
		TxtAdd = "Đang trong giai đoạn chinh chiến."
	elseif State == 5 then
		TxtAdd = "Thời gian nhận thưởng."
	end
	
	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Kèn báo hiệu trận chiến lãnh thổ đã cất lên , hãy tập trung lực lượng và chuẩn bị chiến đấu <br>Mỗi tuần vào <color=green> 20:00-21:30 thứ 7 và chủ nhật </color>, các Bang hội có thể tấn công các lạnh thổ trong trò chơi , sau khi chiếm được lãnh thổ có thể nhận danh vọng , bạc khóa , huyền tinh và các loại trang bị thần bí khác.<br><br><b><color=#ff4800>"..TxtAdd.."</color></b>")
	dialog:AddSelection(1, "<color=#fdf200>Từ điển tranh đoạt lãnh thổ tranh đoạt chiếm</color>")
	dialog:AddSelection(2, "Quân nhu Lãnh Thổ chiến")

	if State == 1 or State == 2 then
		dialog:AddSelection(4, "<b><color=#1aff00>Tuyên Chiến</color></b>")
	elseif State == 3 or State == 4 then
		TxtAdd = "Đang trong giai đoạn chinh chiến."
	elseif State == 5 then
		dialog:AddSelection(3, "<b><color=#1aff00>Phần thưởng chinh chiến</color></b>")
	end


	dialog:AddSelection(6, "Mua trang bị danh vọng Lãnh thổ chiến")
	dialog:AddSelection(7, "Ta không mua nữa")
	dialog:Show(npc, player)
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function QuanLanhTho:OnSelection(scene, npc, player, selectionID, otherParams)
	
	-- ************************** --
	 
	local dialog = GUI.CreateNPCDialog()
	
	-- Nếu ấn vào tuyên chiến
	if selectionID == 4 or selectionID == 3  then
		Player.NpcGuildWarBattle(npc,player)
	end
	
	if selectionID == 1 or selectionID == 2 or selectionID == 5  then
		dialog:AddText("Chức năng chưa mở")
		dialog:Show(npc, player)
	end
	--[[ if selectionID == 41 then 
		dialog:AddText("Trong thời gian Lãnh thổ tranh đoạt chiến , thành viên bang đều được nhận quân nhu.")
		dialog:AddSelection(410,"Nhận quân nhu")
		dialog:AddSelection(411,"Nếu muốn nhân nhiều hơn , cần bang chủ chủ trong thời gian Lãnh thổ tranh đoạt")
		dialog:AddSelection(412,"Thiết lập thêm quân nhu bang hội")
		dialog:AddSelection(413,"Trở về")
		dialog:AddSelection(147,"Kết thúc đối thoại")
		dialog:Show(npc, player)
	end
	if selectionID == 410 then
		dialog:AddText("Chỉ có lúc tuyền chiến và chinh chiến (20:00-21:30)")
		dialog:AddSelection(147,"Kết thúc đối thoại")
		dialog:Show(npc, player)
	end
	if selectionID == 411 then
		dialog:AddText("Giang hồ hiểm ác, hệ thống phúc lợi kiếm hiệp tặng thuốc cho bạn, hiệp sĩ phải mang theo, để đảm bảo an toàn.<br>Hôm nay bạn có thể nhận được <color=#00F089> i rương thuốc miễn phí </color>")
		dialog:AddSelection(4111,"Hồi Huyết Đơn-Rương")
		dialog:AddSelection(4112,"Hồi Nội Đơn-Rương")
		dialog:AddSelection(4113,"Càn khôn Tạo Hóa Hoàn-Rương")
		dialog:AddSelection(147,"Ta chỉ đến xem")
		dialog:Show(npc, player)
	end
	if selectionID == 412 then
		dialog:AddText("Chỉ có bang chủ mới được thiết lập ngạch quân nhu")
		dialog:AddSelection(147,"Kết thúc đối thoại")
		dialog:Show(npc, player)
	end
	-- ************************** --
	if selectionID == 42 then
		dialog:AddText("bla bla<color=#00F089> i <color>")
		dialog:AddSelection(421,"Nhận thưởng danh vọng")
		dialog:AddSelection(422,"Nhận thưởng điểm tích lũy")
		dialog:AddSelection(423,"Nhận thưởng xếp hạng")
		dialog:AddSelection(424,"nhận quân hưởng")
		dialog:AddSelection(147,"Ta chỉ đến xem")
		dialog:Show(npc, player)
	end
	
	-- ************************** --
	if selectionID == 43 then
		dialog:AddText("bla bla<color=#00F089> i <color>")
		dialog:AddSelection(431,"Nhận thưởng danh vọng")
		dialog:AddSelection(432,"Nhận thưởng điểm tích lũy")
		dialog:AddSelection(433,"Nhận thưởng xếp hạng")
		dialog:AddSelection(434,"nhận quân hưởng")
		dialog:AddSelection(147,"Ta chỉ đến xem")
	dialog:Show(npc, player)
	end
	-- ************************** --
	if selectionID == 45 then
		dialog:AddText("bla bla<color=#00F089> i <color>")
		dialog:Show(npc, player)
	end
		-- ************************** --]]
	if selectionID == 6 then
		Player.OpenShop(npc, player, 147)
        GUI.CloseDialog(player)			
	end
	if selectionID == 7 then
		GUI.CloseDialog(player)		
	end
end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function QuanLanhTho:OnItemSelected(scene, npc, player, itemID, sotherParams)

	-- ************************** --
	
	-- ************************** --

end