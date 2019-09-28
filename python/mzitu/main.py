from scrapy.cmdline import execute

import sys
import os



filePath = os.path.abspath(__file__)
dirPath = os.path.dirname(filePath)
sys.path.append(dirPath)

execute(["scrapy", "crawl", "ximalaya"])

