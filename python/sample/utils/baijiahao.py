import requests
from bs4 import BeautifulSoup
from urllib.parse import urlparse
from utils.siteBase import siteBase
from utils.log import MyLog, getLogger

class baijiahao(siteBase):

    def __init__(self, url):
        
        super().__init__(url)

    # def _encoding(self):
    #     return 'GBK'

    def _parse(self, soup):
        
        title = soup.select_one("div.article-title h2").string

        souce = soup.select_one("div.article-desc p.author-name" ).string

        updateTime1 = soup.select_one("div.article-desc div.article-source .date" ).string
        updateTime2 = soup.select_one("div.article-desc div.article-source .time" ).string

      

        body_content = str(soup.select_one("div.article div.article-content" ))

        #or string in body_content:
            #print(repr(string))
        #print(title, souce, updateTime1, updateTime2, body_content)



