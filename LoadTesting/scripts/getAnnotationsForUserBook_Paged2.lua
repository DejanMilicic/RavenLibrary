json = require "json"
file = io.open("annotations.json", "r")
data = file:read("*a")
annotations = json.decode(data)
count_of_users = table.getn(annotations)
counter = math.random(count_of_users) -- Each thread has separate start position

function Split(s, delimiter)
    result = {};
    for match in (s..delimiter):gmatch("(.-)"..delimiter) do
        table.insert(result, match);
    end
    return result;
end

request = function()
	local r = {}
	counter = counter + 1
	index = counter % count_of_users
	if index == 0 then
		index = 1
	end 
	item = annotations[index]
	local page = 0
	local pageSize = 10
	local parts = Split(item.id, '-')
	path = "/annotations/userbook/" .. page * pageSize ..  "/" .. pageSize .. "/?userId=" .. parts[1] .. "&bookId=" .. parts[2]
	r[1] = wrk.format(nil, path)
	
	-- local reqs = 1
	--for i=1,item.annotations,pageSize do
	--	path = "/annotations/userbook/" .. i * pageSize ..  "/" .. pageSize .. "/?userBookId=" .. item.id
	--	r[reqs] = wrk.format(nil, path)
	--	reqs = reqs + 1
	--end
	return table.concat(r)
end
