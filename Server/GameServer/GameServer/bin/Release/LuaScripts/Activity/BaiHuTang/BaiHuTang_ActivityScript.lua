-- Mỗi khi Script được thực thi, ID tương ứng sẽ được lưu trong hệ thống, tại bảng 'Scripts'
-- Dạng đối tượng là dạng Class, được khởi tạo mặc định bởi hệ thống, và sau đó được lưu tại bảng
-- Khi sử dụng dạng Class, cần phải kế thừa Class được hệ thống sinh ra, và dòng lệnh bên dưới để làm điều đó
-- ID Script được khai báo ở file ScriptIndex.xml, thay thế giá trị '000000' bên dưới thành ID tương ứng
local BaiHuTang_ActivityScript = Scripts[400002]

-- ************************** --
-- Loại sự kiện
local ListActivities = {
	-- Chuẩn bị Bạch Hổ Đường
	Prepare = {
		[100] = {
			ActivityID = 100,
			ActivityLevel = 80,
			SceneIDs = {
				226,			-- Bạch Hổ Đường 1_Đông (sơ 1)
				227,			-- Bạch Hổ Đường 1_Nam (sơ 1)
				228,			-- Bạch Hổ Đường 1_Tây (sơ 1)
				229,			-- Bạch Hổ Đường 1_Bắc (sơ 1)
				
				234,			-- Bạch Hổ Đường 1_Đông (cao 1)
				235,			-- Bạch Hổ Đường 1_Nam (cao 1)
				236,			-- Bạch Hổ Đường 1_Tây (cao 1)
				237,			-- Bạch Hổ Đường 1_Bắc (cao 1)
			},
			ScriptID = 400003,
		},
	},
	-- Bắt đầu Bạch Hổ Đường - 1
	Round1 = {
		[101] = {
			ActivityID = 101,
			ActivityLevel = 80,
			SceneIDs = {
				226,			-- Bạch Hổ Đường 1_Đông (sơ 1)
				227,			-- Bạch Hổ Đường 1_Nam (sơ 1)
				228,			-- Bạch Hổ Đường 1_Tây (sơ 1)
				229,			-- Bạch Hổ Đường 1_Bắc (sơ 1)
			},
			ScriptID = 400003,
		},
		[106] = {
			ActivityID = 106,
			ActivityLevel = 120,
			SceneIDs = {
				234,			-- Bạch Hổ Đường 1_Đông (cao 1)
				235,			-- Bạch Hổ Đường 1_Nam (cao 1)
				236,			-- Bạch Hổ Đường 1_Tây (cao 1)
				237,			-- Bạch Hổ Đường 1_Bắc (cao 1)
			},
			ScriptID = 400003,
		},
	},
	-- Bắt đầu Bạch Hổ Đường - 2
	Round2 = {
		[102] = {
			ActivityID = 102,
			ActivityLevel = 80,
			SceneIDs = {
				230,			-- Bạch Hổ Đường 2_Âm (sơ 1)
				231,			-- Bạch Hổ Đường 2_Dương (sơ 1)
			},
			ScriptID = 400004,
		},
		[107] = {
			ActivityID = 107,
			ActivityLevel = 120,
			SceneIDs = {
				238,			-- Bạch Hổ Đường 2_Âm (cao 1)
				239,			-- Bạch Hổ Đường 2_Dương (cao 1)
			},
			ScriptID = 400004,
		},
	},
	-- Bắt đầu Bạch Hổ Đường - 3
	Round3 = {
		[103] = {
			ActivityID = 103,
			ActivityLevel = 80,
			SceneIDs = {
				232,			-- Bạch Hổ Đường 3 (sơ 1)
			},
			ScriptID = 400005,
		},
		[108] = {
			ActivityID = 108,
			ActivityLevel = 120,
			SceneIDs = {
				240,			-- Bạch Hổ Đường 3 (cao 1)
			},
			ScriptID = 400005,
		},
	},
	End = 104,
}
-- ************************** --

-- ****************************************************** --
--	Hàm này được gọi khi sự kiện được khởi tạo
--		activity: Activity - Sự kiện hiện tại
-- ****************************************************** --
function BaiHuTang_ActivityScript:OnInit(activity)

	-- ************************** --
	local activityID = activity:GetID()
	-- ************************** --
	--System.WriteToConsole("BaiHuTang_ActivityScript:OnInit => ID: " .. activityID)
	-- ************************** --
	if self:IsActivityExist(activityID, ListActivities.Prepare) == true then				-- Chuẩn bị Bạch Hổ Đường
		GUI.SendSystemEventNotification("Sự kiện Bạch Hổ Đường đã bắt đầu chuẩn bị, người chơi có thể thông qua Môn Đồ Bạch Hổ Đường ở Đại Điện để tiến vào tranh đoạt báu vật bên trong Bạch Hổ Đường.")
		
		-- Thiết lập cấp độ sự kiện
		activity:SetLocalVariable(1, ListActivities.Prepare[activityID].ActivityLevel)
		
		-- Duyệt danh sách bản đồ sự kiện
		for idx, sceneID in ipairs(ListActivities.Prepare[activityID].SceneIDs) do
			-- Bản đồ tương ứng
			local scene = EventManager.GetScene(sceneID)
			-- Nếu bản đồ tồn tại
			if scene ~= nil then
				-- Thực thi sự kiện Prepare
				System.CallScriptFunction(ListActivities.Prepare[activityID].ScriptID, "Prepare", activity, scene)
			end
		end
		
		-- Thiết lập mở báo danh Bạch Hổ Đường
		Global_Params.BaiHuTang_BeginRegistered = true
		Global_Params.BaiHuTang_Stage = 1
	elseif self:IsActivityExist(activityID, ListActivities.Round1) == true then		-- Bạch Hổ Đường - 1
		-- Thiết lập ngừng báo danh Bạch Hổ Đường
		Global_Params.BaiHuTang_BeginRegistered = false
		Global_Params.BaiHuTang_Stage = 2
		
		GUI.SendSystemEventNotification("Sự kiện Bạch Hổ Đường đã bắt đầu. Ai sẽ là người đoạt được báu vật bên trong Bạch Hổ Đường và trở thành cao thủ võ lâm đây?")
		
		-- Thiết lập cấp độ sự kiện
		activity:SetLocalVariable(1, ListActivities.Round1[activityID].ActivityLevel)
	
		-- Duyệt danh sách bản đồ sự kiện
		for idx, sceneID in ipairs(ListActivities.Round1[activityID].SceneIDs) do
			-- Bản đồ tương ứng
			local scene = EventManager.GetScene(sceneID)
			-- Nếu bản đồ tồn tại
			if scene ~= nil then
				-- Thực thi sự kiện Prepare
				System.CallScriptFunction(ListActivities.Round1[activityID].ScriptID, "Prepare", activity, scene)
			end
		end
	elseif self:IsActivityExist(activityID, ListActivities.Round2) == true then		-- Bạch Hổ Đường - 2
		-- Thiết lập ngừng báo danh Bạch Hổ Đường
		Global_Params.BaiHuTang_BeginRegistered = false
		Global_Params.BaiHuTang_Stage = 3
		
		-- Thiết lập cấp độ sự kiện
		activity:SetLocalVariable(1, ListActivities.Round2[activityID].ActivityLevel)
	
		-- Duyệt danh sách bản đồ sự kiện
		for idx, sceneID in ipairs(ListActivities.Round2[activityID].SceneIDs) do
			-- Bản đồ tương ứng
			local scene = EventManager.GetScene(sceneID)
			-- Nếu bản đồ tồn tại
			if scene ~= nil then
				-- Thực thi sự kiện Prepare
				System.CallScriptFunction(ListActivities.Round2[activityID].ScriptID, "Prepare", activity, scene)
			end
		end
	elseif self:IsActivityExist(activityID, ListActivities.Round3) == true then		-- Bạch Hổ Đường - 3
		-- Thiết lập ngừng báo danh Bạch Hổ Đường
		Global_Params.BaiHuTang_BeginRegistered = false
		Global_Params.BaiHuTang_Stage = 4
		
		-- Thiết lập cấp độ sự kiện
		activity:SetLocalVariable(1, ListActivities.Round3[activityID].ActivityLevel)
	
		-- Duyệt danh sách bản đồ sự kiện
		for idx, sceneID in ipairs(ListActivities.Round3[activityID].SceneIDs) do
			-- Bản đồ tương ứng
			local scene = EventManager.GetScene(sceneID)
			-- Nếu bản đồ tồn tại
			if scene ~= nil then
				-- Thực thi sự kiện Prepare
				System.CallScriptFunction(ListActivities.Round3[activityID].ScriptID, "Prepare", activity, scene)
			end
		end
	elseif activityID == ListActivities.End then				-- Kết thúc Bạch Hổ Đường
		-- Thiết lập ngừng báo danh Bạch Hổ Đường
		Global_Params.BaiHuTang_BeginRegistered = false
		Global_Params.BaiHuTang_Stage = 0
	end
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi sự kiện bắt đầu
--		activity: Activity - Sự kiện hiện tại
-- ****************************************************** --
function BaiHuTang_ActivityScript:OnStart(activity)

	-- ************************** --
	local activityID = activity:GetID()
	-- ************************** --
	--System.WriteToConsole("BaiHuTang_ActivityScript:OnStart => ID: " .. activityID)
	-- ************************** --
	if self:IsActivityExist(activityID, ListActivities.Round1) == true then		-- Bạch Hổ Đường - 1
		-- Duyệt danh sách bản đồ sự kiện
		for idx, sceneID in ipairs(ListActivities.Round1[activityID].SceneIDs) do
			-- Bản đồ tương ứng
			local scene = EventManager.GetScene(sceneID)
			-- Nếu bản đồ tồn tại
			if scene ~= nil then
				-- Thực thi sự kiện OnStart
				System.CallScriptFunction(ListActivities.Round1[activityID].ScriptID, "OnStart", activity, scene)
			end
		end
	elseif self:IsActivityExist(activityID, ListActivities.Round2) == true then		-- Bạch Hổ Đường - 2
		-- Duyệt danh sách bản đồ sự kiện
		for idx, sceneID in ipairs(ListActivities.Round2[activityID].SceneIDs) do
			-- Bản đồ tương ứng
			local scene = EventManager.GetScene(sceneID)
			-- Nếu bản đồ tồn tại
			if scene ~= nil then
				-- Thực thi sự kiện OnStart
				System.CallScriptFunction(ListActivities.Round2[activityID].ScriptID, "OnStart", activity, scene)
			end
		end
	elseif self:IsActivityExist(activityID, ListActivities.Round3) == true then		-- Bạch Hổ Đường - 3
		-- Duyệt danh sách bản đồ sự kiện
		for idx, sceneID in ipairs(ListActivities.Round3[activityID].SceneIDs) do
			-- Bản đồ tương ứng
			local scene = EventManager.GetScene(sceneID)
			-- Nếu bản đồ tồn tại
			if scene ~= nil then
				-- Thực thi sự kiện OnStart
				System.CallScriptFunction(ListActivities.Round3[activityID].ScriptID, "OnStart", activity, scene)
			end
		end
	end
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi liên tục chừng nào sự kiện còn tồn tại
--		activity: Activity - Sự kiện hiện tại
-- ****************************************************** --
function BaiHuTang_ActivityScript:OnTick(activity)

	-- ************************** --
	local activityID = activity:GetID()
	-- ************************** --
	--System.WriteToConsole("BaiHuTang_ActivityScript:OnTick => ID: " .. activityID)
	-- ************************** --
	if self:IsActivityExist(activityID, ListActivities.Round1) == true then		-- Bạch Hổ Đường - 1
		-- Duyệt danh sách bản đồ sự kiện
		for idx, sceneID in ipairs(ListActivities.Round1[activityID].SceneIDs) do
			-- Bản đồ tương ứng
			local scene = EventManager.GetScene(sceneID)
			-- Nếu bản đồ tồn tại
			if scene ~= nil then
				-- Thực thi sự kiện OnTick
				System.CallScriptFunction(ListActivities.Round1[activityID].ScriptID, "OnTick", activity, scene)
			end
		end
	elseif self:IsActivityExist(activityID, ListActivities.Round2) == true then		-- Bạch Hổ Đường - 2
		-- Duyệt danh sách bản đồ sự kiện
		for idx, sceneID in ipairs(ListActivities.Round2[activityID].SceneIDs) do
			-- Bản đồ tương ứng
			local scene = EventManager.GetScene(sceneID)
			-- Nếu bản đồ tồn tại
			if scene ~= nil then
				-- Thực thi sự kiện OnTick
				System.CallScriptFunction(ListActivities.Round2[activityID].ScriptID, "OnTick", activity, scene)
			end
		end
	elseif self:IsActivityExist(activityID, ListActivities.Round3) == true then		-- Bạch Hổ Đường - 3
		-- Duyệt danh sách bản đồ sự kiện
		for idx, sceneID in ipairs(ListActivities.Round3[activityID].SceneIDs) do
			-- Bản đồ tương ứng
			local scene = EventManager.GetScene(sceneID)
			-- Nếu bản đồ tồn tại
			if scene ~= nil then
				-- Thực thi sự kiện OnTick
				System.CallScriptFunction(ListActivities.Round3[activityID].ScriptID, "OnTick", activity, scene)
			end
		end
	end
	-- ************************** --

end

-- ****************************************************** --
--	Hàm này được gọi khi sự kiện bị đóng
--		activity: Activity - Sự kiện hiện tại
-- ****************************************************** --
function BaiHuTang_ActivityScript:OnClose(activity)

	-- ************************** --
	local activityID = activity:GetID()
	-- ************************** --
	--System.WriteToConsole("BaiHuTang_ActivityScript:OnClose => ID: " .. activityID)
	-- ************************** --
	if self:IsActivityExist(activityID, ListActivities.Round1) == true then		-- Bạch Hổ Đường - 1
		-- Duyệt danh sách bản đồ sự kiện
		for idx, sceneID in ipairs(ListActivities.Round1[activityID].SceneIDs) do
			-- Bản đồ tương ứng
			local scene = EventManager.GetScene(sceneID)
			-- Nếu bản đồ tồn tại
			if scene ~= nil then
				-- Thực thi sự kiện OnClose
				System.CallScriptFunction(ListActivities.Round1[activityID].ScriptID, "OnClose", activity, scene)
			end
		end
	elseif self:IsActivityExist(activityID, ListActivities.Round2) == true then		-- Bạch Hổ Đường - 2
		-- Duyệt danh sách bản đồ sự kiện
		for idx, sceneID in ipairs(ListActivities.Round2[activityID].SceneIDs) do
			-- Bản đồ tương ứng
			local scene = EventManager.GetScene(sceneID)
			-- Nếu bản đồ tồn tại
			if scene ~= nil then
				-- Thực thi sự kiện OnClose
				System.CallScriptFunction(ListActivities.Round2[activityID].ScriptID, "OnClose", activity, scene)
			end
		end
	elseif self:IsActivityExist(activityID, ListActivities.Round3) == true then		-- Bạch Hổ Đường - 3
		-- Duyệt danh sách bản đồ sự kiện
		for idx, sceneID in ipairs(ListActivities.Round3[activityID].SceneIDs) do
			-- Bản đồ tương ứng
			local scene = EventManager.GetScene(sceneID)
			-- Nếu bản đồ tồn tại
			if scene ~= nil then
				-- Thực thi sự kiện OnClose
				System.CallScriptFunction(ListActivities.Round3[activityID].ScriptID, "OnClose", activity, scene)
			end
		end
	end
	-- ************************** --

end



-- ===================================================================================== --
-- ===================================================================================== --

-- ****************************************************** --
--	Kiểm tra sự kiện có ID tương ứng có tồn tại trong danh sách thiết lập không
-- ****************************************************** --
function BaiHuTang_ActivityScript:IsActivityExist(activityID, list)

	-- ************************** --
	if list[activityID] ~= nil then
		return true
	else
		return false
	end
	-- ************************** --
	
end