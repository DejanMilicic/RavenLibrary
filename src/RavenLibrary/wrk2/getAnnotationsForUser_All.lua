json = require "json"
file = io.open("data.json", "r")
data = file:read("*a")
users = json.decode(data)
count_of_users = table.getn(users)
counter = math.random(count_of_users) -- Each thread has separate start position

request = function()
	local r = {}
	counter = counter + 1
	index = counter % count_of_users
	if index == 0 then
		index = 1
	end 
	item = users[index]
	path = "/annotations/userbook?userBookId=" .. item.id -- todo fix this enfpoint
	r[0] = wrk.format(nil, path)
	return table.concat(r)
end
