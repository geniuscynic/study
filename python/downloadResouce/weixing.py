from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support import expected_conditions
from selenium.webdriver.support.select import Select
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.common.by import By


import time
import re
import shutil
import os

import utils

browser = webdriver.Chrome(executable_path = "chromedriver.exe")

url = 'https://mp.weixin.qq.com/s?__biz=MzU1OTE2OTA1OA==&mid=2247487497&idx=3&sn=aef7c7dedefd5999d24c7cba6ad53009&chksm=fc1a3b44cb6db252ff387df51571dc157a4a64e619fe04fd38db75b9222a2f089af139c9e295&mpshare=1&scene=23&srcid=02107unx0bGjdSXLIdMsMTsY&sharer_sharetime=1581495632415&sharer_shareid=0fff91004fb326d55097e809c2c6634e#rd'
browser.get(url)

elements = browser.find_elements_by_tag_name('img')
i = 1
for e in elements:
    src =  e.get_attribute("data-src")
    if(src == None):
        continue
    
    fileName = "yuwen/{:0>3}.webp".format(i)
    utils.download(src, fileName)
    utils.covert(fileName, "jpg")
    i = i + 1
    time.sleep(1)





input("请输入上传地址： ")