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
	path = "/annotations/userbook?userBookId=" .. item.id
	r[1] = wrk.format(nil, path)
	return table.concat(r)
end
