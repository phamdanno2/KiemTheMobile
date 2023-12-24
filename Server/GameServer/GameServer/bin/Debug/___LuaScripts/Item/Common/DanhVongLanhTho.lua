-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '200005' bên dưới thành ID tương ứng
local USED_LIMIT = 500 ;
local CAMP = 801;
local Data = 
{
	{500, 10},
	{200, 12},
	{100, 14},
}
local DanhVongLanhTho = Scripts[200073]

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn sử dụng vật phẩm, kiểm tra điều kiện có được dùng hay không
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể sử dụng, False nếu không thỏa mãn
-- ****************************************************** --
function DanhVongLanhTho:OnPreCheckCondition(scene, item, player, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> DanhVongLanhTho:OnPreCheckCondition")--
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
function DanhVongLanhTho:OnUse(scene, item, player, otherParams)

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
	if (GetValue >= USED_LIMIT) then -- một ngày có thể ăn được bao nhiêu lệnh bài
		player:AddNotification("Mỗi ngày chỉ có thể sử dụng 500 vật phẩm này")
		return;
	end
	local nAddTimes = 1;
	local POINT = DanhVongLanhTho:GetRepute(GetValue, nAddTimes);
	player:AddNotification("Hôm nay đã dùng "..POINT.." ")
	if POINT <= 0 then
		player:AddNotification("Sử dụng Vật Phẩm bị lỗi, hãy liên hệ:(..GM..)");
		return;
	end
	if(GetValue==-1) then
		GetValue = 0;
	end

	Player.SetValueOfDailyRecore(player,item:GetItemID(),GetValue+1)


	for key,value in pairs(Global_CAMP) do
		if CAMP == key then
			Player.AddRetupeValue(player,CAMP,POINT)
			Player.RemoveItem(player,item:GetID())
			player:AddNotification("Hôm nay đã dùng "..(GetValue+1).." =>"..item:GetName().." + "..POINT.." "..value.NameActivity.."");
		end
	end-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi có sự kiện người chơi ấn vào một trong số các chức năng cung cấp bởi vật phẩm thông qua Item Dialog
--		scene: Scene - Bản đồ hiện tại
--		item: Item - Vật phẩm tương ứng
--		player: Player - NPC tương ứng
--		selectionID: number - ID chức năng
-- ****************************************************** --
function DanhVongLanhTho:OnSelection(scene, item, player, selectionID, otherParams)

	-- ************************** --
	--player:AddNotification("Enter -> DanhVongLanhTho:OnSelection, selectionID = " .. selectionID)--
	
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
function DanhVongLanhTho:OnItemSelected(scene, item, player, itemID)

	-- ************************** --

	-- ************************** --

end
function DanhVongLanhTho:GetRepute(nTimes, nAddTimes)
	if nTimes < 0 or nAddTimes <= 0 then
		return 0;
	end
	local POINT = 0;
	local nBaseIndex;
	for i = 1,3 do
		if nTimes < Data[i][1] then
			nBaseIndex = i;
		end
	end
	if nAddTimes + nTimes > Data[1][1] then
		return 0;
	end
	local nCurrIndex = nBaseIndex;
	while nCurrIndex > 0 do
		local nDiff = (nTimes + nAddTimes) - Data[nCurrIndex][1];
		if  nDiff <= 0 then
			POINT = POINT + Data[nCurrIndex][2]*nAddTimes
			break;
		else
			local nIncrease = Data[nCurrIndex][1]-nTimes;
			POINT = POINT +  Data[nCurrIndex][2]*(nIncrease);
			nTimes = Data[nCurrIndex][1];
			nAddTimes = nAddTimes - nIncrease;
		end
		
		nCurrIndex = nCurrIndex - 1;
	end
	return POINT;
end