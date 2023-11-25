-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000151' bên dưới thành ID tương ứng
local CoPhongHa = Scripts[000151]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào NPC
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function CoPhongHa:OnOpen(scene, npc, player, otherParams)

	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Nhân chi sơ, Tính bản thiện . Ta có thể giúp gì cho nhà ngươi ")
	dialog:AddSelection(5, "<color=#fbe66f>[Hỗ Trợ]</color> <color=#06f455>Vũ Khí Tân Thủ</color>.")
	--[[ dialog:AddSelection(1, "<color=#fbe66f>[Hệ thống]</color> <color=#06f455>Nhận đồng trên web</color>.")
	dialog:AddSelection(2, "<color=#51e1fb>Nhận thưởng hoạt động </color>.") ]]
	dialog:AddSelection(100, "<color=#51e1fb>Kết thúc đối thoại </color>.")
	dialog:Show(npc, player)					

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi NPC thông qua NPC Dialog
--		scene: Scene - Bản đồ hiện tại
--		npc: NPC - NPC tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function CoPhongHa:OnSelection(scene, npc, player, selectionID, otherParams)
	local dialog = GUI.CreateNPCDialog()
--[[ 	if selectionID == 1 then
		dialog:AddText("Xin chào "..player:GetName().." ,Hình như bạn không có nạp trên hệ thống nên không có đồng cho bạn rút!!!")
		dialog:AddSelection(100, "<color=#51e1fb>Kết thúc đối thoại </color>.")
		dialog:Show(npc, player)
	end	
	if selectionID ==2 then
		dialog:AddText("Sau đây là danh sách các hoạt động mà bạn có thể nhận được thưởng")
		--Chức năng chưa mở hoạt động --
		dialog:AddSelection(100, "<color=#51e1fb>Kết thúc đối thoại </color>.")
		
		dialog:Show(npc, player)
	end ]]
	if selectionID == 5 then
		--Thieu Lam
		if player:GetFactionID() == Global_FactionID.None then
			SuGiaSuKien:ShowDialog(npc, player, "Bằng hữu <color=red>Chưa gia nhập môn phái</color>!")
		end 
		if player:GetFactionID() == Global_FactionID.ShaoLin then
			Player.AddItemLua(player, 1390, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1391, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1392, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1393, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1394, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1395, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1396, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1397, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1398, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1399, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1400, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1401, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1402, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1403, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1404, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1404, 1, -1, 1,0,-1)
		end

		--Thiên vương
		if player:GetFactionID() == Global_FactionID.TianWang then
			Player.AddItemLua(player, 1410, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1411, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1412, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1413, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1414, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1415, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1416, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1417, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1418, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1419, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1408, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1409, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1406, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1405, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1418, 1, -1, 1,0,-1)
		end

		--Đường môn
		if player:GetFactionID() == Global_FactionID.TangMen then
			--Vũ Khí 
			Player.AddItemLua(player, 3853, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 3843, 1, -1, 1,0,-1)
			
			Player.AddItemLua(player, 8156, 1, -1, 1,0,-1) --Nón Trục Lộc
			Player.AddItemLua(player, 7737, 1, -1, 1,0,-1)	-- Áo Vũ Uy
			Player.AddItemLua(player, 8121, 1, -1, 1,0,-1)  -- trục lọc viêm đế yêu đái
			Player.AddItemLua(player, 8177, 1, -1, 1,0,-1) -- thủy hoàng hộ uyển
			Player.AddItemLua(player, 8537, 1, -1, 1,0,-1)	--Tiêu Dao Hậu Nghệ Lữ
			Player.AddItemLua(player, 8566, 1, -1, 1,0,-1) --Liên Trục Lộc 
			Player.AddItemLua(player, 8686, 1, -1, 1,0,-1) -- Nhẫn Vũ Uy
			Player.AddItemLua(player, 8781, 1, -1, 1,0,-1) -- Hàn Băng Bội
			Player.AddItemLua(player, 8851, 1, -1, 1,0,-1) -- Sát Thần Phù
			-- Player.AddItemLua(player, 1463, 1, -1, 1,0,-1)
			-- Player.AddItemLua(player, 1464, 1, -1, 1,0,-1)
			-- Player.AddItemLua(player, 1470, 1, -1, 1,0,-1)
			-- Player.AddItemLua(player, 1471, 1, -1, 1,0,-1)
			-- Player.AddItemLua(player, 1472, 1, -1, 1,0,-1)
			-- Player.AddItemLua(player, 1473, 1, -1, 1,0,-1)
			-- Player.AddItemLua(player, 1474, 1, -1, 1,0,-1)
			-- Player.AddItemLua(player, 1465, 1, -1, 1,0,-1)
		end

		--Ngũ độc
		if player:GetFactionID() == Global_FactionID.WuDu then
			Player.AddItemLua(player, 1450, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1451, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1452, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1452, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1453, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1454, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1445, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1446, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1447, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1448, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1449, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1455, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1456, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1457, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1458, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1459, 1, -1, 1,0,-1)
		end

		--Nga my
		if player:GetFactionID() == Global_FactionID.EMei then
			Player.AddItemLua(player, 1420, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1422, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1422, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1423, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1424, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1425, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1426, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1427, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1428, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1429, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1430, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1431, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1432, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1433, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1434, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1434, 1, -1, 1,0,-1)
		end

		--Thúy yên
		if player:GetFactionID() == Global_FactionID.CuiYan then
			Player.AddItemLua(player, 1435, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1436, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1437, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1438, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1439, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1440, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1441, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1442, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1443, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1444, 1, -1, 1,0,-1)
		end

		--Cái bang
		if player:GetFactionID() == Global_FactionID.GaiBang then
			Player.AddItemLua(player, 1480, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1481, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1482, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1483, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1484, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1485, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1486, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1487, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1488, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1489, 1, -1, 1,0,-1)
		end

		--Thiên nhẫn
		if player:GetFactionID() == Global_FactionID.TianRen then
			Player.AddItemLua(player, 1495, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1496, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1497, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1498, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1499, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1490, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1491, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1492, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1493, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1494, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1500, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1501, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1502, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1503, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1504, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1502, 1, -1, 1,0,-1)
		end

		--Võ đang
		if player:GetFactionID() == Global_FactionID.WuDang then
			Player.AddItemLua(player, 1510, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1511, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1512, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1513, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1514, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1505, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1506, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1507, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1507, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1508, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1509, 1, -1, 1,0,-1)
		end
		
		--Côn lôn
		if player:GetFactionID() == Global_FactionID.KunLun then
			Player.AddItemLua(player, 1515, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1516, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1517, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1518, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1519, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1519, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1520, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1521, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1522, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1523, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1524, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1525, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1526, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1527, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1528, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1529, 1, -1, 1,0,-1)
		end

		--Đại Lý Đoàn Thị
		if player:GetFactionID() == Global_FactionID.DaLiDuanShi then
			Player.AddItemLua(player, 1583, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1584, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1585, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1586, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1587, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1588, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1589, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1590, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1591, 1, -1, 1,0,-1)
			Player.AddItemLua(player, 1592, 1, -1, 1,0,-1)
		end

		--MinhGiao
		if player:GetFactionID() == Global_FactionID.MingJiao then
			
		end
		GUI.CloseDialog(player)
	end	
	
	if selectionID == 100 then
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
function CoPhongHa:OnItemSelected(scene, npc, player, itemID, otherParams)

	-- ************************** --
	
	-- ************************** --

end