import os
from pydub import AudioSegment
from concurrent.futures import ThreadPoolExecutor 
import threading

class AudioHelp(object):

    def __init__(self, dir):
        self.__dir = dir #"download"
        self.__types = {
            ".m4a": lambda filepath: AudioSegment.from_file(filepath)
        }

        #self.__p = Pool(4)


    def changeFormat(self, to_type):
        files = os.listdir(self.__dir)
        
        thread_pool = ThreadPoolExecutor(5)

        for path in files:
            paths = os.path.splitext(path)

            thread_pool.submit(self.__change_file_ext, self.__dir + "/" + path, paths[1], to_type)
            #self.__p.apply_async(self.__change_file_ext, args=(self.__dir + "/" + path, paths[1], to_type))
            #newName = paths[0] + to_type
            #self.__change_file_ext(self.__dir + "/" + path, paths[1], to_type)
            
        #self.__p.close()
        #self.__p.join()
        thread_pool.shutdown(True)

    #sound = AudioSegment.from_mp3(source_file_path)

    def __change_file_ext(self, filepath, from_type, to_type):  
        #print ("进程是{}").format(os.getpid())
        print(threading.current_thread().name)

        newName = filepath.replace(from_type, "." + to_type)

        #print(newName)
        if(os.path.exists(newName)):
            return

        

        song = self.__types[from_type](filepath)

        
        song.export(newName, format = to_type)

        print(newName)


def start():
    audioHelp = AudioHelp("download")
    audioHelp.changeFormat("mp3")

# p = Pool(4)
# p.apply_async(start)
# p.close()
# p.join()
start()
print("结束")


