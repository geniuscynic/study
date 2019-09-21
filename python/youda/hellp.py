from selenium import webdriver
import time
import os
import shutil

browser = webdriver.Chrome()
browser.get('https://auokm1.auo.com/ContractSafetyMgt/Webform/Training/QueryTrainingClass.aspx')

elem = browser.find_element_by_name('txtLogInId')
elem.send_keys('9132050773940103XT')

elem = browser.find_element_by_name('txtPassword')
elem.send_keys('65071281')

# while True:
#         #val = browser.find_element_by_id('txtCaptcha').text
#         val = input('Input valico:')
#         if val and len(val)>0:
#             print(val)
#             browser.find_element_by_id('txtCaptcha').send_keys(val)
#             break

                    
                
# elem = browser.find_element_by_name('btnLogin')
# elem.click()

url = 'https://auokm1.auo.com/ContractSafetyMgt/Webform/Training/SignUpClassAddBatch.aspx?ClassId=0B1833424AD2A267&FormNo=59CC28B91F277EC9'

while True:
        #val = browser.find_element_by_id('txtCaptcha').text
        val = input('输入网页地址开始上传照片:')
        if val and len(val)>0:
            print(val)
            url = val
            #browser.get(url)
            #browser.find_element_by_id('txtCaptcha').send_keys(val)
            break

#browser.get(url)
#while True:
        #val = browser.find_element_by_id('txtCaptcha').text
        #val = input('Input valico:')
        #if val and len(val)>0:
            #print(val)
            #break


nowhandle = browser.current_window_handle  # 获得当前窗口
#print(nowhandle)
oldname= "default.jpg"
newname= ""


def upload():
        #browser.switch_to.window(nowhandle)
        browser.get(url)
        trElements = browser.find_elements_by_tag_name('tr')
        for trElement in trElements:
            #print(browser.current_window_handle)
            #trElements = browser.find_elements_by_tag_name('tr')
            #print("tr" + trElement.text)
            tdElem = trElement.find_elements_by_tag_name('td')
            if len(tdElem) == 0:
                continue

            #print(tdElem[10].text)
            if tdElem[10].text != " ":
                continue
            else:
                newname =  tdElem[5].text + ".jpg"
                shutil.copyfile(oldname,newname)
                newname = 'D:\\py\\' + newname
            #print(tdElem[11].text)
            #tdElem[11].click()
            #browser.implicitly_wait(2)
            print("正在上传: " + tdElem[5].text)

            btn = tdElem[11].find_element_by_tag_name('a')
            btn.click()

            flag = 0
            allhandles = browser.window_handles  # 获得所有窗口
            for handle in allhandles:  # 循环判断窗口是否为当前窗口
               if handle != nowhandle:
                   browser.switch_to.window(handle)  # 切换窗口
                   #time.sleep(2)
                   browser.find_element_by_name("fulPic").send_keys(newname)
                   browser.find_element_by_name("btnUpload").click()
                   browser.switch_to.window(nowhandle)
                   os.remove(newname)
                   #browser.implicitly_wait(20)
                   upload()
                   #time.sleep(10)
                   
                   #trElements = browser.find_elements_by_tag_name('tr')
                   #flag=1
                   
                   #browser.close()
            #if flag == 1:
                #break

upload()
print("上传结束")
#upload()

    #print(tdElem[11].text)
    #print(href)
    #browser.execute_script("window.open('" + href + "')");
#btnElements = browser.find_elements(By.XPATH, '//button[text()="上傳照片"]')
#https://auokm1.auo.com/ContractSafetyMgt/Webform/Training/SignUpClassAddBatch.aspx?ClassId=24B92B7252EF59F0&FormNo=4B1F2C9E120DDEE2





