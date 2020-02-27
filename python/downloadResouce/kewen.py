import requests

#"yingyu" shuxue
kemu = "shuxue"
#downloadFolder = "yingyu/"

def download_kewen():
    url = "http://www.fhebook.cn/h5book/202002/{}/2b/files/mobile/{}.jpg?x-oss-process=image/resize,h_970,w_689/format,webp&200202134850"

    for i in range(31,108):
        down_res = requests.get(url.format(kemu, i)) 
        file_name = kemu + "/{}.jpg".format(i)
        with open(file_name,"wb") as code:
            code.write(down_res.content)



download_kewen()

#url = "https://mp.weixin.qq.com/s?__biz=MzU1OTE2OTA1OA==&mid=2247487497&idx=3&sn=aef7c7dedefd5999d24c7cba6ad53009&chksm=fc1a3b44cb6db252ff387df51571dc157a4a64e619fe04fd38db75b9222a2f089af139c9e295&mpshare=1&scene=23&srcid=02107unx0bGjdSXLIdMsMTsY&sharer_sharetime=1581495632415&sharer_shareid=0fff91004fb326d55097e809c2c6634e#rd"
#down_res = requests.get(url) 

#print(down_res.text)