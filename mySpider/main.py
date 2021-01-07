import os
BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
from scrapy.cmdline import execute
execute(["scrapy","crawl","shiyebian"])