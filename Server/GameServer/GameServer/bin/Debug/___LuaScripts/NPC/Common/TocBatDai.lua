-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000138' bên dưới thành ID tương ứng
local TocBatDai = Scripts[000142]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function TocBatDai:OnOpen(scene, npc, player,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(""..npc:GetName().."---"..player:GetName().."<br><color=#eaff00>:Người Mông Cổ thích kết bạn với người dũng cảm,</color> thẳng thắn. Ngạc Luân Hà Nguyên hoan nghênh các hiệp sĩ từ cấp 100 trở lên ở các máy chủ mờ từ 126 ngày trở lên đến khiêu chiến. Nếu chiến thắng sẽ nhận được phần thưởng rất hậu hĩnh!")
	dialog:AddSelection(1, "Kết thúc đối thoại.")
	
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
function TocBatDai:OnSelection(scene, npc, player, selectionID, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 1 then
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
function TocBatDai:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --

	-- ************************** --

end


