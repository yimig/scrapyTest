import datetime
import re


class Article:
    id = 0
    title = ""
    content = ""
    files = {}
    publish_date = None

    incoming_year = 0
    city = ""
    area = ""
    company = ""
    job = ""
    end_date = None

    __city_suffix = ['市']
    __area_suffix = ['区', '县', '镇', '自治州', '自治旗']

    def get_map(self):
        return {"title": self.title,
                "content": self.content,
                "publish_date": str(self.publish_date),
                "files": str(self.files.keys()),
                "incoming_year": self.incoming_year,
                "city": self.city,
                "area": self.area,
                "company": self.company,
                "job": self.job,
                }

    def add_title(self, title: str):
        self.title = title

    def add_content(self, content_list: list):
        self.content = self.__content_filter(content_list)

    def add_files(self, xpath_files):
        self.files = self.__get_files(xpath_files)

    def add_date(self, date_str: str):
        self.publish_date = self.__date_filter(date_str)

    def __content_merger(self, content_list: list):
        content = ""
        for content_piece in content_list:
            content += content_piece+'\n'
        return content

    def __content_filter(self, content_list: list):
        return self.__content_merger(content_list).replace('\u3000', ' ')

    def __date_filter(self, raw_date_str: str):
        '发布时间：2021-01-04 10:54:33　来源：'
        raw_date_str = re.sub('\s', ' ', raw_date_str)
        date_str = re.findall('\d{4}-\d{2}-\d{2}', raw_date_str)
        dt = None
        if date_str:
            date_list = self.__intalize(date_str[0].split('-'))
            time_str = re.findall('\d{2}:\d{2}:\d{2}', raw_date_str)
            if time_str:
                time_list = self.__intalize(time_str[0].split(':'))
                dt = datetime.datetime(year=date_list[0], month=date_list[1], day=date_list[2], hour=time_list[0], minute=time_list[1], second=time_list[2])
            else:
                dt = datetime.datetime(year=date_list[0], month=date_list[1], day=date_list[2])
        else:
            date_str = re.findall('(\d{4})年(\d{2}|\d)月(\d{2}|\d)日', self.content)[0]
            date_int = self.__intalize(date_str)
            try:
                dt = datetime.datetime(year=date_int[0], month=date_int[1], day=date_int[2])
            except:
                pass
        return dt

    # 将字符串列表转换成整型列表
    def __intalize(self, str_list: list):
        res_list = []
        for item in str_list:
            res_list.append(int(item))
        return res_list

    def __get_files(self, xpath_res: list):
        file_dict = {}
        for i in range(len(xpath_res)):
            raw_file_path = xpath_res[i].attrib['href']
            if raw_file_path.split('.')[-1] in ['doc', 'docx', 'xls', 'xlsx', 'pdf', 'txt']:
                file_dict.update({xpath_res[i].root.text: raw_file_path})
        # print(file_dict)
        return file_dict

    def __get_inside(self, raw_str: str, start_list: list, end_list: list, pos: int):
        res_str = ""
        end_pos = 0
        for start_str in start_list:
            start_pos = raw_str.find(start_str)
            if start_pos > 0:
                for end_str in end_list:
                    end_pos = raw_str.find(end_str)
                    if end_pos > 0:
                        if start_pos < end_pos:
                            res_str = raw_str[start_pos + 1:end_pos + 1]
                            pos = end_pos
                            return [pos, res_str]
        return [pos, res_str]

    def __get_incoming_year(self, title: str, pos: int):
        temp = re.match('\d{4}年', title)
        if temp:
            self.incoming_year = int(title[:4])
            pos += 4
        return pos

    def __get_city(self, title: str, pos: int):
        temp = self.__get_inside(title, '年', self.__city_suffix, pos)
        if temp[0] > -1:
            self.city = temp[1]
            pos = temp[0]
        return pos

    def __get_area(self, title: str, pos: int):
        temp = self.__get_inside(title, self.__city_suffix, self.__area_suffix, pos)
        if temp[0] > -1:
            self.area = temp[1]
            pos = temp[0]
        return pos

    def __get_company(self, title: str, pos: int):
        temp = self.__get_inside(title, title[pos], '招聘', pos)
        if temp[0] > -1:
            self.company = temp[1][:-1]
            pos = temp[0]
        return pos

    def __get_job(self, title: str, pos: int):
        if pos < len(title)-4:
            self.job = title[pos+2:-2]
            pos = len(title)-2
        return pos

    def __get_end_date(self, content: str):
        r_content = content.replace('日讯', '')
        date_list = []
        for date_str in re.findall('(\d{4})年(\d{2}|\d)月(\d{2}|\d)日', r_content):
            date_int = self.__intalize(date_str)
            try:
                date_list.append(datetime.datetime(year=date_int[0], month=date_int[1], day=date_int[2]))
            except:
                pass
        short_date_list = re.findall('(至|-|—)(\d{2}|\d)月(\d{2}|\d)日', r_content)
        if len(short_date_list) > 0:
            short_date = short_date_list[-1]
            end_pos = r_content.find(short_date[1] + '月' + short_date[2] + '日')
            year_list = re.findall('(\d{4})年', r_content[:end_pos])
            date_list.append(
                datetime.datetime(year=int(year_list[-1]), month=int(short_date[1]), day=int(short_date[2])))
        if len(date_list) > 0:
            self.end_date = max(date_list)

    def __analyze_title(self, title: str):
        '2021年福建水利电力职业技术学院招聘专任教师及辅导员公告'
        pos = 0
        pos = self.__get_incoming_year(title, pos)
        pos = self.__get_city(title, pos)
        pos = self.__get_area(title, pos)
        pos = self.__get_company(title, pos)
        self.__get_job(title, pos)

    def __analyze_content(self, content: str):
        self.__get_end_date(content)

    def begin_analyze(self):
        self.__analyze_title(self.title)
        self.__analyze_content(self.content)


    def __init__(self, id:int, title: str, content_list: list, xpath_files, date_str: str):
        self.id = id
        self.add_title(title)
        self.add_content(content_list)
        self.add_files(xpath_files)
        self.add_date(date_str)
        self.begin_analyze()

    def __init__(self, id: int):
        self.id = id

    def __str__(self):
        return "title:"+self.title+"\ncontent:"+self.content+"\n"
