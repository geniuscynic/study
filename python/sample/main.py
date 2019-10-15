import requests
from bs4 import BeautifulSoup
from urllib.parse import urlparse

def getContent(url, encoding='utf-8'):
    headers = {
        'Accept': 'text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3',
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36',
        'Upgrade-Insecure-Requests': '1'
    }
 
    response = requests.get(url, headers = headers)
    response.encoding = encoding
    return response.text


def getHotWords():
    url = "http://top.baidu.com/buzz?b=1&fr=topindex"
    content =  getContent(url,encoding='GBK')
    soup = BeautifulSoup(content, "lxml")
    #news = soup.find_all(name="a", text="新闻")
    news = soup.select('tr a.list-title')

    print("step1")
    for new in news:
        # if(new.string != "新闻"):
        #     continue

        yield new.string
        #yield new['href']

def getWordUrl(words):
    url = "https://www.baidu.com/s?rtt=1&bsst=1&cl=2&word={}"
    i=0
    for word in words:
        
        i += 1
        currentUrl = url.format(word)
        #url = url.replace("http://", "https://")
        content =  getContent(currentUrl)
        soup = BeautifulSoup(content, "lxml")
        newUrl = soup.select("div.result h3.c-title a")
        
        yield newUrl
        #print(newUrl[0]['href'])

        # if(i==2):
        #     return


def parseUrl(url):
    res=urlparse(url)
    
    dict = {
        "ent.163.com": parse_wangyi
    }

    dict[res.netloc](url)

def parse_wangyi(url):
    content = getContent(url, encoding='gbk')
    soup = BeautifulSoup(content, "lxml")

    title = soup.select_one("div.post_content_main h1")

    post_source = soup.select_one("div.post_content_main div.post_time_source" ).stripped_strings

    updateTime = repr(post_source.send(None)).replace('\\u3000来源:', '').replace("'", '')
    souce = repr(post_source.send(None)).replace("'", '')

    body_content = soup.select("div.post_content_main div.post_body div.post_text p" )

    #or string in body_content:
         #print(repr(string))
    print(updateTime, souce, body_content)

#words = getHotWords()
#urls = getWordUrl(words)

url = "http://ent.163.com/19/1014/09/EREJKEU900038FO9.html"
parseUrl(url)
#content = getContent(url)
#print(content)
