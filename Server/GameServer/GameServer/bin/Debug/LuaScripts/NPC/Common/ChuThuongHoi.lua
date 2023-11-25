-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000035' bên dưới thành ID tương ứng
local ChuThuongHoi = Scripts[000035]

--****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function ChuThuongHoi:OnOpen(scene, npc, player, otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText(""..npc:GetName().." Giúp ta hoành thành 40 nhiệm vụ ngưới có thể nhận được rất nhiều bạc khóa và huyền tinh.</br>Tham gia nhiệm vụ Thương hội phải thỏa mãn nhưng điều kiện sau: <br> <color=#7ce8f8>1:Đạt cấp 60 <br>2:Uy danh giang hồ đạt 50 điểm. <br>3:Hoàn thành nhiệm vụ chính tuyến vl50. <br>4:Mỗi tuần chỉ được nhận 1 nhiệm vụ Thương Hội.</color> ")
	dialog:AddSelection(1, "Giới thiệu nhiệm vụ thương hội")
	 dialog:AddSelection(2, "Nhận thư thương hội")
	dialog:AddSelection(4, "Nhận hộp thu thập Lệnh bài thương hội") 
	dialog:AddSelection(5, "Ta muốn nói chuyện khác")
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
function ChuThuongHoi:OnSelection(scene, npc, player, selectionID,otherParams)

	-- ************************** --
	local dialog = GUI.CreateNPCDialog()
	if selectionID == 1 then
		dialog:AddText(""..npc:GetName().." Giúp ta hoành thành 40 nhiệm vụ ngưới có thể nhận được rất nhiều bạc khóa và huyền tinh.</br>Tham gia nhiệm vụ Thương hội phải thỏa mãn nhưng điều kiện sau: <br> <color=#7ce8f8>1:Đạt cấp 60 <br>2:Uy danh giang hồ đạt 50 điểm. <br>3:Hoàn thành nhiệm vụ chính tuyến vl50. <br>4:Mỗi tuần chỉ được nhận 1 nhiệm vụ Thương Hội.</color> <br> Nhiệm vụ Thương Hội có các nhiệm vụ sau :<br><color=#7ce8f8>1:Đưa thư : Đưa giấy viết thư của Thương Hội đến chỗ người chỉ định (Người Liên Lạc Thương Hội sẽ xuất hiện trong bản đồ chỉ định)<br>2.Tìm vật: Thu thập đạo cụ chỉ định cho Thương hội (nhập được trong các hoạt động tương ứng)<br>3.Sưu tấm : Địa chỉ điểm chỉ định sưu tầm vật ohaarm chỉ định (vật phẩm được quái vật bảo vệ , phải đi cùng tổ đội) ")
		dialog:AddSelection(5, "Kết thúc đối thoại")
		dialog:Show(npc, player)
	end
	if selectionID == 2 or selectionID == 3 or selectionID == 4 then
		dialog:AddText("Chức năng chưa mở")
		dialog:Show(npc, player)
	end 
	if  selectionID == 5 then
		GUI.CloseDialog(player)
	end
	-- ************************** --
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function ChuThuongHoi:OnItemSelected(scene, npc, player, itemID,otherParams)

	-- ************************** --

	-- ************************** --

end