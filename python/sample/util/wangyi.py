import requests
from bs4 import BeautifulSoup
from urllib.parse import urlparse
from util.siteBase import siteBase

class wangyi(siteBase):

    def __init__(self, url):
        
        super().__init__(url)

    def _parse(self, soup):
        print("site_wangyi.parse")



