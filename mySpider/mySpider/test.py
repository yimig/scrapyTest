str1="2020年台州市生态环境局编外用工招聘公告(第一次)"
from mySpider.mySpider.spiders.mysql_connector import MysqlConnector


def remove_bracket(raw_str: str, is_recursion: bool = False):
    if raw_str[-1] == '(' or raw_str[:-1] == '（':
        return remove_bracket(raw_str[:-1])
    elif is_recursion or raw_str[-1] == ')' or raw_str[:-1] == '）':
        return remove_bracket(raw_str[:-1], True)
    else:
        return raw_str

def get_inside(raw_str: str, start_str: str, end_str: str):
    res_str = ""
    start_pos = raw_str.find(start_str)
    if start_pos > 0:
        end_pos = raw_str.find(end_str)
        if end_pos > 0:
            if start_pos < end_pos:
                res_str = raw_str[start_pos+1:end_pos+1]
    return res_str


if __name__ == '__main__':
    msc = MysqlConnector()
    print('done')
