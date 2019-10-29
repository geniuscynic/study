import requests
from bs4 import BeautifulSoup
from urllib.parse import urlparse
from utils.siteBase import siteBase
from utils.log import MyLog, getLogger

class wangyi(siteBase):

    def __init__(self, url):
        
        super().__init__(url)

    def _encoding(self):
        return 'GBK'

    def _parse(self, soup):
        
        title = soup.select_one("div.post_content_main h1").string

        post_source = soup.select_one("div.post_content_main div.post_time_source" ).stripped_strings

        updateTime = repr(post_source.send(None)).replace('\\u3000来源:', '').replace("'", '')
        souce = repr(post_source.send(None)).replace("'", '')

        body_content = soup.select("div.post_content_main div.post_body div.post_text p" )

        #or string in body_content:
            #print(repr(string))
        #print(updateTime, souce, body_content)



