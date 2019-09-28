# -*- coding: utf-8 -*-
import scrapy
import time
import random
#sys.path.append('../')
#print(sys.path)
from mzitu.utils import commonUtils
import json

#sort 0 正序 1 逆序
class XimalayaSpider(scrapy.Spider):
    name = 'ximalaya'
    allowed_domains = ['www.ximalaya.com']
    url = 'https://www.ximalaya.com/revision/play/album?albumId=15919139&pageNum={}&sort=0&pageSize=30'
    pageNum = 1

    def start_requests(self):
        currentUrl = self.url.format(self.pageNum)
        yield scrapy.Request(url = currentUrl)

    def parse(self, response):
        #allLink = response.css("li._c2 a::attr(href)")
        albunm = json.loads(response.text)

        for item in albunm['data']['tracksAudioPlay']:
            houzhui = item['src'].split(".")[-1]

            name = item['trackName'].encode('utf-8').decode()
            yield {
                'name': "{0:0>3}_{1}.{2}".format(item['index'], name.replace("【鼎鼎爸爸双语故事】", ""), houzhui),
                #'name': "{}.{}".format(item['trackName'], houzhui),
                'file_urls': [item['src']]
            }
            #print(item['index'], item['trackName'], item['src'])

        if(albunm['data']['hasMore']):

            self.pageNum += 1
            nextUrl = self.url.format(self.pageNum)
            yield scrapy.Request(url = nextUrl)

        #yield scrapy.Request(response.urljoin("/revision/time"), callback = self.parseTime)
        #for link in allLink[0:2]:
            #detaiUrl = response.urljoin(link)
            #yield scrapy.Request(url = detaiUrl, callback = self.parseDetail)

        pass

    def parseTime(self, response):

        serverTime = response.text

        signText = 'ximalaya-{}'.format(serverTime)
        #print(signText)
        signText = "{}({}){}({}){}".format(commonUtils.get_md5(signText),
                                         random.randint(1,100),
                                         serverTime,
                                         random.randint(1,100),
                                         int(time.time()*1000))
        print(signText)

        #yield scrapy.Request(response.urljoin("/revision/play/album?albumId=3493173&pageNum=1&sort=1&pageSize=30"))
        pass
    