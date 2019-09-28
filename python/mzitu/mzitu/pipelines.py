# -*- coding: utf-8 -*-

# Define your item pipelines here
#
# Don't forget to add your pipeline to the ITEM_PIPELINES setting
# See: https://docs.scrapy.org/en/latest/topics/item-pipeline.html
from scrapy.pipelines.images import FilesPipeline
import scrapy

class MzituPipeline(object):
    def process_item(self, item, spider):
        return item


class XimalangyaDownloadPipeline(FilesPipeline):
 
    def get_media_requests(self, item, info):
        for url in item['file_urls']:
            yield scrapy.Request(url, meta={'item': item})
 
    def file_path(self, request, response=None, info=None):
        item = request.meta['item']  # 通过上面的meta传递过来item
        #index = request.meta['index']
        #car_name = item['car_name'][index] + "." + request.url.split('/')[-1].split('.')[-1]
        down_file_name = item['name']
        return down_file_name

