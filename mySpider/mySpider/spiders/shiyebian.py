import scrapy
from scrapy.http import Request
from spiders.article import Article
from .article_writer import ArticleWriter, ArticleWriterType


class ShiyebianSpider(scrapy.Spider):
    id = 363300
    error_count = 0
    name = 'shiyebian'
    allowed_domains = ['www.shiyebian.net']
    start_urls = ['http://www.shiyebian.net/xinxi/'+str(id)+'.html']
    writer = None
    sql_connector = None

    def __init__(self):
        self.writer = ArticleWriter(ArticleWriterType.MYSQL)

    def __get_url(self):
        return 'http://www.shiyebian.net/xinxi/'+str(self.id)+'.html'

    def remove_bracket(self, raw_str: str, is_recursion: bool = False):
        if raw_str[-1] == '(' or raw_str[:-1] == '（':
            return self.remove_bracket(raw_str[:-1])
        elif is_recursion or raw_str[-1] == ')' or raw_str[:-1] == '）':
            return self.remove_bracket(raw_str[:-1], True)
        else:
            return raw_str


    def parse(self, response):
        title = response.xpath('//h1/text()').extract_first()
        article = Article(self.id)
        if title == "服务器错误":
            self.error_count += 1
            print('errorcount++:'+str(self.error_count))
        elif self.remove_bracket(title)[-2:] in ['公告', '通告', '简章']:
            self.error_count = 0
            #print("get:"+title)
            article.add_title(title)
            article.add_content(response.xpath('//div[@class="content"]/div[@class="zhengwen"]/p/text()').extract())
            article.add_files(response.xpath('//div[@class="content"]/div[@class="zhengwen"]/p/a'))
            article.add_date(response.xpath('//div[@class="content"]/div[@class="info"]/text()').extract_first())
            article.begin_analyze()
            self.writer.write(article)
        self.id += 1
        print(str(article))
        if self.error_count < 10:
            yield Request(self.__get_url(), callback=self.parse)
