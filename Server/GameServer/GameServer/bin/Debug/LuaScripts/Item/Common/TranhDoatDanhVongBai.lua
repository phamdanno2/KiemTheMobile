-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200005' bên dưới thành ID tương ứng
local ADD_SHENGWANG = {100, 500, 1000}
local CAMP = 801;
local POINT =1;
local TranhDoatDanhVongBai = Scripts[200072]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function TranhDoatDanhVongBai:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> TranhDoatDanhVongBai:OnPreCheckCondition")--
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
function TranhDoatDanhVongBai:OnUse(scene, item, player, otherParams)

	-- ************************** --
	-- ************************** --
	local nLevel = item:GetItemLevel()

	if (nLevel ~=1) then
		player:AddNotification("Vật phẩm không hợp lệ!")
		return;

	end

	if player:GetFactionID()==0 then
		player:AddNotification("Chưa nhập môn phái!")
		return;
	end
	-- số ngày n ăn được bao nhiêu 
	local GetValue = Player.GetValueOfDailyRecore(player,item:GetItemID())
	

	if(GetValue==-1) then
		GetValue = 0;
	end

	Player.SetValueOfDailyRecore(player,item:GetItemID(),GetValue+1)

	local szFactionName = player:GetFactionName();

	-- lấy hế
	--local DBID = 600 + player:GetSeries();
	--loại lệnh bài
	 POINT = ADD_SHENGWANG[nLevel];
	--AddRetupeValue : cậu lệnh ăn lệnh bài
	
	for key,value in pairs(Global_CAMP) do
		if CAMP == key then
			Player.AddRetupeValue(player,CAMP,POINT)
			Player.RemoveItem(player,item:GetID())
			player:AddNotification("Hôm nay đã dùng "..(GetValue+1).." =>"..item:GetName().." + "..POINT.." "..value.NameActivity.."");
		
		end
	end
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function TranhDoatDanhVongBai:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> TranhDoatDanhVongBai:OnSelection, selectionID = " .. selectionID)--
	
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
function TranhDoatDanhVongBai:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end