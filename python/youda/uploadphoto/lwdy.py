from selenium import webdriver
from selenium.webdriver.common.keys import Keys
import re
import shutil
import os

browser = webdriver.Chrome(executable_path = "chromedriver.exe")

url = 'https://max.book118.com/html/2018/0830/5300123031001312.shtm'

browser.get(url)
browser.find_element_by_id("full").click()

items = browser.find_elements_by_css_selector('.item img').get_attribute("src")

input()