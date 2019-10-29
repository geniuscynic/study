import requests
from bs4 import BeautifulSoup
from urllib.parse import urlparse
from utils.siteBase import siteBase
from utils.log import MyLog, getLogger

class ifeng(siteBase):

    def __init__(self, url):
        
        super().__init__(url)

    # def _encoding(self):
    #     return 'GBK'

    def _parse(self, soup):
        
        title = soup.select_one("div.artical-_Qk9Dp2t h1.topic-3bY8Hw-9").string

        updateTime = soup.select_one("div.artical-_Qk9Dp2t p.time-hm3v7ddj span").string

        souce = soup.select_one("div.artical-_Qk9Dp2t span.publisher-26PKOCqr span").get_text()

        body_content = str(soup.select("div.main_content-LcrEruCc p" ))

        #or string in body_content:
            #print(repr(string))
        print(title, updateTime, souce, body_content)



