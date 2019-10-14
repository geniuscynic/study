import requests

r = requests.get("http://top.baidu.com/buzz?b=1&fr=topindex")

print(r.encoding)
print(r.text)
