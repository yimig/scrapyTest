from .article import Article
from .mysql_connector import MysqlConnector
import csv
import enum


class ArticleWriterType(enum.Enum):
    CSV = 0
    MYSQL = 1


class ArticleWriter:
    file_name = ''
    write_type = None
    mysql_connector = None

    def __init__(self, write_type: ArticleWriterType, file_name: str = None):
        self.write_type = write_type
        if write_type == ArticleWriterType.CSV:
            self.file_name = file_name
        elif write_type == ArticleWriterType.MYSQL:
            self.mysql_connector = MysqlConnector()

    def __write_csv(self, article: Article):
        with open(self.file_name, 'a+', encoding='utf-8') as csvfile:
            field_names = ['title', 'content', 'publish_date', 'files', 'incoming_year', 'city', 'area', 'company', 'job']
            writer = csv.DictWriter(csvfile, fieldnames=field_names)

            writer.writeheader()
            writer.writerow(article.get_map())

    def __write_mysql(self, article: Article):
        self.mysql_connector.set_article(article)


    def write(self, article: Article):
        if self.write_type == ArticleWriterType.CSV:
            self.__write_csv(article)
        elif self.write_type == ArticleWriterType.MYSQL:
            self.__write_mysql(article)


