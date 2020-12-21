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

function findLast(haystack, needle)
    local i=haystack:match(".*"..needle.."()")
    if i==nil then return nil else return i-1 end
end

request = function()
	local r = {}
	counter = counter + 1
	index = counter % count_of_users
	if index == 0 then
		index = 1
	end 
	item = annotations[index]
	midSection = Split(item.id, '/')[3]
	userId = "users/" .. string.sub(midSection, 0, findLast(midSection, '-')-1)
	local pageSize = 10
	local reqs = 1
	for i=1,item.annotations,pageSize do
		path = "/annotations/user/" .. i * pageSize ..  "/" .. pageSize .. "/?userId=" .. userId
		r[reqs] = wrk.format(nil, path)
		reqs = reqs + 1
	end
	return table.concat(r)
end
