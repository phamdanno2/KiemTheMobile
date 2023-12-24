-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200077' bên dưới thành ID tương ứng
local GETMONEYLEVEL = {
    [1] = {120000,2000},
    [2] = {120000,2000}
};
local POINT = 2;

local MAXCOUNT=20;
local TrungDuLong = Scripts[200081]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function TrungDuLong:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> TrungDuLong:OnPreCheckCondition")--
	-- ************************** --
	return true
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function TrungDuLong:OnUse(scene, item, player, otherParams)

	-- ************************** --
	-- ************************** --
    local ItemLevel = item.GetItemLevel();

    local nGetMoney = GETMONEYLEVEL[ItemLevel][1];
    local nGetBindMoney = GETMONEYLEVEL[ItemLevel][2];

    local szMsg = string.format(
        "Bạn sử dụng <color=yellow>%s</color> có thể nhận được 1 trong 2:\n\n    <color=yellow>%s Bạc Khóa</color>\n    <color=yellow>%s Đồng khóa</color>\n\nChọn 1 trong 2, bạn muốn đổi lấy gì?",
        item.GetName(), nGetMoney, nGetBindMoney);
    -- ************************** --
    -- ************************** --
    local dialog = GUI.CreateItemDialog()

    dialog:AddText(szMsg)

    dialog:AddSelection(1, "Đổi bạc Khóa")
    dialog:AddSelection(2, "Đổi Đồng khóa")
    dialog:AddSelection(3, "Để ta suy nghĩ đã")

    dialog:Show(item, player)
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function TrungDuLong:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
    local GetValue = Player.GetValueOfDailyRecore(player,item:GetItemID())
	--player:AddNotification("Enter -> TrungDuLong:OnSelection, selectionID = " .. selectionID)--
    if selectionID == 1 or selectionID == 2 then
        local ItemLevel = item.GetItemLevel();

        local MoneyWillBeAdd = GETMONEYLEVEL[ItemLevel][selectionID];

        local szType = "Bạc Khóa";
        if selectionID == 2 then
            szType = "Đồng khóa";
        end

        local szMsg = string.format("Bạn xác nhận đổi <color=yellow>%s %s</color> không?", MoneyWillBeAdd,
            szType);
        -- ************************** --
        -- ************************** --
        local dialog = GUI.CreateItemDialog()

        dialog:AddParam(1, selectionID);

        dialog:AddText(szMsg)

        dialog:AddSelection(4, "Ta đồng ý")
        dialog:AddSelection(5, "Để ta suy nghĩ đã")

        dialog:Show(item, player)

    end
	-- ************************** --
    if selectionID == 4 then

        local ItemLevel = item.GetItemLevel();
		local Prestige = (player:GetPrestige()+POINT);
        for key, value in pairs(otherParams) do
            System.WriteToConsole(string.format("[%d] = %s", key, value))
        end

        if (GetValue >= MAXCOUNT) then -- một ngày có thể ăn được bao nhiêu lệnh bài
            player:AddNotification("Mỗi ngày chỉ có thể sử dụng "..MAXCOUNT.." vật phẩm này")
            return;
        end

        local Type = tonumber(otherParams[1]);

        System.WriteToConsole(Type)

        local MoneyWillBeAdd = GETMONEYLEVEL[ItemLevel][Type];

        local MoneyType = 0;

        if Type == 2 then
            MoneyType = 3
        end
          Player.SetValueOfDailyRecore(player,item:GetItemID(),GetValue+1)
		  Player.RemoveItem(player, item:GetID())
        if Player.AddMoney(player, MoneyWillBeAdd, MoneyType) then
			player:SetPrestige(Prestige) 
			GUI.CloseDialog(player)
        else
            player:AddNotification("Có lỗi khi sử dụng vật phẩm")
			GUI.CloseDialog(player)
        end
        if MoneyType == 0 then
            player:AddNotification("Hôm nay đã dùng "..(GetValue+1).."==> <color=red>"..item:GetName().."</color>+ "..MoneyWillBeAdd.."Bạc Khóa +2 điểm Uy danh giang hồ");
        else if MoneyType == 3 then
             player:AddNotification("Hôm nay đã dùng "..(GetValue+1).."==> <color=red>"..item:GetName().."</color>+ "..MoneyWillBeAdd.."Đồng khóa +2 điểm Uy danh giang hồ");
        else
            player:AddNotification("Có lỗi khi sử dụng vật phẩm")
            GUI.CloseDialog(player)
        end
        end
    end
    if selectionID == 3 or selectionID == 5 then
        GUI.CloseDialog(player)
    end

 
	-- ************************** --
end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function TrungDuLong:OnItemSelected(scene, item, player, itemID,otherParams)

	-- ************************** --
  

    -- ************************** --

end