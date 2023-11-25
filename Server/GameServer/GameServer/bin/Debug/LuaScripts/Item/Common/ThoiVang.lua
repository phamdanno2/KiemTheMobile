local GETMONEYLEVEL = {
    [1] = {150000, 200000},
    [2] = {1500000, 2000000}
};
-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200005' bên dưới thành ID tương ứng
local ThoiVang = Scripts[200026]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function ThoiVang:OnPreCheckCondition(scene, item, player, otherParams)

    -- ************************** --
    -- player:AddNotification("Enter -> ThoiVang:OnPreCheckCondition")--
    -- ************************** --
    return true
    -- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi để thực thi Logic khi người sử dụng vật phẩm, sau khi đã thỏa mãn hàm kiểm tra điều kiện
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function ThoiVang:OnUse(scene, item, player, otherParams)

    local ItemLevel = item.GetItemLevel();

    local nGetMoney = GETMONEYLEVEL[ItemLevel][1];
    local nGetBindMoney = GETMONEYLEVEL[ItemLevel][2];

    local szMsg = string.format(
        "Bạn sử dụng <color=yellow>%s</color> có thể nhận được 1 trong 2:\n\n    Chọn 1 nhận được <color=yellow>%s Bạc</color>\n    Chọn 2 nhận được <color=yellow>%s Bạc khóa</color>\n\nChọn 1 trong 2, Hãy cân nhắc thật kĩ trước khi chọn , bạn muốn đổi lấy gì?",
        item.GetName(), nGetMoney, nGetBindMoney);
    local dialog = GUI.CreateItemDialog()

    dialog:AddText(szMsg)

    dialog:AddSelection(1, "Đổi bạc thường")
    dialog:AddSelection(2, "Đổi bạc khóa")
    dialog:AddSelection(3, "Để ta suy nghĩ đã")

    dialog:Show(item, player)
end
function ThoiVang:OnSelection(scene, item, player, selectionID, otherParams)

    if selectionID == 1  then
		local szType = "Bạc";
        local ItemLevel = item.GetItemLevel();
		
		-- player:AddNotification("Level  vật phẩm " .. ItemLevel .. " và lựa chọn của bạn" .. selectionID .. "")
        local MoneyWillBeAdd = GETMONEYLEVEL[ItemLevel][selectionID];

        local MoneyType = 1;		
		Player.RemoveItem(player, item:GetID())

        if Player.AddMoney(player, MoneyWillBeAdd, MoneyType) then
			player:AddNotification("Sử dụng " .. item:GetName() .. " thành công nhận được " .. (MoneyWillBeAdd/10000) .. " vạn " .. szType .. ".")
			GUI.CloseDialog(player)
        else
            player:AddNotification("Có lỗi khi sử dụng vật phẩm " .. item:GetName() .. "")
			GUI.CloseDialog(player)
        end
		return
    	
	end
    if  selectionID == 2 then
		local szType = "Bạc khóa";
        local ItemLevel = item.GetItemLevel();
        local MoneyWillBeAdd = GETMONEYLEVEL[ItemLevel][selectionID];
        local MoneyType = 0;		
		Player.RemoveItem(player, item:GetID())

        if Player.AddMoney(player, MoneyWillBeAdd, MoneyType) then
			player:AddNotification("Sử dụng " .. item:GetName() .. " thành công nhận được " .. (MoneyWillBeAdd/10000) .. " vạn " .. szType .. ".")
			GUI.CloseDialog(player)
        else
            player:AddNotification("Có lỗi khi sử dụng vật phẩm " .. item:GetName() .. "")
			GUI.CloseDialog(player)
        end
		return
    	
	end
    item:FinishUsing(player)

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function ThoiVang:OnItemSelected(scene, item, player, itemID)

    -- ************************** --

    -- ************************** --

end
