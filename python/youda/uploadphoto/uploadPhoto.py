from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support import expected_conditions
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.common.by import By

import re
import shutil
import os

#browser = webdriver.Chrome(executable_path = "chromedriver.exe")
browser = webdriver.Firefox(executable_path = "./geckodriver")

url = 'https://auokm1.auo.com/ContractSafetyMgt/Webform/Vendor/LoginVendor.aspx'
login = '9132050773940103XT'
passssword = '65071281'
prefix = 'https://auokm1.auo.com/ContractSafetyMgt/Webform/Training/'
current_dir = "/home/zjw/workplace/python/youda/uploadphoto/"
template_photo_path = current_dir + 'template.jpg'

browser.get(url)
browser.find_element_by_id("txtLogInId").send_keys(login)
browser.find_element_by_id("txtPassword").send_keys(passssword)

def start():
    url = input("请输入上传地址： ")
    while(not url):
        url = input("请输入上传地址")

    #url ='https://auokm1.auo.com/ContractSafetyMgt/Webform/Training/SignUpClassAddBatch.aspx?ClassId=38A57266245DDC27&FormNo=06119FE44D839613'
    browser.get(url)

    listUrl = []
    #browser.find_element_by_xpath
    elements = browser.find_elements_by_css_selector("#ctl00_contentBody_gdvSignUpWorker a")
    for element in elements:
        if(element.text != '上傳照片'):
            continue

        new_eltement = element.find_element_by_xpath('../preceding-sibling::td[1]').text.strip()
        if(new_eltement != '' ):
            continue

        new_url = element.get_attribute('href')
        listUrl.append(new_url)
        #browser.find_element_by_tag_name('body').send_keys(Keys.COMMAND + 't') 
        #browser.get(url)
        #browser.find_element_by_tag_name('body').send_keys(Keys.COMMAND + 'w') 
        #pass

    current_handle = browser.current_window_handle

    for photoUrl in listUrl:
        pattern = '.*UserKeyId=(.*)'
        idcard = re.match(pattern, photoUrl).group(1)
        new_path = current_dir + idcard + '.jpg'
        shutil.copy(template_photo_path, new_path)

        js="window.open('%s')" % photoUrl
        browser.execute_script(js)

        # 输出当前窗口句柄（百度）
        #photo_handle = browser.current_window_handle


        # 获取当前窗口句柄集合（列表类型）
        #handles = browser.window_handles
        #print(handles)  # 输出句柄集合

        browser.switch_to.window(browser.window_handles[1])

        WebDriverWait(browser, 10).until(expected_conditions.presence_of_element_located((By.ID, "fulPic")))

        browser.find_element_by_id("fulPic").send_keys(new_path)
        browser.find_element_by_id('btnUpload').click()
        os.remove(new_path)
        browser.switch_to.window(current_handle)
        


    browser.switch_to.window(current_handle)
    browser.get(url)

    print('照片全部上传玩')
    start()
       
start()


