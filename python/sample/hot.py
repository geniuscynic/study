import requests
from bs4 import BeautifulSoup
from urllib.parse import urlparse

class hot(object):
    __headers = {
        'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3',
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36',
        'Upgrade-Insecure-Requests': '1'
    }

    # dict = {
    #     "ent.163.com": parse_wangyi
    # }

    def __init__(self):
        pass

    def __getContent(self, url, encoding='utf-8'):
        response = requests.get(url, headers = self.__headers)
        response.encoding = encoding
        return response.text

    def __getHotWords(self):
        url = "http://top.baidu.com/buzz?b=1&fr=topindex"
        content =  self.__getContent(url,encoding='GBK')
        soup = BeautifulSoup(content, "lxml")
        #news = soup.find_all(name="a", text="新闻")
        news = soup.select('tr a.list-title')

        print("step1")
        for new in news:
            yield new.string
            #yield new['href']

    def __getWordUrl(self, word):
        url = "https://www.baidu.com/s?rtt=1&bsst=1&cl=2&word={}".format(word)
        content =  self.__getContent(currentUrl)
        soup = BeautifulSoup(content, "lxml")
        newUrl = soup.select("div.result h3.c-title a")
        return newUrl

    def __getdUrlsFromWord(self, words):
        for word in words:
           yield self.__getWordUrl(word)

    
    def __getDetail(self, url):
        pass


    def start(self):
        words = self.__getHotWords()
        urls = self.__getdUrlsFromWord(words)
    