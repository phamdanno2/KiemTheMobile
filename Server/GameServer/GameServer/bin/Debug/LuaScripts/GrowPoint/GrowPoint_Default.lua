-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000000' bên dưới thành ID tương ứng
local GrowPoint_Default = Scripts[300001]

-- ****************************************************** --
local ProductID = {
	[1548] = { ItemID = 2497, ItemNumber = 1 },			-- Thổ Phục Linh
	[1549] = { ItemID = 2498, ItemNumber = 1 },			-- Sơn Dược
	[1550] = { ItemID = 2499, ItemNumber = 1 },			-- Cửu Tiết Xương Bồ
	[1551] = { ItemID = 2500, ItemNumber = 1 },			-- Đại Thanh Đầu
	[1552] = { ItemID = 2504, ItemNumber = 1 },			-- Địa Căn Thảo
	[1542] = { ItemID = 2544, ItemNumber = 1 },			-- Hài cốt nữ
	[1600] = { ItemID = 2515, ItemNumber = 1 },			-- Lương Thảo bị cướp
	[1601] = { ItemID = 2516, ItemNumber = 1 },			-- Châu Báu bị cướp
	[584] = { ItemID = 2585, ItemNumber = 1 },			-- Sinh thiết khoáng thạch
	[1528] = { ItemID = 2566, ItemNumber = 1 },			-- Tử ngọc khoáng thạch
	[633] = { ItemID = 2557, ItemNumber = 1 },			-- kim khoáng thạch
	[1559] = { ItemID = 2574, ItemNumber = 1 },			-- di hài thiên trúc tăng
	[1558] = { ItemID = 2568, ItemNumber = 1 },			-- thanh độc thảo
	[1557] = { ItemID = 2567, ItemNumber = 1 },			-- hàn diêm
	[1584] = { ItemID = 2867, ItemNumber = 1 },			-- hoa thi vu
	[1807] = { ItemID = 2847, ItemNumber = 1 },			--tiên mật quả
	[1630] = { ItemID = 2941, ItemNumber = 1 },			-- Sen Mẫu Đơn
	[1631] = { ItemID = 2942, ItemNumber = 1 },			-- Bách Hương Quả
	[1632] = { ItemID = 2943, ItemNumber = 1 },			-- Huyết Phong Đằng
	[1634] = { ItemID = 2945, ItemNumber = 1 },			-- Lục Thủy Tinh
	[1633] = { ItemID = 2944, ItemNumber = 1 },			--Hắc Tinh Thạch
	[1635] = { ItemID = 2946, ItemNumber = 1 },			--Thất Thái Thạch
	
}
-- ****************************************************** --

-- ****************************************************** --
--	Hàm này được gọi khi người chơi ấn vào điểm thu thập, kiểm tra điều kiện có được thu thập hay không
--		scene: Scene - Bản đồ hiện tại
--		growPoint: GrowPoint - Điểm thu thập tương ứng
--		player: Player - NPC tương ứng
--	Return: True nếu thỏa mãn điều kiện có thể thu thập, False nếu không thỏa mãn
-- ****************************************************** --
function GrowPoint_Default:OnPreCheckCondition(scene, growPoint, player)

	-- ************************** --
	--player:AddNotification("GrowPoint_Default => OnPreCheckCondition")
	-- ************************** --
	return true
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi để thực thi Logic liên tục khi thanh Progress Bar đang chạy thực thi thao tác với điểm thu thập
--		scene: Scene - Bản đồ hiện tại
--		growPoint: GrowPoint - Điểm thu thập tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function GrowPoint_Default:OnActivateEachTick(scene, growPoint, player)

	-- ************************** --
	
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi quá trình thu thập hoàn tất
--		scene: Scene - Bản đồ hiện tại
--		growPoint: GrowPoint - Điểm thu thập tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function GrowPoint_Default:OnComplete(scene, growPoint, player)

	-- ************************** --
	player:AddNotification("Thu thập thành công!")
	-- ************************** --
	if not ProductID then 
		return
	end
	-- ************************** --
	local product = ProductID[growPoint:GetResID()]
	Player.AddItemLua(player, product.ItemID, product.ItemNumber, 0, 1)
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi quá trình thu thập bị hủy bỏ
--		scene: Scene - Bản đồ hiện tại
--		growPoint: GrowPoint - Điểm thu thập tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function GrowPoint_Default:OnCancel(scene, growPoint, player)

	-- ************************** --
	--player:AddNotification("GrowPoint_Default => OnCancel")
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi quá trình thu thập thất bại
--		scene: Scene - Bản đồ hiện tại
--		growPoint: GrowPoint - Điểm thu thập tương ứng
--		player: Player - NPC tương ứng
-- ****************************************************** --
function GrowPoint_Default:OnFaild(scene, growPoint, player)

	-- ************************** --
	player:AddNotification("Thu thập thất bại")
	-- ************************** --

end
