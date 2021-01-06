# Define your item pipelines here
#
# Don't forget to add your pipeline to the ITEM_PIPELINES setting
# See: https://docs.scrapy.org/en/latest/topics/item-pipeline.html


# useful for handling different item types with a single interface
from itemadapter import ItemAdapter
import os
import csv
from items import Article

class MyspiderPipeline:
    # def __init__(self):
    #     self.file = open('H:\\a.csv', 'a+', encoding='utf-8', newline='')
    #     self.writer = csv.writer(self.file, dialect='excel')


    def process_item(self, item:Article, spider):
        # print('正在写入')
        # self.writer.writerow(item.title, item.content)
        return item
    #
    # def close_spider(self, spider):
    #     self.file.close()
