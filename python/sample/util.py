from urllib.parse import urlparse
from utils import *

def create_site_factory(url):
    res=urlparse(url)

    site_dict = {
        "ent.163.com": wangyi.wangyi,
        "baijiahao.baidu.com": baijiahao.baijiahao,
        "finance.ifeng.com": ifeng.ifeng,
        'finance.sina.com.cn': sina.sina,
        'news.sina.com.cn': sina_new.sina_new
    }

    if(res.netloc in site_dict):
        return site_dict[res.netloc](url)
    
    return ""