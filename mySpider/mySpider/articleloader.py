from scrapy.loader import ItemLoader
from itemloaders import processors




class ArticleLoader(ItemLoader):
    title_in = processors.Identity()
    title_out = processors.Identity()
    content_in = processors.Join()
    content_out = processors.Identity()