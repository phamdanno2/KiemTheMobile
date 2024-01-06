-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200005' bên dưới thành ID tương ứng
local TuiTanThu = Scripts[000000]
function TuiTanThu:OnPreCheckCondition(scene, item, player, otherParams)
    return true
end

function TuiTanThu:OnOpen(scene, item, player, otherParams)
	local dialog = GUI.CreateNPCDialog()
	dialog:AddText("Xin chào "..player:GetName().."\nĐua top <color=red>Cấp độ</color> từ <color=green>20h00 17/11/2023 đến 23h00 24/12/2023</color>\nĐua top <color=red>Tài Phú</color> từ <color=green>20h00 17/11/2023 đến 23h00 31/12/2023</color> !")

	if player:GetFactionID()==0 then
		dialog:AddSelection(1,"Gia nhập Môn Phái.")
	else
		dialog:AddSelection(40008, "GiftCode")
	end	
	dialog:AddSelection(77777, "Kết thúc đối thoại")
	dialog:Show(item, player)
end

function TuiTanThu:ItemSet()
end

function TuiTanThu:OnSelection(scene, item, player, selectionID, otherParams)

	local dialog = GUI.CreateNPCDialog()
	local TotalPrestige = player:GetPrestige()
	if selectionID == 77777 then
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
	---------------gia nhập môn phái
	if selectionID == 1 then
		local dialog = GUI.CreateNPCDialog()
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
