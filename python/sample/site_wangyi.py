import requests
from bs4 import BeautifulSoup
from urllib.parse import urlparse

class site_wangyi(siteBase):

    def __init__(self, url):
        super().__init__(url)