# -*- coding: utf-8 -*-
import scrapy
import logging

from mzitu.items import mzituItem, defaultItemsLoader


class MzitusSpider(scrapy.Spider):
    name = 'mzitus'
    allowed_domains = ['mzitu.com']
    start_urls = ['https://www.mzitu.com/all/']

    def parse(self, response):
        # print(response.text)
        # self.log("开始提取")
        logging.debug("开始提取")
        allLink = response.css("p.url a::attr(href)").getall()

        for link in allLink[0:2]:
            yield scrapy.Request(url = link, callback = self.parsePage)


    def parsePage(self, response):
        '''
        title = response.css(".main-title::text").get()
        category = response.css(".main-meta a::text").get()
        publishTime = response.css(".main-meta span:nth-child(2)::text").get()
        imgUrls = response.css(".main-image img::attr(src)").getall()
        tags = response.css(".main-tags a::text").getall()


        info = {
            'title': title,
            'category': category,
            'publishTime': publishTime,
            'imgUrls': imgUrls,
            'tags': tags
        }
        '''

        itemLoader = defaultItemsLoader(item = mzituItem(), response = response)
        itemLoader.add_css("title", ".main-title::text")
        itemLoader.add_css("category", ".main-meta a::text")
        itemLoader.add_css("publishTime", ".main-meta span:nth-child(2)::text")
        itemLoader.add_css("imgUrls", ".main-image img::attr(src)")
        itemLoader.add_css("tags", ".main-tags a::text")
        itemLoader.add_value("url_obj_id", response.url)
        itemLoader.add_value("url", response.url)

        yield self.fetctNextUrl(response, itemLoader).send(None)
        # allPageNavs = response.css(".pagenavi a").getall()

    def fetctNextUrl(self, response, itemLoader):
        """
        获取下个页面的链接, 没有则返回
        :param response:
        :param itemLoader:
        :return:
        """
        nextPage = response.css(".pagenavi a:nth-last-of-type(1)")
        if "下一页" in nextPage.css(" ::text").get():
            nextPage = nextPage.css("::attr(href)").get()
            yield scrapy.Request(url = nextPage, callback = self.parseOtherPage, cb_kwargs = {'itemLoader': itemLoader})
        else:
            # nextPage = ""
            item = itemLoader.load_item()
            yield item

    def parseOtherPage(self, response, itemLoader):
        """
        获取其他页面的image url
        :param itemLoader:
        :param response:
        :return:
        """
        # imgUrl = response.css(".main-image img::attr(src)").getall()
        itemLoader.add_css("imgUrls", ".main-image img::attr(src)")

        # info = response.cb_kwargs['info']
        # info['imgUrls'].add(imgUrl)
        yield self.fetctNextUrl(response, itemLoader).send(None)
