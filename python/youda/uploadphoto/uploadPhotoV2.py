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

browser = webdriver.Chrome(executable_path = "chromedriver.exe")
#browser = webdriver.Firefox(executable_path = "./geckodriver")

url = 'https://auokm1.auo.com/ContractSafetyMgt/Webform/Vendor/LoginVendor.aspx'
login = '9132050773940103XT'
passssword = '65071281'
prefix = 'https://auokm1.auo.com/ContractSafetyMgt/Webform/Training/'
#current_dir = "/home/zjw/workplace/python/youda/uploadphoto/"
current_dir = "D:/workplace/git/study/python/youda/uploadphoto/template/"
template_photo_path = current_dir + 'shwj.xlsx'

browser.get(url)
browser.find_element_by_id("txtLogInId").send_keys(login)
browser.find_element_by_id("txtPassword").send_keys(passssword)

listUrl = []

def selectAll():
   
    selectElements = browser.find_elements_by_tag_name("select")

    for selectElement in selectElements:
        if('GvPageSizeList' in selectElement.get_attribute('id')):
            Select(selectElement).select_by_visible_text("All")
            break
        
       

def findNeedUploadUrl():
    checkedElemets = browser.find_elements_by_css_selector("#ctl00_contentBody_Form_c_SignUpForm1_AddBatch_c_AddBatchWorkers1_Trainee_c_ExtendGridView1 a")

    for element in checkedElemets:
        if(element.text != "上传审核文件"):
            continue
        
        isUpload = element.find_element_by_xpath('../preceding-sibling::td[2]').text.strip()
        if(isUpload == "Y"):
            continue

        name = element.find_element_by_xpath('../preceding-sibling::td[6]').text.strip()
        new_url = element.get_attribute('href')
        listUrl.append(new_url)

        new_path = current_dir + name + '.xlsx'
        if not (os.path.exists(new_path)):
            print("文件不存在", name)
            continue

        return new_url, name

    return "", ""

def upload(name, photoUrl):
    current_handle = browser.current_window_handle

    new_path = current_dir + name + '.xlsx'
       
    js="window.open('%s')" % photoUrl
    browser.execute_script(js)

    

    while(len(browser.window_handles) == 1):
        time.sleep(1)
        
    print(len(browser.window_handles))
    browser.switch_to.window(browser.window_handles[1])
    time.sleep(1)
        
    WebDriverWait(browser, 10).until(expected_conditions.presence_of_element_located((By.ID, "ctl00_contentBody_FileUpload1")))


    browser.find_element_by_id("ctl00_contentBody_FileUpload1").send_keys(new_path)
    browser.find_element_by_id('ctl00_contentBody_Button1').click()
    time.sleep(1)
    #browser.find_element_by_id('ctl00_contentBody_BtnDone').click()
    browser.find_element_by_id('ctl00_contentBody_Button2').click()
    time.sleep(1)
    # dig_alert = browser.switch_to.alert
    # time.sleep(1)
    # dig_alert.accept()
    # time.sleep(1)
    #os.remove(new_path)
    browser.switch_to.window(current_handle)
    time.sleep(1)


def start():
    url = input("请输入上传地址： ")
    while(not url):
        url = input("请输入上传地址")

    #url ='https://auokm1.auo.com/ContractSafetyMgt/Webform/Training/SignUpClass/SignUpQuery.aspx?ClassId=82E36AD6A91D9021'
    browser.get(url)

    selectAll()

    
    
    #browser.find_element_by_xpath
    elements = browser.find_elements_by_css_selector("#ctl00_contentBody_Query_c_ClassWorkers1_Trainee_c_ExtendGridView1 a")
    for element in elements:
        if(element.text != '未上传'):
            continue

        element.find_element_by_xpath('../preceding-sibling::td[1]/a').click()
        #findCheckElements()
        break
       
    while(True):
        photoUrl, name = findNeedUploadUrl()
        if(photoUrl == ""):
            break

        upload(name, photoUrl)
        WebDriverWait(browser, 100).until_not(expected_conditions.presence_of_element_located((By.CLASS_NAME, "blockUI")))

    

    print('照片全部上传玩')
    start()
    
       
start()


