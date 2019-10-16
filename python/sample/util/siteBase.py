import requests
from bs4 import BeautifulSoup
from urllib.parse import urlparse
from util.log import MyLog

class siteBase(object):

    __headers = {
        'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3',
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36',
        'Upgrade-Insecure-Requests': '1'
    }

    def __init__(self, url):
        self.__url = url


    def _encoding(self):
        return 'utf-8'

    def _getContent(self):
        response = requests.get(self.__url, headers = self.__headers)
        response.encoding = self._encoding
        return response.text

    def _parse(self, soup):
        MyLog.getLogger().log("siteBase.parse")

    def start(self):
        content = self._getContent()
        soup = BeautifulSoup(content, "lxml")
        self._parse(soup)