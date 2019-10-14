import requests
from bs4 import BeautifulSoup

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

def step2(words):
    url = "https://www.baidu.com/s?rtt=1&bsst=1&cl=2&word={}"
    i=0
    for word in words:
        
        i += 1
        currentUrl = url.format(word)
        #url = url.replace("http://", "https://")
        content =  getContent(currentUrl)
        soup = BeautifulSoup(content, "lxml")
        newUrl = soup.select("div.result h3.c-title a")
        print(newUrl[0])

        if(i==2):
            return


urls = getHotWords()
step2(urls)
#url = "https://www.baidu.com/s?rtt=1&bsst=1&cl=2&word=%E6%9D%8E%E5%BF%83%E8%8D%89%E6%BA%BA%E4%BA%A1%E9%80%9A%E6%8A%A5"
#content = getContent(url)
#print(content)
