import hashlib


def get_md5(ori_string):
    """
    md5 字符串
    :param ori_string:
    :return:
    """
    if isinstance(ori_string, str):
        ori_string = ori_string.encode("utf-8")

    m = hashlib.md5()
    m.update(ori_string)
    return m.hexdigest()
