local PlayerEvent = Scripts[999999]

function PlayerEvent:OnLogin(scene, player, first)
	if player:GetLevel() < 10 then
		player:SetLevel(10)
		Player.AddItemLua(player,781,1,-1,1)
	end
	
	if Player.CountItemInBag(player, 781) <= 0 then
		Player.AddItemLua(player,781,1,-1,1)
	end
	----------------------------
	PlayerEvent:CMDLOGIN(player);
end

function PlayerEvent:CMDLOGIN(player)
	local nThang = System.GetMonth()
	if nThang < 10 then
		nThang = string.format("0%s",nThang)
	end
	local nNgay = System.GetDate()
	if nNgay < 10 then
		nNgay = string.format("0%s",nNgay)
	end
	local nGio = System.GetHour()
	if nGio < 10 then
		nGio = string.format("0%s",nGio)
	end
	local nPhut = System.GetMinute()
	if nPhut < 10 then
		nPhut = string.format("0%s",nPhut)
	end
	local nTimeht = string.format("%s%s%s%s%s",System.GetYear(),nThang,nNgay,nGio,nPhut)
	nTimeht = tonumber(nTimeht)
	local online = player:GetPlayerOnline()
	System.WriteToConsole("["..nTimeht.."] ["..online.."] PlayerEvent:OnLogin ["..player:GetID().."]["..player:GetUserID().."] "..player:GetName())
end

function PlayerEvent:OnLogout(scene, player)
	local nThang = System.GetMonth()
	if nThang < 10 then
		nThang = string.format("0%s",nThang)
	end
	local nNgay = System.GetDate()
	if nNgay < 10 then
		nNgay = string.format("0%s",nNgay)
	end
	local nGio = System.GetHour()
	if nGio < 10 then
		nGio = string.format("0%s",nGio)
	end
	local nPhut = System.GetMinute()
	local nPhut = System.GetHour()
	if nPhut < 10 then
		nPhut = string.format("0%s",nPhut)
	end
	local nTimeht = string.format("%s%s%s%s%s",System.GetYear(),nThang,nNgay,nGio,nPhut)
	nTimeht = tonumber(nTimeht)
	local online = player:GetPlayerOnline()
	System.WriteToConsole("["..nTimeht.."] ["..online.."] PlayerEvent:OnLogout ["..player:GetID().."]["..player:GetUserID().."] "..player:GetName())
	----------------------------------------------------------------
end

function PlayerEvent:OnDisconnected(scene, player)
	
end

function PlayerEvent:OnReconnect(scene, player)
	-- ************************** --
	--System.WriteToConsole("PlayerEvent:OnReconnect => " .. player:GetName())
	-- ************************** --
end

function PlayerEvent:OnChangeScene(scene, player, toScene)
	-- ************************** --
	--System.WriteToConsole("PlayerEvent:OnChangeScene => " .. player:GetName() .. " From: " .. scene:GetName() .. " - To: " .. toScene:GetName())
	-- ************************** --
end

function PlayerEvent:OnEnterScene(scene, player)
	System.WriteToConsole("PlayerEvent:OnEnterScene => " .. player:GetName())
end

function PlayerEvent:OnDie(scene, player, killerObj)
	-- ************************** --
	--System.WriteToConsole("PlayerEvent:OnDie => " .. player:GetName() .. " - Killer: " .. killerObj:GetName())
	-- ************************** --
end

function PlayerEvent:OnKillObject(scene, player, deadObj)
	-- ************************** --
	--System.WriteToConsole("PlayerEvent:OnKillObject => " .. player:GetName() .. " - DeadObj: " .. deadObj:GetName())
	-- ************************** --
end

function PlayerEvent:OnRelive(scene, player)
	-- ************************** --
	--System.WriteToConsole("PlayerEvent:OnRelive => " .. player:GetName())
	-- ************************** --
end

function PlayerEvent:OnHitObject(scene, player, obj, nDamage)
	-- ************************** --
	--System.WriteToConsole("PlayerEvent:OnHitObject => " .. player:GetName() .. " - Obj: " .. obj:GetName() .. " - Damage: " .. nDamage)
	-- ************************** --
end

function PlayerEvent:OnBeHit(scene, player, obj, nDamage)
	
	-- ************************** --
	--System.WriteToConsole("PlayerEvent:OnBeHit => " .. player:GetName() .. " - Obj: " .. obj:GetName() .. " - Damage: " .. nDamage)
	-- ************************** --
	
end
