    -- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200077' bên dưới thành ID tương ứng
local ADD_SHENGWANG = {8600,44000,57000,1000,500,5000,4800}
local USED_LIMIT	= 100 ;
local DuLongDanhVongLenh = Scripts[200077]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function DuLongDanhVongLenh:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> DuLongDanhVongLenh:OnPreCheckCondition")--
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
function DuLongDanhVongLenh:OnUse(scene, item, player, otherParams)

	-- ************************** --
	-- ************************** --
	local nLevel = item:GetItemLevel()

	if (nLevel < 1 or nLevel > 7 ) then
		player:AddNotification("Vật phẩm không hợp lệ!")
		return;

	end
	if player:GetFactionID()==0 then
		player:AddNotification("Chưa nhập môn phái!")
		return;
	end

	local GetValue = Player.GetValueOfDailyRecore(player,item:GetItemID())
	if (GetValue >= USED_LIMIT) then 
		player:AddNotification("Mỗi ngày chỉ có thể sử dụng 100 vật phẩm này")
		return;
	end

	if(GetValue==-1) then
		GetValue = 0;
	end
	Player.SetValueOfDailyRecore(player,item:GetItemID(),GetValue+1)
	local szFactionName = player:GetFactionName();
	
	local POINT = ADD_SHENGWANG[nLevel];
	
    if(nLevel == 1 ) then
        Player.AddRetupeValue(player,504,POINT)
    elseif(nLevel == 2 ) then
        Player.AddRetupeValue(player,801,POINT)
    elseif(nLevel == 3) then
        Player.AddRetupeValue(player,701,POINT)
    elseif(nLevel == 4 ) then
        Player.AddRetupeValue(player,502,POINT)
    elseif(nLevel == 5 ) then
        Player.AddRetupeValue(player,1001,POINT)
    elseif(nLevel == 6 ) then
        Player.AddRetupeValue(player,505,POINT)
    elseif(nLevel == 7 ) then
        Player.AddRetupeValue(player,1101,POINT)
    end
        Player.RemoveItem(player,item:GetID())
        player:AddNotification("Hôm nay đã dùng "..(GetValue+1).."==> "..item:GetName().."+"..POINT.."");

	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function DuLongDanhVongLenh:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> DuLongDanhVongLenh:OnSelection, selectionID = " .. selectionID)--
	
	-- ************************** --
	
	item:FinishUsing(player)
	

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi chọn một trong các vật phẩm, và ấn nút Xác nhận cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		itemID: number - ID vật phẩm được chọn
-- ****************************************************** --
function DuLongDanhVongLenh:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end