import requests
from bs4 import BeautifulSoup
from urllib.parse import urlparse
from utils.siteBase import siteBase
from utils.log import MyLog, getLogger

class sina_new(siteBase):

    def __init__(self, url):
        
        super().__init__(url)

    # def _encoding(self):
    #     return 'GBK'

    def _parse(self, soup):
        
        title = soup.select_one(".main-title").string

        updateTime = soup.select_one("div.date-source span.date").string

        souce = soup.select_one("div.date-source .source").get_text()

        body_content = str(soup.select("div.article p" ))

        #or string in body_content:
            #print(repr(string))
        print(title, updateTime, souce, body_content)



