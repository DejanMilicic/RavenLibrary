json = require "json"
file = io.open("annotations.json", "r")
data = file:read("*a")
annotations = json.decode(data)
count_of_users = table.getn(annotations)
counter = math.random(count_of_users) -- Each thread has separate start position

request = function()
	local r = {}
	counter = counter + 1
	index = counter % count_of_users
	if index == 0 then
		index = 1
	end 
	item = annotations[index]
	local pageSize = 10
	local reqs = 1
	for i=1,item.annotations,pageSize do
		path = "/annotations/userbook/" .. i * pageSize ..  "/" .. pageSize .. "/?userBookId=" .. item.id
		r[reqs] = wrk.format(nil, path)
		reqs = reqs + 1
	end
	return table.concat(r)
end
