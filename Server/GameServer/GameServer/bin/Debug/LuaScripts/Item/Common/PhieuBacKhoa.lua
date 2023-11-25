local PhieuBacKhoa = Scripts[200032]

-- ************************** --
local MoneyType = 0
function PhieuBacKhoa:OnPreCheckCondition(scene, item, player, otherParams)

    -- ************************** --
    return true
    -- ************************** --

end

function PhieuBacKhoa:OnUse(scene, item, player, otherParams)
		-- Lượng tiền nhận được
		local amount = item:GetItemValue()
		-- Xóa vật phẩm
		Player.RemoveItem(player, item:GetID())
		-- Thêm tiền tương ứng
		Player.AddMoney(player, amount, MoneyType)
		-- Thông báo
		player:AddNotification("Sử dụng " .. item:GetName() .. " thành công nhận được " .. item:GetItemValue() .. ".")
		-- ************************** --
		-- Đóng khung
		GUI.CloseDialog(player)
end

function PhieuBacKhoa:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	if selection == 2 then
		GUI.CloseDialog(player)
		return
	end
	-- ************************** --
	if selection == 1 then
		-- Lượng tiền nhận được
		local amount = item:GetItemValue()
		-- Xóa vật phẩm
		Player.RemoveItem(player, item:GetID())
		-- Thêm tiền tương ứng
		Player.AddMoney(player, amount, MoneyType)
		-- Đóng khung
		GUI.CloseDialog(player)
		return
    end
	-- ************************** --

end
function PhieuBacKhoa:OnItemSelected(scene, item, player, itemID)

    -- ************************** --

    -- ************************** --

end
