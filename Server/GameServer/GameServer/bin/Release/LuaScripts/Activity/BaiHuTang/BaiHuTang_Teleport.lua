-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000000' bên dưới thành ID tương ứng
local BaiHuTang_Teleport = Scripts[400006]

-- ****************************************************** --
--	Hàm này được gọi khi đối tượng bắt đầu tiến vào vùng ảnh hưởng của khu vực động
--		scene: Scene - Bản đồ hiện tại
--		dynArea: DynamicArea - Đối tượng khu vực động
--		obj: {Player, Monster} - Đối tượng tương ứng
-- ****************************************************** --
function BaiHuTang_Teleport:OnEnter(scene, dynArea, obj)

	-- ************************** --
	--System.WriteToConsole("BaiHuTang_Teleport:OnEnter")
	-- ************************** --
	
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi đối tượng đứng trong vùng ảnh hưởng của khu vực động
--		scene: Scene - Bản đồ hiện tại
--		dynArea: DynamicArea - Đối tượng khu vực động
--		obj: {Player, Monster} - Đối tượng tương ứng
-- ****************************************************** --
function BaiHuTang_Teleport:OnStayTick(scene, dynArea, obj)

	-- ************************** --
	--System.WriteToConsole("BaiHuTang_Teleport:OnStayTick")
	-- ************************** --
	if obj:GetObjectType() == Global_ObjectTypes.OT_CLIENT then
		local params = String.Split(dynArea:GetTag(), '_')
		local sceneID = tonumber(params[1])
		local posX = tonumber(params[2])
		local posY = tonumber(params[3])
		
		-- Chuyển bản đồ tương ứng
		obj:ChangeScene(sceneID, posX, posY)
	end
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi đối tượng rời khỏi vùng ảnh hưởng của khu vực động
--		scene: Scene - Bản đồ hiện tại
--		dynArea: DynamicArea - Đối tượng khu vực động
--		obj: {Player, Monster} - Đối tượng tương ứng
-- ****************************************************** --
function BaiHuTang_Teleport:OnLeave(scene, dynArea, obj)

	-- ************************** --
	--System.WriteToConsole("BaiHuTang_Teleport:OnLeave")
	-- ************************** --

	-- ************************** --

end