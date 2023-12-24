-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000007' bên dưới thành ID tương ứng
local HieuUyMoBinhMongCoTayHa = Scripts[000007]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function HieuUyMoBinhMongCoTayHa:OnOpen(scene, npc, player, otherParams)

	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Ở đây không có tính năng ngươi muốn tham gia ,hãy đến thành khác xem thử ! <br><color=green> 21 giờ và 23 giờ tăng thêm 1 trận chiến trường Phượng Tượng , có thể đến chỗ báo danh ở Đại lý báo danh tham gia.<color>")
	dialog:AddSelection(1, "Giới thiệu tính năng Mông Cổ Tây Hạ")				
	dialog:AddSelection(2, "Bảng Vinh Dự")				
	dialog:AddSelection(3, "Để ta sũy nghĩa đã")				
	dialog:Show(npc, player)					

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function HieuUyMoBinhMongCoTayHa:OnSelection(scene, npc, player, selectionID,otherParams)
	local dialog = GUI.CreateNPCDialog()

	if selectionID == 1 then 
		dialog:AddText(npc:GetName()":<br>*Chiến trường Mông Cổ tây Hạ mới có thêm cơ chế trợ công , người trợ công có thể nhận được 20% điểm của người tiêu diêt <br>* Sau 2 tháng mở server sẽ không căn cứ cấp của người chơi để xếp vào chiến trường Dương Châu hay Phượng Tường , mà căn cứ vào cấp tài phú : từ 3-4 tháng người chơi cóHốn Thiên trở lên mới được vào chiến trường Phượng Tường<br>*Khi Báo danh sẽ tăng thêm cơ chế cận Bằng Sức chiên đấu của hai phe chênh lệch nhau từ 5000trở lên , thì không thể báo danh vào phe có sức chiến đấu cao hơn <br>* Mỗi ngày vào lúc 13 giờ mở Chiến Trường Mông Cổ Tây Hạ sẽ điều chỉnh thành dạng không thể tổ đội<br>*Số lượng Đao tệ và giá bán danh hiệu khi chiến trường cũng được thay đổi<br>* Mỗi ngày hệ thống sẽ chọn 23 giờ và ngẫu nhiên 1 trận chiến trường Mông Cổ Tây Hạ (chỉ dành cho Phượng Tường) làm <color=yellow> Chiến Trường Vinh Dự <color> , Sau khi 2 trận này kết thúc, có thể dùng điểm đổi [hộp quà Vinh Dự Mông Cổ Tây Hạ] , và [Huy Chương Vinh Dự Mông Cổ Tây Hạ ], Khi mở quà được nhận thưởng ngẫu nhiên , nộp huy chương có thể giam gia xếp hạng mỗi tuần")
		dialog:AddSelection(3,"Kết thúc đối thoại")	
		dialog:Show(npc, player)
	elseif selectionID == 2 then
		dialog:AddText("TODO")
	elseif selectionID == 3 then
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
function HieuUyMoBinhMongCoTayHa:OnItemSelected(scene, npc, player, itemID,otherParams)

	-- ************************** --

	-- ************************** --

end