import pymysql
from .article import Article
import json

class MysqlConnector:
    conn = None

    def __get_connection(self):
        self.conn = pymysql.connect(host='127.0.0.1',
                                    user='shiyebian_user',
                                    password='1320',
                                    database='shiyebian')

    def __execute_sql(self, sql: str):
        cursor = self.conn.cursor()
        #cursor.execute(sql)
        try:
            cursor.execute(sql)
            self.conn.commit()
        except:
            self.conn.rollback()

    def __get_config(self, key: str):
        cursor = self.conn.cursor()
        sql = "select config_dict.`value` from config_dict where config_dict.`key` = '%s'" % key
        cursor.execute(sql)
        res = cursor.fetchone()[0]
        return res

    def __set_config(self, key: str, value: str):
        sql = "update config_dict set config_dict.`value` = '%s' where config_dict.`key` = '%s'" % (value, key)
        self.__execute_sql(sql)

    def set_article(self, article_obj: Article):
        files_str = json.dumps(article_obj.files, ensure_ascii=False)
        sql = "insert into article values('%d','%s','%s','%s','%s','%d','%s','%s','%s','%s','%s')" % (article_obj.id, article_obj.title, article_obj.content, files_str, article_obj.publish_date, article_obj.incoming_year, article_obj.city, article_obj.area, article_obj.company, article_obj.job, article_obj.end_date)
        self.__execute_sql(sql)

    def set_latest_id(self, id: int):
        self.__set_config('latest_id', str(id))

    def get_latest_id(self):
        cursor = self.conn.cursor()
        sql = "select max(id) from article"
        cursor.execute(sql)
        res = cursor.fetchone()[0]
        return int(res)

    def close_connection(self):
        self.conn.close()


    def __init__(self):
        self.__get_connection()
