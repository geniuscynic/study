# -*- coding: utf-8 -*-

# Define here the models for your scraped items
#
# See documentation in:
# https://docs.scrapy.org/en/latest/topics/items.html

import scrapy
from scrapy.loader import ItemLoader
from scrapy.loader.processors import MapCompose, TakeFirst, Identity
import re

from mzitu.utils.commonUtils import get_md5


def date_convert(value):
    pattern = ".*?(\d{4}.*)"
    match_obj = re.match(pattern, value)
    if match_obj:
        value = match_obj.group(1)

    return value


class defaultItemsLoader(ItemLoader):
    default_output_processor = TakeFirst()


class mzituItem(scrapy.Item):
    # define the fields for your item here like:
    # name = scrapy.Field()
    url_obj_id = scrapy.Field(
        input_processor = MapCompose(get_md5)
    )

    url = scrapy.Field()

    title = scrapy.Field()

    category = scrapy.Field()

    publishTime = scrapy.Field(
        input_processor = MapCompose(date_convert)
    )

    imgUrls = scrapy.Field(
        output_processor = Identity()
    )

    tags = scrapy.Field(
        output_processor = Identity()
    )

    pass
