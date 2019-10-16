import requests
from bs4 import BeautifulSoup
from urllib.parse import urlparse
from util.siteBase import siteBase
from util.log import MyLog

class wangyi(siteBase):

    def __init__(self, url):
        
        super().__init__(url)

    def _parse(self, soup):
        
        MyLog.getLogger().log("site_wangyi.parse")



