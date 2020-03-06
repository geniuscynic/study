import os

folder = "template"
fileNames = os.listdir("template")

for fileName in fileNames:
    if (not "(1)" in fileName):
        continue

    newName = fileName.replace("(1)", "")

    newName = os.path.join(folder, newName)
    if(os.path.exists(newName)):
        continue
        
    os.rename(os.path.join(folder, fileName), newName)
    print(newName)