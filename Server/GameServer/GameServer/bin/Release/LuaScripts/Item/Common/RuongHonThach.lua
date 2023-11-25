-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200005' bên dưới thành ID tương ứng
local tbLevel = {
    [1] = 100,
    [2] = 1000
}

local RuongHonThach = Scripts[200028]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function RuongHonThach:OnPreCheckCondition(scene, item, player, otherParams)

    -- ************************** --
    -- player:AddNotification("Enter -> RuongHonThach:OnPreCheckCondition")--
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
function RuongHonThach:OnUse(scene, item, player, otherParams)

    local ItemLevel = item:GetItemLevel();

    local Value = tbLevel[ItemLevel];
    -- ************************** --
    -- ************************** --
    local dialog = GUI.CreateItemDialog()
    dialog:AddText("Bạn có chắc chắn muôn sử dụng mở rương để nhận :" .. Value ..
                       " Ngũ Hành Hồn Thạch?")

    dialog:AddSelection(1, "Ta đồng ý")
    dialog:AddSelection(2, "Ta suy nghĩ đã")

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
function RuongHonThach:OnSelection(scene, item, player, selectionID, otherParams)

    local ItemLevel = item:GetItemLevel();
    local Value = tbLevel[ItemLevel];

    if (selectionID == 1) then

        if Player.AddItemLua(player, 506, Value,-1, 1) then

            Player.RemoveItem(player, item:GetID())

        else
			player:AddNotification("Có lỗi khi sử dụng vật phẩm")
        end

		GUI.CloseDialog(player)

    else
        if selectionID == 2 then
            GUI.CloseDialog(player)
        end

    end

  
end

  -- ****************************************************** --
    --	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi vật phẩm thông qua Item Dialog
    --		scene: Scene - Bản đồ hiện tại
    --		item: Item - Vật phẩm tương ứng
    --		player: Player - NPC tương ứng
    --		itemID: number - ID vật phẩm được chọn
    -- ****************************************************** --
    function RuongHonThach:OnItemSelected(scene, item, player, itemID)

        -- ************************** --

        -- ************************** --

    end
